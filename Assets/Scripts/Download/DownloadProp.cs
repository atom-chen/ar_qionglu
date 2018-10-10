using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 下载道具
/// 缩略图名称：thumb.png  DownloadFile/preview/道具ID/thumb.png
/// 卖家秀图片: bset.jpg   DownloadFile/preview/道具ID/bset.jpg
/// 卖家秀套图: 1-n.jpg    
/// DownloadFile/preview/道具ID/1.jpg
/// DownloadFile/preview/道具ID/2.jpg
/// DownloadFile/preview/道具ID/3.jpg
/// 
/// 本地道具为一个prefab
/// 本地道具存放位置  : Resources/ObjPrefabs/道具ID  
/// 
/// 
/// 引导页：
/// 1、APP引导页              DownloadFile/GUID/ID
/// 2、主页的引导页           DownloadFile/MainGUID/ID
/// </summary>
public class DownloadProp : Singleton<DownloadProp>
{
    #region Public Actions

    /// <summary>
    /// 有了新的大分类
    /// </summary>
    public Action<CategoryComponentItem> NewCategoryAction;

    /// <summary>
    /// 有了新的道具
    /// </summary>
    public Action<CategoryComponentItem> NewComponentAction;

    /// <summary>
    /// 需要更新的道具
    /// </summary>
    public Action<CategoryComponentItem> UpdateComponentAction;

    #endregion Public Actions

    /// <summary>
    /// 下载进度
    /// </summary>
    private float _percent;

    private List<Downloader.DownloadUnit> _downloadList = new List<Downloader.DownloadUnit>();

    private List<Downloader.DownloadUnit> _previewdownloadList = new List<Downloader.DownloadUnit>();

    private Downloader downloader;
    private Downloader shareDownloader;
    private Downloader previewDownloader;

    protected override void Init()
    {
        base.Init();
        downloader = new Downloader();
        previewDownloader = new Downloader();
        shareDownloader = new Downloader();
    }

    #region 公用方法

    /// <summary>
    /// 获取引导页页面  路径为 GUID/id/
    /// </summary>
    public void GetGuidPage()
    {
        string previewSavePath = PublicAttribute.LocalFilePath + "GUID/";
        HttpBase.GET(PortClass.Instance.Guid, ((request, response) =>
         {
             if (response == null || !response.IsSuccess)
             {
                 DebugManager.Instance.LogError("请求失败！");
                 return;
             }
             JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
             if (content["codeMsg"].ToString() != "SUCCESS")
             {
                 return;
             }
             if (content["data"] == null)
             {
                 return;
             }
             JsonData date = content["data"];

             if (date != null && date.Count > 0)
             {
                 for (int i = 0; i < date.Count; i++)
                 {
                     GUID guid = new GUID();
                     guid.id = int.Parse(date[i]["id"].ToString());
                     if (date[i]["url"] != null)
                     {
                         guid.url = date[i]["url"].ToString();
                     }
                     if (date[i]["des"] != null)
                     {
                         guid.des = date[i]["des"].ToString();
                     }
                     guid.name = date[i]["name"].ToString();
                     guid.pageIndex = int.Parse(date[i]["pageIndex"].ToString());

                     ///若果pageIndex大于100 该引导页图为主页的引导页
                     if (guid.pageIndex>100)
                     {
                         previewSavePath = PublicAttribute.LocalFilePath + "MainGUID/";
                     }
                     if (!Directory.Exists(previewSavePath + guid.pageIndex))
                     {
                         Directory.CreateDirectory(previewSavePath + guid.pageIndex);
                     }

                     //PreviewSavePath(PortClass.Instance.GetPagePerview, guid.id.ToString(), previewSavePath, guid);
                     JsonClass.Instance.GUIDList.Add(guid);
                 }
             }
             ///previewSavePath所有子文件夹集合  用来判断老旧的资源文件 是否需要删除
             var pathlist = Utility.GetFolder(previewSavePath);
             List<string> idlist = JsonClass.Instance.GUIDList.Select(guid => guid.pageIndex.ToString()).ToList();
             foreach (var item in pathlist.Where(item => !idlist.Contains(item)))
             {
                 Directory.Delete(Path.Combine(previewSavePath, item));
             }
             //////删除文件夹内多余的图片/视频
         }
      ));
    }

