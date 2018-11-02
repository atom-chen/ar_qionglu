using ElviraFrame.AB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ElviraFrame;
public class YiyouStaticDataManager : SingletonMono<YiyouStaticDataManager>
{

    //删除委托的原型
    public delegate void OnDestroyHandle(GameObject obj);
    //定义委托
    public OnDestroyHandle OnDestroyGo;


    //禁音委托的原型
    public delegate void OnSilenceHandle(float value);
    //定义委托
    public OnSilenceHandle OnSilenceGo;

    [Header("湖泊Sprites")]
    public List<Sprite> hupoSprite = new List<Sprite>();
    [Header("森林Sprites")]
    public List<Sprite> senlinSprite = new List<Sprite>();
    [Header("沙滩Sprites")]
    public List<Sprite> shatanSprite = new List<Sprite>();
    [Header("夜晚Sprites")]
    public List<Sprite> yewanSprite = new List<Sprite>();
    [Header("隔空传爱Sprites")]
    public List<Sprite> gekongchuanaiSprite = new List<Sprite>();
    [Header("古镇Sprites")]
    public List<Sprite> guzhenSprite = new List<Sprite>();
    [Header("雪地Sprites")]
    public List<Sprite> xuediSprite = new List<Sprite>();
    [Header("草原Sprites")]
    public List<Sprite> caoyuanSprite = new List<Sprite>();
    [Header("山川Sprites")]
    public List<Sprite> shanchuanSprite = new List<Sprite>();

    [Header("字体Sprites")]
    public List<Sprite> zitiSprite = new List<Sprite>();
    [Header("字体点击后Sprites")]
    public List<Sprite> zitiClickSprite = new List<Sprite>();

    [Header("写字材质球Sprites")]
    public List<Material> materialsList = new List<Material>();
    [Header("海鸥材质球Sprites")]
    [SerializeField]
    private List<Material> haiouMaterialsList = new List<Material>();
    public Dictionary<string, List<Sprite>> spriteDic = new Dictionary<string, List<Sprite>>();
    internal Font currentFont;

    public string modelName = string.Empty;
    public GameObject ShowModel;

    public string modelText = string.Empty;
    Light mainLight;



    public UnityEngine.Font CurrentFont
    {
        get {

            if (currentFont==null)
            {
                currentFont= Resources.Load<Font>("Fonts/" +GlobalParameter.defaultFont); 
            }
            return currentFont;

        }
        set { currentFont = value; }
    }
    public override void Awake()
    {
        base.Awake();

        if (hupoSprite.Count != 0)
        {
            spriteDic.Add("hupo", hupoSprite);
        }

        if (senlinSprite.Count != 0)
        {
            spriteDic.Add("senlin", senlinSprite);
        }
        if (shatanSprite.Count != 0)
        {
            spriteDic.Add("shatan", shatanSprite);
        }
        if (yewanSprite.Count != 0)
        {
            spriteDic.Add("yewan", yewanSprite);
        }
        if (gekongchuanaiSprite.Count != 0)
        {
            spriteDic.Add("gekongchuanai", gekongchuanaiSprite);
        }
        if (guzhenSprite.Count != 0)
        {
            spriteDic.Add("guzhen", guzhenSprite);
        }
        if (xuediSprite.Count != 0)
        {
            spriteDic.Add("xuedi", xuediSprite);
        }
        if (caoyuanSprite.Count != 0)
        {
            spriteDic.Add("caoyuan", caoyuanSprite);
        }
        if (shanchuanSprite.Count != 0)
        {
            spriteDic.Add("shanchuan", shanchuanSprite);
        }
        if (zitiSprite.Count != 0)
        {
            spriteDic.Add("ziti", zitiSprite);
        }
        if (    zitiClickSprite.Count!=0)
        {
            spriteDic.Add("ziticlick", zitiClickSprite);
        }
    }
    /// <summary>
    /// 获得九大场景对应的sprite   List，以及字体的List
    /// 场景名字如下
    /// 1 hupo
    /// 2 senlin
    /// 3  shatan
    /// 4yewan
    /// 5  gekongchuanai
    /// 6guzhen
    /// 7xuedi
    /// 8 caoyuan
    /// 9  shanchuan
    /// 
    /// 
    /// 10  ziti
    /// 11  ziticlick
    /// 12  caiziqiu
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public List<Sprite> GetSprites(string sceneName)
    {
        List<Sprite> resultList = new List<Sprite>();
        spriteDic.TryGetValue(sceneName, out resultList);
        if (resultList != null)
        {
            return resultList;
        }
        return null;
    }


    public Material GetMaterial(string Name)
    {
        Material tempMaterial = materialsList.Find(mat => mat.name == Name);
        if (tempMaterial!=null)
        {
        return tempMaterial;
        }
        else
        {
            return null;
        }
    }
    public List<Material> GetHaiouMaterial( )
    {
        return haiouMaterialsList;
    }



    public Hashtable GetHashtable()
    {
        Hashtable custom = new Hashtable();
        custom.Add("modelName", modelName);
    
            custom.Add("modelText", modelText);
       
        custom.Add("modelTransform", ShowModel.transform);
        custom.Add("sceneName", SceneManager.GetActiveScene().name + "Restore");

        custom.Add("modelTextFont", CurrentFont);
        custom.Add("Light", GameObject.FindGameObjectWithTag(Tags.Light).GetComponent<Light>());

        return custom;
    }
    private string sceneName = "scene_yiyou";
    private string ABSelfname = "scene_yiyou/prefabs.ab";


    /// <summary>
    ///加载AB包进入内存 
    /// </summary>
    public void StartLoadABAssets()
{
	StartCoroutine(AssetBundleMgr.GetInstance().LoadAssetBundlePack(sceneName, ABSelfname, LoadABComplete));
}

    /// <summary>
    /// 整体AB包加载完毕之后将图片设置为可点击状态
    /// </summary>
    /// <param name="abName"></param>
    private void LoadABComplete(string abName)
    {
        SelectModelPanel.Instance.SetItemInteractable(true);
    }

    public GameObject LoadGameObject(string  goName)
    {
       return  AssetBundleMgr.GetInstance().LoadAsset(sceneName, ABSelfname, goName, true) as GameObject;
    }


    public void DisposeAB()
    {
        AssetBundleMgr.GetInstance().DisposeAllAssets(sceneName);
    }



    public void OnDestroyGameObject()
    {
        if (OnDestroyGo != null)
        {
            OnDestroyGo(gameObject);
        }
        OnDestroyGo = null;
    }


    public void OnSilenceGameObject(float value)
    {
        if (OnSilenceGo != null)
        {
            OnSilenceGo(value);
        }

    }


    public void HandleClear()
    {
        OnDestroyGo = null;
        OnSilenceGo = null;
        FingerTouchEL.Instance.touchBeginGo = null;
        FingerTouchEL.Instance.touchEndGo = null;
    }
}
