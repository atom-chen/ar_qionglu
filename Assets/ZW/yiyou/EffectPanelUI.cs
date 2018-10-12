using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EffectPanelUI : SingletonMono<EffectPanelUI>
{
    GameObject canvas;
 
    GameObject effectPanelUIGo, togglePanel;
    Slider lightIntensitySlider;
    //Slider lightColorSlider;


    Button showButton, hideButton;
    Toggle intensityToggle, lightToggle, rotateToggle;



 
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

    public UnityEngine.GameObject EffectPanelGo
    {
        get
        {
            if (effectPanelUIGo == null)
            {
                effectPanelUIGo = Canvas.transform.Find("EffectPanel").gameObject;
            }
            return effectPanelUIGo;
        }
        set { effectPanelUIGo = value; }
    }
    public UnityEngine.GameObject TogglePanel
    {
        get
        {
            if (togglePanel == null)
            {
                togglePanel = EffectPanelGo.transform.Find("TogglePanel").gameObject;
            }
            return togglePanel;
        }
        set { togglePanel = value; }
    }

    //public UnityEngine.UI.Slider LightColorSlider
    //{
    //    get
    //    {

    //        if (lightColorSlider == null)
    //        {
    //            lightColorSlider = EffectPanelGo.transform.Find("LightColorSlider").GetComponent<Slider>();


    //        }

    //        return lightColorSlider;
    //    }
    //    set { lightColorSlider = value; }
    //}

    public override void Awake()
    {
        base.Awake();

        Canvas = GameObject.Find("Canvas").gameObject;


        showButton = EffectPanelGo.transform.Find("ShowButton").GetComponent<Button>();
        hideButton = TogglePanel.transform.Find("HideButton").GetComponent<Button>();

        showButton.onClick.AddListener(ShowToggle);
        hideButton.onClick.AddListener(HideToggle);




        intensityToggle = TogglePanel.transform.Find("IntensityToggle").GetComponent<Toggle>();
        lightToggle = TogglePanel.transform.Find("LightToggle").GetComponent<Toggle>();
        rotateToggle= TogglePanel.transform.Find("RotateToggle").GetComponent<Toggle>();


        intensityToggle.onValueChanged.AddListener(ShowIntensity);
        lightToggle.onValueChanged.AddListener(ShowLight);
        rotateToggle.onValueChanged.AddListener(ShowRotate);




        TogglePanel.gameObject.SetActive(false);

    }

    private void ShowRotate(bool arg0)
    {
        if (arg0)
        {
#if UNITY_IOS || UNITY_IPHONE
                     GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.Scale);
#elif UNITY_ANDROID
            if (SceneManager.GetActiveScene().name=="yiyou")
            {
                GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.Scale);
            }
            else
            {
                WikiSLAMUIController.Instance.ShowSliderPanel(ChangeSliderEnum.Scale);
            }
#endif
        }
    }

    private void ShowLight(bool arg0)
    {
        if (arg0)
        {
#if UNITY_IOS || UNITY_IPHONE
                     GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.Light);
#elif UNITY_ANDROID
            if (SceneManager.GetActiveScene().name == "yiyou")
            {
                GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.Light);
            }
            else
            {
                WikiSLAMUIController.Instance.ShowSliderPanel(ChangeSliderEnum.Light);
            }
#endif
        }
    }

    private void ShowIntensity(bool arg0)
    {
        if (arg0)
        {
#if UNITY_IOS || UNITY_IPHONE
                     GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.Intensity);
#elif UNITY_ANDROID
            if (SceneManager.GetActiveScene().name == "yiyou")
            {
                GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.Intensity);
            }
            else
            {
                WikiSLAMUIController.Instance.ShowSliderPanel(ChangeSliderEnum.Intensity);
            }
#endif
        }
    }

    private void HideToggle()
    {
        TogglePanel.gameObject.SetActive(false);
        showButton.gameObject.SetActive(true);
#if UNITY_IOS || UNITY_IPHONE
                     GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.None);
#elif UNITY_ANDROID
        if (SceneManager.GetActiveScene().name == "yiyou")
        {
            GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.None);
        }
        else
        {
            WikiSLAMUIController.Instance.ShowSliderPanel(ChangeSliderEnum.None);
        }
#endif
    }

    private void ShowToggle()
    {
        TogglePanel.gameObject.SetActive(true);
        showButton.gameObject.SetActive(false);
#if UNITY_IOS || UNITY_IPHONE
                     GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.Intensity);
#elif UNITY_ANDROID
        if (SceneManager.GetActiveScene().name == "yiyou")
        {
            GroundPlaneUI.Instance.ShowSliderPanel(ChangeSliderEnum.Intensity);
        }
        else
        {
            WikiSLAMUIController.Instance.ShowSliderPanel(ChangeSliderEnum.Intensity);
        }
#endif
    }

    /// <summary>
    /// 设置模型的颜色的整体值
    /// </summary>
    /// <param name="colorValue"></param>
    //  private void SetColorValue(float colorValue)
    //  {
    //      Debug.Log("ColorValue111111111=" + colorValue);

    //      WriteItem writeItem = GameObject.FindObjectOfType<WriteItem>();
    //      writeItem.SetLightColor(LightColorSlider.value);
    //   lightColorSliderValue.text = LightColorSlider.value.ToString();
    //  }
}
