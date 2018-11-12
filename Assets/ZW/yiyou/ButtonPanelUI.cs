using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using UnityEngine.SceneManagement;
using ElviraFrame;
#if  UNITY_IOS||UNITY_IPHONE
using com.mob;
#endif


public class ButtonPanelUI : SingletonMono<ButtonPanelUI>
{


    GameObject buttonPanelUIGo;

    string movieSavePath = string.Empty;

    Button shotBtn;
    Button saveBtn;
    Button shareBtn;
    Button reshotBtn;


    RawImage showShotImage;
    public CanvasGroup uiCanvas;
    Text resultTipText;

    string imageSavePath = string.Empty;
    string imageSaveName = string.Empty;

    bool isShareVideo = false;

    KongmingLatern deng;
    Sandcastle castale;
    [HideInInspector]
    public ShareSDK ssdk;
    #region Get/Set

 

 

    public UnityEngine.GameObject ButtonPanelGo
    {
        get
        {
            if (buttonPanelUIGo == null)
            {
                buttonPanelUIGo = GameObject.Find("Canvas/ButtonPanel").gameObject;
            }
            return buttonPanelUIGo;
        }
        set { buttonPanelUIGo = value; }
    }



    public Text ResultTipText
    {
        get
        {

            if (resultTipText == null)
            {
                resultTipText = ButtonPanelGo.transform.Find("ResultTipText").GetComponent<Text>();
            }
            return resultTipText;

        }
        set { resultTipText = value; }
    }


    public Button ShotBtn
    {
        get
        {
            if (shotBtn == null)
            {
                shotBtn = ButtonPanelGo.transform.Find("ShotBtn").GetComponent<Button>();


            }

            return shotBtn;
        }
        set { shotBtn = value; }
    }



    public Button SaveBtn
    {
        get
        {
            if (saveBtn == null)
            {
                saveBtn = ButtonPanelGo.transform.Find("SaveBtn").GetComponent<Button>();
   
            }
            return saveBtn;
        }
        set { saveBtn = value; }
    }



    public Button ShareBtn
    {
        get
        {
            if (shareBtn == null)
            {
                shareBtn = ButtonPanelGo.transform.Find("ShareBtn").GetComponent<Button>();

            }
            return shareBtn;
        }
        set { shareBtn = value; }
    }



    public Button ReshotBtn
    {
        get
        {
            if (reshotBtn == null)
            {
                reshotBtn = ButtonPanelGo.transform.Find("ReshotBtn").GetComponent<Button>();

  
            }

            return reshotBtn;
        }
        set { reshotBtn = value; }
    }

    internal void Init()
    {
        ShotBtn.gameObject.SetActive(true);
        SaveBtn.gameObject.SetActive(false);
        ReshotBtn.gameObject.SetActive(false);
        ShareBtn.gameObject.SetActive(false);
        ResultTipText.gameObject.SetActive(false);
        ShowShotImage.gameObject.SetActive(false);
      
    }

    public RawImage ShowShotImage
    {
        get
        {

            if (showShotImage == null)
            {
                showShotImage = ButtonPanelGo.transform.Find("ShowShotImage").GetComponent<RawImage>();

            }
            return showShotImage;

        }
        set { showShotImage = value; }
    }

    #endregion

    #region    按钮点击事件
    /// <summary>
    /// 重拍
    /// </summary>
    private void ReShotBtnClick()
    {
        Debug.Log("ReShotBtnClick");
        ShotBtn.gameObject.SetActive(true);
        SaveBtn.gameObject.SetActive(false);
        ReshotBtn.gameObject.SetActive(false);
        ShareBtn.gameObject.SetActive(false);
        ResultTipText.gameObject.SetActive(false);
       
        ShowShotImage.texture = null;
        ShowShotImage.gameObject.SetActive(false);
        FingerTouchEL.Instance.targetGameObject = YiyouStaticDataManager.Instance.ShowModel;
        YiyouStaticDataManager.Instance.OnSilenceGameObject(0.5f);
        EffectPanelUI.Instance.EffectPanelGo.gameObject.SetActive(true);
#if UNITY_IOS || UNITY_IPHONE
        GroundPlaneUI.Instance.ShouZujiBtn(false);

#elif UNITY_ANDROID

        if (SceneManager.GetActiveScene().name == "yiyou")
        {
            GroundPlaneUI.Instance.ShouZujiBtn(false);

        }
        else
        {
            WikiSLAMUIController.Instance.ShouZujiBtn(false);
        }
#endif
    }

