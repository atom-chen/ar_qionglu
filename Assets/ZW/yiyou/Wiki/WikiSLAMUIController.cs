using ElviraFrame.AB;
using ElviraFrame.ScrollView;
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




    protected GameObject selectModelPanel;
    protected GameObject effectPanel;



    public GameObject inputSureButton, backButton, zujiButton,helpButton,cancelButton;
    public GameObject helpImage, sliderPanel;
    public GameObject intensitySlider, lightSlider, rotateSlider;


    Slider intensityBar;
    Slider leftBar;
    Slider upBar;
    Slider rotateBar;


    Text intensityText,rotateText;
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
        //zujiButton.GetComponent<Button>().onClick.AddListener(ZujiBtnClick);
        helpButton.GetComponent<Button>().onClick.AddListener(HelpBtnClick);
        cancelButton.GetComponent<Button>().onClick.AddListener(CancelBtnClick);


         intensityBar = intensitySlider.gameObject.GetComponentInChildren<Slider>();
         leftBar = lightSlider.transform.Find("LeftSlider").GetComponent<Slider>();
         upBar = lightSlider.transform.Find("UpSlider").GetComponent<Slider>();
         rotateBar = rotateSlider.gameObject.GetComponentInChildren<Slider>();

        intensityBar.onValueChanged.AddListener(SetIntensityValue);
        leftBar.onValueChanged.AddListener(SetLightLeftValue);
        upBar. onValueChanged.AddListener(SetLightUpValue);
        rotateBar.onValueChanged.AddListener(SetRotateValue);


        intensityBar.minValue = 0.1f;
        intensityBar.maxValue = 2f;
        intensityBar.value = 1f;

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
            ButtonPanelGo.SetActive(false);
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
        WikiSLAMController.Instance.SetRotateValue(rotateBar.value);
        rotateText.text = ((int)rotateBar.value).ToString();
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

    public void ShowSliderPanel(ChangeSliderEnum changeSliderEnum)
    {

        sliderPanel.gameObject.SetActive(true);
        switch (changeSliderEnum)
        {
            case ChangeSliderEnum.Intensity:
                intensitySlider.gameObject.SetActive(true);
                intensityBar.value = WikiSLAMController.Instance.mainLight.GetComponent<Light>().intensity;
                intensityText.text = (intensityBar.value).ToString("f2");
                lightSlider.gameObject.SetActive(false);
                rotateSlider.gameObject.SetActive(false);
                break;
            case ChangeSliderEnum.Light:
                intensitySlider.gameObject.SetActive(false);
                lightSlider.gameObject.SetActive(true);

                leftBar.value = WikiSLAMController.Instance.mainLight.transform.parent.localEulerAngles.y;
                upBar.value = WikiSLAMController.Instance.mainLight.transform.eulerAngles.x;



                rotateSlider.gameObject.SetActive(false);
                break;
            case ChangeSliderEnum.Scale:
                intensitySlider.gameObject.SetActive(false);
                lightSlider.gameObject.SetActive(false);
                rotateSlider.gameObject.SetActive(true);
                rotateBar.value = WikiSLAMController.Instance.showGameObject.transform.localEulerAngles.y;
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

    internal void InitializeUI()
    {
        SelectModelPanel.gameObject.SetActive(true);

        EffectPanelGo.gameObject.SetActive(false);
    //    ButtonPanel.gameObject.SetActive(false);
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
        helpImage.GetComponentInChildren<ScrollViewPanel>().Init();
    }

    private void ZujiBtnClick()
    {
        StartCoroutine("LoadNext", "Track");
    }

    IEnumerator LoadNext(string nextSceneName)
    {

        YiyouStaticDataManager.Instance.DisposeAB();
        TrackDataManager.Instance.SaveStringToFile();
        GlobalParameter.isNeedRestore = false;
        yield return new WaitForSeconds(0.1f);
        UnityHelper.LoadNextScene(nextSceneName);
    }



    public virtual void BackBtnClick()
    {
        if (FirstUseTipManager.Instance.isTipTour)
        {
            return;
        }

        if (ButtonPanelGo.gameObject.activeSelf)
        {

            if (ButtonPanelGo.transform.Find("ShowShotImage").gameObject.activeSelf == true)
            {
                ButtonPanelGo.transform.Find("ShowShotImage").gameObject.SetActive(false);
                ButtonPanelUI.Instance.Init();
        
             
                Debug.Log("111");
            }
            else
            {
                Debug.Log("222");
                EffectPanelUI.Instance.HideToggle();

                ButtonPanel.gameObject.SetActive(false);

                SelectModelPanel.gameObject.SetActive(true);
                WriteManager.Instance.ShowInputPanel(false);
                WikiSLAMController.Instance.ResetScene();
                SetIntroductionText("请将镜头朝向地面并选择合影道具");
                YiyouStaticDataManager.Instance.HandleClear();
                sliderPanel.gameObject.SetActive(false);
            }
            ShouZujiBtn(false);
        }
       else  if (SelectModelPanel.gameObject.activeSelf == false)
        {

            EffectPanelUI.Instance.HideToggle();

            ButtonPanel.gameObject.SetActive(false);

            SelectModelPanel.gameObject.SetActive(true);
            WriteManager.Instance.ShowInputPanel(false);
            WikiSLAMController.Instance.ResetScene();
            SetIntroductionText("请将镜头朝向地面并选择合影道具");
            YiyouStaticDataManager.Instance.HandleClear();
            ShouZujiBtn(false);
            sliderPanel.gameObject.SetActive(false);
        }
        else
        {

        
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


#if UNITY_IOS || UNITY_IPHONE
        GroundPlaneUI.Instance.ShouZujiBtn(true);

#elif UNITY_ANDROID

        if (SceneManager.GetActiveScene().name == "yiyou")
        {
            GroundPlaneUI.Instance.ShouZujiBtn(true);

        }
        else
        {
            WikiSLAMUIController.Instance.ShouZujiBtn(true);
        }
#endif
        FirstUseTipManager.Instance.ShowNextTip(TipType.ShotTip);
    }
    public void SetIntroductionText(string ssss, bool flag = true)
    {
        m_Instructions.gameObject.SetActive(flag);
        if (flag)
            m_Instructions.text = ssss;
    //    ButtonPanelGo.gameObject.SetActive(!flag);
    }
    internal void ShowEffectPanel(bool v)
    {
   
            EffectPanelGo.gameObject.SetActive(v);
  //          EffectPanelUI.Instance.HideToggle();
        EffectPanelUI.Instance.ShowToggle();

    }
    internal void ShowButtonPanel(bool v = true)
    {
        ButtonPanelGo.gameObject.SetActive(v);
        ButtonPanelUI.Instance.Init();
    }
    public void ShouZujiBtn(bool flag)
    {
      //  zujiButton.gameObject.SetActive(flag);
        helpButton.gameObject.SetActive(!flag);
    }
}