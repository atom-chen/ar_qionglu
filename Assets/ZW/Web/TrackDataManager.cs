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



    IEnumerator Start()
    {
        CopyFile();

        yield return new WaitForSeconds(1f);
        string reader = JsonManager.ReadJsonFromFilePath("PointMap.json");
        if (reader != null)
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
    private void CopyFile()
    {
        string htmlFilePath = PublicAttribute.LocalFilePath + "Web/";
        string htmlPath = PublicAttribute.LocalFilePath + "Web/a.txt";
        
        if (!File.Exists(htmlPath))
        {
            if (!Directory.Exists(Path.GetDirectoryName(htmlPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(htmlPath));
            }
            FileStream fs = new FileStream(htmlPath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Close();
     
        }

       string sourceFile = Application.streamingAssetsPath + "/Web/CustomOverlay.html";
            string targetFile = htmlFilePath + "CustomOverlay.html";

            if (!File.Exists(targetFile))
            {
                File.Copy(sourceFile, targetFile, true);

            }

            string jsonsourceFile = Application.streamingAssetsPath + "/Web/PointMap.json";
            string jsontargetFile = htmlFilePath + "PointMap.json";
            if (!File.Exists(jsontargetFile))
            {
                File.Copy(jsonsourceFile, jsontargetFile, true);
            }

    }

    private void AddToLinkList(PointClass pointDataParams)
    {
        Debug.Log("AddToLinkList");
        if (pointDataParams == null)
        {
            return;
        }
        Debug.Log(pointDataParams.data.Count);
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
        if (SceneManager.GetActiveScene().name == "Track")
        {
            WebView.Instance.CreateWebView();

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
            string count = pointDataParams.data[i].count;
            List<Paths> path = pointDataParams.data[i].paths;

            PointScrollShowClass ptClass = new PointScrollShowClass();
            ptClass.id = id;
            ptClass.jingdu = jingdu;
            ptClass.weidu = weidu;
            ptClass.count = count;
            ptClass.paths = new Dictionary<string, Dictionary<string, List<string>>>();

            Dictionary<string, List<string>> pathChild = new Dictionary<string, List<string>>();
            for (int j = 0; j < path.Count; j++)
            {
                List<string> imagePathList = new List<string>();



                for (int k = 0; k < path[j].timepaths.Count; k++)
                {
                    imagePathList.Add(path[j].timepaths[k].imagepath);
                    Debug.Log(path[j].timepaths[k].imagepath);
                }
                pathChild.Add(path[j].time, imagePathList);
                Debug.Log(path[j].time);
                //ptClass.paths.Add(path[j].time, pathChild);
            }
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


        string jingdu = x.ToString("F8");
        string weidu = y.ToString("F8");
        Debug.Log("x========" + jingdu + ";;;;;;y============" + weidu+ ";;;;;;;;;;;imagepath" + imagepath);

        String date = System.DateTime.Now.ToString("yyyy/MM/dd");
        Debug.Log("date======="+date);
        date = date.Replace("/", "");
 
        Debug.Log("date=======" + date+ ";;;;;;;x====" + jingdu + ";;;;;;y=======" + weidu + ";;;;;;imagepath" + imagepath+";;;;;;;;;;"+maxIndex.ToString());
        AddPoint(jingdu, weidu, imagepath, "1", (maxIndex+1).ToString());
        AddPointToPointClass(maxIndex+1, jingdu, weidu, imagepath, date);
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
    public void AddPointToPointClass(int index, string jingdu, string weidu, string imagepath, string date)
    {

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
                //如果有当前ID
                if (id == index && pointweidu == weidu && pointjingdu == jingdu)
                {
                    List<Paths> path = pointData.data[i].paths;
                    List<Timepaths> timepathses = new List<Timepaths>();
                    for (int j = 0; j < path.Count; j++)
                    {
                        //如果有当前的日期
                        Timepaths tt;
                        if (path[j].time == date)
                        {
                            timepathses = path[j].timepaths;
                            var pathsCount = timepathses.Count;
                            tt = new Timepaths(pathsCount.ToString(), imagepath);
                            timepathses.Add(tt);
                            break;
                        }
                        //如果没有当前的日期
                        else if (j == path.Count - 1)
                        {
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
                    Data data = new Data((index).ToString(), jingdu, weidu, "1", ppp);
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
            Data data = new Data((index).ToString(), jingdu, weidu, "1", ppp);
            pointData.data.Add(data);
        }
        UpdateImageShowClass(pointData);
    }

    /// <summary>
    /// 往点位集合里面加点,jingdu：经度 ；   weidu：纬度；   imagepath: 图片路径,  count：点位图片的数量，默认1
    /// </summary>
    /// <param name="jingdu"></param>
    /// <param name="weidu"></param>
    /// <param name="imagepath"></param>
    private void AddPoint(string jingdu, string weidu, string imagepath, string count = "1", string id = "1")
    {
        pointLinkList.InsertAtLast(jingdu, weidu, imagepath, count, maxIndex.ToString());
    }

    public void SaveStringToFile()
    {
        JsonManager.SaveJsonToFile(pointData);
    }

}
