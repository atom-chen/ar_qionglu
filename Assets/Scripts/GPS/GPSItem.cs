using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSItem : MonoBehaviour
{
    /// <summary>
    /// 实景点ID
    /// </summary>
    public string id;

    /// <summary>
    /// 实景点名称
    /// </summary>
    public string name;

    /// <summary>
    /// 实景点经度
    /// </summary>
    public string locationX;

    /// <summary>
    /// 实景点纬度
    /// </summary>
    public string locationY;

    /// <summary>
    /// 实景点海拔
    /// </summary>
    public string height;
    /// <summary>
    /// 实景点详情
    /// </summary>
    public string content;

    /// <summary>
    /// 网页详情地址
    /// </summary>
    public string address;

    /// <summary>
    /// 景点分类 
    /// </summary>
    public string typeName;

    public IconFollow icon;

    public Thumbnail thumbnail;

    private void Start()
    {
       
    }
    public void ShowObj(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
