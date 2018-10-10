using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using ElviraFrame.AB;
using ElviraFrame;




/// <summary>
/// SLAM基类
/// </summary>
public abstract class SLAMController : SingletonMono<SLAMController>
{
    //[HideInInspector]
    protected GameObject showGameObject;
    protected string showGameObjectName;
    protected List<MeshRenderer> renderMaterialsList = new List<MeshRenderer>();
    protected List<SkinnedMeshRenderer> skinMaterialsList = new List<SkinnedMeshRenderer>();
    string modelName = string.Empty;

    public void SetMode(string goName)
    {
        modelName = goName;
        showGameObjectName = goName;
        YiyouStaticDataManager.Instance.modelName = goName;
        if (showGameObject != null)
        {
            Destroy(showGameObject);
        }

        LoadComplete(showGameObjectName);
    }

    public   virtual void LoadComplete(string modelName)
    {
        //2、提取资源
        if (showGameObject != null)
        {
            Destroy(showGameObject);
        }
        showGameObject = YiyouStaticDataManager.Instance.LoadGameObject(modelName);
        if (showGameObject)
        {
            showGameObject = Instantiate(showGameObject);
            showGameObject.name = showGameObjectName;
        }
        skinMaterialsList.Clear();
        renderMaterialsList.Clear();
        if (showGameObjectName == "wurenji")
        {
            SkinnedMeshRenderer[] ziTransform = showGameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (ziTransform.Length != 0)
            {

                foreach (var item in ziTransform)
                {
                    if (item.name == "zi")
                    {
                        skinMaterialsList.Add(item);
                    }
                }
            }
        }
        else
        {
            MeshRenderer[] ziTransform = showGameObject.transform.GetComponentsInChildren<MeshRenderer>();
            if (ziTransform.Length != 0)
            {

                foreach (var item in ziTransform)
                {
                    if (item.name == "zi")
                    {
                        renderMaterialsList.Add(item);
                    }
                }
            }
        }

        SetMterials(showGameObject.GetComponent<WriteItem>().goodsEnum);


        showGameObject.gameObject.SetActive(false);
        GroundPlaneUI.Instance.SelectModelPanel.gameObject.SetActive(false);
        GroundPlaneUI.Instance.SetIntroductionText("请将镜头朝向地面");
    }

    protected virtual void SetMterials(GoodsWriteEnum goodsWriteEnum)
    {

        switch (goodsWriteEnum)
        {

            case GoodsWriteEnum.Single:
                if (showGameObjectName == "wurenji")
                {
                    if (skinMaterialsList.Count != 0)
                    {
                        Debug.Log("dddd111111111");
                        foreach (SkinnedMeshRenderer item in skinMaterialsList)
                        {
                            item.material = YiyouStaticDataManager.Instance.GetMaterial("Single");
                        }
                    }
                }
                else
                {
                    if (renderMaterialsList.Count != 0)
                    {
                        Debug.Log("dddd2222222");
                        foreach (MeshRenderer item in renderMaterialsList)
                        {
                            item.material = YiyouStaticDataManager.Instance.GetMaterial("Single");
                        }
                    }
                }

                break;
            case GoodsWriteEnum.Two:
                break;
            case GoodsWriteEnum.Three:
                break;
            default:
                break;
        }
    }


    public virtual  void ResetScene()
    {

    }
}
