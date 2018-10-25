using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ElviraFrame;
public class TrackUIManager : SingletonMono<TrackUIManager>
{

    public Text titleText;
    private int curState = 0;

    public Button backBtn;




    GameObject bigImage = null;

    public GameObject loadTipImage;

    public GameObject resultGo;
    public UnityEngine.UI.Button BackBtn
    {
        get
        {
            if (backBtn == null)
            {
                backBtn = transform.Find("Title/BackButton").GetComponent<Button>();

            }
            return backBtn;
        }
        set { backBtn = value; }
    }

    private void BackBtnClick()
    {
        if (bigImage != null)
        {
            Destroy(bigImage.gameObject);
        }
        else
        {
            switch (curState)
            {
                case 0:
                    {
                        WebView.Instance.ClearWebView();
                        SceneManager.LoadScene("main");
                        curState--;
                        curState = Mathf.Clamp(curState, 0, 1);
                    }
                    break;
                case 1:
                    {
                        Destroy(GalleryImageManager.Instance.ImageScrollView);
                        WebView.Instance.CreateWebView();
                        SetTitleText("我的足迹", 0);
                        curState--;
                        curState = Mathf.Clamp(curState, 0, 1);
                    }
                    break;
                default:
                    
                    break;
            }
        }



    }



    public UnityEngine.UI.Text TitleText
    {
        get
        {
            if (titleText == null)
            {
                titleText = transform.Find("Title").GetComponent<Text>();
            }
            return titleText;
        }
        set { titleText = value; }
    }
    internal void SetTitleText(string v1, int v2)
    {

        TitleText.text = v1;
        curState = v2;
    }

    private void Start()
    {

        BackBtn.onClick.AddListener(BackBtnClick);
        loadTipImage.gameObject.SetActive(true);

        resultGo.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {

            resultGo.SetActive(false);
            WebView.Instance.CreateWebView();
        });


        resultGo.SetActive(false);
    }



    public void ShowBigImage(Sprite sps)
    {
        bigImage = Instantiate<GameObject>(Resources.Load<GameObject>("Model/ShowImage"), this.transform);

        bigImage.gameObject.SetActive(true);
        bigImage.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        bigImage.GetComponent<RectTransform>().offsetMax = new Vector2(0, -90);
        bigImage.transform.Find("Image").GetComponent<Image>().sprite = sps;
        //FingerTouchEL.Instance.targetGameObject = go;
    }
    internal void LoadStart()
    {
        loadTipImage.gameObject.SetActive(true);
    }
    public void LoadComplete()
    {
        loadTipImage.gameObject.SetActive(false);
    }

    public void LoadError()
    {
        resultGo.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (loadTipImage.activeSelf)
        {
            loadTipImage.transform.Rotate(-Vector3.forward * 100f * Time.deltaTime, Space.Self);
        }
    }


}
