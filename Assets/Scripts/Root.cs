using com.moblink.unity3d;
using System.IO;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Root : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        InitCreateFile();
    }

    private void Start()
    {
        Loom.Initialize();
    }

    #region 全局的场景还原监听函数

    public static MobLinkScene tempScene = null;


    public void Notify(string message)
    {
        Debug.Log("点击了通知!!!,传回的消息是    " + message);
        NotifyType nt =  JsonMapper.ToObject<NotifyType>(message);
        Debug.Log("action :  "+   nt.action+  "id:"+nt.id);
    }


    /// <summary>
    /// 全局的场景还原监听函数
    /// </summary>
    /// <param name="scene"></param>
    protected static void OnRestoreScene(MobLinkScene scene)
    {
        if (scene != null)
        {
            tempScene = scene;
            SceneManager.LoadScene(tempScene.customParams["sceneName"].ToString());
        }
    }

    #endregion 全局的场景还原监听函数

    private void OnGUI()
    {
        //检测鼠标事件
        HardWareManager.Instance.MouseLoop();
    }

    private void Update()
    {
        //检测键盘事件
        HardWareManager.Instance.KeyboardLoop();
        TimerManager.Instance.Loop();
    }







    /// <summary>
    /// 初始化创建文件/文件夹
    /// 1、Token文件
    /// </summary>
    private void InitCreateFile()
    {
        //若token.josn存在
        if (!File.Exists(PublicAttribute.TokenFilePath))
        {
            if (!Directory.Exists(Path.GetDirectoryName(PublicAttribute.TokenFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(PublicAttribute.TokenFilePath));
            }
            FileStream fs = new FileStream(PublicAttribute.TokenFilePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Close();
        }
    }
}