using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using ThreadPriority = System.Threading.ThreadPriority;

/// <summary>
/// 断点续传下载器
/// </summary>
public class Downloader
{
    /// <summary>
    /// 下载文件单元
    /// </summary>
    public class DownloadUnit
    {
        public string downUrl;
        public string fileName;
        public string savePath;
        public string md5;
        public string size;
        public string endpoint;

        private string fileSuffix;

        /// <summary>
        /// 后缀名
        /// </summary>
        public string FileSuffix
        {
            get
            {
                fileSuffix = Path.GetExtension(downUrl);
                return fileSuffix;
            }
            set { fileSuffix = value; }
        }

        private string fullPath;

        /// <summary>
        /// 文件保存完整路径+文件名+后缀名
        /// </summary>
        public string FullPath
        {
            get
            {
                if (string.IsNullOrEmpty(fullPath))
                {
                    fullPath = savePath + "/" + fileName;
                }
                return fullPath;
            }
            set { fullPath = value; }
        }

        public DownloadUnit(string url, string path, string fileName, string md5, string size)
        {
            this.downUrl = url;
            this.savePath = path;
            this.fileName = fileName;
            this.md5 = md5;
            this.size = size;
        }
    };

    public class OSSFile
    {
        public string endpoint;
        public string objectName;

        /// <summary>
        /// OSS文件
        /// </summary>
        /// <param name="endpoint">类型</param>
        /// <param name="objectName">物体名称</param>
        public OSSFile(string endpoint, string objectName)
        {
            this.endpoint = endpoint;
            this.objectName = objectName;
        }
    }

    /// <summary>
    /// 下载文件返回的大小
    /// </summary>
    public class Size
    {
        /// <summary>
        /// 当前文件本地大小
        /// </summary>
        public float LocalSize;

        /// <summary>
        /// 当前文件服务器大小
        /// </summary>
        public float ServerSize;

        /// <summary>
        /// 下载列表中所有文件在本地的大小
        /// </summary>
        public float TotalLocalSize;

        /// <summary>
        /// 下载列表中所有文件在服务器的大小
        /// </summary>
        public float TotalServerSize;
    }

    private const int oneReadLen = 32768;            // 一次读取长度 32768 = 32*kb   https://calc.itzmx.com/
    private const int ReadWriteTimeOut = 2 * 1000;  // 超时等待时间
    private const int TimeOutWait = 50 * 1000;       // 超时等待时间
    private const int MaxTryTime = 3;

    /// <summary>
    /// 是否暂停下载
    /// </summary>
    public bool IsStop;

    public bool isStop
    {
        get { return isStop; }
        set
        {
            if (!value)
            {
                // BatchDownload
            }
        }
    }

    /// <summary>
    /// 批量下载  判断完成后 需要支持EasyThread.Stop
    /// 开线程
    /// </summary>
    /// <param name="list">所有需要下载的内容</param>
    /// <param name="callback">当前文件本地大小、当前文件服务器大小、所有文件本地大小、所有文件服务器大小、当前文件</param>
    /// <param name="errorCallback"></param>
    public void BatchDownload(List<DownloadUnit> list, Action<Size, DownloadUnit, bool> callback, System.Action<DownloadUnit> errorCallback = null)
    {
        //Debug.Log("1");
        EasyThread downloadThread = null;
        downloadThread = new EasyThread(() =>
        {
            download(list, callback, downloadThread, errorCallback);
            downloadThread.Stop();
        });
        downloadThread.Start();
    }

    /// <summary>
    /// 单个下载
    /// 开线程
    /// </summary>
    /// <param name="downUnit"></param>
    /// <param name="precentCallback">下载进度回调</param>
    public void SingleDownload(DownloadUnit downUnit, System.Action<long, long, bool> callback, System.Action<DownloadUnit> errorCallback = null)
    {
        if (File.Exists(downUnit.FullPath))
        {
            if (callback != null)
            {
                callback(1, 1, true);
            }
            return;
        }
        EasyThread downloadThread = null;
        downloadThread = new EasyThread(() =>
        {
            download(downUnit, callback, downloadThread, errorCallback);
            downloadThread.Stop();
        });
        downloadThread.Start();
    }

