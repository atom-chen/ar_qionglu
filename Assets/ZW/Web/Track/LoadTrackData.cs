
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrackData : SingletonMono<LoadTrackData>
{

    PointClass pointData = new PointClass();


    public LinkList pointLinkList = new LinkList();

    public List<PointScrollShowClass> items = new List<PointScrollShowClass>();

    public override void Awake()
    {
        base.Awake();
  
        string reader = ElviraFrame.JsonManager.ReadJsonFromFilePath(UnityHelper.LocalFilePath + "Web/", "PointMap.json");
        if (!string.IsNullOrEmpty(reader))
        {

        Debug.Log("reader不为空=====" + reader);
            pointData = JsonMapper.ToObject<PointClass>(reader);
            //读取完之后分三部分
            //1、读取对应到地图上的点位集合
            AddToMapList(pointData);
            //2、读取到相册点位的集合
            AddToGallery(pointData);
            //3、生成Web
            WebView.Instance.CreateWebView();
        }
        else
        {
            Debug.Log("reader为空=====" + reader);

        }
    }
    /// <summary>
    /// 读取对应到地图上的点位集合
    /// </summary>
    /// <param name="pointData"></param>
    private void AddToMapList(PointClass pointDataParams)
    {
        //   Debug.Log("AddToLinkList");
        if (pointDataParams == null || pointDataParams.data.Count == 0)
        {
            return;
        }
        //  Debug.Log(pointDataParams.data.Count);
        for (int i = 0; i < pointDataParams.data.Count; i++)
        {
            string id = pointDataParams.data[i].id;

            string jingdu = pointDataParams.data[i].jingdu;
            string weidu = pointDataParams.data[i].weidu;
            string count = pointDataParams.data[i].count;
            List<Paths> path = pointDataParams.data[i].paths;
            List<Timepaths> timepathses = path[path.Count-1].timepaths;
            string imagepath = timepathses[timepathses.Count-1].imagepath;
            pointLinkList.InsertAtLast(jingdu, weidu, imagepath, id,count);
        }
    }
    /// <summary>
    /// 读取到相册点位的集合
    /// </summary>
    /// <param name="pointData"></param>
    private void AddToGallery(PointClass pointDataParams)
    {
        //   Debug.Log("AddToLinkList");
        if (pointDataParams == null || pointDataParams.data.Count == 0)
        {
            return;
        }
        for (int i = 0; i < pointDataParams.data.Count; i++)
        {
            string id = pointDataParams.data[i].id;
            string jingdu = pointDataParams.data[i].jingdu;
            string weidu = pointDataParams.data[i].weidu;
            int imagecount =int.Parse( pointDataParams.data[i].count);
            List<Paths> path = pointDataParams.data[i].paths;
            PointScrollShowClass ptClass = new PointScrollShowClass();
            ptClass.id = id;
            ptClass.jingdu = jingdu;
            ptClass.weidu = weidu;
            ptClass.paths = new Dictionary<string, Dictionary<string, List<string>>>();

            Dictionary<string, List<string>> pathChild = new Dictionary<string, List<string>>();
            for (int j = 0; j < path.Count; j++)
            {
                List<string> imagePathList = new List<string>();
                for (int k = 0; k < path[j].timepaths.Count; k++)
                {
                    imagePathList.Add(path[j].timepaths[k].imagepath);
               
                }
                pathChild.Add(path[j].time, imagePathList);
            }
            ptClass.count = imagecount.ToString();
            ptClass.paths.Add(id, pathChild);
            items.Add(ptClass);
        }
    }

   
}
