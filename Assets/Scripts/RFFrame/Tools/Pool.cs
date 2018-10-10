/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     GameObjectPool.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-25 
 *Description:    
 *History: 
*/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameObjectPool
{
    [SerializeField]
    public string name;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int maxAmount;
    [NonSerialized]
    public List<GameObject> goList = new List<GameObject>();

    public GameObjectPool(string name, GameObject prefab, int maxAmount= 5)
    {
        this.name = name;

        this.prefab = prefab;

        this.maxAmount = maxAmount;
    }

    private int count = 1;
    /// <summary>
    /// 表示从资源池中获取一个实例
    /// </summary>
    public GameObject GetInst()
    {
        if (goList.Count >= maxAmount)
        {
            Utility.DestoryGo(goList[0]);
            goList.RemoveAt(0);
        }
        foreach (GameObject go in goList)
        {
            if (go.activeInHierarchy == false)
            {
                return go;
            }
        }

        GameObject tmp = GameObject.Instantiate(prefab);
        tmp.name = count++ + "";
        //GameObject tmp = Utility.InstantiateGo(prefab, true);
        goList.Add(tmp);
        return tmp;
    }
}