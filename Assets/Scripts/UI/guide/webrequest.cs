using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class webrequest : MonoBehaviour {
    private UniWebView web;
    public static webrequest instance;
    public GameObject webobj;
    public Text title;
    void Awake()
    {
        instance = this;
    }

    private Scene sc;
	// Use this for initialization
	void Start () { 

    }
    public void LoadWeb(string url)
    {
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
        web.SetHeaderField("Authorization", PublicAttribute.Token);
        web.Load(url);
        web.Show();
    }

    public void LoadWebSetTitle(string url,string name)
    {
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
        web.SetHeaderField("Authorization", PublicAttribute.Token);
        web.Load(url);
        web.Show();
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

        if (action=="ARScan")
        {           
            UnityHelper.LoadNextScene("ARScan");
        }
        else if (action == "Panorama")
        {
            sc = SceneManager.GetActiveScene();
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
            
            UnityHelper.LoadNextScene("gpsConvert");
        }
        else if (action == "Visit")
        {
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
        obj.GetComponent<UniWebView>().Stop();
        Destroy(obj);
    }
}
