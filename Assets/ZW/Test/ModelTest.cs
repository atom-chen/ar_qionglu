using ElviraFrame.AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTest : MonoBehaviour {
    private string sceneName = "scene_yiyou";
    private string ABSelfname = "scene_yiyou/prefabs.ab";

    static int index = 1;
    private void OnEnable()
    {
        Debug.Log("EffectPanel++++" + (index++));
    }


    // Use this for initialization
    void Start () {
        StartCoroutine(AssetBundleMgr.GetInstance().LoadAssetBundlePack(sceneName, ABSelfname, null));
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
    GameObject        showGameObject = AssetBundleMgr.GetInstance().LoadAsset(sceneName, ABSelfname, "haiou", true) as GameObject;
            // showGameObject = Resources.Load<GameObject>("Prefabs/" + modelName);
            if (showGameObject)
            {
                showGameObject = Instantiate(showGameObject);
                showGameObject.transform.localEulerAngles = Vector3.zero;
                showGameObject.transform.localPosition = Vector3.zero;
                showGameObject.transform.localScale = Vector3.one;

            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject showGameObject = AssetBundleMgr.GetInstance().LoadAsset(sceneName, ABSelfname, "kongmingdeng", true) as GameObject;
            // showGameObject = Resources.Load<GameObject>("Prefabs/" + modelName);
            if (showGameObject)
            {
                showGameObject = Instantiate(showGameObject);
                showGameObject.transform.localEulerAngles = Vector3.zero;
                showGameObject.transform.localPosition = Vector3.zero;
                showGameObject.transform.localScale = Vector3.one;

            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject showGameObject = AssetBundleMgr.GetInstance().LoadAsset(sceneName, ABSelfname, "kongmingdengGo", true) as GameObject;
            // showGameObject = Resources.Load<GameObject>("Prefabs/" + modelName);
            if (showGameObject)
            {
                showGameObject = Instantiate(showGameObject);
                showGameObject.transform.localEulerAngles = Vector3.zero;
                showGameObject.transform.localPosition = Vector3.zero;
                showGameObject.transform.localScale = Vector3.one;

            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameObject showGameObject = AssetBundleMgr.GetInstance().LoadAsset(sceneName, ABSelfname, "shabao", true) as GameObject;
            // showGameObject = Resources.Load<GameObject>("Prefabs/" + modelName);
            if (showGameObject)
            {
                showGameObject = Instantiate(showGameObject);
                showGameObject.transform.localEulerAngles = Vector3.zero;
                showGameObject.transform.localPosition = Vector3.zero;
                showGameObject.transform.localScale = Vector3.one;

            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject showGameObject = AssetBundleMgr.GetInstance().LoadAsset(sceneName, ABSelfname, "shitou", true) as GameObject;
            // showGameObject = Resources.Load<GameObject>("Prefabs/" + modelName);
            if (showGameObject)
            {
                showGameObject = Instantiate(showGameObject);
                showGameObject.transform.localEulerAngles = Vector3.zero;
                showGameObject.transform.localPosition = Vector3.zero;
                showGameObject.transform.localScale = Vector3.one;

            }
        }
    }
}
