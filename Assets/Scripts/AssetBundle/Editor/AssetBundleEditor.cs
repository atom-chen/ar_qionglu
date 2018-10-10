using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// AssetBundle编辑
/// </summary>
public class AssetBundleEditor
{
    #region 自动做标记

    //步骤
    //1.找到资源保存的文件夹
    //2.遍历里面的每个场景文件夹
    //3.遍历场景文件夹里的所有文件系统
    //4.如果访问的是文件夹：再继续访问里面的所有文件系统，直到找到 文件 （递归）
    //5.找到文件 就要修改他的 assetbundle labels
    //6.用 AssetImporter 类 修改名称和后缀
    //7.保存对应的 文件夹名 和 具体路径 

    [MenuItem("WBAssetBundle/Set AssetBundle Labels")]
    public static void SetAssetBundleLabels()
    {
        //移除所有没有使用的标记
        AssetDatabase.RemoveUnusedAssetBundleNames();

        //1.找到资源保存的文件夹
        string assetDirectory = Application.dataPath + "/Resources/techan/";
        //Debug.Log(assetDirectory);

        DirectoryInfo directoryInfo = new DirectoryInfo(assetDirectory);
        DirectoryInfo[] ScenicDirectories = directoryInfo.GetDirectories();
        //2.遍历里面的每个场景文件夹
        foreach (DirectoryInfo tmpDirectoryInfo in ScenicDirectories)
        {
            string ScenicDirectory = assetDirectory + "/" + tmpDirectoryInfo.Name;
            DirectoryInfo ScenicDirectoryInfo = new DirectoryInfo(ScenicDirectory);
            DirectoryInfo[] ScenicPointDirectories = ScenicDirectoryInfo.GetDirectories();

            foreach (DirectoryInfo DirectoryInfo in ScenicPointDirectories)
            {
                string ScenicPointDirectory = ScenicDirectory + "/" + DirectoryInfo.Name;
                DirectoryInfo ScenicPointDirectoryInfo = new DirectoryInfo(ScenicPointDirectory);

                //错误检测
                if (ScenicPointDirectoryInfo == null)
                {
                    Debug.LogError(ScenicDirectory + " 不存在!");
                    return;
                }
                else
                {
                    Dictionary<string, string> namePahtDict = new Dictionary<string, string>();

                    //3.遍历场景文件夹里的所有文件系统
                    //sceneDirectory
                    //E:\ar_travel_liangshan\code\travel_U3D\travel_U3D\Assets\DownloadFile\RES\sichuan\xichang\
                    int index = ScenicPointDirectory.LastIndexOf("/");
                    //Debug.Log(ScenicPointDirectory);
                    string sceneName = ScenicPointDirectory.Substring(index + 1);
                    sceneName = "techan";
                    //sceneName = sceneName +"/"+ sceneName;
                    onSceneFileSystemInfo(ScenicPointDirectoryInfo, sceneName, namePahtDict);

                    onWriteConfig(sceneName, namePahtDict);
                }
            }

        }

        AssetDatabase.Refresh();

        Debug.Log("设置成功");
    }

