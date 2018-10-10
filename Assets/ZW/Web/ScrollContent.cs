using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollContent : MonoBehaviour
{
    public static ScrollContent _Instance;


    public List<GameObject> childPanel=new List<GameObject>();


    void Awake()
    {
        _Instance = this;

        childPanel.Clear();

    }
    /// <summary>
    /// 生成ContentChildPanel
    /// </summary>
    public void AddContentChildPanel(string  key,List<string>  paths)
    {
        GameObject go = Instantiate<GameObject>(Resources.Load<GameObject>("ContentChildPanel"), this.transform);
        go.GetComponent<ContentChildPanel>().AddImageChild( key,paths);
        childPanel.Add(go);
        Updateheight();
    }

    public void Updateheight()
    {
        float height=0f;
        foreach (GameObject item in childPanel)
        {
            height += item.GetComponent<RectTransform>().sizeDelta.y;
        }

        if (childPanel.Count==0)
        {
            height = 2560f;
        }

    //this.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, height);
 this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1440f,height);
    }
}
