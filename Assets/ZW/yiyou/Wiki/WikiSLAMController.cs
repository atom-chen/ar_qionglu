using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Wikitude;
using System;

using Plane = UnityEngine.Plane;
using ElviraFrame;

public class WikiSLAMController : SingletonMono<WikiSLAMController>
{
    public InstantTracker Tracker;
    public InstantTrackable instantTrackable;


    public Image ActivityIndicator;

    [HideInInspector]
    public Color EnabledColor = new Color(0.2f, 0.75f, 0.2f, 0.8f);
    [HideInInspector]
    public Color DisabledColor = new Color(1.0f, 0.2f, 0.2f, 0.8f);


    private HashSet<GameObject> _activeModels = new HashSet<GameObject>();
    private InstantTrackingState _currentState = InstantTrackingState.Initializing;
    private GridRenderer _gridRenderer;


    string modelName = string.Empty;
    public GameObject showGameObject;
    protected string showGameObjectName;
     List<MeshRenderer> renderMaterialsList = new List<MeshRenderer>();
     List<SkinnedMeshRenderer> skinMaterialsList = new List<SkinnedMeshRenderer>();
    public List<GameObject> btns = new List<GameObject>();
  
    private bool _isTracking = false;
  public  Light mainLight;

    public InstantTrackingState CurrentState
    {
        get { return _currentState; }
    }




    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;


        //Tracker.SMARTEnabled = true;
        //Tracker.DeviceHeightAboveGround = 1.2f;
        //Tracker.OnTargetsLoaded.AddListener(OnTargetsLoaded);
        //Tracker.OnErrorLoadingTargets.AddListener(OnErrorLoadingTargets);
        //Tracker.OnStateChanged.AddListener(OnStateChanged);
        //Tracker.OnError.AddListener(OnError);
        //Tracker.OnScreenConversionComputed.AddListener(OnScreenConversionComputed);



        //instantTrackable.OnInitializationStarted.AddListener(OnSceneRecognized);
        //instantTrackable.OnInitializationStopped.AddListener(OnSceneLost);
        //instantTrackable.OnSceneRecognized.AddListener(OnSceneRecognized);
        //instantTrackable.OnSceneLost.AddListener(OnSceneLost);
        _gridRenderer = GetComponent<GridRenderer>();

