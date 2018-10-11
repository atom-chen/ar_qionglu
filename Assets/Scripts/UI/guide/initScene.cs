using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class Ports
{
    public string address; //地址
    public string portnum; //端口号
}
public class PortsStatus
{
    public connect[] allports;
}

public class connect
{
    public connect()
    {
        address = "";
        portnum = "";
    }
    public string address; //地址
    public string portnum; //端口号
}
public class initScene : MonoBehaviour
{
    private List<Ports> allport = new List<Ports>();
    public GameObject guide,ads;

    public RectTransform splash;
    public static string localFilePath;

    public GameObject root;
    // Use this for initialization
    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            localFilePath = Application.persistentDataPath + "/port.json";
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            localFilePath = Application.persistentDataPath + "/port.json";
        }
        Debug.Log(localFilePath);
        if (!File.Exists(localFilePath))
        {
            string jsonText = Resources.Load<TextAsset>("port").ToString();
            JsonData jsonData = JsonMapper.ToObject(jsonText);
            for (int i = 0; i < jsonData["allports"].Count; i++)
            {
                allport.Add(JsonMapper.ToObject<Ports>(jsonData["allports"][i].ToJson().ToString()));
            }

            PortsStatus gameStatus = new PortsStatus();
            gameStatus.allports = new connect[jsonData["allports"].Count];

            for (int i = 0; i < jsonData["allports"].Count; i++)
            {
                gameStatus.allports[i] = new connect();
                gameStatus.allports[i].address = allport[i].address;
                gameStatus.allports[i].portnum = allport[i].portnum;
            }
            string json = JsonMapper.ToJson(gameStatus);
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            var ss = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

            File.WriteAllText(localFilePath, ss, Encoding.UTF8);

        }
        else
        {
            StreamReader sr = new StreamReader(localFilePath);
            string jsonText = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            JsonData jsonData = JsonMapper.ToObject(jsonText);
            for (int i = 0; i < jsonData["allports"].Count; i++)
            {
                allport.Add(JsonMapper.ToObject<Ports>(jsonData["allports"][i].ToJson().ToString()));
                Debug.Log(allport[i].address+allport[i].portnum);
            }

            PublicAttribute.URL =allport[0].address + allport[0].portnum;
            Debug.Log(PublicAttribute.URL);
        }
    }
    IEnumerator Start ()
    {
        yield return new WaitForSeconds(1);
        root.SetActive(true);
        yield return new WaitForSeconds(3);
        guide.SetActive(true);
        splash.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
	    ads.SetActive(false);
    }

    public void HideAds()
    {
        ads.SetActive(false);
    }

    public void GetState()
    {

    }
}
