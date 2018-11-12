/*******
* Copyright (C)2018    Administrator 
* 创建人:              Administrator  
* 创建时间:            2018/6/15 星期五 15:38:15    
****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class PushPointClass
{
    public string name;
    public string radius;

    public List<PushMsg> msgs;

    public PushPointClass()
    {
        radius = string.Empty;
        msgs = new List<PushMsg>();
    }
}



[Serializable]
public class PushMsg
{

    public string id;
    public string locationX;
    public string locationY;
    public string height;
    public string time;
    public string url;
    public string title;
    public string msg;
    public string type;
    public string dbid;
    public PushMsg()
    {

    }
    public PushMsg(string id, string jingdu, string weidu, string height, string time, string url, string title, string msg,string type,string  dbid)
    {

        this.id = id;
        this.locationX = jingdu;
        this.locationY = weidu;
        this.height = height;
        this.time = time;
        this.url = url;
        this.title = title;
        this.msg = msg;
        this.type = type;
        this.dbid = dbid;
    }
}






