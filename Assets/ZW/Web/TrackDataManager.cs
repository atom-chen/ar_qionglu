using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Text;
using UnityEngine.UI;
using ElviraFrame;
using UnityEngine.SceneManagement;

/// <summary>
/// 管理足迹的静态数据类
/// </summary>
public class TrackDataManager : SingletonMono<TrackDataManager>
{

    PointClass pointData = new PointClass();


    public LinkList pointLinkList = new LinkList();

    int maxIndex = 0;

    public List<PointScrollShowClass> items = new List<PointScrollShowClass>();
    public override void Awake()
    {
        base.Awake();



    }
    void Start()
    {
        string reader = JsonManager.ReadJsonFromFilePath(UnityHelper.LocalFilePath + "Web/", "PointMap.json");
        if (!string.IsNullOrEmpty(reader))
        {
          Debug.Log("reader不为空=====" + reader);
            pointData = JsonMapper.ToObject<PointClass>(reader);
            AddToLinkList(pointData);

        }
        else
        {
         Debug.Log("reader为空=====" + reader);

        }

    }

    private void AddToLinkList(PointClass pointDataParams)
    {
     //   Debug.Log("AddToLinkList");
        if (pointDataParams == null)
        {
            return;
        }
      //  Debug.Log(pointDataParams.data.Count);
        for (int i = 0; i < pointDataParams.data.Count; i++)
        {
            string id = pointDataParams.data[i].id;
            maxIndex = int.Parse(id);
            string jingdu = pointDataParams.data[i].jingdu;
            string weidu = pointDataParams.data[i].weidu;
            string count = pointDataParams.data[i].count;
            List<Paths> path = pointDataParams.data[i].paths;
            List<Timepaths> timepathses = path[0].timepaths;
            string imagepath = timepathses[0].imagepath;
            AddPoint(jingdu, weidu, imagepath, count, id);
            UpdateImageShowClass(pointDataParams);
        }
  
    }
    /// <summary>
    /// 更新PointClass
    /// </summary>
    /// <param name="pointDataParams"></param>
    private void UpdateImageShowClass(PointClass pointDataParams)
    {
        if (pointDataParams == null)
        {
            return;
        }

        for (int i = 0; i < pointDataParams.data.Count; i++)
        {
            string id = pointDataParams.data[i].id;
            string jingdu = pointDataParams.data[i].jingdu;
            string weidu = pointDataParams.data[i].weidu;
         
            List<Paths> path = pointDataParams.data[i].paths;
            PointScrollShowClass ptClass = new PointScrollShowClass();
            ptClass.id = id;
            ptClass.jingdu = jingdu;
            ptClass.weidu = weidu;
            ptClass.paths = new Dictionary<string, Dictionary<string, List<string>>>();
            int imagecount = 0;
            Dictionary<string, List<string>> pathChild = new Dictionary<string, List<string>>();
            for (int j = 0; j < path.Count; j++)
            {
                List<string> imagePathList = new List<string>();
                for (int k = 0; k < path[j].timepaths.Count; k++)
                {
                    imagePathList.Add(path[j].timepaths[k].imagepath);
                    imagecount++;
                }
                pathChild.Add(path[j].time, imagePathList);
            }
            ptClass.count = imagecount.ToString();
            ptClass.paths.Add(id, pathChild);
            items.Add(ptClass);
        }
    }


