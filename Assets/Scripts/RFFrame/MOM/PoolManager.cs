/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     PoolManager.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-24 
 *Description:    
 *History: 
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager :Singleton<PoolManager>{

    private static string poolConfigPathPrefix = "Assets/";
    private const string poolConfigPathMiddle = "Pool";
    private const string poolConfigPathPostfix = ".asset";
    public static string PoolConfigPath
    {
        get
        {
            return poolConfigPathPrefix + poolConfigPathMiddle + poolConfigPathPostfix;
        }
    }
    private Dictionary<string, GameObjectPool> poolDict;

    protected override void Init()
    {
        poolDict = new Dictionary<string, GameObjectPool>();

        //GameObjectPoolList poolList = Resources.Load<GameObjectPoolList>(poolConfigPathMiddle);
        //foreach (GameObjectPool pool in poolList.poolList)
        //{
        //    poolDict.Add(pool.name, pool);
        //}
    }

    public GameObject GetInstance(string poolName,GameObject go = null,int maxAmount = 5)
    {
        GameObjectPool pool;
        if (!poolDict.ContainsKey(poolName))
        {
            pool = new GameObjectPool(poolName, go, maxAmount);
            poolDict.Add(poolName, pool);
        }
        else
        {
            pool = poolDict[poolName];
        }
        return pool.GetInst();

    }
}
