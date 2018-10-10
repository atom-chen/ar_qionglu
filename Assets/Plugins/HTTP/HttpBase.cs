using BestHTTP;

/**
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     HttpBase.cs
 *Author:       若飞
 *Version:      1.0
 *UnityVersion：5.3.2f1
 *Date:         2017-04-27
 *Description:
 *History:
*/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HttpBase
{
    /// <summary>
    /// OSS  Get
    /// </summary>
    /// <param name="url"></param>
    /// <param name="post"></param>
    /// <param name="OnRequestFinished"></param>
    /// <param name="token"></param>
    public static void OssGet(string url, KeyValuePair<string, string>[] post, OnRequestFinishedDelegate OnRequestFinished, string token = null)
    {
        BestHTTP.HTTPRequest request = new BestHTTP.HTTPRequest(new Uri(url), OnRequestFinished);
        if (post != null)
        {
            foreach (var item in post)
            {
                request.SetHeader(item.Key, item.Value);
            }
        }
        request.Send();
    }


    /// <summary>
    /// Post 方法
    /// </summary>
    /// <param name="url"></param>
    /// <param name="post"></param>
    /// <param name="OnRequestFinished"></param>
    public static void POST(string url, KeyValuePair<string, string>[] post, OnRequestFinishedDelegate OnRequestFinished,string token =null)
    {
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, OnRequestFinished);
        if (post != null)
        {
            foreach (var item in post)
            {
                request.AddField(item.Key, item.Value);
            }
            if (!string.IsNullOrEmpty(token))
            {
                request.SetHeader("Authorization", token);
            }
        }
        request.Send();
    }


    /// <summary>
    /// 发送POST JSOn请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="message"></param>
    /// <param name="OnRequestFinished"></param>
    public static void POSTJSON(string url,string message, OnRequestFinishedDelegate OnRequestFinished)
    {
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, OnRequestFinished);
        request.SetHeader("Content-Type", "application/json; charset=UTF-8");
        request.RawData = System.Text.Encoding.UTF8.GetBytes(message);
        request.Send();
    }
    
    /// <summary>
    /// 下载
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="onProgress"></param>
    /// <param name="onRequestFinished"></param>
    public static void Download(string filePath, OnDownloadProgressDelegate onProgress, OnRequestFinishedDelegate onRequestFinished)
    {
        var request = new HTTPRequest(new Uri(filePath), onRequestFinished);
        request.OnProgress += onProgress;
        request.Send();
    }

    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bytes"></param>
    /// <param name="textureName"></param>
    /// <param name="onRequestFinished"></param>
    public static void UpLoadTexture(string url, byte[] bytes, string textureName, OnRequestFinishedDelegate onRequestFinished)
    {
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, onRequestFinished);
        request.AddField("enctype", "multipart/form-data");
        request.AddBinaryData("file", bytes, textureName, "image/png");
        request.Send();
    }

/// <summary>
/// 上传第三方用户信息
/// </summary>
/// <param name="url"></param>
/// <param name="post"></param>
/// <param name="bytes">用户头像</param>
/// <param name="textureName"></param>
/// <param name="onRequestFinished"></param>
    public static void UploadUserInfo(string url, KeyValuePair<string, string>[] post, byte[] bytes, string textureName, OnRequestFinishedDelegate onRequestFinished)
    {
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, onRequestFinished);
        if (post != null)
        {
            foreach (var item in post)
            {
                request.AddField(item.Key, item.Value);
            }
        }
        request.AddField("enctype", "multipart/form-data");
        request.AddBinaryData("file", bytes, textureName, "image/png");
        request.Send();
    }

    /// <summary>
    /// 修改用户头像
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bytes"></param>
    /// <param name="textureName"></param>
    /// <param name="onRequestFinished"></param>
    /// <param name="token"></param>
    public static void ModifiUserIcon(string url, byte[] bytes, string textureName, OnRequestFinishedDelegate onRequestFinished, string token = null)
    {
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, onRequestFinished);
        request.AddField("enctype", "multipart/form-data");
        request.AddBinaryData("file", bytes, textureName, "image/png");
        if (!string.IsNullOrEmpty(token))
        {
            request.SetHeader("Authorization", token);
        }
        request.Send();
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bytes"></param>
    /// <param name="ObjName"></param>
    /// <param name="ObjType"></param>
    /// <param name="onRequestFinished"></param>
    public static void UpLoadObj(string url, byte[] bytes, string ObjName, string ObjType, OnRequestFinishedDelegate onRequestFinished)
    {
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, onRequestFinished);
        request.AddField("enctype", "multipart/form-data");
        request.AddBinaryData("file", bytes, ObjName, ObjType);
        request.Send();
    }

    /// <summary>
    /// Get方法
    /// </summary>
    /// <param name="SQL"></param>
    /// <param name="onRequestFinished"></param>
    public static void GET(string SQL,OnRequestFinishedDelegate onRequestFinished, string token = null)
    {
        var request = new HTTPRequest(new Uri(SQL), onRequestFinished);
        if (!string.IsNullOrEmpty(token))
        {
            request.SetHeader("Authorization", token);
        }
        request.Send();
    }

    public static void ShareComponent(string url, string key, byte[] AudioBytes, string json, OnRequestFinishedDelegate onRequestFinished)
    {
        Debug.Log("上传用户分享文件  " + json);
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, onRequestFinished);
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
        request.AddField("enctype", "multipart/form-data");
        request.AddField("key", key);
        request.AddBinaryData("audioFiles", AudioBytes, "vsz.wav", "Audio/wav");
        request.AddBinaryData("jsonFile", jsonBytes, "vsz.json", "json");
        request.Send();
    }
}