using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackUIManager : SingletonMono<TrackUIManager> {

    public Text titleText;
    private int curState=0;

    public Button backBtn;



    GameObject showImage;
    public GameObject ShowImage
    {
        get {

            if (showImage == null)
            {
                showImage = transform.Find("ShowImage").gameObject ;
            }
            return showImage;
        }
        set { showImage = value; }
    }
    public UnityEngine.UI.Button BackBtn
    {
        get {
            if (backBtn == null)
            {
                backBtn = transform.Find("Title/BackButton").GetComponent<Button>();
                backBtn.onClick.AddListener(BackBtnClick);
            }
            return backBtn;
        }
        set { backBtn = value; }
    }

    private void BackBtnClick()
    {
        if (curState==1)
        {
            Destroy(GalleryImageManager.Instance.ImageScrollView);
            WebView.Instance.CreateWebView();
        }
        else
        {
            SceneManager.LoadScene("main");
        }
        curState--;
        curState = Mathf.Clamp(curState, 0, 1);
    }

    public UnityEngine.UI.Text TitleText
    {
        get {
            if (titleText==null)
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
        //   WebView.Instance.CreateWebView();


        ShowImage.GetComponentInChildren<Button>().onClick.AddListener(CancelClick);
        ShowImage.gameObject.SetActive(false);
    }

    private void CancelClick()
    {
        ShowImage.gameObject.SetActive(false);
    }

    public void ShowBigImage(Sprite  sps)
    {

        ShowImage.gameObject.SetActive(true);
        ShowImage.GetComponent<Image>().sprite = sps;
    }
}
