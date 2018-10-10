using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 加载进度
/// </summary>
public delegate void LoadProgress(string bundleName, float progress);

/// <summary>
/// 加载完成时候的调用
/// </summary>
public delegate void LoadComplete(string bundleName);

/// <summary>
/// 加载assetbundle的回调
/// </summary>
public delegate void LoadAssetBundleCallback(string sceneName, string bundleName);

/// <summary>
/// 加入购物车的回调
/// </summary>
/// <param name="state"></param>
/// <param name="card"></param>
public delegate void AddChangeItemToCard(string state);


public class AssetUtil
{

}

