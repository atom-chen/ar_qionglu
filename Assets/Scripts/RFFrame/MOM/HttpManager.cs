using LitJson;

/**
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName:     HttpManager.cs
 *Author:       若飞
 *Version:      1.0
 *UnityVersion：5.3.2f1
 *Date:         2017-04-27
 *Description:
 *History:
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class HttpManager : Singleton<HttpManager>
{
    #region 初始化工作

    /// <summary>
    /// 动态资源下载进度
    /// </summary>
    public Action<float> DownloadPercent;

    private Downloader previewDownloader;

    protected override void Init()
    {
        base.Init();
        downloader = new Downloader();
        ossDownloader = new Downloader();
        previewDownloader = new Downloader();
        batchDownloader = new Downloader();
    }

    #endregion 初始化工作

    #region 获取静态资源

    /// <summary>
    /// 获取景点信息
    /// </summary>
    public void GetScenicSpotInfo(Action<bool> callback = null)
    {
        HttpBase.GET(PortClass.Instance.ScenicSpot, ((request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                DebugManager.Instance.LogError("请求失败！");
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            Debug.Log(response.DataAsText.Trim());
            JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
            if (content["status"].ToString() != "200")
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            if (content["data"] == null)
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            JsonData date = content["data"];
            if (date != null && date.Count > 0)
            {
                for (int i = 0; i < date.Count; i++)
                {
                    ScenicSpotItem ssi = new ScenicSpotItem();
                    ssi.id = int.Parse(date[i]["id"].ToString());
                    ssi.name = date[i]["name"].ToString();
                    ssi.description = date[i]["description"].ToString();
                    var tempthumb = date[i]["thumbnail"];
                    ssi.thumbnail = new Thumbnail()
                    {
                        id = tempthumb["id"].ToString(),
                        md5 = tempthumb["md5"].ToString(),
                        extName = tempthumb["extName"].ToString(),
                        size = tempthumb["size"].ToString(),
                        endPoint = "vsz-scenery-dot",
                        address = "vsz-scenery-dot" + PublicAttribute.OSSUri + tempthumb["md5"].ToString() + "." + tempthumb["extName"].ToString(),
                        localPath = PublicAttribute.ThumbPath + tempthumb["id"].ToString() + "." + tempthumb["extName"].ToString()
                    };
                    //Download(ssi.thumbnail);
                    ssi.locationX = date[i]["locationX"].ToString();
                    ssi.locationY = date[i]["locationY"].ToString();
                    ssi.height = date[i]["height"].ToString();
                    ssi.sceneryArea = new SceneryArea()
                    {
                        id = int.Parse(date[i]["sceneryArea"]["id"].ToString()),
                        name = date[i]["sceneryArea"]["name"].ToString()
                    };
                    ssi.address = date[i]["address"].ToString();
                    JsonClass.Instance.ScenicSpotInfoS.Add(ssi);
                }
                if (callback != null)
                {
                    callback(true);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 获取主页特色景点信息
    /// </summary>
    public void GetTraitScenicSpotInfo(Action<bool> callback = null)
    {
        HttpBase.GET(PortClass.Instance.TraitScenicSpot, ((request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                if (callback != null)
                {
                    callback(false);
                }
                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            Debug.Log(response.DataAsText.Trim());
            JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
            if (content["status"].ToString() != "200")
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            if (content["data"] == null)
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            JsonData date = content["data"];
            if (date != null && date.Count > 0)
            {
                for (int i = 0; i < date.Count; i++)
                {
                    ScenicSpotItem ssi = new ScenicSpotItem();
                    ssi.id = int.Parse(date[i]["id"].ToString());
                    ssi.name = date[i]["name"].ToString();
                    ssi.description = date[i]["description"].ToString();

                    var tempthumb = date[i]["thumbnail"];
                    ssi.thumbnail = new Thumbnail()
                    {
                        endPoint = "vsz-special-scenery",
                        address = "vsz-special-scenery" + PublicAttribute.OSSUri + tempthumb["md5"].ToString() + "." + tempthumb["extName"].ToString(),

                        id = tempthumb["id"].ToString(),
                        md5 = tempthumb["md5"].ToString(),
                        extName = tempthumb["extName"].ToString(),
                        size = tempthumb["size"].ToString(),
                        localPath = PublicAttribute.ThumbPath + tempthumb["id"].ToString() + "." + tempthumb["extName"].ToString()
                    };
                    //Download(ssi.thumbnail);
                    ssi.locationX = date[i]["locationX"].ToString();
                    ssi.locationY = date[i]["locationY"].ToString();
                    ssi.height = date[i]["height"].ToString();
                    ssi.sceneryArea = new SceneryArea()
                    {
                        id = int.Parse(date[i]["sceneryArea"]["id"].ToString()),
                        name = date[i]["sceneryArea"]["name"].ToString()
                    };
                    ssi.address = date[i]["address"].ToString();
                    JsonClass.Instance.TraitScenicSpotInfoS.Add(ssi);
                }
                if (callback != null)
                {
                    callback(true);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 获取主页的商家信息
    /// </summary>
    public void GetShopSInfo(Action<bool> callback = null)
    {
        HttpBase.GET(PortClass.Instance.ShopsInfo, ((request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                if (callback != null)
                {
                    callback(false);
                }
                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            Debug.Log(response.DataAsText.Trim());
            JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
            if (content["status"].ToString() != "200")
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            if (content["data"] == null)
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            JsonData date = content["data"];
            if (date != null && date.Count > 0)
            {
                for (int i = 0; i < date.Count; i++)
                {
                    ScenicSpotItem ssi = new ScenicSpotItem();
                    ssi.id = int.Parse(date[i]["id"].ToString());
                    ssi.name = date[i]["name"].ToString();
                    ssi.description = date[i]["description"].ToString();

                    var tempthumb = date[i]["thumbnail"];
                    ssi.thumbnail = new Thumbnail()
                    {
                        endPoint = "vsz-business",
                        address = "vsz-business" + PublicAttribute.OSSUri + tempthumb["md5"].ToString() + "." + tempthumb["extName"].ToString(),

                        id = tempthumb["id"].ToString(),
                        md5 = tempthumb["md5"].ToString(),
                        extName = tempthumb["extName"].ToString(),
                        size = tempthumb["size"].ToString(),
                        localPath = PublicAttribute.ThumbPath + tempthumb["id"].ToString() + "." + tempthumb["extName"].ToString()
                    };
                    //Download(ssi.thumbnail);
                    ssi.locationX = date[i]["locationX"].ToString();
                    ssi.locationY = date[i]["locationY"].ToString();
                    ssi.height = date[i]["height"].ToString();
                    ssi.sceneryArea = new SceneryArea()
                    {
                        id = int.Parse(date[i]["sceneryArea"]["id"].ToString()),
                        name = date[i]["sceneryArea"]["name"].ToString()
                    };
                    ssi.address = date[i]["address"].ToString();
                    JsonClass.Instance.ShopInfoS.Add(ssi);
                }
                if (callback != null)
                {
                    callback(true);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 获取主页的特产信息
    /// </summary>
    public void GetLocalSpecialtyInfo(Action<bool> callback = null)
    {
        HttpBase.GET(PortClass.Instance.LocalSpecialty, ((request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                if (callback != null)
                {
                    callback(false);
                }

                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            Debug.Log(response.DataAsText.Trim());
            JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
            if (content["status"].ToString() != "200")
            {
                if (callback != null)
                {
                    callback(false);
                }

                return;
            }
            if (content["data"] == null)
            {
                if (callback != null)
                {
                    callback(false);
                }

                return;
            }
            JsonData date = content["data"];
            if (date != null && date.Count > 0)
            {
                for (int i = 0; i < date.Count; i++)
                {
                    ScenicSpotItem ssi = new ScenicSpotItem();
                    ssi.id = int.Parse(date[i]["id"].ToString());
                    ssi.name = date[i]["name"].ToString();
                    ssi.description = date[i]["description"].ToString();

                    var tempthumb = date[i]["thumbnail"];
                    ssi.thumbnail = new Thumbnail()
                    {
                        endPoint = "vsz-special-product",
                        address = "vsz-special-product" + PublicAttribute.OSSUri + tempthumb["md5"].ToString() + "." + tempthumb["extName"].ToString(),

                        id = tempthumb["id"].ToString(),
                        md5 = tempthumb["md5"].ToString(),
                        extName = tempthumb["extName"].ToString(),
                        size = tempthumb["size"].ToString(),
                        localPath = PublicAttribute.ThumbPath + tempthumb["id"].ToString() + "." + tempthumb["extName"].ToString()
                    };
                    //Download(ssi.thumbnail);
                    ssi.locationX = date[i]["locationX"].ToString();
                    ssi.locationY = date[i]["locationY"].ToString();
                    ssi.height = date[i]["height"].ToString();
                    ssi.sceneryArea = new SceneryArea()
                    {
                        id = int.Parse(date[i]["sceneryArea"]["id"].ToString()),
                        name = date[i]["sceneryArea"]["name"].ToString()
                    };
                    ssi.address = date[i]["address"].ToString();
                    JsonClass.Instance.LocalSpecialtyS.Add(ssi);
                }
                if (callback != null)
                {
                    callback(true);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 获取景区列表信息
    /// </summary>
    public void GetAreaInfo(Action<bool> callback = null)
    {
        Debug.Log(PortClass.Instance.AreaInfo);
        HttpBase.GET(PortClass.Instance.AreaInfo, ((request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                DebugManager.Instance.LogError("请求失败！");
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            Debug.Log("获取景区列表     +      /" + response.DataAsText.Trim());
            JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
            if (content["status"].ToString() != "200")
            {
                if (callback != null)
                {
                    callback(false);
                }

                return;
            }
            if (content["data"] == null)
            {
                if (callback != null)
                {
                    callback(false);
                }

                return;
            }
            JsonData date = content["data"];
            if (date != null && date.Count > 0)
            {
                for (int i = 0; i < date.Count; i++)
                {
                    AreaInfo AI = new AreaInfo()
                    {
                        id = date[i]["id"].ToString(),
                        describe = date[i]["desc"].ToString(),
                        name = date[i]["name"].ToString()
                    };
                    if (!JsonClass.Instance.AreaInfoS.Contains(AI))
                    {
                        JsonClass.Instance.AreaInfoS.Add(AI);
                    }
                }
                if (callback != null)
                {
                    callback(true);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 下载缩略图内容
    /// </summary>
    /// <param name="thumb">缩略图信息</param>
    /// <param name="callback">回调</param>
    public void Download(Thumbnail thumb, Action callback)
    {
        string savePath = thumb.localPath;
        string url = "http://" + thumb.address;
        string size = thumb.size;
        string endpoint = thumb.endPoint;
        string fileName;
        Downloader.OSSFile os = new Downloader.OSSFile(endpoint, thumb.md5 + "." + thumb.extName);
        if (string.IsNullOrEmpty(thumb.id))
        {
            fileName = thumb.md5 + "." + thumb.extName;
        }
        else
        {
            fileName = thumb.id + "." + thumb.extName;
        }
        string md5 = thumb.md5;
        //若不存在该文件的目录str
        //1、创建文件目录
        //2、执行下载流程
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            SingleOSSDownload(savePath, url, os, callback, md5, fileName);
        }
        //若存在该文件目录
        else
        {
            //若存在该文件
            //1、比较本地文件与服务器文件之间的大小
            //2、若相同 不执行下载
            //3、若不相同 执行下载流程
            if (File.Exists(savePath))
            {
                //本地文件大小
                var file = File.OpenRead(savePath);
                if (string.Equals(Utility.GetMd5Hash(file), md5))
                {
                    //Debug.Log("文件存在   " + savePath);
                    callback();
                    return;
                }
                //1、删除本地文件
                //2、下载新的文件
                else
                {
                    File.Delete(savePath);
                    SingleOSSDownload(savePath, url, os, callback, md5, fileName);
                }
            }
            //若不存在该文件
            //1、执行下载流程
            else
            {
                SingleOSSDownload(savePath, url, os, callback, md5, fileName);
            }
        }
    }

    /// <summary>
    /// 获取广告类信息
    /// 1：启动页，2：广告页，3：引导页，4：Banner
    /// </summary>
    /// <param name="callback"></param>

    #endregion 获取静态资源

    #region 获取动态资源

    /// <summary>
    /// 获取所有的动态资源
    /// </summary>
    public void DynamicResources()
    {
        string areaId;
        foreach (var info in JsonClass.Instance.AreaInfoS)
        {
            areaId = info.id;
            DynamicResoucesInfos DIs = new DynamicResoucesInfos();
            PublicAttribute.AreaResoucesDic.Add(areaId, DIs);

            foreach (var RIF in DIs.ResourcesInfos)
            {
                if (RIF.ResourcesKey == "vsz-scenery-guide")
                {
                    Debug.Log(RIF.GetAll + "&areaId=" + areaId);
                }

                RIF.LocalPath = RIF.LocalPath + areaId;
                RIF.JsonLocalPath = RIF.LocalPath + "/" + RIF.ResourcesKey + ".json";
                RIF.UT = UpdateType.none;

                if (!Directory.Exists(Path.GetDirectoryName(RIF.JsonLocalPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(RIF.JsonLocalPath));
                }
                if (!File.Exists(RIF.JsonLocalPath))
                {
                    FileStream fs = new FileStream(RIF.JsonLocalPath, FileMode.OpenOrCreate);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Close();
                }

                Debug.Log("接口   " + RIF.GetAll + "&areaId=" + areaId);
                HttpBase.GET(RIF.GetAll + "&areaId=" + areaId, ((request, response) =>
                {
                    if (response == null || !response.IsSuccess)
                    {
                        DebugManager.Instance.LogError("请求失败！");
                        return;
                    }
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    Debug.Log(response.DataAsText.Trim());
                    if (content["status"].ToString() != "200")
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
                            var item = date[i];
                            DynamicItem pi = new DynamicItem
                            {
                                id = int.Parse(item["id"].ToString()),
                                typeId = item["typeId"].ToString(),
                                typeName = item["typeName"].ToString(),
                                versionName = item["versionName"].ToString(),
                                type = item["type"].ToString()
                            };
                            pi.description = RIF.ResourcesKey == "vsz-more-change" ? item["description"].ToString() : "";
                            var baseEntity = item["baseEntity"];
                            pi.baseEntity = new BaseEntity
                            {
                                id = baseEntity["id"].ToString(),
                                name = baseEntity["name"].ToString()
                            };

                            if (item["pageThumbnail"].ToString() != "")
                            {
                                var pageThumbnail = item["pageThumbnail"];

                                pi.PageThumbnail = new Thumbnail()
                                {
                                    id = pageThumbnail["id"].ToString(),

                                    endPoint = RIF.ResourcesKey,
                                    address = RIF.ResourcesKey + PublicAttribute.OSSUri + pageThumbnail["md5"].ToString() + "." + pageThumbnail["extName"].ToString(),
                                    extName = pageThumbnail["extName"].ToString(),
                                    size = pageThumbnail["size"].ToString(),
                                    localPath = RIF.LocalPath + "/" + pageThumbnail["md5"].ToString() + "." + pageThumbnail["extName"].ToString(),
                                    md5 = pageThumbnail["md5"].ToString()
                                };
                            }

                            var tempthumb = baseEntity["thumbnail"];
                            pi.baseEntity.thumbnail = new Thumbnail()
                            {
                                endPoint = item["type"].ToString(),
                                address = item["type"].ToString() + PublicAttribute.OSSUri + tempthumb["md5"].ToString() + "." + tempthumb["extName"].ToString(),
                                id = tempthumb["id"].ToString(),
                                md5 = tempthumb["md5"].ToString(),
                                extName = tempthumb["extName"].ToString(),
                                size = tempthumb["size"].ToString(),
                                localPath = PublicAttribute.ThumbPath + tempthumb["id"].ToString() + "." + tempthumb["extName"].ToString()
                            };

                            var tempscenery = baseEntity["sceneryArea"];
                            pi.baseEntity.sceneryArea = new SceneryArea()
                            {
                                id = int.Parse(tempscenery["id"].ToString()),
                                name = tempscenery["name"].ToString()
                            };
                            if (!string.IsNullOrEmpty(item["versionFiles"].ToString()))
                            {
                                var versionFiles = item["versionFiles"];
                                if (versionFiles != null && versionFiles.Count > 0)
                                {
                                    for (int j = 0; j < versionFiles.Count; j++)
                                    {
                                        var tempversion = versionFiles[j];
                                        VersionFilesItem vfi = new VersionFilesItem()
                                        {
                                            md5 = tempversion["md5"].ToString(),
                                            filename = tempversion["filename"].ToString(),
                                            extName = tempversion["extName"].ToString(),
                                            size = tempversion["size"].ToString(),
                                            endPoint = RIF.ResourcesKey,
                                            address = RIF.ResourcesKey + PublicAttribute.OSSUri + tempversion["md5"].ToString() + "." + tempversion["extName"].ToString(),
                                            localPath = RIF.LocalPath + "/" + tempversion["filename"].ToString() //+ "." + tempversion["extName"].ToString()
                                        };
                                        pi.VersionFilesItems.Add(vfi);
                                    }
                                }
                            }
                            else
                            {
                                pi.VersionFilesItems = null;
                            }
                            pi.baseEntity.locationX = baseEntity["locationX"].ToString();
                            pi.baseEntity.locationY = baseEntity["locationY"].ToString();
                            pi.baseEntity.height = baseEntity["height"].ToString();
                            //pi.baseEntity.content = baseEntity["content"].ToString();
                            pi.baseEntity.address = PublicAttribute.URL + pi.type + "/nav/detailhtml?id=" + pi.baseEntity.id;
                            RIF.DIS.Add(pi);
                        };
                        //若初次下载文件
                        if (string.IsNullOrEmpty(File.ReadAllText(RIF.JsonLocalPath)))
                        {
                            RIF.UT = UpdateType.full;
                        }
                        else
                        {
                            Debug.Log(RIF.JsonLocalPath);
                            var lpi = JsonMapper.ToObject<LocalPannoramaInfo>(File.ReadAllText(RIF.JsonLocalPath));
                            foreach (var item in from item in lpi.PIS let temp = RIF.DIS.SingleOrDefault(t => t.typeId + "_" + t.baseEntity.id == item.typeId + "_" + item.baseEntity.id) where temp == null select item)
                            {
                                RIF.UT = UpdateType.update;
                            }
                            foreach (var panoramaItem in RIF.DIS)
                            {
                                var lpitemp = lpi.PIS.SingleOrDefault(t => t.typeId + "_" + t.baseEntity.id == panoramaItem.typeId + "_" + panoramaItem.baseEntity.id);
                                if (lpitemp != null)
                                {
                                    if (lpitemp.versionName != panoramaItem.versionName)
                                    {
                                        RIF.UT = UpdateType.update;
                                    }
                                }
                                else
                                {
                                    RIF.UT = UpdateType.update;
                                }
                            }
                        }
                    }
                    response.Dispose();
                }), PublicAttribute.Token);
            }
        }
    }

    /// <summary>
    /// 根据景区ID 更新资源
    /// </summary>
    public void DynamicCheekUpdateByArea(string areaID)
    {
        List<DynamicResourcesInfo> dis = new List<DynamicResourcesInfo>();
        foreach (var resourcesInfo in from info in PublicAttribute.AreaResoucesDic.Where(infose => infose.Key == areaID) from resourcesInfo in info.Value.ResourcesInfos let SPI = new ServerPanoramaInfo() select resourcesInfo)
        {
            Debug.Log(resourcesInfo.UT + "    " + resourcesInfo.ResourcesKey);
            dis.Add(resourcesInfo);
            switch (resourcesInfo.UT)
            {
                case UpdateType.none:
                    break;

                case UpdateType.full:
                    //下载所有的文件，并将信息保存到PublicAttribute.LocalPanoramaJson中
                    foreach (var item in resourcesInfo.DIS)
                    {
                        if (item.VersionFilesItems != null)
                        {
                            foreach (var fileItem in item.VersionFilesItems)
                            {
                                batchlist.Add(new Downloader.DownloadUnit(fileItem.address, Path.GetDirectoryName(fileItem.localPath), fileItem.filename, fileItem.md5, fileItem.size), new Downloader.OSSFile(fileItem.endPoint, fileItem.md5 + "." + fileItem.extName));
                            }
                        }
                        if (item.PageThumbnail != null)
                        {
                            batchlist.Add(new Downloader.DownloadUnit(item.PageThumbnail.address, Path.GetDirectoryName(item.PageThumbnail.localPath), item.PageThumbnail.md5 + "." + item.PageThumbnail.extName, item.PageThumbnail.md5, item.PageThumbnail.size), new Downloader.OSSFile(item.PageThumbnail.endPoint, item.PageThumbnail.md5 + "." + item.PageThumbnail.extName));
                        }
                    }
                    foreach (var filesItem in resourcesInfo.DIS.Where(filesItem => !string.IsNullOrEmpty(filesItem.baseEntity.thumbnail.address)))
                    {
                        var temp = filesItem.baseEntity.thumbnail;
                        batchlist.Add(new Downloader.DownloadUnit(temp.address, Path.GetDirectoryName(temp.localPath).Replace(@"\", "/"), Path.GetFileName(temp.localPath), temp.md5, temp.size), new Downloader.OSSFile(temp.endPoint, temp.md5 + "." + temp.extName));
                    }
                    break;

                case UpdateType.update:
                    var lpi = JsonMapper.ToObject<LocalPannoramaInfo>(File.ReadAllText(resourcesInfo.JsonLocalPath));
                    //若本地json中的全景点 服务器没有 则删除本地数据
                    //使用typeId_baseEntity.id确认同一个景点
                    //进行对比  删除/增加/更新
                    foreach (var item in from item in lpi.PIS let temp = resourcesInfo.DIS.SingleOrDefault(t => t.typeId + "_" + t.baseEntity.id == item.typeId + "_" + item.baseEntity.id) where temp == null select item)
                    {
                        Debug.Log(item.typeId + "_" + item.baseEntity.id);
                        File.Delete(item.baseEntity.thumbnail.localPath);
                        foreach (var itemm in item.VersionFilesItems)
                        {
                            File.Delete(itemm.localPath);
                        }
                    }
                    //实景导览 不需要更新文件 接口中pageThumbnail/versionFiles为空
                    if (resourcesInfo.ResourcesKey != "scenery_guide")
                    {
                        foreach (var panoramaItem in resourcesInfo.DIS)
                        {
                            if (panoramaItem.PageThumbnail != null)
                            {
                                batchlist.Add(new Downloader.DownloadUnit(panoramaItem.PageThumbnail.address, Path.GetDirectoryName(panoramaItem.PageThumbnail.localPath), panoramaItem.PageThumbnail.md5 + "." + panoramaItem.PageThumbnail.extName, panoramaItem.PageThumbnail.md5, panoramaItem.PageThumbnail.size), new Downloader.OSSFile(panoramaItem.PageThumbnail.endPoint, panoramaItem.PageThumbnail.md5 + "." + panoramaItem.PageThumbnail.extName));
                            }
                            var lpitemp = lpi.PIS.SingleOrDefault(t => t.typeId + "_" + t.baseEntity.id == panoramaItem.typeId + "_" + panoramaItem.baseEntity.id);
                            if (lpitemp != null)
                            {
                                if (lpitemp.versionName != panoramaItem.versionName)
                                {
                                    //使用更新接口获取最新资源
                                    DynamicCheckUpdate(lpitemp.versionName, panoramaItem.typeId, panoramaItem.baseEntity.id, resourcesInfo, areaID);
                                }
                            }
                            else
                            {
                                //下载该资源
                                foreach (var item in panoramaItem.VersionFilesItems)
                                {
                                    //Download(item.localPath, item.address, item.size);
                                    batchlist.Add(new Downloader.DownloadUnit(item.address, Path.GetDirectoryName(item.localPath).Replace(@"\", "/"), item.filename, item.md5, item.size), new Downloader.OSSFile(item.endPoint, item.md5 + "." + item.extName));
                                }
                            }
                        }
                        ServerPanoramaInfo SPI = new ServerPanoramaInfo();
                        foreach (var di in dis)
                        {
                            SPI.PIS = di.DIS;
                            File.WriteAllText(di.JsonLocalPath, JsonMapper.ToJson(SPI));
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        //BatchDownloadFile(dis);
        BatchOSSDownloadFile(dis);
    }

    /// <summary>
    /// 动态资源更新
    /// </summary>
    private void DynamicCheckUpdate(string version, string typeId, string staticId, DynamicResourcesInfo DFI, string areaId = null)
    {
        if (string.IsNullOrEmpty(areaId))
        {
            areaId = PublicAttribute.AreaId;
        }
        Debug.Log(DFI.CheckUpdate + version + "&typeId=" + typeId + "&staticId=" + staticId + "&platform=" + PublicAttribute.PlatformInt + "&areaId=1" + "          /DFI.ResourcesKey/          :" + DFI.ResourcesKey);
        HttpBase.GET(DFI.CheckUpdate + version + "&typeId=" + typeId + "&staticId=" + staticId + "&platform=" + PublicAttribute.PlatformInt + "&areaId=" + areaId, ((request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            Debug.Log("动态资源更新       " + response.DataAsText.Trim());
            JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
            if (content["status"].ToString() != "200")
            {
                return;
            }
            if (content["data"] == null)
            {
                return;
            }
            JsonData data = content["data"];
            string jsonName = data["file"].ToString();
            string jsonmd5 = data["md5"].ToString();

            SingleOSSDownload(DFI.JsonLocalPath + jsonName, PublicAttribute.URL + "resource?filename=" + jsonName + "&type=" + DFI.ResourcesKey, new Downloader.OSSFile(DFI.ResourcesKey, jsonName), (()
                   =>
               {
                   var temp = File.ReadAllText(DFI.JsonLocalPath + jsonName);
                   Debug.Log("需要更新的内容          :" + temp);
                   var json = JsonMapper.ToObject(temp);
                   if (json != null & json.Count > 0)
                   {
                       for (int i = 0; i < json.Count; i++)
                       {
                           string filename = json[i]["filename"].ToString();
                           string md5 = json[i]["md5"].ToString();
                           string extName = json[i]["extName"].ToString();
                           string type = json[i]["type"].ToString();
                           string opt = json[i]["opt"].ToString();
                           switch (opt)
                           {
                               case "add":
                                   Loom.QueueOnMainThread((() =>
                                   {
                                       SingleOSSDownload(DFI.LocalPath + md5 + "." + extName, PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=" + DFI.ResourcesKey, new Downloader.OSSFile(DFI.ResourcesKey, md5 + "." + extName), null, null, filename);
                                       //SingleDownload(DFI.LocalPath + md5 + "." + extName, PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=" + DFI.ResourcesKey, null, null, filename);
                                   }));
                                   break;

                               case "delete":
                                   Loom.QueueOnMainThread((() =>
                                   {
                                       Debug.Log("删除文件  " + DFI.LocalPath + filename + "." + extName);
                                       File.Delete(DFI.LocalPath + filename + "." + extName);
                                   }));
                                   break;

                               case "update":
                                   Loom.QueueOnMainThread(() =>
                                   {
                                       File.Delete(DFI.LocalPath + filename + "." + extName);
                                       Debug.Log("删除文件  " + DFI.LocalPath + filename + "." + extName);
                                       SingleOSSDownload(DFI.LocalPath + md5 + "." + extName, PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=" + DFI.ResourcesKey, new Downloader.OSSFile(DFI.ResourcesKey, md5 + "." + extName), null, null, filename);
                                       //SingleDownload(DFI.LocalPath + md5 + "." + extName, PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=" + DFI.ResourcesKey, null, null, filename);
                                   });
                                   break;

                               case "prototype":
                                   break;
                           }
                       }
                   }
               }), jsonmd5);

            //SingleDownload(DFI.JsonLocalPath + jsonName, PublicAttribute.URL + "resource?filename=" + jsonName + "&type=" + DFI.ResourcesKey, (() =>
            //{
            //    var temp = File.ReadAllText(DFI.JsonLocalPath + jsonName);
            //    Debug.Log("需要更新的内容          :" + temp);
            //    var json = JsonMapper.ToObject(temp);
            //    if (json != null & json.Count > 0)
            //    {
            //        for (int i = 0; i < json.Count; i++)
            //        {
            //            string filename = json[i]["filename"].ToString();
            //            string md5 = json[i]["md5"].ToString();
            //            string extName = json[i]["extName"].ToString();
            //            string type = json[i]["type"].ToString();
            //            string opt = json[i]["opt"].ToString();
            //            switch (opt)
            //            {
            //                case "add":
            //                    Loom.QueueOnMainThread((() =>
            //                    {
            //                        SingleDownload(DFI.LocalPath + md5 + "." + extName, PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=" + DFI.ResourcesKey, null, null, filename);
            //                    }));
            //                    break;

            //                case "delete":
            //                    Loom.QueueOnMainThread((() =>
            //                    {
            //                        Debug.Log("删除文件  " + DFI.LocalPath + filename + "." + extName);
            //                        File.Delete(DFI.LocalPath + filename + "." + extName);
            //                    }));
            //                    break;

            //                case "update":
            //                    Loom.QueueOnMainThread(() =>
            //                    {
            //                        File.Delete(DFI.LocalPath + filename + "." + extName);
            //                        Debug.Log("删除文件  " + DFI.LocalPath + filename + "." + extName);
            //                        SingleDownload(DFI.LocalPath + md5 + "." + extName, PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=" + DFI.ResourcesKey, null, null, filename);
            //                    });
            //                    break;

            //                case "prototype":
            //                    break;
            //            }
            //        }
            //    }
            //}), jsonmd5);
            //response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 获取实景导览信息
    /// </summary>
    public void GetNavigationInfo(Action<bool> callback)
    {
        HttpBase.GET(PortClass.Instance.GetAllNavigation, ((request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                DebugManager.Instance.LogError("请求失败！");
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
            Debug.Log(response.DataAsText.Trim());
            if (content["status"].ToString() != "200")
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            if (content["data"] == null)
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            JsonData date = content["data"];
            if (date != null && date.Count > 0)
            {
                for (int i = 0; i < date.Count; i++)
                {
                    var item = date[i];
                    NavigationInfo ni = new NavigationInfo
                    {
                        id = item["id"].ToString(),
                        type = item["type"].ToString()
                    };
                    var baseEntity = item["baseEntity"];
                    ni.baseEntity = new BaseEntity
                    {
                        id = baseEntity["id"].ToString(),
                        name = baseEntity["name"].ToString()
                    };
                    var tempthumb = baseEntity["thumbnail"];
                    ni.baseEntity.thumbnail = new Thumbnail()
                    {
                        id = tempthumb["id"].ToString(),
                        md5 = tempthumb["md5"].ToString(),
                        extName = tempthumb["extName"].ToString(),
                        size = tempthumb["size"].ToString(),
                        endPoint = ni.type,
                        address = ni.type + PublicAttribute.OSSUri + tempthumb["md5"].ToString() + "." + tempthumb["extName"].ToString(),
                        localPath = PublicAttribute.ThumbPath + tempthumb["id"].ToString() + "." + tempthumb["extName"].ToString()
                    };

                    var tempscenery = baseEntity["sceneryArea"];
                    ni.baseEntity.sceneryArea = new SceneryArea()
                    {
                        id = int.Parse(tempscenery["id"].ToString()),
                        name = tempscenery["name"].ToString()
                    };
                    ni.baseEntity.locationX = baseEntity["locationX"].ToString();
                    ni.baseEntity.locationY = baseEntity["locationY"].ToString();
                    ni.baseEntity.height = baseEntity["height"].ToString();
                    ni.baseEntity.address = PublicAttribute.URL + ni.type + "/nav/detailhtml?id=" + ni.baseEntity.id;
                    if (!JsonClass.Instance.NavigationInfos.Contains(ni))
                    {
                        JsonClass.Instance.NavigationInfos.Add(ni);
                    }
                };
            }
            if (callback != null)
            {
                callback(true);
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 获取启动页
    /// </summary>
    /// <param name="callback"></param>
    public void GetLaunch(Action<bool> callback = null)
    {
        GetAdsItem("1", JsonClass.Instance.LaunchPages, callback);
    }

    /// <summary>
    /// 获取广告页
    /// </summary>
    /// <param name="callback"></param>
    public void GetAds(Action<bool> callback = null)
    {
        GetAdsItem("2", JsonClass.Instance.AdsPages, callback);
    }

    /// <summary>
    /// 获取引导页
    /// </summary>
    /// <param name="callback"></param>
    public void GetGuids(Action<bool> callback = null)
    {
        GetAdsItem("3", JsonClass.Instance.GuidPages, callback);
    }

    /// <summary>
    /// 获取banner页
    /// </summary>
    /// <param name="callback"></param>
    public void GetBanner(Action<bool> callback = null)
    {
        GetAdsItem("4", JsonClass.Instance.BannerPages, callback);
    }


    #endregion 获取动态资源

    #region 与登陆相关的所有接口调用

    /// <summary>
    /// 下载图片
    /// </summary>
    /// <param name="url">路径</param>
    /// <param name="fileName">保存名称</param>
    /// <param name="savaPath">保存路径</param>
    /// <param name="callBack">返回byte[]</param>
    public void DownloadTexture(string url, string fileName, string savaPath, Action<byte[]> callBack)
    {
        HttpBase.Download(url, ((request, downloaded, length) =>
        {
        }), ((request, response) =>
        {
            if (response.IsSuccess)
            {
                FileStream fs = new FileStream(savaPath + fileName, FileMode.Create);
                fs.Write(response.Data, 0, response.Data.Length);
                fs.Flush();
                fs.Close();
                if (callBack != null)
                {
                    callBack(response.Data);
                }
            }
            response.Dispose();
        }));
    }

    /// <summary>
    /// 使用帐号密码登陆
    /// </summary>
    /// <param name="userName">帐号</param>
    /// <param name="pwd">密码</param>
    public void Login_UserPwd(string userName, string pwd, Action<string> callback)
    {
        HttpBase.POST(PortClass.Instance.UserPwdLogin, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("username", userName), new KeyValuePair<string, string>("password", pwd),
        }, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    if (response.DataAsText.Trim().Contains("1010"))
                    {
                        PublicAttribute.Token = "VSZ " + content["data"].ToString();
                    }
                    callback(content["status"].ToString());
                }
            }
            else
            {
                if (callback != null)
                {
                    callback("Error");
                }
            }
            response.Dispose();
        }));
    }

    /// <summary>
    /// 使用短信验证码登陆
    /// </summary>
    /// <param name="phoneNo"></param>
    /// <param name="smsCode"></param>
    /// <param name="callback"></param>
    public void Login_SMSS(string phoneNo, string smsCode, Action<string> callback)
    {
        HttpBase.POST(PortClass.Instance.SMSLogin, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("telephone", phoneNo), new KeyValuePair<string, string>("code", smsCode),
        }, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    if (response.DataAsText.Trim().Contains("1010"))
                    {
                        PublicAttribute.Token = "VSZ " + content["data"].ToString();
                    }
                    callback(content["status"].ToString());
                }
            }
            else
            {
                if (callback != null)
                {
                    callback("Error");
                }
            }
            response.Dispose();
        }));
    }

    /// <summary>
    /// 获取短信验证码
    /// </summary>
    /// <param name="phoneNo"></param>
    /// <param name="callback"></param>
    public void GetSMSS(string phoneNo, Action<string> callback)
    {
        HttpBase.GET(PortClass.Instance.GetSMSS + phoneNo, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    callback(content["status"].ToString());
                }
            }
            else
            {
                if (callback != null)
                {
                    callback("Error");
                }
            }
            response.Dispose();
        }));
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="pwd"></param>
    /// <param name="smsCode"></param>
    /// <param name="callback"></param>
    public void Register(string userName, string pwd, string smsCode, Action<string> callback)
    {
        HttpBase.POST(PortClass.Instance.Register, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("username", userName), new KeyValuePair<string, string>("password", pwd), new KeyValuePair<string, string>("code", smsCode),
        }, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    callback(content["status"].ToString());
                }
            }
            else
            {
                if (callback != null)
                {
                    callback("Error");
                }
            }
            response.Dispose();
        }));
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="userName">用户名 即是手机号</param>
    /// <param name="newPwd">新的密码</param>
    /// <param name="smsCode">短信验证码</param>
    /// <param name="callback"></param>
    public void ResetPwd(string userName, string newPwd, string smsCode, Action<string> callback)
    {
        HttpBase.POST(PortClass.Instance.ResetPwd, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("username", userName), new KeyValuePair<string, string>("password", newPwd), new KeyValuePair<string, string>("code", smsCode),
        }, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    callback(content["status"].ToString());
                }
            }
            else
            {
                if (callback != null)
                {
                    callback("Error");
                }
            }
            response.Dispose();
        }));
    }

    /// <summary>
    /// 第三方登录
    /// 1、未绑定手机号 1013
    /// 2、登录成功 1010
    /// 3、登录失败v
    /// </summary>
    /// <param name="openID"></param>
    /// <param name="callback"></param>
    public void ThirdPartyLogin(string openID, Action<int> callback)
    {
        HttpBase.POST(PortClass.Instance.ThirdPartyLogin, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("openId", openID)
        }, (request, response) =>
        {
            if (response == null)
            {
                if (callback != null)
                {
                    callback(3);
                }
                return;
            }
            if (response.IsSuccess)
            {
                if (response.DataAsText.Trim().Contains("1013"))
                {
                    //未绑定手机号
                    callback(1);
                }
                else if (response.DataAsText.Trim().Contains("1010"))
                {
                    //登录成功 ，返回Token
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    PublicAttribute.Token = "VSZ " + content["data"].ToString();
                    callback(2);
                }
                else
                {
                    callback(3);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(3);
                }
            }
            response.Dispose();
        });
    }

    /// <summary>
    /// 用于第三方登陆后绑定手机号
    /// </summary>
    /// <param name="phoneNo">手机号</param>
    /// <param name="smsCode">验证码</param>
    /// <param name="thirdCode">第三方的OpenID</param>
    /// <param name="userIcon">用户头像 </param>
    /// <param name="userName">用户昵称</param>
    public void BindingPhoneNo(string phoneNo, string smsCode, string thirdCode, byte[] userIcon, string userName, Action<string> callback)
    {
        HttpBase.UploadUserInfo(PortClass.Instance.BinDingPhoneNo, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("username", phoneNo),
            new KeyValuePair<string, string>("code", smsCode),
            new KeyValuePair<string, string>("thirdCode", thirdCode),
            new KeyValuePair<string, string>("nickName", userName),
        }, userIcon, "UserIcon.png", ((request, response) =>
          {
              if (response == null)
              {
                  if (callback != null)
                  {
                      callback("Error");
                  }
                  return;
              }
              if (response.IsSuccess)
              {
                  if (response.DataAsText.Trim().Contains("1010"))
                  {
                      JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                      PublicAttribute.Token = "VSZ " + content["data"].ToString();
                  }
                  if (callback != null)
                  {
                      JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                      callback(content["status"].ToString());
                  }
              }
              else
              {
                  if (callback != null)
                  {
                      callback("Error");
                  }
              }
              response.Dispose();
          }));
    }
    /// <summary>
    /// 换绑手机号
    /// </summary>
    /// <param name="phoneNo"></param>
    /// <param name="smsCode"></param>
    /// <param name="thirdCode"></param>
    /// <param name="userIcon"></param>
    /// <param name="userName"></param>
    /// <param name="callback"></param>
    public void ChangePhoneNo(string phoneNo, string smsCode, Action<string> callback)
    {
        Debug.Log(phoneNo +"  "+ smsCode);
        HttpBase.POST(PortClass.Instance.PhoChange, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("telephone", phoneNo), new KeyValuePair<string, string>("code", smsCode),
        }, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    if (response.DataAsText.Trim().Contains("1010"))
                    {
                        PublicAttribute.Token = "VSZ " + content["data"].ToString();
                    }
                    callback(content["status"].ToString());
                }
            }
            else
            {
                if (callback != null)
                {
                    callback("Error");
                }
            }
            response.Dispose();
        }),PublicAttribute.Token);
    }
    /// <summary>
    /// 通过token获取用户信息
    /// </summary>
    /// <param name="token"></param>
    /// <param name="callback"></param>
    public void GetUserInfoByToken(Action<bool> callback)
    {
        HttpBase.GET(PortClass.Instance.UserInfoByToken, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    if (content["status"].ToString() != "200")
                    {
                        if (callback != null)
                        {
                            callback(false);
                        }
                        return;
                    }
                    if (content["data"] == null)
                    {
                        if (callback != null)
                        {
                            callback(false);
                        }
                        return;
                    }
                    Debug.Log(response.DataAsText.Trim().ToString());
                    JsonData date = content["data"];
                    string phoneNo = date["username"].ToString();
                    string userName = date["userExtDto"]["userNickName"].ToString();
                    Debug.Log(phoneNo);
                    var userHeadPhoto = date["userExtDto"]["userHeadPhoto"];

                    string downloadUrl = PublicAttribute.URL + "resource?filename=" + userHeadPhoto["md5"].ToString() + "." +
                                         userHeadPhoto["extName"].ToString() + "&type=user_photo&token=" + PublicAttribute.Token;
                    Debug.Log(downloadUrl);
                    HttpBase.Download(downloadUrl, ((originalRequest, downloaded, length) => { }), ((originalRequest,
                           httpResponse) =>
                       {
                           if (httpResponse.IsSuccess)
                           {
                               Texture2D tex = httpResponse.DataAsTexture2D;

                               FileStream fs = new FileStream(PublicAttribute.LocalFilePath + "APP/icon.png", FileMode.Create);
                               fs.Write(httpResponse.Data, 0, httpResponse.Data.Length);
                               fs.Flush();
                               fs.Close();

                               Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0f, 0f));
                               PublicAttribute.UserInfo = new UserInfo()
                               {
                                   PhoneNo = phoneNo,
                                   NickName = userName,
                                   UserIcon = sprite,
                               };
                               if (callback != null)
                               {
                                   callback(true);
                               }
                               return;
                           }
                           httpResponse.Dispose();
                       }));
                }
                else
                {
                    if (callback != null)
                    {
                        callback(false);
                    }
                }
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    /// <param name="callback"></param>
    public void Logout(Action<bool> callback)
    {
        HttpBase.POST(PortClass.Instance.Logout, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("token", PublicAttribute.Token),
        }, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    callback(response.DataAsText.Trim().Contains("200"));
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 反馈
    /// </summary>
    public void Suggest(string message, Action<bool> callback)
    {
        HttpBase.POST(PortClass.Instance.suggest, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("content", message),
        }, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    callback(true);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 修改用户的昵称
    /// </summary>
    public void ModifiUserNickName(string nickName, Action<bool> callback)
    {
        HttpBase.POST(PortClass.Instance.ModifiNickName, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("name", nickName),
        }, ((request, response) =>
        {
            if (response.IsSuccess)
            {
                if (callback != null)
                {
                    callback(response.DataAsText.Trim().Contains("200"));
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            response.Dispose();
        }), PublicAttribute.Token);
    }

    /// <summary>
    /// 修改用户头像
    /// </summary>
    /// <param name="iconPath">本地头像路径</param>
    /// <param name="callback"></param>
    public void ModifiUserIcon(string iconPath, Action<bool> callback)
    {
        HttpBase.Download(iconPath, ((request, downloaded, length) =>
        {
        }), ((request, response) =>
        {
            if (response.IsSuccess)
            {
                HttpBase.ModifiUserIcon(PortClass.Instance.ModifiUserIcon, response.Data, Path.GetFileName(iconPath), ((
                         originalRequest, httpResponse) =>
                     {
                         Debug.Log(httpResponse.DataAsText.Trim().ToString());
                         if (httpResponse.IsSuccess)
                         {
                             if (callback != null)
                             {
                                 callback(httpResponse.DataAsText.Trim().Contains("200"));
                             }
                         }
                         else
                         {
                             if (callback != null)
                             {
                                 callback(false);
                             }
                         }
                         httpResponse.Dispose();
                     }), PublicAttribute.Token);
            }
            response.Dispose();
        }
        ));
    }

    #endregion 与登陆相关的所有接口调用

    #region 到此一游相关的所有接口

    /// <summary>
    /// 获取到此一游的分类列表
    /// </summary>
    public void Visit_GetType(Action<bool> callback)
    {
        HttpBase.GET(PortClass.Instance.Tohere_TypeList, ((request, response) =>
         {
             if (response.IsSuccess)
             {
                 Debug.Log("获取到到此一游的分类");
                 JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                 if (content["status"].ToString() != "200")
                 {
                     if (callback != null)
                     {
                         callback(false);
                     }
                     return;
                 }
                 if (content["data"] == null)
                 {
                     if (callback != null)
                     {
                         callback(false);
                     }
                     return;
                 }
                 JsonData date = content["data"];
                 if (date != null && date.Count > 0)
                 {
                     for (int i = 0; i < date.Count; i++)
                     {
                         string id = date[i]["id"].ToString();
                         string name = date[i]["name"].ToString();
                         if (!JsonClass.Instance.VisitTypeList.ContainsKey(id))
                         {
                             JsonClass.Instance.VisitTypeList.Add(id, name);
                         }
                     }
                     if (callback != null)
                     {
                         callback(true);
                     }
                 }
             }
             response.Dispose();
         }), PublicAttribute.Token);
    }

    /// <summary>
    /// 获取到此一游的所有道具
    /// </summary>
    public void Visit_GetAll(Action<bool> callback)
    {
        Debug.Log(PortClass.Instance.Tohere_GetAll + "platform=" + PublicAttribute.PlatformInt + "&areaId=" + PublicAttribute.AreaId);

        HttpBase.GET(PortClass.Instance.Tohere_GetAll + "platform=" + PublicAttribute.PlatformInt + "&areaId=" + PublicAttribute.AreaId, ((request, response) =>
                 {
                     if (response == null || !response.IsSuccess)
                     {
                         DebugManager.Instance.LogError("请求失败！");
                         if (callback != null)
                         {
                             callback(false);
                         }
                         return;
                     }
                     Debug.Log("获取所有的道具       " + response.DataAsText.Trim());
                     JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                     if (content["status"].ToString() != "200")
                     {
                         if (callback != null)
                         {
                             callback(false);
                         }

                         return;
                     }
                     if (content["data"] == null)
                     {
                         if (callback != null)
                         {
                             callback(false);
                         }

                         return;
                     }
                     JsonData item = content["data"];
                     if (item != null && item.Count > 0)
                     {
                         for (int i = 0; i < item.Count; i++)
                         {
                             VisitInfo vi = new VisitInfo()
                             {
                                 id = item[i]["id"].ToString(),
                                 description = item[i]["description"].ToString(),
                                 name = item[i]["name"].ToString(),
                                 typeIds = item[i]["typeIds"].ToString(),
                                 versionName = item[i]["versionName"].ToString(),
                             };
                             var thumbnail = item[i]["thumbnail"];
                             vi.Thumbnail = new Thumbnail()
                             {
                                 endPoint = "vsz-to-here",
                                 address = "vsz-to-here" + PublicAttribute.OSSUri + thumbnail["md5"].ToString() + "." + thumbnail["extName"].ToString(),
                                 id = thumbnail["id"].ToString(),
                                 extName = thumbnail["extName"].ToString(),
                                 size = thumbnail["size"].ToString(),
                                 localPath = PublicAttribute.VisitPath + thumbnail["md5"].ToString() + "." + thumbnail["extName"].ToString(),
                                 md5 = thumbnail["md5"].ToString()
                             };
                             var pageThumbnail = item[i]["pageThumbnail"];
                             vi.PageThumbnail = new Thumbnail()
                             {
                                 endPoint = "vsz-to-here",
                                 address = "vsz-to-here" + PublicAttribute.OSSUri + pageThumbnail["md5"].ToString() + "." + pageThumbnail["extName"].ToString(),

                                 id = pageThumbnail["id"].ToString(),
                                 extName = pageThumbnail["extName"].ToString(),
                                 size = pageThumbnail["size"].ToString(),
                                 localPath = PublicAttribute.VisitPath + pageThumbnail["md5"].ToString() + "." + pageThumbnail["extName"].ToString(),
                                 md5 = pageThumbnail["md5"].ToString()
                             };
                             var versionFiles = item[i]["versionFiles"];
                             for (int j = 0; j < versionFiles.Count; j++)
                             {
                                 var tempversion = versionFiles[j];
                                 VersionFilesItem vfi = new VersionFilesItem()
                                 {
                                     endPoint = "vsz-to-here",
                                     address = "vsz-to-here" + PublicAttribute.OSSUri + tempversion["md5"].ToString() + "." + tempversion["extName"].ToString(),

                                     md5 = tempversion["md5"].ToString(),
                                     filename = tempversion["filename"].ToString(),
                                     extName = tempversion["extName"].ToString(),
                                     size = tempversion["size"].ToString(),
                                     //address = PublicAttribute.URL + "resource?filename=" + tempversion["md5"] + "." + tempversion["extName"] + "&type=to_here",
                                     localPath = PublicAttribute.VisitPath + tempversion["filename"].ToString() //+ "." + tempversion["extName"].ToString()
                                 };
                                 vi.VersionFilesItems.Add(vfi);
                             }

                             string visitjson = PublicAttribute.VisitPath + vi.name + ".json";
                             if (!File.Exists(visitjson))
                             {
                                 FileStream fs = File.Create(visitjson);
                                 fs.Close();
                             }
                             if (string.IsNullOrEmpty(File.ReadAllText(visitjson)))
                             {
                                 vi.UT = UpdateType.full;
                             }
                             else
                             {
                                 //Debug.Log(visitjson);
                                 var localVersion = File.ReadAllText(visitjson);
                                 if (string.Equals(vi.versionName, localVersion))
                                 {
                                     vi.UT = UpdateType.none;
                                 }
                                 else
                                 {
                                     vi.UT = UpdateType.update;
                                 }
                             }
                             if (!JsonClass.Instance.VisitInfoS.Contains(vi))
                             {
                                 JsonClass.Instance.VisitInfoS.Add(vi);
                             }
                         }
                     }
                     if (callback != null)
                     {
                         callback(true);
                     }
                 }), PublicAttribute.Token);
    }

    /// <summary>
    /// 下载道具
    /// </summary>
    /// <param name="name">道具名称</param>
    /// <param name="percent">下载进度</param>
    public void Visit_Download(string name, Action<float> percent)
    {
        var vi = JsonClass.Instance.VisitInfoS.Find(t => t.name == name);
        //List<Downloader.DownloadUnit> _visitDownloadList = new List<Downloader.DownloadUnit>();
        string localVserion = File.ReadAllText(PublicAttribute.VisitPath + name + ".json");

        switch (vi.UT)
        {
            case UpdateType.none:
                Debug.Log("不需要更新道具:  " + name);
                break;

            case UpdateType.full:
                Debug.Log("本地没有道具:  " + name);

                foreach (var item in vi.VersionFilesItems)
                {
                    VisitBatchlist.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + item.md5 + "." + item.extName + "&type=vsz-to-here", PublicAttribute.VisitPath, item.filename, item.md5, item.size),new Downloader.OSSFile("vsz-to-here",item.md5+"."+item.extName));
                    //_visitDownloadList.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + item.md5 + "." + item.extName + "&type=vsz-to-here", PublicAttribute.VisitPath, item.filename, item.md5, item.size));
                }
                VisitBatchlist.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + vi.PageThumbnail.md5 + "." + vi.PageThumbnail.extName + "&type=vsz-to-here", PublicAttribute.VisitPath, vi.PageThumbnail.md5 + "." + vi.PageThumbnail.extName, vi.PageThumbnail.md5, vi.PageThumbnail.size), new Downloader.OSSFile("vsz-to-here", vi.PageThumbnail.md5 + "." + vi.PageThumbnail.extName));
                //_visitDownloadList.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + vi.PageThumbnail.md5 + "." + vi.PageThumbnail.extName + "&type=to_here", PublicAttribute.VisitPath, vi.PageThumbnail.md5 + "." + vi.PageThumbnail.extName, vi.PageThumbnail.md5, vi.PageThumbnail.size));
                VisitBatchDownloadFile(name, vi.versionName, percent);
                break;

            case UpdateType.update:
                Debug.Log("更新到此一游的道具    " + PortClass.Instance.Tohere_Check + "version=" + localVserion + "&name=" + name + "&platform=" + PublicAttribute.PlatformInt + "&areaId=1");
                HttpBase.GET(PortClass.Instance.Tohere_Check + "version=" + localVserion + "&name=" + name + "&platform=" + PublicAttribute.PlatformInt + "&areaId=1", ((request, response) =>
                {
                    if (response == null || !response.IsSuccess)
                    {
                        DebugManager.Instance.LogError("请求失败！");
                        return;
                    }
                    Debug.Log("到此一游更新       " + response.DataAsText.Trim());
                    JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
                    if (content["status"].ToString() != "200")
                    {
                        return;
                    }
                    if (content["data"] == null)
                    {
                        return;
                    }
                    JsonData data = content["data"];
                    string jsonName = data["file"].ToString();
                    string jsonmd5 = data["md5"].ToString();

                    if (string.IsNullOrEmpty(jsonName))
                    {
                        percent(1);
                        return;
                    }


                    SingleOSSDownload(PublicAttribute.VisitPath + jsonName, PublicAttribute.URL + "resource?filename=" + jsonName + "&type=vsz-to-here",new Downloader.OSSFile("vsz-to-here", jsonName),(()
                        =>
                    {
                        Loom.QueueOnMainThread((() =>
                        {
                            string temp = File.ReadAllText(PublicAttribute.VisitPath + jsonName);
                            Debug.Log("到此一游需要更新的内容          :" + temp);
                            var json = JsonMapper.ToObject(temp);
                            if (json != null & json.Count > 0)
                            {
                                for (int i = 0; i < json.Count; i++)
                                {
                                    string filename = json[i]["filename"].ToString();
                                    string md5 = json[i]["md5"].ToString();
                                    string extName = json[i]["extName"].ToString();
                                    string type = json[i]["type"].ToString();
                                    string opt = json[i]["opt"].ToString();
                                    string size = json[i]["size"].ToString();
                                    switch (opt)
                                    {
                                        case "add":
                                            Debug.Log("新增文件   " + filename);
                                            VisitBatchlist.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=vsz-to-here", PublicAttribute.VisitPath, filename, md5, size), new Downloader.OSSFile("vsz-to-here", md5 + "." + extName));
                                            //_visitDownloadList.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=vsz-to-here", PublicAttribute.VisitPath, filename, md5, size));
                                            break;

                                        case "delete":
                                            Debug.Log("删除文件  " + PublicAttribute.VisitPath + filename);
                                            File.Delete(PublicAttribute.VisitPath + filename);
                                            break;

                                        case "update":
                                            Debug.Log("更新文件   " + filename);
                                            File.Delete(PublicAttribute.VisitPath + filename + "." + extName);
                                            Debug.Log("删除文件  " + PublicAttribute.VisitPath + filename + "." + extName);
                                            VisitBatchlist.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=vsz-to-here", PublicAttribute.VisitPath, filename, md5, size), new Downloader.OSSFile("vsz-to-here", md5 + "." + extName));
                                            //_visitDownloadList.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=vsz-to-here", PublicAttribute.VisitPath, filename, md5, size));
                                            break;

                                        case "prototype":
                                            break;
                                    }
                                }
                            }
                            VisitBatchDownloadFile(name, vi.versionName, percent);
                        }));
                    }), jsonmd5);
                    response.Dispose();
                }), PublicAttribute.Token);

                //SingleDownload(PublicAttribute.VisitPath + jsonName, PublicAttribute.URL + "resource?filename=" + jsonName + "&type=vsz-to-here", (() =>
                //    {
                //        Loom.QueueOnMainThread((() =>
                //        {
                //            string temp = File.ReadAllText(PublicAttribute.VisitPath + jsonName);
                //            Debug.Log("到此一游需要更新的内容          :" + temp);
                //            var json = JsonMapper.ToObject(temp);
                //            if (json != null & json.Count > 0)
                //            {
                //                for (int i = 0; i < json.Count; i++)
                //                {
                //                    string filename = json[i]["filename"].ToString();
                //                    string md5 = json[i]["md5"].ToString();
                //                    string extName = json[i]["extName"].ToString();
                //                    string type = json[i]["type"].ToString();
                //                    string opt = json[i]["opt"].ToString();
                //                    string size = json[i]["size"].ToString();
                //                    switch (opt)
                //                    {
                //                        case "add":
                //                            Debug.Log("新增文件   " + filename);
                //                            VisitBatchlist.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=vsz-to-here", PublicAttribute.VisitPath, filename, md5, size), new Downloader.OSSFile("vsz-to-here", md5 + "." + extName));
                //                            //_visitDownloadList.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=vsz-to-here", PublicAttribute.VisitPath, filename, md5, size));
                //                            break;

                //                        case "delete":
                //                            Debug.Log("删除文件  " + PublicAttribute.VisitPath + filename);
                //                            File.Delete(PublicAttribute.VisitPath + filename);
                //                            break;

                //                        case "update":
                //                            Debug.Log("更新文件   " + filename);
                //                            File.Delete(PublicAttribute.VisitPath + filename + "." + extName);
                //                            Debug.Log("删除文件  " + PublicAttribute.VisitPath + filename + "." + extName);
                //                            VisitBatchlist.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=vsz-to-here", PublicAttribute.VisitPath, filename, md5, size), new Downloader.OSSFile("vsz-to-here", md5 + "." + extName));
                //                            //_visitDownloadList.Add(new Downloader.DownloadUnit(PublicAttribute.URL + "resource?filename=" + md5 + "." + extName + "&type=vsz-to-here", PublicAttribute.VisitPath, filename, md5, size));
                //                            break;

                //                        case "prototype":
                //                            break;
                //                    }
                //                }
                //            }
                //            VisitBatchDownloadFile(name, vi.versionName, percent);
                //        }));
                //    }), jsonmd5);
                //    response.Dispose();
                //}), PublicAttribute.Token);
                break;

            default:
                break;
        }
    }

    #endregion 到此一游相关的所有接口

    #region Private

    /// <summary>
    ///
    /// </summary>
    /// <param name="callback"></param>
    private void GetAdsItem(string id, List<Ads> lists, Action<bool> callback)
    {
        Debug.Log(PortClass.Instance.Advertisement + id);
        HttpBase.GET(PortClass.Instance.Advertisement + id, ((request, response) =>
        {
            if (response == null || !response.IsSuccess)
            {
                if (callback != null)
                {
                    callback(false);
                }
                DebugManager.Instance.LogError("请求失败！");
                return;
            }
            Debug.Log(response.DataAsText.Trim());
            JsonData content = JsonMapper.ToObject(response.DataAsText.Trim());
            if (content["status"].ToString() != "200")
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            if (content["data"] == null)
            {
                if (callback != null)
                {
                    callback(false);
                }
                return;
            }
            JsonData date = content["data"];
            if (date != null && date.Count > 0)
            {
                for (int i = 0; i < date.Count; i++)
                {
                    var item = date[i];
                    Ads ads = new Ads
                    {
                        id = item["id"].ToString(),
                        order = item["order"].ToString(),
                        content = item["content"].ToString(),
                        type = item["type"].ToString(),
                        address = item["address"].ToString()
                    };
                    var tempthumb = item["thumbnail"];
                    ads.Thumbnail = new Thumbnail()
                    {
                        endPoint = "vsz-advertisement",
                        address = "vsz-advertisement" + PublicAttribute.OSSUri + tempthumb["md5"].ToString() + "." + tempthumb["extName"].ToString(),

                        id = tempthumb["id"].ToString(),
                        md5 = tempthumb["md5"].ToString(),
                        extName = tempthumb["extName"].ToString(),
                        size = tempthumb["size"].ToString(),
                        localPath = PublicAttribute.AdsPath + tempthumb["id"].ToString() + "." + tempthumb["extName"].ToString()
                    };
                    lists.Add(ads);
                }
                if (callback != null)
                {
                    callback(true);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            response.Dispose();
        })
        );
    }

    /// <summary>
    /// 单个内容的下载
    /// </summary>
    /// <param name="savePath">文件保存路径 包含了文件名和后缀名</param>
    /// <param name="URL">下载的路径</param>
    /// <param name="md5">文件的MD5值</param>
    private void SingleDownload(string savePath, string URL, Action callback = null, string md5 = null, string fileName = null, string size = null)
    {
        //临时
        //URL +=
        //    "&token=VSZ eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxMzQzODg5NzI4OCIsImNyZWF0ZWQiOjE1MzQzMTQxNDMwOTgsImV4cCI6MTU3MDMxNDE0M30.EhiTf0W8gFb2HKQINu4S9i9GwwR34lu7RMWe5BZ2P_aggYfXgLj2w_wlVqPPfbGTIXq021agQ5OVCOXISi3XAw";
        URL = URL + "&token=" + PublicAttribute.Token;
        if (md5 == null)
        {
            md5 = Path.GetFileNameWithoutExtension(savePath);
        }
        if (fileName == null)
        {
            fileName = Path.GetFileName(savePath);
        }
        savePath = Path.GetDirectoryName(savePath);

        Debug.Log(savePath + "   " + md5 + "   url  " + URL + "        fileName   : " + fileName);
        previewDownloader.SingleDownload(new Downloader.DownloadUnit(URL, savePath, fileName, md5, size), ((crrentSize, endSize, done) =>
         {
             if (done)
             {
                 if (callback != null)
                 {
                     callback();
                 }
             }
         }));
    }

    private Downloader ossDownloader;

    /// <summary>
    /// 在阿里云的OSS服务器上下载单个物体
    /// </summary>
    /// <param name="savePath">本地保存路径</param>
    /// <param name="URL">下载链接</param>
    /// <param name="of">OSS文件</param>
    /// <param name="callback"></param>
    /// <param name="md5"></param>
    /// <param name="fileName"></param>
    /// <param name="size"></param>
    private void SingleOSSDownload(string savePath, string URL, Downloader.OSSFile of, Action callback = null, string md5 = null, string fileName = null, string size = null)
    {
        if (md5 == null)
        {
            md5 = Path.GetFileNameWithoutExtension(savePath);
        }
        if (fileName == null)
        {
            fileName = Path.GetFileName(savePath);
        }
        savePath = Path.GetDirectoryName(savePath);

        //Debug.Log(savePath + "   " + md5 + "   url  " + URL + "        fileName   : " + fileName);

        ossDownloader.OSSdownload(new Downloader.DownloadUnit(URL, savePath, fileName, md5, size), of, (b
            =>
        {
            if (b)
            {
                if (callback != null)
                {
                    callback();
                }
            }
        }));
    }

    #region 动态资源批量下载

    private List<Downloader.DownloadUnit> _downloadList = new List<Downloader.DownloadUnit>();
    private Downloader downloader;

    private Dictionary<Downloader.DownloadUnit, Downloader.OSSFile> batchlist = new Dictionary<Downloader.DownloadUnit, Downloader.OSSFile>();
    private Downloader batchDownloader;

    Dictionary<Downloader.DownloadUnit, Downloader.OSSFile> VisitBatchlist = new Dictionary<Downloader.DownloadUnit, Downloader.OSSFile>();

    /// <summary>
    /// 下载进度
    /// </summary>
    private float _percent;

    /// <summary>
    /// 下载道具中的文件
    /// </summary>
    /// <param name="savePath">版本号保存路径</param>
    /// <param name="version">版本号</param>
    /// <param name="op">当前道具的op</param>
    /// <param name="id">当前道具的id</param>
    //public float DownLoadtotalSize = 0;
    //public float DownLoadcurSize = 0;
    private void BatchDownloadFile(List<DynamicResourcesInfo> dis)
    {
        if (_downloadList.Count == 0)
        {
            //if (DownloadPercent != null)
            //{
            //    DownloadPercent(1);
            //}
            return;
        }
        //Debug.Log("开始批量");
        //for (int i = 0; i < _downloadList.Count; i++)
        //{
        //    var unit = _downloadList[i];
        //    if (File.Exists(unit.FullPath))
        //    {
        //        //本地文件大小
        //        var file = File.OpenRead(unit.FullPath);
        //        var md5 = Utility.GetMd5Hash(file);
        //        Debug.Log("    unit.FullPath： " + unit.FullPath+"     本地md5     "+ md5+"        服务器md5   "+unit.md5);
        //        if (string.Equals(Utility.GetMd5Hash(file), unit.md5))
        //        {
        //            //不执行下载流程
        //            _downloadList.Remove(_downloadList[i]);
        //            Debug.Log("文件已经存在 不需要下载   ：" + unit.FullPath);
        //        }
        //    }
        //    Debug.Log(unit.FullPath + "   " + unit.md5 + "   url  " + unit.downUrl);
        //}
        foreach (var unit in _downloadList)
        {
            unit.downUrl = unit.downUrl + "&token=" + PublicAttribute.Token;
        }
        downloader.BatchDownload(_downloadList, (size, unit, done) =>
        {
            //Debug.Log(_downloadList[0].downUrl);
            _percent = size.TotalLocalSize / (float)size.TotalServerSize;
            //Debug.Log(_percent);
            if (DownloadPercent != null)
            {
                DownloadPercent(_percent);
            }
            //Debug.Log(size.TotalLocalSize / (float)size.TotalServerSize);
            if (size.TotalLocalSize == size.TotalServerSize && done)
            {
                if (DownloadPercent != null)
                {
                    DownloadPercent(1);
                }
                Debug.Log("---------所有文件下载完毕---------");

                //所有下载成功之后将本地版本号 改为 服务器版本
                Loom.QueueOnMainThread(() =>
                {
                    ServerPanoramaInfo SPI = new ServerPanoramaInfo();
                    foreach (var di in dis)
                    {
                        SPI.PIS = di.DIS;
                        //File.WriteAllText(PublicAttribute.LocalPanoramaJson, JsonMapper.ToJson(SPI));
                        File.WriteAllText(di.JsonLocalPath, JsonMapper.ToJson(SPI));
                    }
                });
            }
        }, (error) => { DebugManager.Instance.LogError("下载失败  " + error.fileName); });
    }

    private void BatchOSSDownloadFile(List<DynamicResourcesInfo> dis)
    {
        if (batchlist.Count == 0)
        {
            if (DownloadPercent != null)
            {
                DownloadPercent(1);
            }
            return;
        }
        int count = 0;
        foreach (var file in batchlist)
        {
            file.Key.downUrl = "http://" + file.Key.downUrl;
            //DownLoadtotalSize += float.Parse(file.Key.size);
        }
        batchDownloader.BatchOSSDownload(batchlist, (() =>
         {
             Debug.Log("动态资源下载完成");
             DownloadPercent(1);
             //所有下载成功之后将本地版本号 改为 服务器版本
             Loom.QueueOnMainThread(() =>
             {
                 ServerPanoramaInfo spi = new ServerPanoramaInfo();
                 foreach (var di in dis)
                 {
                     spi.PIS = di.DIS;
                     File.WriteAllText(di.JsonLocalPath, JsonMapper.ToJson(spi));
                 }
             });
         }));
    }

    #endregion 动态资源批量下载

    #region 到此一游资源批量下载

    /// <summary>
    /// 到此一游批量下载
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="version">版本号</param>
    /// <param name="_visitDownloadList"></param>
    /// <param name="Percent"></param>
    private void VisitBatchDownloadFile(string name, string version,Action<float> Percent)
    {
        Debug.Log("批量下载文件");
        Downloader _visitDownloader = new Downloader();
        float _percent;
        if (VisitBatchlist.Count == 0)
        {
            if (Percent != null)
            {
                Percent(1);
            }
            return;
        }
        //foreach (var unit in _visitDownloadList)
        //{
        //    unit.downUrl = unit.downUrl + "&token=" + PublicAttribute.Token;
        //    Debug.Log(unit.savePath);
        //}
        foreach (var file in VisitBatchlist)
        {
            file.Key.downUrl = "http://" + file.Key.downUrl;
        }

        _visitDownloader.BatchOSSDownload(VisitBatchlist,(() =>
        {
            if (Percent != null)
            {
                Percent(1);
            }
            Loom.QueueOnMainThread(() => { File.WriteAllText(PublicAttribute.VisitPath + name + ".json", version); });
        }));

        //_visitDownloader.BatchDownload(_visitDownloadList, (size, unit, done) =>
        //{
        //    //Debug.Log(_downloadList[0].downUrl);
        //    _percent = size.TotalLocalSize / (float)size.TotalServerSize;
        //    //Debug.Log(_percent);
        //    if (Percent != null)
        //    {
        //        Percent(_percent);
        //    }
        //    if (size.TotalLocalSize == size.TotalServerSize && done)
        //    {
        //        if (Percent != null)
        //        {
        //            Percent(_percent);
        //        }
        //        Debug.Log("---------所有文件下载完毕---------");
        //        //所有下载成功之后将本地版本号 改为 服务器版本
        //        Loom.QueueOnMainThread(() => { File.WriteAllText(PublicAttribute.VisitPath + name + ".json", version); });
        //    }
        //}, (error) => { DebugManager.Instance.LogError("下载失败  " + error.fileName); });
    }

    #endregion 到此一游资源批量下载

    #endregion Private
}