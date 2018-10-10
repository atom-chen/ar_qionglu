/*==============================================================================
Copyright (c) 2017-2018 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
==============================================================================*/

using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using ElviraFrame.AB;
using ElviraFrame;

public class PlaneManager : SingletonMono<PlaneManager>
{
  

    #region PUBLIC_MEMBERS
    public PlaneFinderBehaviour m_PlaneFinder;
    public MidAirPositionerBehaviour m_MidAirPositioner;
    //[HideInInspector]
    public GameObject showGameObject;
    protected string showGameObjectName;
    public List<MeshRenderer> renderMaterialsList = new List<MeshRenderer>();
    public List<SkinnedMeshRenderer> skinMaterialsList = new List<SkinnedMeshRenderer>();
    public static bool GroundPlaneHitReceived;
    public  PlaneMode planeMode = PlaneMode.GROUND;


    public GameObject planeAnchor, midAirAnchor;


    const string unsupportedDeviceTitle = "Unsupported Device";
    const string unsupportedDeviceBody =
        "This device has failed to start the Positional Device Tracker. " +
        "Please check the list of supported Ground Plane devices on our site: " +
        "\n\nhttps://library.vuforia.com/articles/Solution/ground-plane-supported-devices.html";

    public static bool AnchorExists
    {
        get { return anchorExists; }
        private set { anchorExists = value; }
    }


    #endregion // PUBLIC_MEMBERS


    #region PRIVATE_MEMBERS


    protected StateManager m_StateManager;
    protected SmartTerrain m_SmartTerrain;
    protected PositionalDeviceTracker m_PositionalDeviceTracker;
    protected ContentPositioningBehaviour m_ContentPositioningBehaviour;


    protected GroundPlaneUI m_GroundPlaneUI;
    protected AnchorBehaviour m_PlaneAnchor, m_MidAirAnchor;
    protected int AutomaticHitTestFrameCount;
    protected int m_AnchorCounter;
    protected bool uiHasBeenInitialized;
    protected static bool anchorExists; // backs public AnchorExists property
    bool isPlaced = false;



    Light mainLight;
    #endregion // PRIVATE_MEMBERS



    #region MONOBEHAVIOUR_METHODS

