using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using UnityEngine.UI;

public class mainPageUI : MonoBehaviour
{
    public static mainPageUI instance;
    public RectTransform[] ChildPanel;
    public RectTransform[] ConnectPanel;

    public DOTweenAnimation my_panel;
    public GameObject DownPanel;

    public static AreaInfo SceneID;
    public static DynamicResoucesInfos curScenicInfo;

    public Image loadingImg;
    public Text loadingText;
    public static float fillamount;
    public Image[] HeadImg;
    public Sprite[] HeadSpr;
    public Text[] photonTxt, UserName;
    private bool ischecking;
    private static bool[] firstDown = new bool[]{false,false, false, false, false, false, false, false, false };

    #region 换绑手机号页面
    // 登陆界面弹出提示框
    public LoginUIPopupPage PP;
    public InputField BindingPage_PhoneNoInputField;
    public InputField BindingPage_SmsCodeInputField;
    public Button BindingPage_BindingBtn;
    public Button BindingPage_GetSMSBtn;
    #endregion
    public static int HeadId
    {
        set {PlayerPrefs.SetInt("HeadId", value);}
        get { return PlayerPrefs.GetInt("HeadId");}
    }

    public static float scale;
    void Awake()
    {
        instance = this;
        scale = (((float) Screen.height / (float)Screen.width) * 1440)/2560f;
        for (int i = 0; i < ChildPanel.Length; i++)
        {
            ChildPanel[i].sizeDelta=new Vector2(1440,2560 * scale);
        }


        Screen.orientation = ScreenOrientation.Portrait;
        for (int i = 0; i < HeadImg.Length; i++)
        {
            HeadImg[i].sprite = HeadSpr[HeadId];
        }

        //获取景区列表信息
        if (!firstDown[0])
        {
            HttpManager.Instance.GetAreaInfo((b =>
            {
                if (b)
                {
                    firstDown[0] = true;
                    HttpManager.Instance.DynamicResources();
                    SceneID = JsonClass.Instance.AreaInfoS[0];
                }
            }));
        }

        //获取景点信息
        if (!firstDown[1])
        {
            HttpManager.Instance.GetScenicSpotInfo((b =>
            {
                if (b)
                {
                    firstDown[1] = true;
                    ///如果获取信息成功，开始下载缩略图
                    foreach (var info in JsonClass.Instance.ScenicSpotInfoS)
                    {
                        HttpManager.Instance.Download(info.thumbnail, (() =>
                        {
                            SpotList.instance.CreateItems(info);
                        }));
                    }
                }
            }));
        }
        else
        {
            foreach (var info in JsonClass.Instance.ScenicSpotInfoS)
            {
                HttpManager.Instance.Download(info.thumbnail, (() =>
                {
                    SpotList.instance.CreateItems(info);
                }));
            }
        }

        //获取主页特色景点缩略图
        if (!firstDown[2])
        {
            HttpManager.Instance.GetTraitScenicSpotInfo((b =>
            {
                if (b)
                {
                    firstDown[2] = true;
                    Loom.QueueOnMainThread((() =>
                    {
                        ScrollList.instance.GetImageList(0);
                    }));
                }
            }));
        }
        else
        {
            ScrollList.instance.GetImageList(0);
        }

        //获取主页商家缩略图
        if (!firstDown[3])
        {
            HttpManager.Instance.GetShopSInfo((b =>
            {
                if (b)
                {
                    firstDown[3] = true;
                    ///如果获取信息成功，开始下载缩略图
                    foreach (var info in JsonClass.Instance.ShopInfoS)
                    {
                        HttpManager.Instance.Download(info.thumbnail, (() => { }));
                    }
                }
            }));
        }
        else
        {
            
        }

        //获取特产缩略图
        if (!firstDown[4])
        {
            HttpManager.Instance.GetLocalSpecialtyInfo((b =>
            {
                if (b)
                {
                    firstDown[4] = true;
                    ///如果获取信息成功，开始下载缩略图
                    foreach (var info in JsonClass.Instance.LocalSpecialtyS)
                    {
                        HttpManager.Instance.Download(info.thumbnail, (() => {  }));
                    }
                }
            }));
        }
        else
        {
            
        }

        //获取到此一游道具
        if (!firstDown[5])
        {
            HttpManager.Instance.Visit_GetAll((b =>
            {
                //获取信息成功后
                if (b)
                {
                    firstDown[5] = true;
                    //下载道具的缩略图信息
                    foreach (var info in JsonClass.Instance.VisitInfoS)
                    {
                        HttpManager.Instance.Download(info.Thumbnail, (() => { GoodList.instance.CreateItems(info); }));
                    }
                }
            }));
        }
        else
        {
            foreach (var info in JsonClass.Instance.VisitInfoS)
            {
                GoodList.instance.CreateItems(info);
            }

        }

        BindingPage_BindingBtn.onClick.AddListener((() =>
        {
            if (VerifyPhoneNo(BindingPage_PhoneNoInputField.text) && VerifySMSCode(BindingPage_SmsCodeInputField.text))
            {
                HttpManager.Instance.ChangePhoneNo(BindingPage_PhoneNoInputField.text, BindingPage_SmsCodeInputField.text, (PopupInfo));
            }
            else
            {
                PP.ShowPopup("格式不正确", "请输入正确的格式");
            }
        }));

        BindingPage_GetSMSBtn.onClick.AddListener((() =>
        {
            if (VerifyPhoneNo(BindingPage_PhoneNoInputField.text))
            {
                FreezeButton(BindingPage_GetSMSBtn);
                HttpManager.Instance.GetSMSS(BindingPage_PhoneNoInputField.text, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
            }
            else
            {
                PP.ShowPopup("格式不正确", "请输入正确的手机号");
            }
        }));
    }
    void Start ()
	{
        CoroutineWrapper.EXES(1f, () =>
        {
            PublicAttribute.AreaResoucesDic.TryGetValue(SceneID.id, out curScenicInfo);
            ChangeList.instance.GetImageList();
        });
        CoroutineWrapper.EXES(5f, () =>
        {
            if (!ischecking)
            {
                Debug.LogError("自动下载");
                ischecking = true;
                HttpManager.Instance.DynamicCheekUpdateByArea(SceneID.id);

                //CoroutineWrapper.EXES(60, () =>
                //{
                //    ischecking = false;
                //    CheckProgress();
                //});
            }
        });
        HttpManager.Instance.DownloadPercent = f =>
	    {
	        Debug.LogError("进度: " + f.ToString("#0.000"));
	        fillamount = f;
        };

        //HttpManager.Instance.GetUserInfoByToken((b =>
        //{
        //    if (b)
        //    {
        //        for (int i = 0; i < UserName.Length; i++)
        //        {
        //            UserName[i].text = PublicAttribute.UserInfo.NickName;
        //           }

        //        for (int i = 0; i < photonTxt.Length; i++)
        //        {
        //            photonTxt[i].text = PublicAttribute.UserInfo.PhoneNo;
        //        }

        //        Debug.Log(PublicAttribute.UserInfo.NickName + "       " + PublicAttribute.UserInfo.PhoneNo);
	    //    }
	    //}));

	    for (int i = 0; i < UserName.Length; i++)
	    {
	        UserName[i].text = PublicAttribute.UserInfo.NickName;
	    }

	    for (int i = 0; i < photonTxt.Length; i++)
	    {
	        photonTxt[i].text = PublicAttribute.UserInfo.PhoneNo;
	    }
    }

    public void CheckProgress()
    {
        if (fillamount < 1)
        {
            HttpManager.Instance.DynamicCheekUpdateByArea(SceneID.id);
            CoroutineWrapper.EXES(60f, () =>
            {
                CheckProgress();
            });
        }
    }
    private static void GetFillAmount(float amount)
    {
        fillamount = amount;
    }

    private bool isdown;
    void Update()
    {
        loadingImg.fillAmount = fillamount;
        loadingText.text = ((int)(loadingImg.fillAmount * 100)).ToString() + "%";
        if (fillamount >= 1 && !isdown)
        {
            isdown = true;
            DownPanel.SetActive(false);
            ischecking = true;
            if (isChangeScene)
            {
                LoadScene(lastSceneName);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            HttpManager.Instance.DynamicCheekUpdateByArea(SceneID.id);
        }
    }

    IEnumerator GetDynamicResources()
    {
        if (JsonClass.Instance.AreaInfoS.Count > 0)
        {
            //HttpManager.Instance.DynamicResources();
            SceneID = JsonClass.Instance.AreaInfoS[0];
            yield return new WaitForSeconds(2);
            HttpManager.Instance.DynamicCheekUpdateByArea(SceneID.id);
            yield return new WaitForSeconds(2);
            PublicAttribute.AreaResoucesDic.TryGetValue(SceneID.id, out curScenicInfo);
            ChangeList.instance.GetImageList();
        }
        else
        {
            Debug.Log("nullnullnullnullnullnullnullnull");
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(GetDynamicResources());
        }
    }

    public void ShowDotweenPanel(DOTweenAnimation dot)
    {
        dot.DOPlayForward();
    }
    public void HideDotweenPanel(DOTweenAnimation dot)
    {
        dot.DOPlayBackwards();
    }

    public void ShowGameObjectPanel(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    //跳转场景，判断资源是否完整
    private string lastSceneName;
    private bool isChangeScene;
    public void LoadScene(string scenename)
    {
        isChangeScene = true;
           lastSceneName = scenename;
        if (fillamount >= 1)
        {
            SceneManager.LoadScene(scenename);
        }
        else
        {
            DownPanel.SetActive(true);
            if (!ischecking)
            {
                ischecking = true;
                HttpManager.Instance.DynamicCheekUpdateByArea(SceneID.id);

                //CoroutineWrapper.EXES(60f, () =>
                //{
                //    CheckProgress();
                //});
            }
        }
    }

    public void LogOut()
    {
        HttpManager.Instance.Logout((b =>
        {
            if (!b)
            {

            }
        }));
        File.Delete(PublicAttribute.LocalFilePath + "APP/Token.json");
        SceneManager.LoadScene("Login");
    }

    public void getLocation()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        String location = jo.Call<String>("getLocation");
        debugLog(location);
        Debug.Log("GPS::::::::::::"+location);
    }

    public void debugLog(String msg)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("logTest", msg);
    }
    public void SetHeadID(int id)
    {
        HeadId = id;
        for (int i = 0; i < HeadImg.Length; i++)
        {
            HeadImg[i].sprite = HeadSpr[id];
        }
    }

    //修改名字
    public void SetUserName(Text name)
    {
        HttpManager.Instance.ModifiUserNickName(name.text, (b =>
        {
            if (b)
            {
                for (int i = 0; i < UserName.Length; i++)
                {
                    UserName[i].text = name.text;
                }
                Debug.Log(b);
            }
            else
            {
                for (int i = 0; i < UserName.Length; i++)
                {
                    UserName[i].text = name.text;
                }
            }

            PublicAttribute.UserInfo.NickName = name.text;
        }));
    }
    /// <summary>
    /// 验证手机号是否符合格式
    /// </summary>
    bool VerifyPhoneNo(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (str.Length != 11)
        {
            return false;
        }
        return true;
    } 
    /// <summary>
    /// 验证验证码是否符合格式
    /// </summary>
    bool VerifySMSCode(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (str.Length != 6)
        {
            return false;
        }
        return true;
    }
    private void FreezeButton(Button btn, float time = 15)
    {
        Text text = btn.gameObject.GetComponentInChildren<Text>();
        string oldText = text.text;

        StartCoroutine(changeTime(btn, text, time, oldText));
    }

    IEnumerator changeTime(Button btn, Text text, float time, string oldText)
    {
        btn.interactable = false;
        while (time > 0)
        {
            text.text = time + "";
            //暂停一秒
            yield return new WaitForSeconds(1);
            time--;
        }
        btn.interactable = true;
        text.text = oldText;
    }

    public void Suggest(Text message)
    {
        HttpManager.Instance.Suggest(message.text, (b =>
        {
            if (b)
            {
                message.transform.parent.parent.parent.gameObject.SetActive(false);
                PopupInfo("300");
            }
            else
            {
                PopupInfo("Error");
            }
        }));
    }

    /// <summary>
    /// 根据状态码执行
    /// </summary>
    /// <param name="status"></param>
    public void PopupInfo(string status)
    {
        Debug.Log(status);
        switch (status)
        {
            case "200":
                PP.ShowPopup("请求成功", "请求成功");
                break;
            case "300":
                PP.ShowPopup("意见提交成功", "意见提交成功，我们会尽快查看！");
                break;
            case "Error":
                PP.ShowPopup("请求失败", "请稍候重试");
                break;
            default:
                break;
        }
    }
}