    /// <summary>
    /// 更新道具的缩略资源    保持路径为"preview/id/"
    /// 缩略图 . 卖家秀图片
    /// </summary>
    public void UpdatePreview()
    {
        //预览资源保存本地的路径
        string previewSavePath = PublicAttribute.LocalFilePath + "preview/";
        foreach (var item in JsonClass.Instance.ComponentList)
        {
            //PreviewSavePath(PortClass.Instance.Preview, item.id.ToString(), previewSavePath);
        }
    }

    /// <summary>
    /// 获取道具
    /// </summary>
    /// <param name="id">资源ID号</param>
    /// <param name="callback">返回GameObject</param>
    public void GetComponent(string id, Action<GameObject> callback)
    {
        int CID = int.Parse(id);
        var item = JsonClass.Instance.ComponentList.SingleOrDefault(t => t.id == CID);
        Debug.Log(item.paths[0]);
        //if (!Directory.Exists(PublicAttribute.LocalFilePath + item.paths[0]))
        //{
        //    Debug.Log("本地没有资源，需要下载 ");
        //    //若本地没有下载该资源
        //    DownloadUpdate(CID, "0.0.0", ((str) =>
        //      {
        //          Loom.QueueOnMainThread((() =>
        //          {
        //              //var ab = AssetBundle.LoadFromFile(str + "/main.ab");
        //              //var go = ab.LoadAsset<GameObject>("main");
        //              //callback(go);
        //          }));
        //      }));
        //}
        //else
        //{
        //    Debug.Log("本地已经有资源了,版本号为  " + GetVersionByID(item.id));
        //    DownloadUpdate(CID, GetVersionByID(item.id), ((str) =>
        //      {
        //          //服务器中未发现可用版本   从本地获取prefab道具
        //          if (str == "HaveNoUpdate")
        //          {
        //              var go = GameObject.Instantiate(Resources.Load("ObjPrefabs/" + id)) as GameObject;
        //              callback(go);
        //          }
        //          else
        //          {
        //              Loom.QueueOnMainThread((() =>
        //              {
        //                  var ab = AssetBundle.LoadFromFile(str + "/main.ab");
        //                  var go = ab.LoadAsset<GameObject>("main");
        //                  callback(go);
        //              }));
        //          }
        //      }));
        //}


        DownloadUpdate(CID, GetVersionByID(item.id), ((str) =>
        {
            //服务器中未发现可用版本   从本地获取prefab道具
            if (str == "HaveNoUpdate")
            {
                var go = GameObject.Instantiate(Resources.Load("ObjPrefabs/" + id)) as GameObject;
                callback(go);
            }
            else
            {
                Loom.QueueOnMainThread((() =>
                {
                    var ab = AssetBundle.LoadFromFile(str + "/main.ab");
                    var go = ab.LoadAsset<GameObject>("main");
                    callback(go);
                }));
            }
        }));
    }

