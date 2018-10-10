using ElviraFrame.AB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class KongmingLatern : WriteItem
{


    private string sceneName = "scene_yiyou";
    private string ABSelfname = "scene_yiyou/prefabs.ab";
    private Transform parentGo;
    internal void FlyGameObject()
    {
        parentGo = transform.parent;
        this.gameObject.AddComponent<Fly>();
        int cout = Random.Range(500, 800);
        for (int i = 0; i < cout; i++)
        {

            GameObject go = YiyouStaticDataManager.Instance.LoadGameObject("kongmingdengGo");
            if (go)
            {
                go = Instantiate(go, parentGo);

                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.position = new UnityEngine.Vector3(Random.Range(-100f, 100f), Random.Range(10f, 30f), Random.Range(-100f, 100f));


            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }
}
