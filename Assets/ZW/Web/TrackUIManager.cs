using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ElviraFrame;
using ElviraFrame.ScrollView;

public class TrackUIManager : SingletonMono<TrackUIManager>
{

    public Text titleText;
    private int curState = 0;

    public Button backBtn;




    GameObject bigImage = null;

    public GameObject loadTipImage;

    public GameObject resultGo;

    public List<string> childSps = new List<string>();
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

    public void BackBtnClick()
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

                        curState--;
                        curState = Mathf.Clamp(curState, 0, 1);
                        UnityHelper.LoadNextScene("main");
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
        childSps.Clear();
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

    public void AddChildSprite(string  path)
    {
        if (childSps==null)
        {
            childSps = new List<string>();
            childSps.Clear();
        }
        childSps.Add(path);
    }

    public void ShowBigImage()
    {
        bigImage = Instantiate<GameObject>(Resources.Load<GameObject>("Model/UI/ScrollViewPanel"), this.transform);

        bigImage.gameObject.SetActive(true);
        bigImage.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        bigImage.GetComponent<RectTransform>().offsetMax = new Vector2(0, -130f);
        bigImage.GetComponent<ScrollViewPanel>().AddItem(childSps);
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


        if (Input.GetKeyDown(KeyCode.Z))
        {

            childSps.Add("1.png"); childSps.Add("1.png"); childSps.Add("1.png"); childSps.Add("1.png"); childSps.Add("1.png");
 
        }
    }


}
