﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Text;
using UnityEngine.UI;

/// <summary>
///  地图点击之后的图片显示管理类
/// </summary>
public class GalleryImageManager : SingletonMono<GalleryImageManager>
{
    [HideInInspector]
  public  GameObject ImageScrollView;
    Transform scrollContent;

    private GameObject canvas;
    private GameObject content;
    public List<PointScrollShowClass> items = new List<PointScrollShowClass>();
    public override void Awake()
    {
        base.Awake();
        canvas = GameObject.Find("Canvas");
    }



    /// <summary>
    /// 地图上点位点击之后的加载图片
    /// </summary>
    /// <param name="pathList"></param>
    public void SpawnImage(string id)
    {
        ImageScrollView = Instantiate(Resources.Load<GameObject>("ImageScrollView"), Vector3.zero, Quaternion.identity, canvas.transform);
        ImageScrollView.name = "ImageScrollView";
        content = ImageScrollView.GetComponent<ScrollRect>().content.gameObject;
        items = TrackDataManager.Instance.items;
        int index = GetItemPaths(id);
        if (index == -1)
            return;
        Dictionary<string, Dictionary<string, List<string>>> pathList = items[index].paths;
        if (pathList.Count == 0)
        {
            return;
        }
        else
        {
            foreach (KeyValuePair<string, List<string>> imagepath in pathList[id])
            {
                content.GetComponent<ScrollContent>().AddContentChildPanel(imagepath.Key, imagepath.Value);
            }
        }
    }

    public int GetItemPaths(string id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == id)
            {
                return i;
            }
        }
        return -1;
    }

}