    /// <summary>
    /// 下载
    /// </summary>
    /// <param name="downList"></param>
    /// <param name="callback"></param>
    private void download(List<DownloadUnit> downList, System.Action<Size, DownloadUnit, bool> callback, EasyThread downloadThread, System.Action<DownloadUnit> errorCallback = null)
    {
        // 计算所有要下载的文件大小
        float totalSize = 0;
        float oneSize = 0;
        DownloadUnit unit;
        int i = 0;
        Size size = new Size();
        //下载完成的个数统计 用于判断是否全部完成
        int downCount = 0;

        for (i = 0; i < downList.Count; i++)
        {
            unit = downList[i];
            oneSize = GetWebFileSize(unit.downUrl);
            totalSize += oneSize;
        }

        long currentSize = 0;
        i = 0;
        int count = downList.Count;
        for (i = 0; i < count; i++)
        {
            unit = downList[i];
            long currentFileSize = 0;
            download(unit, (long _currentSize, long _fileSize, bool isDone) =>
            {
                currentFileSize = _currentSize;
                long tempCurrentSize = currentSize + currentFileSize;
                if (callback != null)
                {
                    size.LocalSize = _currentSize;
                    size.ServerSize = _fileSize;
                    size.TotalLocalSize = tempCurrentSize;
                    size.TotalServerSize = totalSize;
                    callback(size, unit, isDone);
                }
            }, downloadThread, (DownloadUnit _unit) =>
             {
                 if (errorCallback != null) errorCallback(_unit);
             });
            currentSize += currentFileSize;
            //Debug.Log("finishe one: i = " + i);
        }
    }

    /// <summary>
    /// 下载
    /// </summary>
    /// <param name="downUnit"></param>
    /// <param name="callback"></param>
    private void download(DownloadUnit downUnit, System.Action<long, long, bool> callback, EasyThread downloadThread, System.Action<DownloadUnit> errorCallback = null)
    {
        //打开上次下载的文件
        long startPos = 0;
        //将文件的后缀名改为临时文件名 .temp
        string tempfilename = downUnit.fileName.Replace(Path.GetExtension(downUnit.fileName), ".temp");
        string tempFile = downUnit.savePath + "/" + tempfilename;
        string InstallFile = downUnit.savePath + "/" + downUnit.fileName;

        //Debug.LogError("文件路径 :" + InstallFile + "  /文件名称/     :" + downUnit.fileName  + "            /服务器的md5/    :" + downUnit.md5);

        long totalSize = GetWebFileSize(downUnit.downUrl);
        //float totalSize;
        //float.TryParse(downUnit.size, out totalSize);
        //Debug.Log(totalSize);
        FileStream fs = null;
        //若此文件已经存在 则直接返回文件的总大小
        if (File.Exists(InstallFile) && string.Equals(Utility.GetMd5Hash(File.OpenRead(InstallFile)), downUnit.md5))
        {
            //不执行下载流程
            Debug.LogError("下载的文件 已经存在                 : " + InstallFile);
            if (callback != null)
            {
                callback(totalSize, totalSize, true);
            }
            return;
        }
        if (File.Exists(tempFile))
        {
            fs = File.OpenWrite(tempFile);
            startPos = fs.Length;
            fs.Seek(startPos, SeekOrigin.Current); //移动文件流中的当前指针
        }
        else
        {
            string direName = Path.GetDirectoryName(tempFile);
            if (!Directory.Exists(direName))
            {
                Directory.CreateDirectory(direName);
            }
            fs = new FileStream(tempFile, FileMode.Create);
            Debug.Log(fs.ToString());
        }
        // 下载逻辑
        HttpWebRequest request = null;
        WebResponse respone = null;
        Stream ns = null;
        try
        {
            request = WebRequest.Create(downUnit.downUrl) as HttpWebRequest;
            request.ReadWriteTimeout = ReadWriteTimeOut;
            request.Timeout = TimeOutWait;
            if (startPos > 0) request.AddRange((int)startPos);  //设置Range值，断点续传
            //向服务器请求，获得服务器回应数据流
            respone = request.GetResponse();
            ns = respone.GetResponseStream();
            long curSize = startPos;
            byte[] bytes = new byte[oneReadLen];
            int readSize = ns.Read(bytes, 0, oneReadLen); // 读取第一份数据
            while (readSize > 0)
            {
                //如果Unity客户端关闭，停止下载
                if (IsStop)
                {
                    if (fs != null)
                    {
                        fs.Flush();
                        fs.Close();
                        fs = null;
                    }
                    if (ns != null) ns.Close();
                    if (respone != null) respone.Close();
                    if (request != null) request.Abort();

                    downloadThread.Stop();
                }
                fs.Write(bytes, 0, readSize);       // 将下载到的数据写入临时文件
                curSize += readSize;

                // 回调一下
                if (callback != null)
                    callback(curSize, totalSize, false);
                // 往下继续读取
                readSize = ns.Read(bytes, 0, oneReadLen);
            }
            fs.Flush();
            fs.Close();
            fs = null;

            File.Move(tempFile, InstallFile);

            var file = File.OpenRead(InstallFile);
            string md5 = Utility.GetMd5Hash(file);
            Debug.LogError("文件路径 :" + InstallFile + "  /文件名称/     :" + downUnit.fileName + "    /本地计算的md5值为/    :" + md5 + "            /服务器的md5/    :" + downUnit.md5 + "         /下载url  /      :" + downUnit.downUrl);

            file.Dispose();
            file.Close();

            if (md5 == downUnit.md5)
            {
                if (callback != null) callback(curSize, totalSize, true);
            }
            else
            {
                if (errorCallback != null)
                {
                    errorCallback(downUnit);
                    Debug.LogError("MD5验证失败");
                }
                File.Delete(InstallFile);
                Debug.LogError("删除       删除      删除      删除      删除      删除      删除      删除      删除      删除      删除           " + InstallFile);
                //downloadThread.Stop();
            }
        }
        catch (WebException ex)
        {
            Debug.LogError(ex);
            if (errorCallback != null)
            {
                errorCallback(downUnit);
                Debug.Log("下载出错：" + ex.Message);
            }
        }
        finally
        {
            if (fs != null)
            {
                fs.Flush();
                fs.Close();
                fs = null;
            }
            if (ns != null) ns.Close();
            if (respone != null) respone.Close();
            if (request != null) request.Abort();
        }
    }

