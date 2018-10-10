/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     GameObjectPoolList.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-25 
 *Description:    
 *History: 
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPoolList : ScriptableObject
{//继承自ScriptableObject 表示吧类GameObjectPoolList变成可以自定义资源配置的文件
    public List<GameObjectPool> poolList;
}