    /// <summary>
    /// 保存
    /// </summary>
    private void SaveBtnClick()
    {
        Debug.Log("SaveBtnClick");
        ScreenshotManager.Instance.SaveImage();
        ShotBtn.gameObject.SetActive(false);
        SaveBtn.gameObject.SetActive(false);
        ReshotBtn.gameObject.SetActive(false);
        ShareBtn.gameObject.SetActive(true);
        TrackDataManager.Instance.AddPoint(imageSaveName);
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
    }

    /// <summary>
    /// 分享
    /// </summary>
    private void ShareBtnClick()
    {
        if (isShareVideo==true)
        {
            ShareScriptsBase.Instance.ShareVideo(movieSavePath);
        }
        else
        {
            ShareScriptsBase.Instance.Share(ScreenshotManager.Instance.savedPath);

        }
    }


    #endregion





    #region 拍照或者截图
    public void ShotShotClick()
    {

        if (isRec==false)
        StartCoroutine(ShotBtnClick());

    }
    /// <summary>
    /// 截图按钮
    /// </summary>
    IEnumerator ShotBtnClick()
    {
        isShareVideo = false;
        ShowShotImage.texture = null;
       // EffectPanelUI.Instance.EffectPanelGo.gameObject.SetActive(false);
        uiCanvas.alpha = 0;
        RecordManager.Instance.ShowCanvas(false);;
        FingerTouchEL.Instance.targetGameObject = null;
        YiyouStaticDataManager.Instance.OnSilenceGameObject(0f);
        yield return new WaitForSeconds(0.1f);
        ScreenshotManager.SaveScreenshot("yiyou");


    }

    public void ShotCaptureClick()
    {
        if (isRec==false)
        {
            StartCoroutine(StartRecoding());
        }
        else
        {
            stopRecoding();
        }

    }
    bool isRec = false;
    float timer = 10f;
    float time = 0;

    IEnumerator StartRecoding()
    {
        isShareVideo = true;
        uiCanvas.alpha = 0;
        RecordManager.Instance.ShowCanvas(true);
        isRec = true;

        yield return new WaitForSeconds(0.01f);
#if UNITY_ANDROID

        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string date = System.DateTime.Now.ToString("hh-mm-ss_dd_MM_yyyy");
         movieSavePath = "/sdcard/DCIM/AR游/" + date + ".mp4";
        date = date.Replace("-", "");
        date = date.Replace("_", "");
        jo.Call("startCaptureRecode", movieSavePath);


#elif UNITY_IOS || UNITY_IPHONE
        
        ShareREC.startRecoring();



#endif


    }

    public void stopRecoding()
    {
        uiCanvas.alpha = 1;
        Debug.Log("Rec::::::::::::::Stop");
        RecordManager.Instance.ShowCanvas(false);
        isRec = false;

#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("stopCaptureRecoding");

       

#elif UNITY_IOS || UNITY_IPHONE
        
   
        FinishedRecordEvent evt = new FinishedRecordEvent(recordFinishedHandler);
		ShareREC.stopRecording(evt);


#endif
    }
    /// <summary>
    /// 录屏开始回调
    /// </summary>
    public void onCaptureRecodeStart()
    {
        isRec = true;
        RecordManager.Instance.StartProcess();
        Debug.Log("Rec::::::::::::::Start");
    }
    /// <summary>
    /// 录屏失败回调
    /// </summary>
    /// <param name="msg"></param>
    public void onCaptureRecodeFailed(string msg)
    {
        uiCanvas.alpha = 1;
        isRec = false;
        Debug.Log("Rec::::::::::::::Failed");

    }

#if UNITY_IOS || UNITY_IPHONE
    void recordFinishedHandler(Exception ex)
    {
                uiCanvas.alpha =1;
        isRec = false;
        ShareREC.playLastRecording();
    }

#endif

#endregion


