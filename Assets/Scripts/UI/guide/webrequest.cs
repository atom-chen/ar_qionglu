using System.Collections;
using System.Collections.Generic;
using mainpage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class webrequest : MonoBehaviour {
    public UniWebView web;
    public static webrequest instance;
    public GameObject webobj;
    public Text title;
    public static bool isLoadedWeb;
    private static string lastUri, lastTitle;
    void Awake()
    {
        instance = this;
    }

    private Scene sc;
	// Use this for initialization
	void Start () { 

    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     if (isLoadedWeb)
        //     {
        //         isLoadedWeb = false;
        //         if (GameObject.Find("webpage") != null)
        //         {                
        //             web.Stop();
        //             web = null;
        //             Destroy(GameObject.Find("webpage"));
        //         }
        //     }
        // } 
    }

    public void LoadWebSetTitle(string url,string name)
    {
        if (name.Length<=1)
        {
            name = "网页详情";
        }
        title.text = name;
        Debug.Log("web::::::" + url);
        if (GameObject.Find("webpage") != null)
        {
            Destroy(GameObject.Find("webpage"));
            web.Stop();
            web = null;
        }
        GameObject obj=  GameObject.Instantiate(webobj);
        obj.transform.name = "webpage";
        obj.SetActive(true);
        web = obj.GetComponent<UniWebView>();
        web.OnMessageReceived += _view_OnMessageReceived;
        web.SetBackButtonEnabled(false);
        web.OnKeyCodeReceived += OnKeyCodeReceived;
        web.SetHeaderField("Authorization", PublicAttribute.Token);
        web.Load(url);
        web.Show();
        lastUri = url;
        lastTitle = name;
        
        isLoadedWeb = true;
    }
    private void OnKeyCodeReceived(UniWebView webView, int keyCode)
    {
        Debug.Log("OnKeyCodeReceived keycode:" + keyCode);
        if (keyCode == 4)
        {
            if (GameObject.Find("webpage") != null)
            {
                isLoadedWeb = false;
                Destroy(GameObject.Find("webpage"));
                web.Stop();
                web = null;
            }
        }
    }
    private void _view_OnMessageReceived(UniWebView webView, UniWebViewMessage message)
    {
        Debug.Log("OnMessageReceived :" + message.RawMessage);
        if (message.Path.Equals("ActionName"))
        {
            var action = message.Args["action"];
            var id = message.Args["id"];
            Debug.Log("收到了消息    " + action + "      id :" + id);
            MessageHandler(action, id);
        }
    }

    private void MessageHandler(string action,string id)
    {
        if (GameObject.Find("webpage") != null)
        {
            Destroy(GameObject.Find("webpage"));
            web.Stop();
            web = null;
        }
        sc = SceneManager.GetActiveScene();
        if (action=="ARScan")
        {
            if (sc.name == "main")
            {
                GameObject.Find("UIMainpage").GetComponent<mainUISet>().LoadScene("ARScan");
            }
            else
                UnityHelper.LoadNextScene("ARScan");
        }
        else if (action == "Panorama")
        {
            if (sc.name=="main")
            {
                ChangeList.instance.ShowCurPanorama(id);
            }
            else if (sc.name=="gpsConvert")
            {
                GpsConvert.instance.TurnChangeScene(id);
            }
           
        }
        else if (action == "Navigation")
        {
            ShowdirectPoint("",float.Parse(id));
            if (sc.name == "main")
            {
                GameObject.Find("UIMainpage").GetComponent<mainUISet>().LoadScene("gpsConvert");
            }
            else
                UnityHelper.LoadNextScene("gpsConvert");
        }
        else if (action == "Visit")
        {  
            if (sc.name == "main")
            {
                GameObject.Find("UIMainpage").GetComponent<mainUISet>().LoadScene("yiyou");
            }
            else
                UnityHelper.LoadNextScene("yiyou");
        }
    }

    public void ShowdirectPoint(string type,float SpotId)
    {
        GlobalInfo.GPSdirect = true;
        GlobalInfo.GPSType = type;
        GlobalInfo.GPSId = SpotId;
    }
    
    public void CloseWebView(GameObject obj)
    {
        if (initScene.instance.isLookingAds)
        {
            initScene.instance.isLookingAds = false;
            initScene.instance.EnterMain();
        }

        isLoadedWeb = false;
        obj.GetComponent<UniWebView>().Stop();
        Destroy(obj);
    }

    public void LoadLastWeb()
    {
        LoadWebSetTitle(lastUri,lastTitle);
    }
}
