using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class webrequest : MonoBehaviour {
    private UniWebView web;
    public static webrequest instance;
    public GameObject webobj;

    void Awake()
    {
        instance = this;
    }
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
            SceneManager.LoadScene("ARScan");
        }
        else if (action == "Panorama")
        {
            SceneManager.LoadScene("ARScan");
        }
        else if (action == "Navigation")
        {
            SceneManager.LoadScene("gpsConvert");
        }
        else if (action == "Visit")
        {
            SceneManager.LoadScene("gpsConvert");
        }
    }
}