    #region OSS下载

    //private Dictionary<DownloadUnit, OSSFile> faillist;
    /// <summary>
    /// 批量下载
    /// </summary>
    /// <param name="list"></param>
    /// <param name="callback"></param>
    public void BatchOSSDownload(Dictionary<DownloadUnit, OSSFile> list, Action callback)
    {
        Dictionary<DownloadUnit, OSSFile> faillist = new Dictionary<DownloadUnit, OSSFile>();
        int cureentDownCount = 0;
        int fallCount = 0;
        //下载完成的个数统计 用于判断是否全部完成

        int i = 0;
        foreach (var file in list)
        {
            //Debug.Log("StartDownload::::" + file.Key.downUrl + "    " + file.Value.endpoint + "    " + file.Key.fileName + "    " + file.Key.size);

            Loom.QueueOnMainThread((() =>
            {
                WriteIntoTxt(i + "StartDownload::::" + file.Key.downUrl + "    " + file.Value.endpoint + "    " + file.Key.fileName + "    " + file.Key.size, "list");
                i++;
            }));

        }
        int j = 0;
        foreach (var file in list)
        {
            EasyThread et = null;
            et = new EasyThread((() =>
            {
                OSSdownload(file.Key, file.Value, (b =>
                {
                    cureentDownCount++;
                    if (b)
                    {

                        Loom.QueueOnMainThread((() =>
                        {
                            WriteIntoTxt(j + "DownloadSuccess::::" + file.Key.downUrl + "    " + file.Value.endpoint + "    " + file.Key.fileName + "    " + file.Key.size, "down");
                            j++;
                        }));


                        Debug.Log("DownloadSuccess::::" + file.Key.downUrl + "    " + file.Value.endpoint + "    " + file.Key.fileName + "    " + file.Key.size);
                        //HttpManager.Instance.DownLoadcurSize += float.Parse(file.Key.size);
                        if (cureentDownCount == list.Count)
                        {
                            if (callback != null)
                            {
                                callback();
                            }
                        }
                    }
                    else
                    {
                        Loom.QueueOnMainThread((() =>
                        {
                            WriteIntoTxt(j + "DownloadFail::::" + file.Key.downUrl + "    " + file.Value.endpoint + "    " + file.Key.fileName + "    " + file.Key.size, "down");
                            j++;
                        }));

                        if (float.Parse(file.Key.size) <= 0 || file.Key.size == null)
                        {
                            //HttpManager.Instance.DownLoadcurSize += float.Parse(file.Key.size);
                            if (cureentDownCount == list.Count)
                            {
                                if (callback != null)
                                {
                                    callback();
                                }
                            }
                        }
                        else
                        {
                            fallCount++;
                            faillist.Add(file.Key, file.Value);
                            Debug.LogError("失败:::::::" + file.Key.downUrl + "    " + file.Value.endpoint + "    " + file.Key.fileName + "    " + file.Key.size);
                        }
                    }

                    Debug.LogWarning("总共  " + list.Count + "    当前  " + cureentDownCount);
                    HttpManager.Instance.DownloadPercent((float)cureentDownCount / (float)list.Count);
                    if (cureentDownCount == list.Count)
                    {
                        callback();
                    }
                    et.Stop();
                }));
            }), ThreadPriority.Highest);
            et.Start();
        };
    }
    StreamWriter writer;
    StreamReader reader;
    public void WriteIntoTxt(string message, string fliename)
    {
        FileInfo file = new FileInfo(Application.persistentDataPath + "/" + fliename + ".txt");
        if (!file.Exists)
        {
            writer = file.CreateText();
        }
        else
        {
            writer = file.AppendText();
        }
        writer.WriteLine(message);
        writer.Flush();
        writer.Dispose();
        writer.Close();
    }

