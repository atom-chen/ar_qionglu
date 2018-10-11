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
    private bool[] firstDown = new bool[]{false,false, false, false, false, false, false, false, false };
    public static int HeadId
    {
        set {PlayerPrefs.SetInt("HeadId", value);}
        get { return PlayerPrefs.GetInt("HeadId");}
    }

    void Awake()
    {
        instance = this;
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
                        HttpManager.Instance.Download(info.thumbnail, (() => { Debug.Log("Done"); }));
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
                        HttpManager.Instance.Download(info.thumbnail, (() => { Debug.Log("Done"); }));
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
        //ABManager.Instance.LogOut();
        HttpManager.Instance.Logout((b =>
        {
            if (!b)
            {

            }
        }));
        //File.Delete(PublicAttribute.LocalFilePath + "APP/Token.json");
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
}
