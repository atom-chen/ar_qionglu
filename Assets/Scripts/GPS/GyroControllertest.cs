using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GyroControllertest : MonoBehaviour
{
#if UNITY_ANDROID
    #region [Private fields]


    private bool gyroEnabled = true;
    private const float lowPassFilterFactor = 0.5f;
    private readonly Quaternion baseIdentity = Quaternion.Euler(90, 0, 0);
    private readonly Quaternion landscapeRight = Quaternion.Euler(0, 0, 90);
    private readonly Quaternion landscapeLeft = Quaternion.Euler(0, 0, -90);
    private readonly Quaternion upsideDown = Quaternion.Euler(0, 0, 180);
    private Quaternion cameraBase;
    private Quaternion calibration = Quaternion.identity;
    private Quaternion baseOrientation = Quaternion.Euler(90, 0, 0);
    private Quaternion baseOrientationRotationFix = Quaternion.identity;
    private Quaternion referanceRotation = Quaternion.identity;
    private bool debug = true;

    #endregion
    #region [Unity events]

    protected void Start()
    {
        cameraBase = this.transform.rotation;
        AttachGyro();
        Input.gyro.enabled = true;
    }
    protected void Update()
    {
        if (gyroEnabled)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), lowPassFilterFactor);
            //transform.rotation = cameraBase*(ConvertRotation(referanceRotation*Input.gyro.attitude)*GetRotFix());
        }

    }

    /* 
     protected void OnGUI()
     {

         if (!debug)
             return;

         GUILayout.Label("方向: " + Screen.orientation);
         GUILayout.Label("校准: " + calibration);
         GUILayout.Label("摄像头底座: " + cameraBase);
         GUILayout.Label("陀螺仪状态: " + Input.gyro.attitude);
         GUILayout.Label("变换的旋转: " + transform.rotation);
         if (GUILayout.Button("ON / OFF陀螺仪: " + Input.gyro.enabled, GUILayout.Height(100)))
         {
             Input.gyro.enabled = !Input.gyro.enabled;
         }
         if (GUILayout.Button("ON / OFF陀螺仪控制器: " + gyroEnabled, GUILayout.Height(100)))
         {
             if (gyroEnabled)
             {
                 DetachGyro();
             }
             else
             {
                 AttachGyro();
             }
         }

         if (GUILayout.Button("更新陀螺仪校准（水平）", GUILayout.Height(80)))
         {
             UpdateCalibration(true);
         }
         if (GUILayout.Button("更新相机基地旋转（水平）", GUILayout.Height(80)))
         {
             UpdateCameraBaseRotation(true);
         }



         if (GUILayout.Button("复位的基本取向", GUILayout.Height(80)))
         {
             ResetBaseOrientation();
         }



         if (GUILayout.Button("复位旋转摄像头", GUILayout.Height(80)))
         {
             transform.rotation = Quaternion.identity;
         }
     }
    */


    #endregion
    #region [Public methods]

    /// <summary>
    /// 将陀螺控制器连接到变换.
    /// </summary>

    private void AttachGyro()
    {

        gyroEnabled = true;
        ResetBaseOrientation();
        UpdateCalibration(true);
        UpdateCameraBaseRotation(true);
        RecalculateReferenceRotation();
    }



    /// <summary>
    /// 将陀螺控制器从变换
    /// </summary>

    private void DetachGyro()
    {
        gyroEnabled = false;
    }

    #endregion
    #region [Private methods]

    /// <summary>
    /// 更新陀螺校准。
    /// </summary>

    private void UpdateCalibration(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            // var fw = (Input.gyro.attitude) * (-Vector3.forward);
            // fw.z = 0;
            // if (fw == Vector3.zero)
            // {
            //     calibration = Quaternion.identity;

            // }
            // else
            // {
            //calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
            // }
            calibration = Quaternion.identity;
        }
        else
        {
            calibration = Input.gyro.attitude;
        }
    }
    /// <summary>
    /// 更新相机底座旋转。
    /// </summary>
    /// <param name='onlyHorizontal'>
    /// 只有Y旋转。
    /// </param>
    public void UpdateCameraBaseRotation(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            var fw = transform.forward;
            fw.y = 0;
            if (fw == Vector3.zero)
            {
                cameraBase = Quaternion.identity;
            }
            else
            {
                cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
            }
        }
        else
        {
            cameraBase = transform.rotation;
        }
    }
    /// <summary>
    /// 将旋转从右交给左手。
    /// </summary>
    /// <returns>
    ///返回角度
    /// </returns>
    /// <param name='q'>
    /// 旋转转换。
    /// </param>
    private static Quaternion ConvertRotation(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
    /// <summary>
    /// 获得不同方向的修复。
    /// </summary>
    /// <returns>
    /// The rot fix.
    /// </returns>
    private Quaternion GetRotFix()
    {

        //#if UNITY_3_5
        if (Screen.orientation == ScreenOrientation.Portrait)
            return Quaternion.identity;
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape)
            return landscapeLeft;
        if (Screen.orientation == ScreenOrientation.LandscapeRight)
            return landscapeRight;
        if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            return upsideDown;
        return Quaternion.identity;

        //#else
        //        return Quaternion.identity;
        //#endif
    }
    /// <summary>
    /// 重新计算的参考系统。
    /// </summary>
    private void ResetBaseOrientation()
    {
        baseOrientationRotationFix = GetRotFix();
        baseOrientation = baseOrientationRotationFix * baseIdentity;

    }

    /// <summary>
    /// 重新计算基准旋转。
    /// </summary>
    private void RecalculateReferenceRotation()
    {
        referanceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);

    }
    #endregion
#elif UNITY_IOS || UNITY_IPHONE

    private float initialYAngle = 0f;
    private float appliedGyroYAngle = 0f;
    private float calibrationYAngle = 0f;
    [SerializeField]
    private bool initDone = false;
    public void Awake()
    {
        Input.compass.enabled = true;
        Input.gyro.enabled = true;
        Input.location.Start();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        //initialYAngle = transform.eulerAngles.y;
        initialYAngle = Input.compass.magneticHeading;
    }

    void Update()
    {
        if(initialYAngle.Equals(0))
        {
            initialYAngle = Input.compass.magneticHeading;
        }
        else
        {
            //transform.rotation = Quaternion.Euler(Input.gyro.attitude.eulerAngles.x, Input.compass.magneticHeading, Input.gyro.attitude.eulerAngles.z);
            ApplyGyroRotation();
            //ApplyCalibration();
        }
    }

    /*
    void OnGUI()
    {
        if (GUILayout.Button("Calibrate", GUILayout.Width(300), GUILayout.Height(100)))
        {
            CalibrateYAngle();
        }
    }
    public void CalibrateYAngle()
    {
        calibrationYAngle = appliedGyroYAngle - initialYAngle; // Offsets the y angle in case it wasn't 0 at edit time.
    }
*/

    void ApplyGyroRotation()
    {
        transform.rotation = Input.gyro.attitude;
        transform.Rotate(0f, 0f, 180f, Space.Self); // Swap "handedness" of quaternion from gyro.
        transform.Rotate(90f, 180f, 0f, Space.World); // Rotate to make sense as a camera pointing out the back of your device.
       // appliedGyroYAngle = transform.eulerAngles.y;
        //CalibrateYAngle();
        ApplyCalibration();
        //appliedGyroYAngle = transform.eulerAngles.y; // Save the angle around y axis for use in calibration.
    }

    void ApplyCalibration()
    {
        transform.Rotate(0f, initialYAngle, 0f, Space.World); // Rotates y angle back however much it deviated when calibrationYAngle was saved.
    }


#endif
}