    void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnVuforiaPaused);
        DeviceTrackerARController.Instance.RegisterTrackerStartedCallback(OnTrackerStarted);
        DeviceTrackerARController.Instance.RegisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);

        m_PlaneFinder.HitTestMode = HitTestMode.AUTOMATIC;



        m_GroundPlaneUI = FindObjectOfType<GroundPlaneUI>();

        m_PlaneAnchor = planeAnchor.GetComponent<AnchorBehaviour>();
        m_MidAirAnchor = midAirAnchor.GetComponent<AnchorBehaviour>();

        isPlaced = false;
        mainLight = GameObject.FindGameObjectWithTag(Tags.Light).GetComponent<Light>();
    }

    void Update()
    {
        if (!VuforiaRuntimeUtilities.IsPlayMode() && !AnchorExists)
        {
            AnchorExists = DoAnchorsExist();
        }

        GroundPlaneHitReceived = (AutomaticHitTestFrameCount == Time.frameCount);
        //Debug.Log("GroundPlaneHitReceived===" + GroundPlaneHitReceived);

        //if (m_status == TrackableBehaviour.Status.TRACKED)
        //{
        //    if (isPlaced == false)
        //    {
        //        SetSurfaceIndicatorVisible(true);

        //    }

        //}
        //else
        //{
        //    SetSurfaceIndicatorVisible(false);
        //}
        //SetSurfaceIndicatorVisible(   GroundPlaneHitReceived &&planeMode == PlaneMode.GROUND);
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy() called.");

        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.UnregisterOnPauseCallback(OnVuforiaPaused);
        DeviceTrackerARController.Instance.UnregisterTrackerStartedCallback(OnTrackerStarted);
        DeviceTrackerARController.Instance.UnregisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region GROUNDPLANE_CALLBACKS

    public void HandleAutomaticHitTest(HitTestResult result)
    {
      //  AutomaticHitTestFrameCount = Time.frameCount;

      //Debug.Log("AutomaticHitTestFrameCount=====" + AutomaticHitTestFrameCount + (AutomaticHitTestFrameCount == Time.frameCount));

      //  if (!uiHasBeenInitialized)
      //  {
      //      uiHasBeenInitialized = m_GroundPlaneUI.InitializeUI();
      //  }
    }
    public virtual void HandleInteractiveHitTest(HitTestResult result)
    {

        if (result == null)
        {
            Debug.LogError("Invalid hit test result!");
            return;
        }
        if (!m_GroundPlaneUI.IsCanvasButtonPressed())
        {
            Debug.Log("HandleInteractiveHitTest() called.");
            // If the PlaneFinderBehaviour's Mode is Automatic, then the Interactive HitTestResult will be centered.
            // PlaneMode.Ground and PlaneMode.Placement both use PlaneFinder's ContentPositioningBehaviour
            m_ContentPositioningBehaviour = m_PlaneFinder.GetComponent<ContentPositioningBehaviour>();
            m_ContentPositioningBehaviour.DuplicateStage = false;
            // Place object based on Ground Plane mode
            if (showGameObject != null)
            {

                showGameObject.gameObject.SetActive(true);
                isPlaced = true;
                if (showGameObjectName== "haiou")
                {
                    showGameObject.GetComponent<Haiou>().Init();
                }
                else
                {
                    FingerTouchEL.Instance.targetGameObject = showGameObject;

                }


                UtilityHelper.EnableRendererColliderCanvas(showGameObject, true);

                switch (showGameObject.GetComponent<WriteItem>().goodsPositionEnum)
                {
                    case PlaneMode.None:
                        break;
                    case PlaneMode.GROUND:
                        m_ContentPositioningBehaviour.AnchorStage = m_PlaneAnchor;
                        m_ContentPositioningBehaviour.PositionContentAtPlaneAnchor(result);
                        showGameObject.transform.parent = planeAnchor.transform;

                        break;
                    case PlaneMode.MIDAIR:
                        m_ContentPositioningBehaviour.AnchorStage = m_MidAirAnchor;
                        m_ContentPositioningBehaviour.PositionContentAtMidAirAnchor(showGameObject.transform);
                        showGameObject.transform.parent = midAirAnchor.transform;
                        break;
                    default:
                        break;
                }


                showGameObject.transform.localPosition = Vector3.zero;
                showGameObject.transform.localEulerAngles = Vector3.zero;
                //    UtilityHelper.RotateTowardCamera(showGameObject);
                YiyouStaticDataManager.Instance.ShowModel = showGameObject;

                WriteItem writeItem = showGameObject.GetComponent<WriteItem>();
                if (writeItem != null && writeItem.goodsEnum != GoodsWriteEnum.None)
                {

                    ShowInput();
                    GroundPlaneUI.Instance.SetIntroductionText("请选择字体并输入文字");
                }
                else
                {
               
                    GroundPlaneUI.Instance.SetIntroductionText("", false);

                   ShowButtonPanel();
                }
         
     ShowEffectPanel();
                GroundPlaneUI.Instance.SetReticleVisiblity(false);
                WriteManager.Instance.SetGoodsEnum(showGameObject.GetComponent<WriteItem>().goodsEnum);
            }
            else
            {
                GroundPlaneUI.Instance.SetIntroductionText("请先选择模型");
            }

        }
    }

    private void ShowInput()
    {

        WriteManager.Instance.ShowInputPanel(true);
    }
    private void ShowEffectPanel()
    {
       
            GroundPlaneUI.Instance.ShowEffectPanel();

        
    }
    
  private void ShowButtonPanel()
    {

        GroundPlaneUI.Instance.ShowButtonPanel();
    }
    public void PlaceObjectInMidAir(Transform midAirTransform)
    {
        if (showGameObject != null && showGameObject.GetComponent<WriteItem>().goodsPositionEnum == PlaneMode.MIDAIR)
        {
            Debug.Log("PlaceObjectInMidAir() called.");

            m_ContentPositioningBehaviour.AnchorStage = m_MidAirAnchor;
            m_ContentPositioningBehaviour.PositionContentAtMidAirAnchor(midAirTransform);
            UtilityHelper.EnableRendererColliderCanvas(showGameObject, true);
            showGameObject.transform.parent = planeAnchor.transform;
            showGameObject.transform.localPosition = Vector3.zero;

            UtilityHelper.RotateTowardCamera(showGameObject);
            ShowInput();
        }
    }

    #endregion // GROUNDPLANE_CALLBACKS


    #region PUBLIC_BUTTON_METHODS

   

    string modelName = string.Empty;
    public void SetMode(string goName)
    {


        if (showGameObject != null)
        {
      
     Destroy(showGameObject);

        }

        YiyouStaticDataManager.Instance.OnDestroyGameObject();
        modelName = goName;
        showGameObjectName = goName;
        YiyouStaticDataManager.Instance.modelName = goName;


        LoadComplete(showGameObjectName);
    }


    public void LoadComplete(string modelName)
    {
        //2、提取资源
        if (showGameObject != null)
        {
            Destroy(showGameObject);
        }
        YiyouStaticDataManager.Instance.OnDestroyGameObject();
        showGameObject =YiyouStaticDataManager.Instance.LoadGameObject(modelName);
        if (showGameObject)
        {
            showGameObject = Instantiate(showGameObject);
            showGameObject.name = showGameObjectName;
        }
  
            switch (showGameObject.GetComponent<WriteItem>().goodsPositionEnum)
            {

                case PlaneMode.GROUND:
                    planeMode = PlaneMode.GROUND;

                    m_PlaneFinder.enabled = true;
                    m_MidAirPositioner.enabled = false;

                    break;
                case PlaneMode.MIDAIR:
                    planeMode = PlaneMode.MIDAIR;

                    m_PlaneFinder.enabled = false;
                    m_MidAirPositioner.enabled = true;

                    break;
                default:
                    break;
            }

            skinMaterialsList.Clear();
            renderMaterialsList.Clear();
            if (showGameObjectName == "wurenji")
            {
                SkinnedMeshRenderer[] ziTransform = showGameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
                if (ziTransform.Length != 0)
                {

                    foreach (var item in ziTransform)
                    {
                        if (item.name == "zi")
                        {
                            skinMaterialsList.Add(item);
                        }
                    }
                }
            }
            else
            {
                MeshRenderer[] ziTransform = showGameObject.transform.GetComponentsInChildren<MeshRenderer>();
                if (ziTransform.Length != 0)
                {

                    foreach (var item in ziTransform)
                    {
                        if (item.name == "zi")
                        {
                            renderMaterialsList.Add(item);
                        }
                    }
                }
            }

            SetMterials(showGameObject.GetComponent<WriteItem>().goodsEnum);
        

        showGameObject.gameObject.SetActive(false);
        GroundPlaneUI.Instance.SelectModelPanel.gameObject.SetActive(false);
        GroundPlaneUI.Instance.SetIntroductionText("请将镜头朝向地面");
    }
    protected virtual void SetMterials(GoodsWriteEnum goodsWriteEnum)
    {

        switch (goodsWriteEnum)
        {

            case GoodsWriteEnum.Single:
                if (showGameObjectName == "wurenji")
                {
                    if (skinMaterialsList.Count != 0)
                    {
                        Debug.Log("dddd111111111");
                        foreach (SkinnedMeshRenderer item in skinMaterialsList)
                        {
                            item.material = YiyouStaticDataManager.Instance.GetMaterial("Single");
                        }
                    }
                }
                else
                {
                    if (renderMaterialsList.Count != 0)
                    {
                        Debug.Log("dddd2222222");
                        foreach (MeshRenderer item in renderMaterialsList)
                        {
                            item.material = YiyouStaticDataManager.Instance.GetMaterial("Single");
                        }
                    }
                }

                break;
            case GoodsWriteEnum.Two:
                break;
            case GoodsWriteEnum.Three:
                break;
            default:
                break;
        }
    }
    public void ResetScene()
    {
        Debug.Log("ResetScene() called.");
 
        showGameObjectName = "";
        DeleteAnchors();
        m_GroundPlaneUI.Reset();
        YiyouStaticDataManager.Instance.OnDestroyGameObject();
        if (showGameObject != null)
        {
            Destroy(showGameObject);

        }
        isPlaced = false;
        SetSurfaceIndicatorVisible(false);
    }

    public void ResetTrackers()
    {
        Debug.Log("ResetTrackers() called.");

        m_SmartTerrain = TrackerManager.Instance.GetTracker<SmartTerrain>();
        m_PositionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();

        // Stop and restart trackers
        m_SmartTerrain.Stop(); // stop SmartTerrain tracker before PositionalDeviceTracker
        m_PositionalDeviceTracker.Reset();
        m_SmartTerrain.Start(); // start SmartTerrain tracker after PositionalDeviceTracker
    }

    #endregion // PUBLIC_BUTTON_METHODS


    #region PRIVATE_METHODS

    void DeleteAnchors()
    {
        m_PlaneAnchor.UnConfigureAnchor();
        m_MidAirAnchor.UnConfigureAnchor();

        AnchorExists = DoAnchorsExist();
    }
    GameObject surface;
    void SetSurfaceIndicatorVisible(bool isVisible)
    {
        surface = m_PlaneFinder.transform.GetChild(0).GetChild(0).gameObject;
        if (isPlaced==false)
        {
            surface.gameObject.SetActive(isVisible);
            GroundPlaneUI.Instance.SetReticleVisiblity(!isVisible);

        }


    }

    bool DoAnchorsExist()
    {
        if (m_StateManager != null)
        {
            IEnumerable<TrackableBehaviour> trackableBehaviours = m_StateManager.GetActiveTrackableBehaviours();

            foreach (TrackableBehaviour behaviour in trackableBehaviours)
            {
                if (behaviour is AnchorBehaviour)
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion // PRIVATE_METHODS


    #region VUFORIA_CALLBACKS

    void OnVuforiaStarted()
    {
        Debug.Log("OnVuforiaStarted() called.");

        m_StateManager = TrackerManager.Instance.GetStateManager();

        // Check trackers to see if started and start if necessary
        m_PositionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
        m_SmartTerrain = TrackerManager.Instance.GetTracker<SmartTerrain>();

        if (m_PositionalDeviceTracker != null && m_SmartTerrain != null)
        {
            if (!m_PositionalDeviceTracker.IsActive)
                m_PositionalDeviceTracker.Start();
            if (m_PositionalDeviceTracker.IsActive && !m_SmartTerrain.IsActive)
                m_SmartTerrain.Start();
        }
        else
        {
            YiyouStaticDataManager.Instance.DisposeAB();

                SceneManager.LoadScene("wikiSLAM");

        }
    }

    void OnVuforiaPaused(bool paused)
    {
        Debug.Log("OnVuforiaPaused(" + paused.ToString() + ") called.");

        if (paused)
            ResetScene();
    }

    #endregion // VUFORIA_CALLBACKS


    #region DEVICE_TRACKER_CALLBACKS

    void OnTrackerStarted()
    {
        Debug.Log("OnTrackerStarted() called.");

        m_PositionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
        m_SmartTerrain = TrackerManager.Instance.GetTracker<SmartTerrain>();

        if (m_PositionalDeviceTracker != null)
        {
            if (!m_PositionalDeviceTracker.IsActive)
                m_PositionalDeviceTracker.Start();

            Debug.Log("PositionalDeviceTracker is Active?: " + m_PositionalDeviceTracker.IsActive +
                      "\nSmartTerrain Tracker is Active?: " + m_SmartTerrain.IsActive);
        }
    }
    TrackableBehaviour.Status m_status=TrackableBehaviour.Status.LIMITED;
    void OnDevicePoseStatusChanged(TrackableBehaviour.Status status, TrackableBehaviour.StatusInfo statusInfo)
    {
        Debug.Log("OnDevicePoseStatusChanged(" + status + ", " + statusInfo + ")");
        m_status = status;
        if (status==TrackableBehaviour.Status.TRACKED)
        {
            SetSurfaceIndicatorVisible(true);

        }
        else
        {
            SetSurfaceIndicatorVisible(false);
        }
    }

    #endregion // DEVICE_TRACKER_CALLBACK_METHODS

    #region     模型效果设置
    internal void SetIntensityValue(float value)
    {
        mainLight.intensity = value;
    }

    internal void SetLightLeftValue(float value)
    {
        Vector3 rot = mainLight.transform.localEulerAngles;
        mainLight.transform.localEulerAngles = new UnityEngine.Vector3(rot.x, value, rot.z);

    }

    internal void SetLightUpValue(float value)
    {
        Vector3 rot = mainLight.transform.parent.localEulerAngles;
        mainLight.transform.parent.localEulerAngles = new UnityEngine.Vector3(value, rot.y, rot.z);
    }

    internal void SetModelScale(float value)
    {
        if (showGameObject != null)
        {
            showGameObject.transform.localScale = Vector3.one * value;
        }
    }
    #endregion
}