    private void recursion(int index, Dictionary<DownloadUnit, OSSFile> list, Action callback)
    {
        // 获得KeyList
        List<DownloadUnit> keyList = list.Keys.ToList();
        // 获得ValueList
        List<OSSFile> valueList = list.Values.ToList();
        // 获得KeyValuePairList
        List<KeyValuePair<DownloadUnit, OSSFile>> kvList = list.ToList();

        OSSdownload(kvList[index].Key, kvList[index].Value, (b =>
        {
            index++;
            if (b)
            {
                if (index == list.Count)
                {
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }
            else
            {
                Debug.Log("失败" + kvList[index].Key.downUrl + "    " + kvList[index].Value.endpoint + "    " +
                          kvList[index].Value.objectName);
            }
            Debug.Log("总共  " + list.Count + "    当前  " + index);
            if (index < list.Count - 1)
            {
                recursion(index, list, callback);
            }
        }));
    }

    public void OSSdownload(DownloadUnit downUnit, OSSFile of, Action<bool> callback)
    {
        //打开上次下载的文件
        long startPos = 0;
        //将文件的后缀名改为临时文件名 .temp
        string tempfilename = downUnit.fileName.Replace(Path.GetExtension(downUnit.fileName), ".temp");
        string tempFile = downUnit.savePath + "/" + tempfilename;
        string InstallFile = downUnit.savePath + "/" + downUnit.fileName;
        //若此文件已经存在 则直接返回文件的总大小
        if (File.Exists(InstallFile) && string.Equals(Utility.GetMd5Hash(File.OpenRead(InstallFile)), downUnit.md5))
        {
            //不执行下载流程
            //Debug.LogError("下载的文件 已经存在                 : " + InstallFile);
            if (callback != null)
            {
                callback(true);
            }
            return;
        }
        ///如果本地已经临时文件 获取临时文件的长度  断点续传
        if (System.IO.File.Exists(tempFile))
        {
            FileStream fs = File.OpenWrite(tempFile);
            startPos = fs.Length;
            fs.Seek(startPos, SeekOrigin.Current); //移动文件流中的当前指针
        }
        //必须要GMT时间
        string expire = DateTime.UtcNow.GetDateTimeFormats('r')[0].ToString();
        //Debug.Log(of.endpoint + "            " + of.objectName);
        //Debug.Log("OSS下载 url   @" + downUnit.downUrl + "@ 签名  @" + Utility.OSSSignature(of.endpoint, of.objectName, expire) + "@ 时间 @" + expire);

        HttpBase.OssGet(downUnit.downUrl, new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("Date",expire),
            new KeyValuePair<string, string>("Authorization",Utility.OSSSignature(of.endpoint,of.objectName,expire)),
            new KeyValuePair<string, string>("Range","bytes="+(int)startPos+"-"),
        }, ((originalRequest, response) =>
        {
            //float fileSize = float.Parse(downUnit.size) / (1024 * 1024);
            //originalRequest.ConnectTimeout = new TimeSpan(600000);
            //originalRequest.Timeout = new TimeSpan(600000);
            FileStream fs = null;
            if (response == null)
            {
                Debug.Log("请求为空"+response.StatusCode);
                //Debug.Log("请求为空" +downUnit.downUrl);
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs = null;
                }

                if (callback != null)
                {
                    callback(false);
                }

                return;
            }
            if (!response.IsSuccess)
            {
               // Debug.Log("请求失败！" + response.StatusCode + " "+downUnit.downUrl);
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs = null;
                }

                if (callback != null)
                {
                    callback(false);
                }

                return;
            }
            try
            {
                if (System.IO.File.Exists(tempFile))
                {
                    fs = File.OpenWrite(tempFile);
                }
                else
                {
                    string direName = Path.GetDirectoryName(tempFile);
                    if (!Directory.Exists(direName))
                    {
                        Directory.CreateDirectory(direName);
                    }

                    fs = new FileStream(tempFile, FileMode.Create);
                }

                var resp_bytes = response.Data;
                fs.Write(resp_bytes, 0, resp_bytes.Length);

                fs.Flush();
                fs.Close();
                fs = null;
                response.Dispose();

                if (!File.Exists(InstallFile))
                {
                    File.Move(tempFile, InstallFile);
                }

                var file = File.OpenRead(InstallFile);
                string md5 = Utility.GetMd5Hash(file);
                //Debug.LogError("文件路径 :" + InstallFile + "  /文件名称/     :" + downUnit.fileName +
                //               "    /本地计算的md5值为/    :" + md5 + "            /服务器的md5/    :" + downUnit.md5 +
                //               "         /下载url  /      :" + downUnit.downUrl);
                file.Dispose();
                file.Close();
                fs = null;
                if (md5 == downUnit.md5)
                {
                    //Debug.LogError("下载成功");
                    if (callback != null)
                    {
                        callback(true);
                    }

                    return;
                }
                else
                {
                    File.Delete(InstallFile);
                    //Debug.LogError(
                    //    "删除       删除      删除      删除      删除      删除      删除      删除      删除      删除      删除           " +
                    //    InstallFile);
                    if (callback != null)
                    {
                        callback(false);
                    }

                    return;
                }
            }
            catch (WebException ex)
            {
                Debug.Log("下载出错：");
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs = null;
                }

                if (callback != null)
                {
                    callback(false);
                }

                return;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs = null;
                }

                response.Dispose();
            }
        }));
    }

    #endregion OSS下载

    /// <summary>
    /// 获取计算网络文件的大小
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public long GetWebFileSize(string url)
    {
        HttpWebRequest request = null;
        WebResponse respone = null;
        long length = 0;
        try
        {
            request = WebRequest.Create(url) as HttpWebRequest;
            request.Timeout = TimeOutWait;
            request.ReadWriteTimeout = ReadWriteTimeOut;
            //向服务器请求，获得服务器回应数据流
            respone = request.GetResponse();
            length = respone.ContentLength;
        }
        catch (WebException e)
        {
            Debug.LogError(e);
        }
        finally
        {
            if (respone != null) respone.Close();
            if (request != null) request.Abort();
        }
        return length;
    }

    public void StopDownload()
    {
        IsStop = true;
    }
}