    /// <summary>
    /// 更新大类 判断是否出现新的大类
    /// </summary>
    public List<CategoryComponentItem> UpdateCategoryInfo()
    {
        List<CategoryComponentItem> CCs = new List<CategoryComponentItem>();
        foreach (var item in JsonClass.Instance.CategoryList)
        {
            foreach (var itemPath in item.paths)
            {
                var path = PublicAttribute.LocalFilePath + itemPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            CCs.Add(item);
        }
        return CCs;
    }

    /// <summary>
    /// 更新道具信息  判断是否出现新的道具
    /// </summary>
    public List<CategoryComponentItem> UpdateComponentInfo()
    {
        List<CategoryComponentItem> CCs = new List<CategoryComponentItem>();
        foreach (var item in JsonClass.Instance.ComponentList)
        {
            foreach (var itemPath in item.paths)
            {
                var path = PublicAttribute.LocalFilePath + itemPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            CCs.Add(item);
        }
        return CCs;
    }

    /// <summary>
    /// 自动更新已经下载了的道具
    /// </summary>
    public void AutoCheckUpdateLocalComponent()
    {
        foreach (var item in JsonClass.Instance.ComponentList)
        {
            var LocalVersionPath = PublicAttribute.LocalFilePath + item.paths[0] + "/version.json";
            //Debug.Log(LocalVersionPath);
            if (File.Exists(LocalVersionPath))
            {
                var version = File.ReadAllText(LocalVersionPath);
                //若服务器中道具版本与本地版本不一致
                if (item.version != version)
                {
                    Debug.Log("需要自动更新的道具有    " + item.name + " 本地该道具的版本为    " + LocalVersionPath);
                    DownloadUpdate(item.id, GetVersionByPath(LocalVersionPath));
                }
            }
        }
    }

    /// <summary>
    /// 获取所有的大分类   湖泊/森林/草地/...
    /// </summary>
    public void GetCategoryInfo()
    {
        //获取版本信息
        HttpBase.GET(PortClass.Instance.Category, (request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            JsonData Item = JsonMapper.ToObject(response.DataAsText.Trim());
            if (Item["codeMsg"].ToString() != "SUCCESS")
            {
                return;
            }
            JsonData filejson = Item["data"];
            for (int i = 0; i < filejson.Count; i++)
            {
                JsonData temp = filejson[i];
                CategoryComponentItem CI = new CategoryComponentItem();

                ParseCateCom(temp, CI);
                JsonClass.Instance.CategoryList.Add(CI);
            }
        });
    }

    /// <summary>
    /// 获取所有的小类信息
    /// </summary>
    public void GetComponentInfo()
    {
        //获取版本信息
        HttpBase.GET(PortClass.Instance.Component, (request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            JsonData Item = JsonMapper.ToObject(response.DataAsText.Trim());
            if (Item["codeMsg"].ToString() != "SUCCESS")
            {
                return;
            }
            JsonData filejson = Item["data"];
            for (int i = 0; i < filejson.Count; i++)
            {
                JsonData temp = filejson[i];
                CategoryComponentItem ci = new CategoryComponentItem();
                ParseCateCom(temp, ci);
                JsonClass.Instance.ComponentList.Add(ci);
            }
        });
    }

    /// <summary>
    ///下载/更新道具  不传version默认为本地没有
    /// </summary>
    /// <param name="id">道具ID</param>
    public void DownloadUpdate(int id, string version = "0.0.0", Action<string> done = null)
    {
        //获取版本信息
        //若是默认的版本号，需要先获取当前道具的最新版本号
        //if (version == "1.0.0")
        //{
        //   var component =  JsonClass.Instance.ComponentList.Single(t => t.id == id);
        //    if (component!=null)
        //    {
        //        version = component.version;
        //    }
        //}
        Debug.Log(PortClass.Instance.CheckUpdate + id + "/" + version);
        HttpBase.GET(PortClass.Instance.CheckUpdate + id + "/" + version, (request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            JsonData Item = JsonMapper.ToObject(response.DataAsText.Trim());

            if (Item["msg"].ToString() == "服务器中未发现可用版本")
            {
                if (done != null)
                {
                    done("HaveNoUpdate");
                }
                return;
            }

            if (Item["codeMsg"].ToString() != "SUCCESS")
            {
                Debug.Log("请求失败！");
                return;
            }
            JsonData itemm = Item["data"];

            var op = itemm["op"].ToString();
            //如果op是full 需要先删除本地文件
            if (op == "full")
            {
                string fullpath = FindFolder(id)[0] + "full.json";
                //获取本地的full文件  根据本地full文件删除原有的东西
                if (File.Exists(fullpath))
                {
                    StreamReader sr = File.OpenText(fullpath);
                    string input = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    var fullfilejson = JsonMapper.ToObject(input.Trim());
                    for (int i = 0; i < (int)fullfilejson.Count; i++)
                    {
                        JsonData temp = fullfilejson[i];
                        foreach (var item in FindFolder(id))
                        {
                            Debug.Log("需要删除的文件:    " + item + temp["originalName"].ToString());
                            if (File.Exists(item + "/" + temp["originalName"].ToString()))
                            {
                                File.Delete(item + "/" + temp["originalName"].ToString());
                            }
                        }
                    }
                }
                GetFullJson(id);
                ///执行下载
            }
            JsonData filejson = itemm["files"];
            if (filejson != null)
            {
                for (int i = 0; i < (int)filejson.Count; i++)
                {
                    JsonData temp = filejson[i];
                    FilesItem fi = ParseFilesItem(temp);
                    JsonClass.Instance.CurrentAppUpdateFileJson.files.Add(fi);
                    switch (temp["op"].ToString())
                    {
                        case "add":
                            //_downloadList.Add(
                                //new Downloader.DownloadUnit(
                                //    PublicAttribute.URL + "/" + fi.url,
                                //    PublicAttribute.LocalFilePath + fi.paths[0], fi.originalName, fi.md5));
                            break;

                        case "update":

                            foreach (var path in fi.paths)
                            {
                                if (File.Exists(PublicAttribute.LocalFilePath + path + "/" + fi.originalName))
                                {
                                    File.Delete(PublicAttribute.LocalFilePath + path + "/" + fi.originalName);
                                }
                            }
                            //_downloadList.Add(
                                //new Downloader.DownloadUnit(
                                //    PublicAttribute.URL + "/" + fi.url,
                                //    PublicAttribute.LocalFilePath + fi.paths[0], fi.originalName, fi.md5));
                            break;

                        case "remove":
                            foreach (var path in fi.paths)
                            {
                                if (File.Exists(PublicAttribute.LocalFilePath + path + "/" + fi.originalName))
                                {
                                    File.Delete(PublicAttribute.LocalFilePath + path + "/" + fi.originalName);
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
                BatchDownloadFile(FindFolder(id), itemm["version"].ToString(), op, id, done);
            }
            else
            {
                done(FindFolder(id)[0]);
            }
        });
    }

    /// <summary>
    /// 下载用户分享内容
    /// </summary>
    /// <param name="key"></param>
    /// <param name="callback"></param>
    public void GetAudioText(string key, Action<AudioClip, TextType> callback)
    {
        TextType tt = new TextType();
        AudioClip ac = null;
        if (string.IsNullOrEmpty(key))
        {
            return;
        }

        ////判断key这个文件夹是否存在
        //string filePath = PublicAttribute.LocalFilePath + key;
        //Debug.Log("语音文件地址" + filePath);
        //if (!Directory.Exists(filePath))
        //{
        //    Directory.CreateDirectory(filePath);
        //}

        HttpBase.GET(PortClass.Instance.ShareInfoDownload + key, (request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            JsonData item = JsonMapper.ToObject(response.DataAsText.Trim());
            if (item["codeMsg"].ToString() != "SUCCESS")
            {
                return;
            }
            JsonData date = JsonMapper.ToObject(item["data"].ToString());
            for (int i = 0; i < date.Count; i++)
            {
                JsonData temp = date[i];
                if (temp["path"] == null)
                {
                    return;
                }
                string pathUrl = temp["path"].ToString();
                //下载音频文件
                HttpBase.GET(PortClass.Instance.ShareDownload + pathUrl, (request1, response1) =>
                {
                    if (response1 == null || !response1.IsSuccess)
                    {
                        DebugManager.Instance.LogError("请求失败！");
                        return;
                    }
                    if (response1.IsSuccess)
                    {
                        if (temp["suffix"].ToString().Contains("json"))
                        {
                            //下载json文件
                            tt = JsonMapper.ToObject<TextType>(response1.DataAsText);
                        }
                        if (temp["suffix"].ToString().Contains("wav"))
                        {
                            ac = WavUtility.ToAudioClip(response1.Data);
                        }
                        callback(ac, tt);
                    }
                });
            }
        });
    }

    /// <summary>
    /// 上传用户分享内容
    /// </summary>
    /// <param name="tt"></param>
    /// <param name="clip"></param>
    public void UploadAudioText(TextType tt, byte[] clip)
    {
        string json = "";
        if (tt != null)
        {
            json = Regex.Unescape(JsonMapper.ToJson(tt));
        }
        HttpBase.ShareComponent(PortClass.Instance.ShareUpload, PublicAttribute.SetSecretKey(), clip, json, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                response.Dispose();
            }
        }));
    }

    #endregion 公用方法

    #region 私有方法

    /// <summary>
    /// 获取服务器当前app版本中道具的full.json文件
    /// </summary>
    /// <param name="Message"></param>
    private void GetFullJson(int id)
    {
        List<FilesItem> FIS = new List<FilesItem>();
        var item = JsonClass.Instance.ComponentList.SingleOrDefault(t => t.id == id);
        string version = item.version;
        Debug.Log("获取道具  " + id + "   的最新版本 " + version + "  的full文件");
        //获取版本信息
        HttpBase.GET(PortClass.Instance.FullJsonJson + id + "/full/" + version, (request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            JsonData Item = JsonMapper.ToObject(response.DataAsText.Trim());
            Debug.Log(response.DataAsText.Trim());
            JsonData filejson = Item["files"];
            for (int i = 0; i < (int)filejson.Count; i++)
            {
                JsonData temp = filejson[i];
                //FilesItem fi = new FilesItem();
                //fi.originalName = temp["originalName"].ToString();
                //fi.url = temp["url"].ToString();
                //fi.type = temp["type"].ToString();
                //fi.op = temp["op"].ToString();
                //fi.md5 = temp["md5"].ToString();
                //fi.size = temp["size"].ToString();                ;
                FIS.Add(ParseFilesItem(temp));
            }

            foreach (var folder in FindFolder(id))
            {
                string fullpath = folder + "/full.json";
                if (!Directory.Exists(Path.GetDirectoryName(fullpath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
                }
                if (!File.Exists(fullpath))
                {
                    FileStream file = new FileStream(fullpath, FileMode.Create, FileAccess.Write);//创建写入文件
                    StreamWriter sw = new StreamWriter(file);
                    sw.WriteLine(JsonMapper.ToJson(FIS));//开始写入值
                    sw.Close();
                    sw.Dispose();
                    file.Close();
                }
                else
                {
                    FileStream file = new FileStream(fullpath, FileMode.Open, FileAccess.Write);
                    StreamWriter sr = new StreamWriter(file);
                    sr.WriteLine(JsonMapper.ToJson(FIS));//开始写入值
                    sr.Close();
                    sr.Dispose();
                    file.Close();
                }
            }
        });
    }

    /// <summary>
    /// 下载道具中的文件
    /// </summary>
    /// <param name="savePath">版本号保存路径</param>
    /// <param name="version">版本号</param>
    /// <param name="op">当前道具的op</param>
    /// <param name="id">当前道具的id</param>
    private void BatchDownloadFile(List<string> savePath, string version, string op, int id, Action<string> callback)
    {
        //foreach (var path in savePath)
        //{
        //    Debug.Log(path);
        //}
        if (_downloadList.Count == 0)
        {
            return;
        }
        downloader.BatchDownload(_downloadList, (size, unit, done) =>
        {
            //Debug.Log(_downloadList[0].downUrl);
            _percent = size.TotalLocalSize / (float)size.TotalServerSize;
            Debug.Log(_percent);
            //Debug.Log(size.TotalLocalSize / (float)size.TotalServerSize);
            if (size.TotalLocalSize == size.TotalServerSize && done)
            {
                DebugManager.Instance.Log("---------所有文件下载完毕---------");
                foreach (var downloadUnit in _downloadList)
                {
                    foreach (var path in savePath)
                    {
                        Debug.Log(downloadUnit.FullPath + "    复制文件到    " + path + "/" + downloadUnit.fileName);
                        //如果文件不存在 则复制
                        if (!File.Exists(path + "/" + downloadUnit.fileName))
                        {
                            File.Copy(downloadUnit.FullPath, path + "/" + downloadUnit.fileName);
                        }
                    }
                }
                //所有下载成功之后将本地版本号 改为 服务器版本
                Loom.QueueOnMainThread(() =>
                {
                    foreach (var path in savePath)
                    {
                        Debug.Log("本地版本路径  " + path);
                        FileInfo file = new FileInfo(path + "/" + "Version.json");
                        StreamWriter sw = file.CreateText();
                        Debug.Log(version);
                        sw.WriteLine(version);
                        sw.Close();
                        sw.Dispose();

                        if (op != "full")
                        {
                            //获取当前版本的full文件
                            GetFullJson(id);
                        }
                    }
                });
                callback(savePath[0]);
            }
        }, (error) =>
        {
            DebugManager.Instance.LogError("下载失败  " + error.fileName);
        });
    }

    /// <summary>
    /// 根据ID获取文件夹
    /// </summary>
    /// <param name="id"></param>
    private List<string> FindFolder(int id)
    {
        //return (from item in JsonClass.Instance.ComponentList where item.id == id select PublicAttribute.LocalFilePath.Remove(PublicAttribute.LocalFilePath.Length - 1) + item.path + "/").FirstOrDefault();

        var item = JsonClass.Instance.ComponentList.SingleOrDefault(t => t.id == id);
        for (int i = 0; i < item.paths.Count; i++)
        {
            //Debug.Log(id +    "父文件夹有：  "+ item.paths[i]);
            if (!item.paths[i].Contains(PublicAttribute.LocalFilePath))
            {
                //item.paths[i] = PublicAttribute.LocalFilePath.Remove(PublicAttribute.LocalFilePath.Length - 1) + item.paths[i];
                item.paths[i] = PublicAttribute.LocalFilePath + item.paths[i];
            }
        }
        return item.paths;
    }

    /// <summary>
    /// 通过ID获取版本号
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private string GetVersionByID(int id)
    {
        string path = FindFolder(id)[0];
        string version = "0.0.0";
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
       // Debug.Log(id + "    的路径为   " + path + @"/version.json");
        if (File.Exists(path + @"\version.json"))
        {
            version = File.ReadAllText(path + @"/version.json");
        }
       // Debug.Log(path + "    version.json" + "  " + version);
        return version;
    }

    /// <summary>
    /// 通过读取文件夹内的文件 获取版本号
    /// </summary>
    /// <param name="path">G:/Work/AR_Foregoer/code/ar_travel_preview/Assets/DownloadFile/File/File/lovers/cloud/version.json</param>
    /// <returns></returns>
    private string GetVersionByPath(string path)
    {
        string version = "0.0.0";
        if (File.Exists(path))
        {
            version = File.ReadAllText(path);
        }
        return version;
    }

    /// <summary>
    /// 解析FileSItem类
    /// </summary>
    /// <param name="itemPreview"></param>
    /// <returns></returns>
    private FilesItem ParseFilesItem(JsonData itemPreview)
    {
        FilesItem fi = new FilesItem();
        fi.originalName = itemPreview["originalName"].ToString();
        fi.url = itemPreview["url"].ToString();
        var temp = itemPreview["paths"];

        if (temp.Count > 0)
        {
            for (int i = 0; i < temp.Count; i++)
            {
                fi.paths.Add(temp[i].ToString().Substring(1));
            }
        }
        fi.op = itemPreview["op"].ToString();
        fi.md5 = itemPreview["md5"].ToString();
        fi.size = itemPreview["size"].ToString();
        return fi;
    }

    /// <summary>
    /// 解析道具类的文件
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private ComponentFileItem ParseComponentFileItem(JsonData item)
    {
        ComponentFileItem cfi = new ComponentFileItem
        {
            name = item["name"].ToString(),
            suffix = item["suffix"].ToString(),
            path = item["path"].ToString(),
            size = item["size"].ToString(),
            md5 = item["md5"].ToString()
        };
        return cfi;
    }

    /// <summary>
    /// 解析CategoryComponentItem类
    /// </summary>
    /// <param name="item">json文件 </param>
    /// <param name="CI"></param>
    private void ParseCateCom(JsonData item, CategoryComponentItem CI)
    {
        var pids = item["pids"];
        if (pids.Count > 0)
        {
            for (int j = 0; j < pids.Count; j++)
            {
                var pid = int.Parse(pids[j].ToString());
                CI.pids.Add(pid);
            }
        }
        CI.id = int.Parse(item["id"].ToString());
        CI.name = item["name"].ToString();
        var paths = item["paths"];
        if (paths.Count > 0)
        {
            for (int j = 0; j < paths.Count; j++)
            {
                CI.paths.Add(paths[j].ToString().Substring(1));
            }
        }

        if (item["des"] != null)
        {
            CI.des = item["des"].ToString();
        }
        var previews = item["previews"];
        if (previews.Count > 0)
        {
            for (int j = 0; j < previews.Count; j++)
            {
                CI.previews.Add(ParseComponentFileItem(previews[j]));
            }
        }
        if (item["version"] != null)
        {
            CI.version = item["version"].ToString();
        }
    }

    /// <summary>
    /// 获取缩略图、引导页资源  判断是否需要更新
    /// </summary>
    /// <param name="port">接口</param>
    /// <param name="id">资源ID</param>
    /// <param name="previewSavePath">本地路径</param>
    //private void PreviewSavePath(string port, string id, string previewSavePath, GUID guid = null)
    //{
    //    string size;

    //    if (guid != null)
    //    {
    //        previewSavePath = previewSavePath + guid.pageIndex + "/";
    //    }
    //    else
    //    {
    //        previewSavePath = previewSavePath + id + "/";
    //    }

    //    //Debug.LogError("缩略图资源链接 +   " + port + id);

    //    HttpBase.GET(port + id, (request, response) =>
    //    {

    //        if (response == null || !response.IsSuccess)
    //        {
    //            DebugManager.Instance.LogError("请求失败！");
    //            return;
    //        }
    //        JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
    //        if (content["codeMsg"].ToString() != "SUCCESS")
    //        {
    //            return;
    //        }
    //        if (content["data"] == null)
    //        {
    //            return;
    //        }
    //        JsonData date = content["data"];
    //        if (date != null && date.Count > 0)
    //        {
    //            for (int i = 0; i < date.Count; i++)
    //            {
    //                ComponentFileItem cfi = ParseComponentFileItem(date[i]);
    //                if (guid != null)
    //                {
    //                    guid.CFIS.Add(cfi);
    //                }
    //                previewSavePath = Path.GetDirectoryName(previewSavePath);
    //                previewSavePath = previewSavePath + "/" + cfi.name;
                    
    //                //Debug.LogError("缩略图资源 +   "  + previewSavePath);

    //                if (!Directory.Exists(Path.GetDirectoryName(previewSavePath)))
    //                {
    //                    Directory.CreateDirectory(Path.GetDirectoryName(previewSavePath));
    //                }
    //                if (File.Exists(previewSavePath))
    //                {
    //                    size = new FileInfo(previewSavePath).Length.ToString();
    //                    if (size == cfi.size)
    //                    {
    //                        //不需要下载‘’
    //                        Debug.Log("不需要下载缩略资源");
    //                    }
    //                    else
    //                    {
    //                        Debug.Log("本地有  需要更新缩略资源+  " + PortClass.Instance.DownloadPreview + cfi.path);
    //                        File.Delete(previewSavePath);
    //                        previewDownloader.SingleDownload(
    //                            new Downloader.DownloadUnit(PortClass.Instance.DownloadPreview + cfi.path, previewSavePath,
    //                                cfi.name, cfi.md5), ((
    //                                    l, l1, arg3) =>
    //                                {
    //                                }), (unit
    //                                    =>
    //                                {
    //                                }));
    //                    }
    //                }
    //                else
    //                {
    //                    Debug.Log("本地没有  需要下载新的缩略资源+  " + PortClass.Instance.DownloadPreview + cfi.path + "     本地保存路径为   " +
    //                              previewSavePath);
    //                    //下载文件
    //                    previewDownloader.SingleDownload(
    //                        new Downloader.DownloadUnit(PortClass.Instance.DownloadPreview + cfi.path,
    //                            Path.GetDirectoryName(previewSavePath), cfi.name, cfi.md5), ((
    //                                l, l1, arg3) =>
    //                            {
    //                            }), (unit
    //                                =>
    //                            {
    //                            }));
    //                }
    //            }
    //        }
    //    });
    //}

    #endregion 私有方法
}