    /// <summary>
    /// 记录配置文件
    /// </summary>
    private static void onWriteConfig(string sceneName, Dictionary<string, string> namePathDict)
    {
        string path = PathUtil.GetAssetBundleOutPath() + "/" + sceneName + "Record.txt"; ;
        //if (!Directory.Exists(FolderPath))
        //    Directory.CreateDirectory(FolderPath);
        //string path = FolderPath + "/" + sceneName + "Record.txt";
        // Debug.Log(path);
       
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            //写二进制
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(namePathDict.Count);

                foreach (KeyValuePair<string, string> kv in namePathDict)
                    sw.WriteLine(kv.Key + " " + kv.Value);
            }
        }

    }

    /// <summary>
    /// 遍历场景文件夹里的所有文件系统
    /// </summary>
    private static void onSceneFileSystemInfo(FileSystemInfo fileSystemInfo, string sceneName, Dictionary<string, string> namePahtDict)
    {
        if (!fileSystemInfo.Exists)
        {
            Debug.LogError(fileSystemInfo.FullName + " 不存在!");
            return;
        }

        DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
        FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
        foreach (var tmpFileSystemInfo in fileSystemInfos)
        {
            FileInfo fileInfo = tmpFileSystemInfo as FileInfo;
            if (fileInfo == null)
            {
                //代表强转失败，不是文件 就是文件夹
                //如果访问的是文件夹：再继续访问里面的所有文件系统，直到找到 文件 （递归）
                onSceneFileSystemInfo(tmpFileSystemInfo, sceneName+"/"+tmpFileSystemInfo.Name, namePahtDict);
                //Debug.Log(tmpFileSystemInfo);
            }
            else
            {
                //就是文件
                //5.找到文件 就要修改他的 assetbundle labels
                setLabels(fileInfo, sceneName, namePahtDict);
                //Debug.Log(fileInfo);
            }
        }
    }

    /// <summary>
    /// 修改资源文件的 assetbundle labels
    /// </summary>
    private static void setLabels(FileInfo fileInfo, string sceneName, Dictionary<string, string> namePahtDict)
    {
        //对unity自身生成的meta文件 无视它
        if (fileInfo.Extension == ".meta")
            return;

        string bundleName = getBundleName(fileInfo, sceneName);
        //Debug.Log(bundleName);
        //E:\ar_travel_liangshan\code\travel_U3D\travel_U3D\Assets\DownloadFile\RES\sichuan\xichang\lushanfengjingqu\lushanshenlingongyuan
        int index = fileInfo.FullName.IndexOf("Assets");
        //Assets\DownloadFile\RES\sichuan\xichang\lushanfengjingqu\lushanshenlingongyuan
        string assetPath = fileInfo.FullName.Substring(index);

        AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
        //用 AssetImporter 类 修改名称和后缀
        assetImporter.assetBundleName = bundleName.ToLower();
        if (fileInfo.Extension == ".unity")
            assetImporter.assetBundleVariant = "u3d";
        else
            assetImporter.assetBundleVariant = "assetbundle";

        string folderName = "";
        //添加到字典里
        if (bundleName.Contains("/"))
            folderName = bundleName.Split('/')[1];
        else
            folderName = bundleName;

        string bundlePath = assetImporter.assetBundleName + "." + assetImporter.assetBundleVariant;
        if (!namePahtDict.ContainsKey(folderName+ assetImporter.assetBundleVariant))
            namePahtDict.Add(folderName+ assetImporter.assetBundleVariant, bundlePath);
    }


    /// <summary>
    /// 获取包名
    /// </summary>
    private static string getBundleName(FileInfo fileInfo, string sceneName)
    {
        string windowsPath = fileInfo.FullName; 
        //E:\ar_travel_liangshan\code\travel_U3D\travel_U3D\Assets\DownloadFile\RES\sichuan\xichang\lushanfengjingqu\lushanshenlingongyuan
        //转换成unity可识别的路径
        string unityPath = windowsPath.Replace(@"\", "/");

        int index = unityPath.IndexOf(sceneName) + sceneName.Length;

        string bundlePath = unityPath.Substring(index + 1);

        string assetName = "";
        if (bundlePath.Contains("/"))
        {
            //Buildings/Folder/Folder/Folder/Folder/Folder/Building4.prefab
            string[] tmp = bundlePath.Split('/');
            assetName = sceneName + "/" + tmp[0];
        }
        else
        {
            //Scene1.unity
            assetName = sceneName;
        }
        if (assetName.Contains("/"))
        {
            string[] tmp2 = assetName.Split('/');
            return assetName + "/" + tmp2[tmp2.Length-1];
        }
        else
        {
            return assetName + "/" +assetName;
        }
    }

    #endregion

    #region 打包
    //  0 BuildAssetBundleOptions.None --构建AssetBundle没有任何特殊的选项

    //  1 BuildAssetBundleOptions.UncompressedAssetBundle --不进行数据压缩。如果使用该项，因为没有压缩\解压缩的过程， AssetBundle的发布和加载会很快，但是AssetBundle也会更大，下载变慢

    //  2 BuildAssetBundleOptions.CollectDependencies  --包含所有依赖关系

    //  4 BuildAssetBundleOptions.CompleteAssets  --强制包括整个资源

    //  8 BuildAssetBundleOptions.DisableWriteTypeTree --在AssetBundle中不包含类型信息。发布web平台时，不能使用该项

    // 16 BuildAssetBundleOptions.DeterministicAssetBundle --使每个Object具有唯一不变的hash ID，可用于增量式发布AssetBundle

    // 32 BuildAssetBundleOptions.ForceRebuildAssetBundle --强制重新Build所有的AssetBundle

    // 64 BuildAssetBundleOptions.IgnoreTypeTreeChanges --忽略TypeTree的变化，不能与DisableTypeTree同时使用

    //128 BuildAssetBundleOptions.AppendHashToAssetBundleName --附加hash到AssetBundle名称中

    //256 BuildAssetBundleOptions.ChunkBasedCompression --Assetbundle的压缩格式为lz4。默认的是lzma格式，下载Assetbundle后立即解压。而lz4格式的Assetbundle会在加载资源的时候才进行解压，只是解压资源的时机不一样
    [MenuItem("WBAssetBundle/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        Debug.Log(EditorUserBuildSettings.activeBuildTarget);
        string outPath = PathUtil.GetAssetBundleOutPath();
        Debug.Log(outPath);
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                BuildPipeline.BuildAssetBundles(outPath, 0, BuildTarget.Android);
                break;
            case BuildTarget.iOS:
                BuildPipeline.BuildAssetBundles(outPath, 0, BuildTarget.iOS);
                break;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                BuildPipeline.BuildAssetBundles(outPath, 0, BuildTarget.StandaloneWindows64);
                break;
            default:
                //BuildPipeline.BuildAssetBundles(outPath, 0, BuildTarget.StandaloneWindows64);
                break;
        }
    }


    #endregion

    #region 一键删除

    [MenuItem("WBAssetBundle/Delete All")]
    static void DeleteAssetBundle()
    {
        string outPath = PathUtil.GetAssetBundleOutPath();

        Directory.Delete(outPath, true);
        File.Delete(outPath + ".meta");

        AssetDatabase.Refresh();
    }

    #endregion

}
