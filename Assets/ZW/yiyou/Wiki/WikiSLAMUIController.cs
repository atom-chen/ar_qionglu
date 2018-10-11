using ElviraFrame.AB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WikiSLAMUIController : SingletonMono<WikiSLAMUIController>
{


    GameObject canvas;

    public Text m_Instructions;



    private PlaneMode planeMode = PlaneMode.None;

    protected GameObject selectModelPanel;
    protected GameObject effectPanel;



    public GameObject inputSureButton, backButton, zujiButton,helpButton,cancelButton;
    public GameObject helpImage, sliderPanel;
    public GameObject intensitySlider, lightSlider, scaleSlider;


    Slider intensityBar;
    Slider leftBar;
    Slider upBar;
    Slider scaleBar;


    Text intensityText, lightLeftText, lightUpText, scaleText;
    private GameObject buttonPanel;


    Button functionBtn;

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
    public UnityEngine.GameObject Canvas
    {
        get
        {
            if (canvas == null)
            {
                canvas = GameObject.Find("Canvas").gameObject;
            }

            return canvas;
        }
        set { canvas = value; }
    }


    public UnityEngine.GameObject ButtonPanel
    {
        get
        {
            if (buttonPanel == null)
            {
                buttonPanel = Canvas.transform.Find("ButtonPanel").gameObject;
            }

            return buttonPanel;


        }
        set { buttonPanel = value; }
    }
    public UnityEngine.GameObject EffectPanelGo
    {
        get
        {

            if (effectPanel == null)
            {
                effectPanel = Canvas.transform.Find("EffectPanel").gameObject;
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
                buttonPanel = Canvas.transform.Find("ButtonPanel").gameObject;
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
                selectModelPanel = Canvas.transform.Find("SelectModelPanel").gameObject;
            }
            return selectModelPanel;
        }
        set { selectModelPanel = value; }
    }


    public override void Awake()
    {
        base.Awake();


   
    }
    protected virtual void Start()
    {

        inputSureButton.GetComponent<Button>().onClick.AddListener(InputSureButton);
        backButton.GetComponent<Button>().onClick.AddListener(BackBtnClick);
        SelectModelPanel.gameObject.SetActive(true);

        EffectPanelGo.gameObject.SetActive(false);
        zujiButton.GetComponent<Button>().onClick.AddListener(ZujiBtnClick);
        helpButton.GetComponent<Button>().onClick.AddListener(HelpBtnClick);
        cancelButton.GetComponent<Button>().onClick.AddListener(CancelBtnClick);


         intensityBar = intensitySlider.gameObject.GetComponentInChildren<Slider>();
         leftBar = lightSlider.transform.Find("LeftSlider").GetComponent<Slider>();
         upBar = lightSlider.transform.Find("UpSlider").GetComponent<Slider>();
         scaleBar = scaleSlider.gameObject.GetComponentInChildren<Slider>();

        intensityBar.onValueChanged.AddListener(SetIntensityValue);
        leftBar.onValueChanged.AddListener(SetLightLeftValue);
        upBar. onValueChanged.AddListener(SetLightUpValue);
        scaleBar.onValueChanged.AddListener(SetScaleValue);


        intensityBar.minValue = 0.1f;
        intensityBar.maxValue = 2f;
        intensityBar.value = 1f;

        leftBar.minValue = -180f;
        leftBar.maxValue = 180f;
        leftBar.value = 0f;
        upBar.minValue = -45f;
        upBar.maxValue = 45f;
        upBar.value = 0f;
        scaleBar.minValue = 0.3f;
        scaleBar.maxValue = 1.3f;

        scaleBar.value = 1f;
        intensityText = intensityBar.transform.GetComponentInChildren<Text>();
        //lightLeftText = leftBar.transform.GetComponentInChildren<Text>();
        //lightUpText = upBar.transform.GetComponentInChildren<Text>();
        scaleText = scaleBar.transform.GetComponentInChildren<Text>();


        ButtonPanel.gameObject.SetActive(false);
        sliderPanel.gameObject.SetActive(false);
        helpImage.gameObject.SetActive(false);
        
        FunctionBtn.onClick.AddListener(FunctionBtnClick);

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
    private void SetScaleValue(float arg0)
    {
        WikiSLAMController.Instance.SetModelScale(scaleBar.value);
        scaleText.text = ((int)((0.01+scaleBar.value )* 100)).ToString()+"%";
    }

    private void SetLightUpValue(float arg0)
    {
        WikiSLAMController.Instance.SetLightUpValue(upBar.value);
        //lightUpText.text = upBar.value.ToString();
    }

    private void SetLightLeftValue(float arg0)
    {
        WikiSLAMController.Instance.SetLightLeftValue(leftBar.value);
        //lightLeftText.text = leftBar.value.ToString();
    }

    private void SetIntensityValue(float arg0)
    {
        WikiSLAMController.Instance.SetIntensityValue(intensityBar.value);
        intensityText.text =( intensityBar.value).ToString("f2");
    }

    public void ShowSliderPanel(ChangeSliderEnum  changeSliderEnum)
    {

        sliderPanel.gameObject.SetActive(true);
        switch (changeSliderEnum)
        {
            case ChangeSliderEnum.Intensity:
                intensitySlider.gameObject.SetActive(true);
                lightSlider.gameObject.SetActive(false);
                scaleSlider.gameObject.SetActive(false);
                break;
            case ChangeSliderEnum.Light:
                intensitySlider.gameObject.SetActive(false);
                lightSlider.gameObject.SetActive(true);
                scaleSlider.gameObject.SetActive(false);
                break;
            case ChangeSliderEnum.Scale:
                intensitySlider.gameObject.SetActive(false);
                lightSlider.gameObject.SetActive(false);
                scaleSlider.gameObject.SetActive(true);
                break;

            case ChangeSliderEnum.None:


                intensitySlider.gameObject.SetActive(false);
                lightSlider.gameObject.SetActive(false);
                scaleSlider.gameObject.SetActive(false);

                break;
            default:
                break;
        }
    }

    internal void InitializeUI()
    {
        SelectModelPanel.gameObject.SetActive(true);

        EffectPanelGo.gameObject.SetActive(false);
        ButtonPanel.gameObject.SetActive(false);
        sliderPanel.gameObject.SetActive(false);
        helpImage.gameObject.SetActive(false);
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

    IEnumerator LoadNext(string nextSceneName)
    {
        Debug.Log("ZujiBtnClick");
        AssetBundleMgr.GetInstance().DisposeAllAssets("scene_yiyou");
        TrackDataManager.Instance.SaveStringToFile();
        GlobalParameter.isNeedRestore = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextSceneName);
    }



    public virtual void BackBtnClick()
    {


        if (ButtonPanelGo.gameObject.activeSelf)
        {

            if (ButtonPanelGo.transform.Find("ShowShotImage").gameObject.activeSelf == true)
            {
                ButtonPanelGo.transform.Find("ShowShotImage").gameObject.SetActive(false);
                ButtonPanelUI.Instance.Init();
            }
            else
            {
                Debug.Log("111");
                EffectPanelGo.gameObject.SetActive(false);

                ButtonPanel.gameObject.SetActive(false);

                SelectModelPanel.gameObject.SetActive(true);
                WriteManager.Instance.ShowInputPanel(false);
                WikiSLAMController.Instance.ResetScene();
                SetIntroductionText("请将镜头朝向地面并选择模型");
                YiyouStaticDataManager.Instance.HandleClear();
            }

        }
       else  if (SelectModelPanel.gameObject.activeSelf == false)
        {

            EffectPanelGo.gameObject.SetActive(false);

            ButtonPanel.gameObject.SetActive(false);

            SelectModelPanel.gameObject.SetActive(true);
            WriteManager.Instance.ShowInputPanel(false);
            WikiSLAMController.Instance.ResetScene();
            SetIntroductionText("请将镜头朝向地面并选择模型");
            YiyouStaticDataManager.Instance.HandleClear();
        }
        else
        {

            YiyouStaticDataManager.Instance.DisposeAB();
            StartCoroutine("LoadNext", "main");
        }



    }

    public void InputSureButton()
    {
        KongmingLatern kongmingLatern = GameObject.FindObjectOfType<KongmingLatern>();
        if (kongmingLatern != null)
        {
            kongmingLatern.FlyGameObject();
        }
        WriteManager.Instance.ShowInputPanel(false);

        ShowEffectPanel(true);
        SetIntroductionText("", false);
        ShowButtonPanel(true);
    }
    public void SetIntroductionText(string ssss, bool flag = true)
    {
        m_Instructions.gameObject.SetActive(flag);
        if (flag)
            m_Instructions.text = ssss;

    }
    internal void ShowEffectPanel(bool v)
    {
        string modelName = YiyouStaticDataManager.Instance.ShowModel.name;
        if (modelName == "shitou" || modelName == "shabao")
        {
            EffectPanelGo.gameObject.SetActive(v);

        }

    }
    internal void ShowButtonPanel(bool v = true)
    {
        ButtonPanelGo.gameObject.SetActive(v);
        ButtonPanelUI.Instance.Init();
    }

}