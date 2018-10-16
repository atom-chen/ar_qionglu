using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : SingletonMono<RecordManager>
{
    Button recordButton;


    Image processImage;


  private  CanvasGroup thisUI;
    public UnityEngine.UI.Image ProcessImage
    {
        get {
            if (processImage == null)
            {
                processImage = transform.Find("RecordBtn/process").GetComponent <Image>();

            }

            return processImage; }
        set { processImage = value; }
    }
    public bool isRec = false;
    float time = 10f;
    float timer = 0;

    public UnityEngine.UI.Button RecordButton
    {
        get {

            if (recordButton==null)
            {
                recordButton = GetComponentInChildren<Button>();
               
            }
            return recordButton;
        }
        set { recordButton = value; }
    }

    public void ShowCanvas(bool flag)
    {
        if (flag)
        {
            thisUI.alpha = 1;
        }
        else
        {
            thisUI.alpha = 0;

        }

        transform.GetComponentInChildren<Button>().enabled = flag;

    }

    // Use this for initialization
    void Start ()
    {
        RecordButton.onClick.AddListener(RecordClick);
        thisUI = this.GetComponent<CanvasGroup>();
        ShowCanvas(true);

    }

    private void RecordClick()
    {
        ButtonPanelUI.Instance.stopRecoding();
    }

    public void StartProcess()
    {
        isRec = true;

        timer = 0f;

        time = 10f;
        ProcessImage.fillAmount = 0f;
    }
    private void Update()
    {
        if (isRec == true)
        {
            timer += Time.deltaTime;
            ProcessImage.fillAmount = timer/time;
            if (timer >= time)
            {
                isRec = false;
            
                timer = 0;
                ButtonPanelUI.Instance.stopRecoding();
                ShowCanvas(false);
            }
        }
    }
}