    private void OnEnable()
    {

        ScreenshotManager.OnScreenshotTaken += OnScreenshotTaken;
        ScreenshotManager.OnScreenshotSaved += OnImageSaved;
        ScreenshotManager.OnScreenshotFinished += OnScreenshotFinished;
        ScreenshotManager.OnImageSavedName += OnImageSavedName;
        ShotBtn.gameObject.SetActive(true);
        ShotBtn.onClick.AddListener(() =>
        {
            StopAllCoroutines();
            StartCoroutine("ShotBtnClick");
        });
        SaveBtn.gameObject.SetActive(false);
        ReshotBtn.gameObject.SetActive(false);
        ShareBtn.gameObject.SetActive(false);
        ResultTipText.gameObject.SetActive(false);
        ShowShotImage.gameObject.SetActive(false);
      
    }

    private void OnScreenshotFinished(string obj)
    {
        ShotBtn.gameObject.SetActive(false);
        SaveBtn.gameObject.SetActive(true);
        ReshotBtn.gameObject.SetActive(true);
        ShareBtn.gameObject.SetActive(false);
       
    }

    private void CheckKongmingdeng()
    {
        deng = GameObject.FindObjectOfType<KongmingLatern>();
        if (deng == null)
        {
            ShotBtn.gameObject.SetActive(true);
            SaveBtn.gameObject.SetActive(false);
            ReshotBtn.gameObject.SetActive(false);
            ShareBtn.gameObject.SetActive(false);

        }
        else
        {

            ShotBtn.gameObject.SetActive(false);
            SaveBtn.gameObject.SetActive(false);
            ReshotBtn.gameObject.SetActive(false);
            ShareBtn.gameObject.SetActive(false);
        }
    }
  

 
    private void Start()
    {

        CheckKongmingdeng();
     
      
        ResultTipText.gameObject.SetActive(false);
        ShowShotImage.gameObject.SetActive(false);
        if (ssdk == null)
        {
            
            ssdk = GameObject.FindObjectOfType<ShareSDK>();
        }

        ssdk.shareHandler += ShareResult;

        ReshotBtn.onClick.AddListener(ReShotBtnClick);
        SaveBtn.onClick.AddListener(SaveBtnClick);
        ShareBtn.onClick.AddListener(ShareBtnClick);
      

    }
 

    private void OnImageSavedName(string obj)
    {
        imageSaveName = obj;
        Debug.Log("Name====" + obj);
    }

    /// <summary>
    /// 图片保存完之后回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnImageSaved(string path)
    {
        imageSavePath = path;

        Debug.Log("imageSavePath====" + imageSavePath);
    }
    /// <summary>
    /// 截图之后回调
    /// </summary>
    /// <param name="txt2D"></param>
    private void OnScreenshotTaken(Texture2D txt2D)
    {
        uiCanvas = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
        uiCanvas.alpha = 1;
        Debug.Log("截图之后回调");
        if (txt2D != null)
        {
            ShowShotImage.gameObject.SetActive(true);
            ShowShotImage.texture = (Texture)txt2D;
        }
    

        ResultTipText.gameObject.SetActive(true);

        ResultTipText.text = "图片已经保存";
        ResultTipText.gameObject.GetComponent<DOTweenAnimation>().DOPlayForward();

        ShotBtn.gameObject.SetActive(false);
        SaveBtn.gameObject.SetActive(true);
        ReshotBtn.gameObject.SetActive(true);
        ShareBtn.gameObject.SetActive(false);

    }


    private void ShareResult(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        ResultTipText.gameObject.SetActive(true);
        if (state == ResponseState.Success)
        {
            ResultTipText.text = type.ToString() + "  分享成功";
        }
        else if (state == ResponseState.Fail)
        {
            ResultTipText.text = type.ToString() + "  分享失败";

        }
        else if (state == ResponseState.Cancel)
        {
            ResultTipText.text = type.ToString() + "  分享取消";
        }
        ResultTipText.GetComponent<DOTweenAnimation>().onStart = null;
        ResultTipText.GetComponent<DOTweenAnimation>().onStart.AddListener(() =>
        {

            ResultTipText.color = Color.green;
        });
        ResultTipText.GetComponent<DOTweenAnimation>().DOPlayForward();
    }
}
