using System;
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


    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        SpawnImage("1");
    //        TrackUIManager.Instance.SetTitleText("相册", 1);
    //        List<string> value = new List<string>() { "yiyou06362802112018.png", "yiyou06363202112018.png", "yiyou06363602112018.png", "yiyou06364202112018.png" };
    //        content.GetComponent<ScrollContent>().AddContentChildPanel("20181102", value);
    //    }

    //}
    /// <summary>
    /// 地图上点位点击之后的加载图片
    /// </summary>
    /// <param name="pathList"></param>
    public void SpawnImage(string id)
    {
        ImageScrollView = Instantiate(Resources.Load<GameObject>("Model/ImageScrollView"), Vector3.zero, Quaternion.identity, canvas.transform);
        ImageScrollView.name = "ImageScrollView";
        ImageScrollView.GetComponent<RectTransform>().offsetMin =new Vector2(0,0);
        ImageScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0, -130);
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
                Debug.Log("imagepath.Key, imagepath.Value===" + imagepath.Key + ",,,," + imagepath.Value);
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
