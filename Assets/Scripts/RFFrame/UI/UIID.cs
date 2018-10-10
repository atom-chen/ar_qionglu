/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     UIID.cs 
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

    /// <summary>
    /// 注意顺序 加到最后
    /// </summary>
    public enum UIID
    {
        Empty,
    }

    public class UIResource
    {
        public static readonly Dictionary<UIID, UIResource> WindowPrefabs = new Dictionary<UIID, UIResource>
    {
        //{UIID.LoadingScene, new UIResource("Prefabs/UIPrefabs/LoadingScene")},
    };
        /// <summary>
        /// 资源路径
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        public UIResource(string path)
        {
            Path = path;
            if (path != null)
                Name = path.Substring(path.LastIndexOf('/') + 1);
        }
    }

