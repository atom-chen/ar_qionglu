using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectItemUI : SingletonMono<SelectItemUI>
{



     Image bgImage;
    Image loadTipImage;

    bool isLoadComplete = false;
    bool isRecgnized = false;
    
    public UnityEngine.UI.Image BgImage
    {
        get {
            if (bgImage==null)
            {
                bgImage = gameObject.transform.Find("BgImage").GetComponent<Image>();
                bgImage.GetComponent<Button>().onClick.AddListener(BgImageClick);
            }
            return bgImage;

        }
        set { bgImage = value; }
    }



    public UnityEngine.UI.Image LoadTipImage
    {
        get {
            if (loadTipImage == null)
            {
                loadTipImage = gameObject.transform.Find("LoadTipImage").GetComponent<Image>();
                loadTipImage.GetComponent<Button>().onClick.AddListener( LoadTipImageClick);
            }
            return loadTipImage;

        }
        set { loadTipImage = value; }
    }


    public void Init(Sprite   sps,bool  isCanClick)
    {
        BgImage.sprite = sps;
        LoadTipImage.gameObject.SetActive(false);
        BgImage.GetComponent<Button>().interactable = isCanClick;
        transform.localScale = Vector3.one;
    }


    public void SetBtnInteractable(bool isCanClick)
    {
        isLoadComplete = true;
       // BgImage.GetComponent<Button>().interactable = isCanClick;
    }
    private void BgImageClick()
    {
#if UNITY_IOS
		            PlaneManager.Instance.SetMode(BgImage.sprite.name);

#elif UNITY_ANDROID
        if (SceneManager.GetActiveScene().name == "yiyou")
        {
            PlaneManager.Instance.SetMode(BgImage.sprite.name);

        }
        else if (SceneManager.GetActiveScene().name == "wikiSLAM")
        {
            WikiSLAMController.Instance.SetMode(BgImage.sprite.name);
          

        }
        else if (SceneManager.GetActiveScene().name == "yiyouEasyAR")
        {
   
       //     EasyARSLAM.Instance.SetMode(BgImage.sprite.name);

        }


#endif
    }

    private void LoadTipImageClick()
    {

    }

    internal void SetActive(bool active)
    {
        isRecgnized = active;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name== "wikiSLAM")
        {
            if (isRecgnized && isLoadComplete)
            {
                BgImage.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            if ( isLoadComplete)
            {
                BgImage.GetComponent<Button>().interactable = true;
            }
        }
    }
}
