using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class GlobalConfig
{
    static GlobalConfig()
    {
        PlayerSettings.Android.keystorePass = "visizen028";
        PlayerSettings.Android.keyaliasName = "artravel";
        PlayerSettings.Android.keyaliasPass = "visizen028";
    }
}

public class CreateAssetBundles : EditorWindow
{

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
    [MenuItem("Assets/CreateAB")]
    private static void Init()
    {
        CreateAssetBundles windos = (CreateAssetBundles)EditorWindow.GetWindow(typeof(CreateAssetBundles));
        windos.Show();
    }

    string _packagePath = "Assets/StreamingAssets/AssetBundles/Android/";
    string _assetBundleName;
    void OnGUI()
    {
        _packagePath = EditorGUILayout.TextField("AssetBundle保存路径", _packagePath);
        _assetBundleName = EditorGUILayout.TextField("单独资源包的名字", _assetBundleName);
        if (GUI.Button(new Rect(60, 120, 180, 40), "所有选择的文件创建为一个包"))
        {
            CreateBundleALL();
        }
        if (GUI.Button(new Rect(320, 60, 230, 40), "所有选择的文件创建为独立的Android包"))
        {
            _packagePath = "Assets/StreamingAssets/AssetBundles/Android/";
            _packageBuddleSelected();
        }
        if (GUI.Button(new Rect(60, 60, 230, 40), "所有选择的文件创建为独立的IOS包"))
        {
            _packagePath = "Assets/StreamingAssets/AssetBundles/IOS/";
            _packageBuddleSelected_IOS();
        }
    }

    //Unfiltered 返回整个选择
    //TopLevel 只返回最上面选择的transform。另一个选定的transform的选定子物体将被过滤掉。
    //Deep 返回选择的物体和它所有的子代
    //ExcludePrefab 排除选择里的所有预制体
    //Editable 排除任何不被修改的对象。
    //Assets 只返回Asset文件夹的资源
    //DeepAssets 如果选择里包含文件夹，则也包括文件夹里的文件和子文件夹。
    void CreateBundleALL()
    {
        if (_packagePath.Length <= 0)
            return;
        if (!Directory.Exists(_packagePath))
        {
            Directory.CreateDirectory(_packagePath);
        }
        //将选中对象一起打包
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        //NewMethod(buildMap);
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        buildMap[0].assetBundleName = SelectedAsset[0].name + ".vsz";
        //GameObject[] objs = Selection.gameObjects; //获取当前选中的所有对象
        string[] itemAssets = new string[SelectedAsset.Length];
        for (int i = 0; i < SelectedAsset.Length; i++)
        {
            itemAssets[i] = AssetDatabase.GetAssetPath(SelectedAsset[i]); //获取对象在工程目录下的相对路径
        }
        buildMap[0].assetNames = itemAssets;
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(_packagePath, buildMap, BuildAssetBundleOptions.None, BuildTarget.Android);
        AssetDatabase.Refresh(); //刷新
        if (manifest == null)
            Debug.LogError("Package AssetBundles Faild.");
        else
            Debug.Log("Package AssetBundles Success.");
    }

    private void NewMethod(AssetBundleBuild[] buildMap)
    {
        buildMap[0].assetBundleName = _assetBundleName + ".vsz";
    }

    void _packageBuddleSelected()
    {
        if (_packagePath.Length <= 0 || !Directory.Exists(_packagePath))
            return;

        GameObject[] objs = Selection.gameObjects;
        AssetBundleBuild[] buildMap = new AssetBundleBuild[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            string[] itemAsset = new string[] { AssetDatabase.GetAssetPath(objs[i]) };
            buildMap[i].assetBundleName = objs[i].name + ".vsz";
            buildMap[i].assetNames = itemAsset;
        }
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(_packagePath, buildMap, BuildAssetBundleOptions.None, BuildTarget.Android);
        AssetDatabase.Refresh();
        if (manifest == null)
            Debug.LogError("Error:Package Failed");
        else
            Debug.Log("Package Success.");
    }

    void _packageBuddleSelected_IOS()
    {
        if (_packagePath.Length <= 0 || !Directory.Exists(_packagePath))
            return;

        GameObject[] objs = Selection.gameObjects;
        AssetBundleBuild[] buildMap = new AssetBundleBuild[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            string[] itemAsset = new string[] { AssetDatabase.GetAssetPath(objs[i]) };
            buildMap[i].assetBundleName = objs[i].name + ".vsz";
            buildMap[i].assetNames = itemAsset;
        }
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(_packagePath, buildMap, BuildAssetBundleOptions.None, BuildTarget.iOS);
        AssetDatabase.Refresh();
        if (manifest == null)
            Debug.LogError("Error:Package Failed");
        else
            Debug.Log("Package Success.");
    }
}