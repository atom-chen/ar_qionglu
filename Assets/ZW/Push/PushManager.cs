using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        InvokeRepeating("GetCurerntLocation", 1f, 5f);
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void GetCurerntLocation()
    {

        String location = GPSManager.Instance.GetBDGPSLocation();
        Debug.Log("GPS::::::::::" + location);

        JsonData zb = JsonMapper.ToObject(location);


        float x = float.Parse(zb["longitude"].ToString());
        float y = float.Parse(zb["latitude"].ToString());


        string jingdu = x.ToString("F8");
        string weidu = y.ToString("F8");
     

        String date = System.DateTime.Now.ToString("yyyy/MM/dd");
        Debug.Log("date=======" + date);
        date = date.Replace("/", "");
        Vector2 gps1 = new Vector2(x, y);


        GetNearEastPoint(gps1);
    }

    /// <summary>
    /// 获取最近的一个景点
    /// </summary>
    /// <param name="gps1"></param>
    /// <returns></returns>
    public int GetNearEastPoint(Vector2 gps1)
    {
        return 1;
    }

    private  float  GetDistance(Vector2 gps1,Vector2 gps2)
    {


        float distance = 0f;
        float xoffset, yoffset;
        float R = 6378137f;
        gps1.x = gps1.x * Mathf.PI / 180.0f;
        gps2.x = gps2.x * Mathf.PI / 180.0f;



        xoffset = gps1.x - gps2.x;
        yoffset = (gps1.x - gps2.y) * Mathf.PI / 180.0f;

        float sa2, sb2;
        sa2 = Mathf.Sin(xoffset / 2.0f);
        sb2 = Mathf.Sin(yoffset / 2.0f);
        distance = 2 * R * Mathf.Asin(Mathf.Sqrt(sa2 * sa2 + Mathf.Cos(gps1.x) * Mathf.Cos(gps2.x) * sb2 * sb2));
        return distance;

     



    }
}
