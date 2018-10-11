//
// Fingers Gestures
// (c) 2015 Digital Ruby, LLC
// Source code may be used for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace ElviraFrame
{
    public class FingerTouchEL : SingletonMono<FingerTouchEL>
    {


        public delegate void TouchBeginHandle(GameObject  obj);

        public TouchBeginHandle touchBeginGo;


        public delegate void TouchEndHandle(GameObject obj);

        public TouchBeginHandle touchEndGo;




        
        [Tooltip("移动速度，越大越慢")]
        private  float moveSpeedDelta = 1000.0f;

  
        public GameObject targetGameObject;


   

        private TapGestureRecognizer tapGesture;
        private TapGestureRecognizer doubleTapGesture;
        private TapGestureRecognizer tripleTapGesture;
        private SwipeGestureRecognizer swipeGesture;
        private PanGestureRecognizer panGesture;
        private ScaleGestureRecognizer scaleGesture;
        private RotateGestureRecognizer rotateGesture;
        private LongPressGestureRecognizer longPressGesture;

        private float nextAsteroid = float.MinValue;
        private GameObject draggingAsteroid;

        private readonly List<Vector3> swipeLines = new List<Vector3>();

        private void BeginDrag(float screenX, float screenY)
        {
            Vector3 pos = new Vector3(screenX, screenY, 0.0f);
            pos = Camera.main.ScreenToWorldPoint(pos);
            RaycastHit2D hit = Physics2D.CircleCast(pos, 10.0f, Vector2.zero);
            if (hit.transform != null)        
            {
                longPressGesture.Reset();
            }
        }

        private void DragTo(float screenX, float screenY)
        {
            if (draggingAsteroid == null)
            {
                return;
            }

            Vector3 pos = new Vector3(screenX, screenY, 0.0f);
            pos = Camera.main.ScreenToWorldPoint(pos);
            draggingAsteroid.GetComponent<Rigidbody2D>().MovePosition(pos);
        }

        private void EndDrag(float velocityXScreen, float velocityYScreen)
        {
            if (draggingAsteroid == null)
            {
                return;
            }

            Vector3 origin = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 end = Camera.main.ScreenToWorldPoint(new Vector3(velocityXScreen, velocityYScreen, 0.0f));
            Vector3 velocity = (end - origin);
            draggingAsteroid.GetComponent<Rigidbody2D>().velocity = velocity;
            draggingAsteroid.GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range(5.0f, 45.0f);
            draggingAsteroid = null;

         
        }

        private void HandleSwipe(float endX, float endY)
        {
            Vector2 start = new Vector2(swipeGesture.StartFocusX, swipeGesture.StartFocusY);
            Vector3 startWorld = Camera.main.ScreenToWorldPoint(start);
            Vector3 endWorld = Camera.main.ScreenToWorldPoint(new Vector2(endX, endY));
            float distance = Vector3.Distance(startWorld, endWorld);
            startWorld.z = endWorld.z = 0.0f;

            swipeLines.Add(startWorld);
            swipeLines.Add(endWorld);

            if (swipeLines.Count > 4)
            {
                swipeLines.RemoveRange(0, swipeLines.Count - 4);
            }

            RaycastHit2D[] collisions = Physics2D.CircleCastAll(startWorld, 10.0f, (endWorld - startWorld).normalized, distance);

            if (collisions.Length != 0)
            {
                Debug.Log("Raycast hits: " + collisions.Length + ", start: " + startWorld + ", end: " + endWorld + ", distance: " + distance);

                Vector3 origin = Camera.main.ScreenToWorldPoint(Vector3.zero);
                Vector3 end = Camera.main.ScreenToWorldPoint(new Vector3(swipeGesture.VelocityX, swipeGesture.VelocityY, Camera.main.nearClipPlane));
                Vector3 velocity = (end - origin);
                Vector2 force = velocity * 500.0f;

                foreach (RaycastHit2D h in collisions)
                {
                    h.rigidbody.AddForceAtPosition(force, h.point);
                }
            }
        }

        private void TapGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                

            }
        }

        private void CreateTapGesture()
        {
            tapGesture = new TapGestureRecognizer();
            tapGesture.StateUpdated += TapGestureCallback;
            tapGesture.RequireGestureRecognizerToFail = doubleTapGesture;
            FingersScript.Instance.AddGesture(tapGesture);
        }

        private void DoubleTapGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
             
               
            }
        }

        private void CreateDoubleTapGesture()
        {
            doubleTapGesture = new TapGestureRecognizer();
            doubleTapGesture.NumberOfTapsRequired = 2;
            doubleTapGesture.StateUpdated += DoubleTapGestureCallback;
            doubleTapGesture.RequireGestureRecognizerToFail = tripleTapGesture;
            FingersScript.Instance.AddGesture(doubleTapGesture);
        }

        private void SwipeGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                HandleSwipe(gesture.FocusX, gesture.FocusY);
            }
        }

        private void CreateSwipeGesture()
        {
            swipeGesture = new SwipeGestureRecognizer();
            swipeGesture.Direction = SwipeGestureRecognizerDirection.Any;
            swipeGesture.StateUpdated += SwipeGestureCallback;
            swipeGesture.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
            FingersScript.Instance.AddGesture(swipeGesture);
        }

        private void PanGestureCallback(GestureRecognizer gesture)
        {

            if (gesture.State == GestureRecognizerState.Executing&&targetGameObject!=null)
            {

                float deltaX = panGesture.DeltaX / moveSpeedDelta*-1f;
                float deltaZ = panGesture.DeltaY / moveSpeedDelta * -1f;
                Vector3 pos = targetGameObject.transform.localPosition;
                pos.x += deltaX;
                pos.z+= deltaZ;
                targetGameObject.transform.localPosition = pos;
            }
        }

        private void CreatePanGesture()
        {
            panGesture = new PanGestureRecognizer();
            panGesture.MinimumNumberOfTouchesToTrack = 1;
            panGesture.StateUpdated += PanGestureCallback;
            FingersScript.Instance.AddGesture(panGesture);
        }

        private void ScaleGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State==GestureRecognizerState.Began && targetGameObject != null)
            {
                if (touchBeginGo != null)
                {
                    touchBeginGo(gameObject);
                }
            }
      
            if (gesture.State == GestureRecognizerState.Executing&&targetGameObject!=null)
            {
                float clamp = Mathf.Clamp(scaleGesture.ScaleMultiplier, 0.3f, 1.3f);
                targetGameObject.transform.localScale *= clamp;
            }

            if (gesture.State == GestureRecognizerState.Ended && targetGameObject != null)
            {
                if (touchEndGo != null)
                {
                    touchEndGo(gameObject);
                }
            }
        }

        private void CreateScaleGesture()
        {
            scaleGesture = new ScaleGestureRecognizer();

       
            scaleGesture.StateUpdated += ScaleGestureCallback;
            FingersScript.Instance.AddGesture(scaleGesture);
        }

        private void RotateGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Executing&&targetGameObject!=null)
            {
                targetGameObject.transform.Rotate(0.0f,  rotateGesture.RotationRadiansDelta * Mathf.Rad2Deg,0.0f);
            }
        }

        private void CreateRotateGesture()
        {
            rotateGesture = new RotateGestureRecognizer();
            rotateGesture.StateUpdated += RotateGestureCallback;
            FingersScript.Instance.AddGesture(rotateGesture);
        }

        private void LongPressGestureCallback(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Began)
            {
                BeginDrag(gesture.FocusX, gesture.FocusY);
            }
            else if (gesture.State == GestureRecognizerState.Executing)
            {
                DragTo(gesture.FocusX, gesture.FocusY);
            }
            else if (gesture.State == GestureRecognizerState.Ended)
            {
                EndDrag(longPressGesture.VelocityX, longPressGesture.VelocityY);
            }
        }

        private void CreateLongPressGesture()
        {
            longPressGesture = new LongPressGestureRecognizer();
            longPressGesture.MaximumNumberOfTouchesToTrack = 1;
            longPressGesture.StateUpdated += LongPressGestureCallback;
            FingersScript.Instance.AddGesture(longPressGesture);
        }

        private void PlatformSpecificViewTapUpdated(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                Debug.Log("You triple tapped the platform specific label!");
            }
        }

        private void CreatePlatformSpecificViewTripleTapGesture()
        {
            tripleTapGesture = new TapGestureRecognizer();
            tripleTapGesture.StateUpdated += PlatformSpecificViewTapUpdated;
            tripleTapGesture.NumberOfTapsRequired = 3;
        
            FingersScript.Instance.AddGesture(tripleTapGesture);
        }

        private static bool? CaptureGestureHandler(GameObject obj)
        {
            // I've named objects PassThrough* if the gesture should pass through and NoPass* if the gesture should be gobbled up, everything else gets default behavior
            if (obj.name.StartsWith("PassThrough"))
            {
                // allow the pass through for any element named "PassThrough*"
                return false;
            }
            else if (obj.name.StartsWith("NoPass"))
            {
                // prevent the gesture from passing through, this is done on some of the buttons and the bottom text so that only
                // the triple tap gesture can tap on it
                return true;
            }

            // fall-back to default behavior for anything else
            return null;
        }
        public bool isCanPan, isCanScale, isCamRotate;
        private void Start()
        {
          
            // don't reorder the creation of these :)
            CreatePlatformSpecificViewTripleTapGesture();
         //   CreateDoubleTapGesture();
           // CreateTapGesture();
            CreateSwipeGesture();
            if (isCanPan)
                 CreatePanGesture();
            if (isCanScale)
                CreateScaleGesture();
            if (isCamRotate)
                CreateRotateGesture();
            CreateLongPressGesture();

            // pan, scale and rotate can all happen simultaneously
            panGesture.AllowSimultaneousExecution(scaleGesture);
            panGesture.AllowSimultaneousExecution(rotateGesture);
            if (isCanScale)
                scaleGesture.AllowSimultaneousExecution(rotateGesture);

            // prevent the one special no-pass button from passing through,
            //  even though the parent scroll view allows pass through (see FingerScript.PassThroughObjects)
            FingersScript.Instance.CaptureGestureHandler = CaptureGestureHandler;
        }

   

        private void LateUpdate()
        {
            if (targetGameObject==null)
            {
                return;
            }
            int touchCount = Input.touchCount;
            if (FingersScript.Instance.TreatMousePointerAsFinger && Input.mousePresent)
            {
                touchCount += (Input.GetMouseButton(0) ? 1 : 0);
                touchCount += (Input.GetMouseButton(1) ? 1 : 0);
                touchCount += (Input.GetMouseButton(2) ? 1 : 0);
            }
            string touchIds = string.Empty;
            int gestureTouchCount = 0;
            foreach (GestureRecognizer g in FingersScript.Instance.Gestures)
            {
                gestureTouchCount += g.CurrentTrackedTouches.Count;
            }
            foreach (GestureTouch t in FingersScript.Instance.CurrentTouches)
            {
                touchIds += ":" + t.Id + ":";
            }
        }



    }
}
