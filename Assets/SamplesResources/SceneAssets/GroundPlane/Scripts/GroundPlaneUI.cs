/*==============================================================================
Copyright (c) 2018 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
==============================================================================*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using ElviraFrame.AB;


using System.Collections;

public class GroundPlaneUI : SingletonMono<GroundPlaneUI>
{



    public Text m_Instructions;
    public CanvasGroup m_ScreenReticle;



    protected GraphicRaycaster m_GraphicRayCaster;
    protected PointerEventData m_PointerEventData;
    protected EventSystem m_EventSystem;





    private PlaneMode planeMode=PlaneMode.None;

    protected GameObject selectModelPanel;
    protected GameObject effectPanel, buttonPanel;

    public GameObject inputSureButton, backButton, zujiButton, helpButton, cancelButton;
    public GameObject helpImage, sliderPanel;
    public GameObject intensitySlider, lightSlider, rotateSlider;


    Slider intensityBar;
    Slider leftBar;
    Slider upBar;
    Slider rotateBar;


    Button functionBtn;

    Text intensityText, lightLeftText, lightUpText, rotateText;


    public UnityEngine.UI.Button FunctionBtn
    {
        get
        {
            if (functionBtn == null)
            {
                functionBtn = transform.Find("BgPanel/FunctionBtn").GetComponent<Button>();
            }
            return functionBtn;

        }
        set { functionBtn = value; }
    }
    public UnityEngine.GameObject EffectPanelGo
    {
        get
        {

            if (effectPanel == null)
            {
                effectPanel = GameObject.Find("Canvas/EffectPanel").gameObject;
            }
            return effectPanel;
        }
        set { effectPanel = value; }
    }

    public UnityEngine.GameObject ButtonPanelGo
    {
        get
        {

            if (buttonPanel == null)
            {
                buttonPanel = GameObject.Find("Canvas/ButtonPanel").gameObject;
            }
            return buttonPanel;
        }
        set { buttonPanel = value; }
    }


    public UnityEngine.GameObject SelectModelPanel
    {
        get
        {

            if (selectModelPanel == null)
            {
                selectModelPanel = GameObject.Find("Canvas/SelectModelPanel").gameObject;
            }
            return selectModelPanel;
        }
        set { selectModelPanel = value; }
    }
  protected virtual  void Start()
    {

     

  
   

        m_GraphicRayCaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();

        Vuforia.DeviceTrackerARController.Instance.RegisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
        inputSureButton.GetComponent<Button>().onClick.AddListener(InputSureButton);
        backButton.GetComponent<Button>().onClick.AddListener(BackBtnClick);
        zujiButton.GetComponent<Button>().onClick.AddListener(ZujiBtnClick);
        helpButton.GetComponent<Button>().onClick.AddListener(HelpBtnClick);
        cancelButton.GetComponent<Button>().onClick.AddListener(CancelBtnClick);

        intensityBar = intensitySlider.gameObject.GetComponentInChildren<Slider>();
        leftBar = lightSlider.transform.Find("LeftSlider").GetComponent<Slider>();
        upBar = lightSlider.transform.Find("UpSlider").GetComponent<Slider>();
        rotateBar = rotateSlider.gameObject.GetComponentInChildren<Slider>();

        intensityBar.onValueChanged.AddListener(SetIntensityValue);
        leftBar.onValueChanged.AddListener(SetLightLeftValue);
        upBar.onValueChanged.AddListener(SetLightUpValue);
        rotateBar.onValueChanged.AddListener(SetRotateValue);


        intensityBar.minValue = 0.1f;
        intensityBar.maxValue = 2f;

        leftBar.minValue = 90f;
        leftBar.maxValue = 270f;
        leftBar.value = 180f;

        upBar.minValue = -30f;
        upBar.maxValue = 90f;
        upBar.value = 30f;



        rotateBar.minValue = -180f;
        rotateBar.maxValue = 180f;
        rotateBar.value = 1f;


        intensityText = intensityBar.transform.GetComponentInChildren<Text>();
        //lightLeftText = leftBar.transform.GetComponentInChildren<Text>();
        //lightUpText = upBar.transform.GetComponentInChildren<Text>();
        rotateText = rotateBar.transform.GetComponentInChildren<Text>();



        SelectModelPanel.gameObject.SetActive(true);
        EffectPanelGo.gameObject.SetActive(false);
        ButtonPanelGo.gameObject.SetActive(false);
        sliderPanel.gameObject.SetActive(false);
        helpImage.gameObject.SetActive(false);
        FunctionBtn.onClick.AddListener(FunctionBtnClick);
        RecordManager.Instance.ShowCanvas(false);

    }
    private void FunctionBtnClick()
    {
#if  UNITY_IOS||UNITY_IPHONE
              GroundPlaneUI.Instance.InitializeUI();

#elif  UNITY_ANDROID
        if (SceneManager.GetActiveScene().name == "wikiSLAM")
        {
            //  WikiSLAMUIController.Instance.BackBtnClick();
            WikiSLAMUIController.Instance.InitializeUI();
        }

        else if (SceneManager.GetActiveScene().name == "yiyou")
        {
            //   GroundPlaneUI.Instance.BackBtnClick();
            GroundPlaneUI.Instance.InitializeUI();
        }


#endif

    }

    private void SetRotateValue(float arg0)
    {
        PlaneManager.Instance.SetModelRotate(rotateBar.value);

        rotateText.text = ((int) rotateBar.value).ToString();
    }

    private void SetLightUpValue(float arg0)
    {
        PlaneManager.Instance.SetLightUpValue(upBar.value);
        //lightUpText.text = upBar.value.ToString();
    }

    private void SetLightLeftValue(float arg0)
    {
        PlaneManager.Instance.SetLightLeftValue(leftBar.value);
        //lightLeftText.text = leftBar.value.ToString();
    }

    private void SetIntensityValue(float arg0)
    {
        PlaneManager.Instance.SetIntensityValue(intensityBar.value);
        intensityText.text = (intensityBar.value ).ToString("f2");
    }




    public void ShowSliderPanel(ChangeSliderEnum changeSliderEnum)
    {

        sliderPanel.gameObject.SetActive(true);
        switch (changeSliderEnum)
        {
            case ChangeSliderEnum.Intensity:
                intensitySlider.gameObject.SetActive(true);
                intensityBar.value = PlaneManager.Instance.mainLight.GetComponent<Light>().intensity;
                intensityText.text = (intensityBar.value).ToString("f2");
                lightSlider.gameObject.SetActive(false);
                rotateSlider.gameObject.SetActive(false);
                break;
            case ChangeSliderEnum.Light:
                intensitySlider.gameObject.SetActive(false);
                lightSlider.gameObject.SetActive(true);

                leftBar.value = PlaneManager.Instance.mainLight.transform.parent.localEulerAngles.y;
                upBar.value = PlaneManager.Instance.mainLight.transform.eulerAngles.x;



                rotateSlider.gameObject.SetActive(false);
                break;
            case ChangeSliderEnum.Scale:
                intensitySlider.gameObject.SetActive(false);
                lightSlider.gameObject.SetActive(false);
                rotateSlider.gameObject.SetActive(true);
                rotateBar.value = PlaneManager.Instance.showGameObject.transform.localEulerAngles.y;
                rotateText.text = ((int)rotateBar.value).ToString();
                break;
            case ChangeSliderEnum.None:
                intensitySlider.gameObject.SetActive(false);
                lightSlider.gameObject.SetActive(false);
                rotateSlider.gameObject.SetActive(false);

                break;
            default:
                break;
        }
    }
    private void CancelBtnClick()
    {
        helpImage.gameObject.SetActive(false);
    }

    private void HelpBtnClick()
    {
        helpImage.gameObject.SetActive(true);
    }
    private void ZujiBtnClick()
    {
        StartCoroutine("LoadNext", "Track");
    }

    public void InitUI()
    {
        Debug.Log("111");
        EffectPanelUI.Instance.HideToggle();
        ButtonPanelGo.gameObject.SetActive(false);
        SelectModelPanel.gameObject.SetActive(true);
        WriteManager.Instance.ShowInputPanel(false);
        PlaneManager.Instance.ResetScene();
        SetReticleVisiblity(true);
        SetIntroductionText("请将镜头朝向地面并选择模型");

        YiyouStaticDataManager.Instance.HandleClear();
    }

    public virtual void BackBtnClick()
    {
        if (ButtonPanelGo.gameObject.activeSelf)
        {

            if (ButtonPanelGo.transform.Find("ShowShotImage").gameObject.activeSelf==true)
            {
                ButtonPanelGo.transform.Find("ShowShotImage").gameObject.SetActive(false);
                ButtonPanelUI.Instance.Init();
            }
            else
            {
                InitUI();

            }
            ShouZujiBtn(false);
        }
        else if (SelectModelPanel.gameObject.activeSelf == false)
        {
            InitUI();
            ShouZujiBtn(false);
        }
        else
        {
            Debug.Log("33");
            StartCoroutine("LoadNext", "main");

            Debug.Log("back");
        }
    }
    IEnumerator LoadNext(string  nextSceneName)
    {
        AssetBundleMgr.GetInstance().DisposeAllAssets("scene_yiyou");
        TrackDataManager.Instance.SaveStringToFile();
        GlobalParameter.isNeedRestore = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextSceneName);
    }
    public void InputSureButton()
    {
        KongmingLatern kongmingLatern = GameObject.FindObjectOfType<KongmingLatern>();
        if (kongmingLatern != null)
        {
            kongmingLatern.FlyGameObject();
        }
            WriteManager.Instance.ShowInputPanel(false);
            EffectPanelGo.gameObject.SetActive(true);
        ShowEffectPanel();
        SetIntroductionText("", false);
        ShowButtonPanel();
    }




    public void SetIntroductionText(string  ssss,bool  flag=true)
    {
        m_Instructions.text = ssss;
        m_Instructions.gameObject.SetActive(flag);
    }
    public void SetReticleVisiblity( bool flag )
    {
        Debug.Log(flag);
        m_ScreenReticle.gameObject.SetActive(flag);
    }
    //void LateUpdate()
    //{
    //    if (PlaneManager.GroundPlaneHitReceived)
    //    {
    //        // We got an automatic hit test this frame

    //        // Hide the onscreen reticle when we get a hit test
    //        m_ScreenReticle.alpha = 0;

    //        m_Instructions.gameObject.SetActive(true);
    //        m_Instructions.enabled = true;


    //    }
    //    else
    //    {
    //        // No automatic hit test, so set alpha based on which plane mode is active
    //        m_ScreenReticle.alpha = 1;

    //        m_Instructions.gameObject.SetActive(true);
    //        m_Instructions.enabled = true;

    //        if (planeMode == PlaneMode.GROUND )
    //        {
    //            SetIntroductionText("请先选择模型");

    //        }
    //        else if (planeMode == PlaneMode.MIDAIR)
    //        {
         
    //            SetIntroductionText("点击屏幕摆放物体");
    //        }
    //    }
    //}

    void OnDestroy()
    {
        Debug.Log("OnDestroy() called.");

        Vuforia.DeviceTrackerARController.Instance.UnregisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
    }

    public void Reset()
    {
      

  
    }



    public void InitializeUI()
    {
        SelectModelPanel.gameObject.SetActive(true);

        EffectPanelGo.gameObject.SetActive(false);
        ButtonPanelGo.gameObject.SetActive(false);
        sliderPanel.gameObject.SetActive(false);
        helpImage.gameObject.SetActive(false);

    }

    public bool IsCanvasButtonPressed()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        m_GraphicRayCaster.Raycast(m_PointerEventData, results);

        bool resultIsButton = false;
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponentInParent<Toggle>() ||
                result.gameObject.GetComponent<Button>())
            {
                resultIsButton = true;
                break;
            }
        }
        return resultIsButton;
    }

   protected void OnDevicePoseStatusChanged(Vuforia.TrackableBehaviour.Status status, Vuforia.TrackableBehaviour.StatusInfo statusInfo)
    {
        //Debug.Log("OnDevicePoseStatusChanged(" + status + ", " + statusInfo + ")");

        switch (statusInfo)
        {
            case Vuforia.TrackableBehaviour.StatusInfo.INITIALIZING:
              //  m_TrackerStatus.text = "Tracker Initializing";
                break;
            case Vuforia.TrackableBehaviour.StatusInfo.EXCESSIVE_MOTION:
               // m_TrackerStatus.text = "Excessive Motion";
                break;
            case Vuforia.TrackableBehaviour.StatusInfo.INSUFFICIENT_FEATURES:
               // m_TrackerStatus.text = "Insufficient Features";
                break;
            default:
              //  m_TrackerStatus.text = "";
                break;
        }

    }

    internal void ShowEffectPanel(bool  flag=true)
    {
        string modelName = YiyouStaticDataManager.Instance.ShowModel.name;
        if (modelName != "haiou" )
        {

            EffectPanelGo.gameObject.SetActive(flag);
            EffectPanelUI.Instance.HideToggle();
        }

    }

    internal void ShowButtonPanel()
    {
        ButtonPanelGo.gameObject.SetActive(true);
        ButtonPanelUI.Instance.Init();
    }


    public void ShouZujiBtn(bool  flag)
    {
        zujiButton.gameObject.SetActive(flag);
        helpButton.gameObject.SetActive(!flag);
    }
}
