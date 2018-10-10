using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using ElviraFrame;
public class WriteItem : SingletonMono<WriteItem>
{
 
    /// <summary>
    /// 需要更改颜色的材质球
    /// </summary>
    public List<Material> Mats=new List<Material>();
    /// <summary>
    /// 需要更改颜色的物体
    /// </summary>
    public List<GameObject> changeColorGos = new List<GameObject>();

    /// <summary>
    /// 灯光强度
    /// </summary>
    float baseLightIntensity;
    Color baseLightColor;
    List<GameObject> ziList = new List<GameObject>();
    public GoodsWriteEnum goodsEnum = GoodsWriteEnum.None;
    public PlaneMode goodsPositionEnum = PlaneMode.GROUND;
    protected virtual void Start()
    {
        YiyouStaticDataManager.Instance.OnDestroyGo += OnDestroyGo;
        FingerTouchEL.Instance.touchBeginGo += TouchBegin;
        FingerTouchEL.Instance.touchEndGo += TouchEnd; 
        Transform[] ziTras = gameObject.GetComponentsInChildren<Transform>(true);
        if (ziTras.Length != 0)
        {
            foreach (Transform item in ziTras)
            {
                if (item.name == "zi")
                {
                    ziList.Add(item.gameObject);
                }
            }
        }
        if (changeColorGos.Count!=0)
        {
            foreach (var go in changeColorGos)
            {
                Material[] ats = go.GetComponent<MeshRenderer>().materials;
                foreach (var item in ats)
                {
                    Mats.Add(item);

                }
           
            }
        }
 


    }

    private void TouchBegin(GameObject obj)
    {
        Debug.Log("Begin");
        Cloth cloth = transform.GetComponentInChildren<Cloth>();
        if (cloth!=null)
        {
            cloth.enabled = false;

        }
    }
    private void TouchEnd(GameObject obj)
    {
       
        Debug.Log("end");
        Cloth cloth = transform.GetComponentInChildren<Cloth>();
        if (cloth != null)
        {
            cloth.enabled = true;

        }
    }

    private void OnDestroyGo(GameObject obj)
    {
        if (this.gameObject)
        {
            Destroy(this.gameObject);
            YiyouStaticDataManager.Instance.OnDestroyGo-= OnDestroyGo;
        }
    }

    internal virtual void SetLightIntensity(float intensityValue)
    {
        //intensityValue *= 10f;
        intensityValue = Mathf.Clamp(intensityValue, 1f, 5f);
     
        Debug.Log("intensityValue2222=" + intensityValue);
    }

    internal void SetLightColor(float value)
    {

        Debug.Log("Value==="+value);
        Color newColor = new Color(1f* value, 1f* value, 1f* value);
        try
        {
            foreach (var mat in Mats)
            {
                mat.DOColor(newColor, "_Color", 0.5f);
         
            }
        }
        catch (Exception)
        {

          
        }
  
    }
}