    /// <summary>
    ///外部往点List里面加点
    /// 从GPSManager._Instance.GetGPS()里面获得经度和纬度值，
    /// Imagepath：是图片的路径
    /// </summary>
    /// <param name="imagepath"></param>
    /// <param name="count"></param>
    public void AddPoint(string imagepath)
    {
        String location = GPSManager.Instance.GetBDGPSLocation();
        Debug.Log("GPS::::::::::" + location);

        JsonData zb = JsonMapper.ToObject(location);


        float x = float.Parse(zb["longitude"].ToString());
        float y = float.Parse(zb["latitude"].ToString());


        string jingdu = x.ToString("F3");
        string weidu = y.ToString("F3");

        String date = System.DateTime.Now.ToString("yyyy/MM/dd");
        date = date.Replace("/", "");
 
     
        AddPointToPointClass( jingdu, weidu, imagepath, date);
        maxIndex++;
    }
    /// <summary>
    /// 往PointClass类里面加点，用于最后的存储刷新文本记录
    /// </summary>
    /// <param name="index"></param>
    /// <param name="jingdu"></param>
    /// <param name="weidu"></param>
    /// <param name="imagepath"></param>
    /// <param name="date"></param>
    public void AddPointToPointClass(string jingdu, string weidu, string imagepath, string date)
    {

        int tempid = -1;
        if (pointData == null)
        {
            return;
        }
        if (pointData.data.Count != 0)
        {
            for (int i = 0; i < pointData.data.Count; i++)
            {
                int id = int.Parse(pointData.data[i].id);
                int count = int.Parse(pointData.data[i].count);
                string pointjingdu = pointData.data[i].jingdu;
                string pointweidu = pointData.data[i].weidu;
                //如果有当前经纬度
                if ( pointweidu == weidu && pointjingdu == jingdu)
                {
                    tempid = id;
                    List<Paths> path = pointData.data[i].paths;
                    List<Timepaths> timepathses = new List<Timepaths>();
                    for (int j = 0; j < path.Count; j++)
                    {
                        //如果有当前的日期
                        Timepaths tt;
                        if (path[j].time == date)
                        {
                            pointData.data[i].count =( ++count).ToString();
                            timepathses = path[j].timepaths;
                            var pathsCount = timepathses.Count;
                            Debug.Log(pathsCount);
                            tt = new Timepaths((++pathsCount).ToString(), imagepath);
                            Debug.Log(pathsCount);
                            timepathses.Add(tt);
                            break;
                        }
                        //如果没有当前的日期
                        else if (j == path.Count - 1)
                        {
                            pointData.data[i].count = (++count).ToString();
                            tt = new Timepaths("1", imagepath);
                            timepathses.Add(tt);
                            j++;
                            Paths pp = new Paths();
                            pp.time = date;
                            pp.timepaths = timepathses;
                            path.Add(pp);
                            break;
                        }
                    }
                    break;
                }
                //如果没有当前ID
                else if (i == pointData.data.Count - 1)
                {
                    Timepaths item = new Timepaths("1", imagepath);
                    List<Timepaths> items = new List<Timepaths>();
                    items.Add(item);
                    Paths paths = new Paths(date, items);
                    List<Paths> ppp = new List<Paths>();
                    ppp.Add(paths);
                    int num = pointData.data.Count;
                    tempid = ++num;
                    Data data = new Data(tempid.ToString(), jingdu, weidu, "1", ppp);
                    pointData.data.Add(data);
                    break;
                }
            }

        }
        else
        {
            Timepaths item = new Timepaths("1", imagepath);
            List<Timepaths> items = new List<Timepaths>();
            items.Add(item);
            Paths paths = new Paths(date, items);
            List<Paths> ppp = new List<Paths>();
            ppp.Add(paths);
            int num = pointData.data.Count;
            tempid = ++num;
            Data data = new Data(tempid.ToString(), jingdu, weidu, "1", ppp);
            pointData.data.Add(data);
        }
        UpdateImageShowClass(pointData);
        AddPoint(jingdu, weidu, imagepath, "1", tempid.ToString());
    }

    /// <summary>
    /// 往点位集合里面加点,jingdu：经度 ；   weidu：纬度；   imagepath: 图片路径,  count：点位图片的数量，默认1
    /// </summary>
    /// <param name="jingdu"></param>
    /// <param name="weidu"></param>
    /// <param name="imagepath"></param>
    private void AddPoint(string jingdu, string weidu, string imagepath, string count = "1", string id = "1")
    {
      //  pointLinkList.InsertAtLast(jingdu, weidu, imagepath, maxIndex.ToString());
    }

    public void SaveStringToFile()
    {
        JsonManager.SaveJsonToFile(pointData);
    }

}