        mainLight = GameObject.FindGameObjectWithTag(Tags.Light).GetComponent<Light>();

    }


    private void Start()
    {

        Tracker.SMARTEnabled = true;
        Tracker.DeviceHeightAboveGround = 1.2f;

        QualitySettings.shadowDistance = 4.0f;
        StartCoroutine(CheckPlatformAssistedTrackingSupport());
        YiyouStaticDataManager.Instance.StartLoadABAssets();

    }

    private IEnumerator CheckPlatformAssistedTrackingSupport()
    {
        yield return null;
        if (Tracker.SMARTEnabled)
        {
            Tracker.IsPlatformAssistedTrackingSupported((SmartAvailability smartAvailability) =>
            {
                UpdateTrackingMessage(smartAvailability);
            });
        }
    }
    private void UpdateTrackingMessage(SmartAvailability smartAvailability)
    {
        if (Tracker.SMARTEnabled)
        {
            string sdk;
            if (Application.platform == RuntimePlatform.Android)
            {
                sdk = "ARCore";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                sdk = "ARKit";
            }
            else
            {
                return;
            }

        }
     
    }
    protected void Update()
    {
        if (btns.Count != 0)
        {
            foreach (var button in btns)
            {
                button.transform.Find("BgImage").GetComponent<Button>().interactable = _isTracking;
            }
        }
       
     if (_isTracking == true)
        {
         if (showGameObject!=null)    
            {
           
     WikiSLAMUIController.Instance.SetIntroductionText("",false);
            }
            else if (showGameObject == null)
            {
                WikiSLAMUIController.Instance.SetIntroductionText("请将镜头朝向地面并选择合影道具");
            }
        }
        else if (_isTracking == false)
        {
            if (showGameObject != null)
            {
                WikiSLAMUIController.Instance.SetIntroductionText("请对准合影道具摆放位置");

            }
           
        }
    }

    internal void SetIntensityValue(float value)
    {
        mainLight.intensity = value;
    }

    internal void SetLightLeftValue(float value)
    {

        Vector3 rot = mainLight.transform.parent.localEulerAngles;
        mainLight.transform.parent.localEulerAngles = new UnityEngine.Vector3(rot.x,-value,rot.z);

    }

    internal void SetLightUpValue(float value)
    {


        Vector3 rot = mainLight.transform.localEulerAngles;

        mainLight.transform.localEulerAngles = new UnityEngine.Vector3(value, rot.y, rot.z);
    }
    private void InitLight(string  goName)
    {
        mainLight.transform.parent.position = GameObject.FindGameObjectWithTag(Tags.MainCamera).transform.position;
        //MainLight.transform.transform.position = Vector3.zero;
        mainLight.transform.localPosition = Vector3.zero;
        mainLight.transform.parent.eulerAngles = new UnityEngine.Vector3(0, 180f, 0);

        mainLight.transform.localEulerAngles = new UnityEngine.Vector3(30, 0f, 0);
        if (goName=="shabao")
        {
            mainLight.intensity = 0;
        }
        else
        {
            mainLight.intensity = 0.5f;
        }
    }
    internal void SetRotateValue(float value)
    {
        if (showGameObject!=null)
        {
            Vector3 rot = showGameObject.transform.localEulerAngles;
            showGameObject.transform.localEulerAngles = new UnityEngine.Vector3(rot.x,value,rot.z);
        }




    }

    public void SetBtnList(List<GameObject>  gos)
    {
        if (gos.Count!=0)
        {
            btns.Clear();

            foreach (var item in gos)
            {
                btns.Add(item);
            }
      
        }

    }
    #region   Model

    public void SetMode(string goName)
    {


        modelName = goName;
        showGameObjectName = goName;
        YiyouStaticDataManager.Instance.modelName = goName;

        if (showGameObject==null)
        {
            Tracker.SetState(InstantTrackingState.Tracking);
        }
        else
        {
            //Tracker.SetState(InstantTrackingState.Initializing);
            YiyouStaticDataManager.Instance.OnDestroyGameObject();
            YiyouStaticDataManager.Instance.OnSilenceGo = null;
            Destroy(showGameObject);



        }
        LoadComplete(showGameObjectName);
 

    }


    public void LoadComplete(string modelName)
    {
        //2、提取资源
        //if (showGameObject != null)
        //{
        //    Destroy(showGameObject);
        //}
        GameObject oldGo = showGameObject;

        showGameObject = YiyouStaticDataManager.Instance.LoadGameObject(modelName);
        if (showGameObject)
        {
            showGameObject = Instantiate(showGameObject);
            showGameObject.name = showGameObjectName;
            //showGameObject.AddComponent<FingerTouchController>();
        }
        Destroy(oldGo);
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
        WikiSLAMUIController.Instance.SelectModelPanel.gameObject.SetActive(false);
        //WikiSLAMUIController.Instance.SetIntroductionText("点击屏幕进行物体摆放");
        PlaceModel();
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
    #endregion




    #region UI Events
    /// <summary>
    /// Initialize按钮点击
    /// </summary>
    public void OnInitializeButtonClicked()
    {
        Tracker.SetState(InstantTrackingState.Tracking);
    }

    public void OnHeightValueChanged(float newHeightValue)
    {

        Tracker.DeviceHeightAboveGround = newHeightValue;
    }



    public void OnResetButtonClicked()
    {
        Tracker.SetState(InstantTrackingState.Initializing);

    }
    #endregion



    #region   Tarcker  Events


    /// <summary>
    /// InstantTrackingState  改变就会调用
    /// </summary>
    /// <param name="newState"></param>
    public void OnStateChanged(InstantTrackingState newState)
    {
        Debug.Log("_currentState===" + _currentState + "      newState==== " + newState);
        _currentState = newState;
        if (newState == InstantTrackingState.Tracking)
        {

        }
        else
        {
            foreach (var model in _activeModels)
            {
                Destroy(model);
            }
            _activeModels.Clear();

        }
    }


    public virtual void OnTargetsLoaded()
    {
        Debug.Log("Targets loaded successfully.");
    }

    public virtual void OnErrorLoadingTargets(Error error)
    {
        Debug.LogError("Error loading targets!");
    }
    public void OnError(Error error)
    {
        Debug.LogError("Instant Tracker error!");
    }



    #endregion



    #region   Tarckable  Events
    public void OnSceneRecognized(InstantTarget target)
    {
        SetSceneActive(true);
        
    }

    public void OnSceneLost(InstantTarget target)
    {
        SetSceneActive(false);
   
    }

    private void SetSceneActive(bool active)
    {
        if (btns.Count!=0)
        {
            foreach (var button in btns)
            {
                button.GetComponent<SelectItemUI>().SetActive(active);
             
            }
        }

        if (_activeModels.Count != 0)
        {
        foreach (var model in _activeModels)
        {
                if (model!=null)
                {
                    model.SetActive(active);

                }
            }
        }


        ActivityIndicator.color = active ? EnabledColor : DisabledColor;

        _gridRenderer.enabled = active;
        _isTracking = active;
        Debug.Log("active======="+ active);
    }

    public void PlaceModel()
    {
       
            if (showGameObject.gameObject.activeSelf == false)
            {
                showGameObject.gameObject.SetActive(true);
                showGameObject.transform.position = Vector3.zero;
            if (showGameObject.name == "wurenji")
            {
                showGameObject.GetComponentInChildren<Cloth>().enabled = false;
            }
            showGameObject.transform.localScale = Vector3.one*0.5f;
            if (showGameObject.name == "wurenji")
            {
                showGameObject.GetComponentInChildren<Cloth>().enabled = true;
            }
            showGameObject.transform.localEulerAngles = Vector3.zero;
         YiyouStaticDataManager.Instance.ShowModel = showGameObject;
                WriteItem writeItem = showGameObject.GetComponent<WriteItem>();
                if (writeItem != null && writeItem.goodsEnum != GoodsWriteEnum.None)
                {
                    WriteManager.Instance.ShowInputPanel(true);
                    WikiSLAMUIController.Instance.SetIntroductionText("请选择字体并且输入文字");
                }
                else
                {
                    WikiSLAMUIController.Instance.SetIntroductionText("", false);
                    WikiSLAMUIController.Instance.ShowButtonPanel(true);
                }
            WikiSLAMUIController.Instance.ShowEffectPanel(true);




            FingerTouchEL.Instance.targetGameObject = showGameObject;
            WriteManager.Instance.SetGoodsEnum(showGameObject.GetComponent<WriteItem>().goodsEnum);
                _activeModels.Add(showGameObject);


            InitLight(showGameObject.name);
            if (showGameObjectName.Contains("haiou"))
                {
                    showGameObject.GetComponent<Haiou>().Init();
                }
            else
            {
                UnityHelper.RotateTowardCameraWiki(GameObject.FindGameObjectWithTag(Tags.MainCamera).gameObject, showGameObject);
                

            }

        }
    }



    public void OnScreenConversionComputed(bool success, Vector2 screenCoordinate, Vector3 pointCloudCoordinate)
    {
        if (success)
        {
           
            Debug.Log("pointCloudCoordinate====" + pointCloudCoordinate);
            if (showGameObject.gameObject.activeSelf == false)
            {
                showGameObject.gameObject.SetActive(true);
                showGameObject.transform.localPosition = pointCloudCoordinate;
                showGameObject.transform.localScale = Vector3.one;
                showGameObject.transform.localEulerAngles = Vector3.zero;

                WriteItem writeItem = showGameObject.GetComponent<WriteItem>();
                if (writeItem != null && writeItem.goodsEnum != GoodsWriteEnum.None)
                {
                    WriteManager.Instance.ShowInputPanel(true);
                    WikiSLAMUIController.Instance.SetIntroductionText("请选择字体并且输入文字");
                }
                else
                {
                    WikiSLAMUIController.Instance.SetIntroductionText("", false);
                    WikiSLAMUIController.Instance.ShowButtonPanel(true);
                }
                EffectPanelUI.Instance.ShowToggle();


                if (showGameObjectName.Contains("haiou"))
                {
                    showGameObject.GetComponent<Haiou>().Init();
                }
                else
                {
                    FingerTouchEL.Instance.targetGameObject = showGameObject;

                }
                YiyouStaticDataManager.Instance.ShowModel = showGameObject;
                WriteManager.Instance.SetGoodsEnum(showGameObject.GetComponent<WriteItem>().goodsEnum);
                _activeModels.Add(showGameObject);
            }
            else
            {
                showGameObject.transform.localPosition = pointCloudCoordinate;
            }


        }
    }
    #endregion


    public void ResetScene()
    {
       
        Debug.Log("ResetScene() called.");

        showGameObjectName = "";
      
        YiyouStaticDataManager.Instance.OnDestroyGameObject();
        if (showGameObject != null)
        {
            Destroy(showGameObject);
            _activeModels.Remove(showGameObject);
        }
        Tracker.SetState(InstantTrackingState.Initializing);
      
    }
           
}
