using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;


public class GroundPlaneUIRestore:GroundPlaneUI
{

    protected override void Start()
    {
  


   
     

        m_GraphicRayCaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();

        Vuforia.DeviceTrackerARController.Instance.RegisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
        inputSureButton.GetComponent<Button>().onClick.AddListener(InputSureButton);
        backButton.GetComponent<Button>().onClick.AddListener(BackBtnClick);
        SelectModelPanel.gameObject.SetActive(false);
        EffectPanelGo.gameObject.SetActive(false);

    }

    public override void BackBtnClick()
    {
        if (effectPanel.gameObject.activeSelf)
        {
            EffectPanelGo.gameObject.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("main");
        }
    }
}

