using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using mainpage;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ARScanManager : MonoBehaviour
{
    public static ARScanManager instance;

    public RawImage ScanGuideImg;
    public Texture[] ScanGuideTex;
    public GameObject ARScanGuide,btnPanel,helpPanel;

    public EdgeDetection ed;
    public DOTweenAnimation scanImg;
    public Text toastObj;
    public GameObject ShotBtn;

    public static string scan_more_Path = "";
    public static string scan_ticket_Path = "";
    public static string scan_native_product_Path = "";
    public static string scan_Conjure_Path = "";


    public ARScanGuide guide;
    public bool isGuide;

    public AudioSource shotclip;
    private void Awake()
    {
        instance = this;

        foreach (var ChangeInfo in mainUISet.curScenicInfo.ResourcesInfos)
        {
            if (ChangeInfo.ResourcesKey == "scan-more")
            {
                scan_more_Path = ChangeInfo.LocalPath;
                Debug.Log(scan_more_Path);
            }
            else if (ChangeInfo.ResourcesKey == "scan-ticket")
            {
                scan_ticket_Path = ChangeInfo.LocalPath;
                Debug.Log(scan_ticket_Path);
            }
            else if (ChangeInfo.ResourcesKey == "scan-native-product")
            {
                scan_native_product_Path = ChangeInfo.LocalPath;
                Debug.Log(scan_native_product_Path);
            }
            else if (ChangeInfo.ResourcesKey == "scan-conjure")
            {
                scan_Conjure_Path = ChangeInfo.LocalPath;
                Debug.Log(scan_Conjure_Path);
            }
        }
    }
    void Start()
    {
        if (PlayerPrefs.GetInt("ARScan") == 0)
        {
            PlayerPrefs.SetInt("ARScan", 1);
            ARScanGuide.SetActive(true);
            isGuide = true;
        }
        else
        {
            isGuide = false;
        }
        //Invoke("LoadAllXML", 1);
        Invoke("GetImageList", 1);
    }

    // private bool showtip;
    public float waittime;
    private void Update()
    {
        waittime += Time.deltaTime;
        if (waittime>30)
        {
            waittime = 0;
            showtoast("扫描未成功？点击右上角按钮获取帮助");
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isShowHelp)
            {
                isShowHelp = false;
                ShowHelpPanel(false);
            }
            else
            {
                for (int i = 0; i < trackerList.Count; i++)
                {
                    trackerList[i].UnloadAB();
                }
                CoroutineWrapper.EXES(0.2f, () => 
                {
                    UnityHelper.LoadNextScene("main");
                });
            }
        }

        // if (ARScanTrackableEventHandler.curCount >= 3 && !showtip)
        // {
        //     showtip = true;
        //     showtoast("扫描太快啦~");
        //     CoroutineWrapper.EXES(2, () =>
        //     {
        //         showtip = false;
        //     });
        // }
        ShowBtn();
    }

    void ShowBtn()
    {
        int count = 0;
        for (int i = 0; i < trackerList.Count; i++)
        {
            if (trackerList[i].istracking)
            {
                count++;
            }
        }

        ShotBtn.SetActive(count > 0);
    }
    public void scanImgTweenOver()
    {
        StartCoroutine(ImgTweenOver());
    }
    public bool IsTween = true;
    public bool isScan = false;
    IEnumerator ImgTweenOver()
    {
        yield return new WaitForSeconds(0.5f);
        IsTween = false;
        ed.enabled = false;
        yield return new WaitForSeconds(1);
        if (!isScan)
        {
            scanImg.DORestart(true);
            IsTween = true;
            yield return new WaitForSeconds(1.5f);
            ed.enabled = true;
        }
        else
        {
            StartCoroutine(ImgTweenOver());
        }
    }

    public void ShowShotBtn()
    {
        ShotBtn.SetActive(true);
    }
    public void HideShotBtn()
    {
        ShotBtn.SetActive(false);
    }
    public void LoadAllXML()
    {
        //1.找到资源保存的文件夹
        string assetDirectory = PublicAttribute.LocalFilePath + "dataset";

        DirectoryInfo directoryInfo = new DirectoryInfo(assetDirectory);

        if (directoryInfo == null)
        {
            Debug.LogError(directoryInfo + " 不存在!");
            return;
        }
        else
        {

            foreach (FileInfo file in directoryInfo.GetFiles("*"))
            {
                int index = file.FullName.LastIndexOf(".");
                string suffix = file.FullName.Substring(index + 1);
                if (suffix == "xml")
                {
                    Debug.Log(file.Name);
                    LoadDataset(file.Name);
                }
            }
            InitImageTarget();
        }
    }

    public void GetImageList()
    {
        foreach (var ChangeInfo in mainUISet.curScenicInfo.ResourcesInfos)
        {
            if (ChangeInfo.ResourcesKey == "scan-ticket" || ChangeInfo.ResourcesKey == "scan-native-product" || ChangeInfo.ResourcesKey == "scan-more" || ChangeInfo.ResourcesKey == "scan-conjure")
            {
                string LocalPath = ChangeInfo.LocalPath;
                Debug.Log(LocalPath);

                DirectoryInfo directoryInfo = new DirectoryInfo(LocalPath);

                if (directoryInfo == null)
                {
                    Debug.LogError(directoryInfo + " 不存在!");
                    return;
                }
                else
                {
                    foreach (FileInfo file in directoryInfo.GetFiles("*"))
                    {
                        int index = file.FullName.LastIndexOf(".");
                        string suffix = file.FullName.Substring(index + 1);
                        if (suffix == "xml")
                        {
                            LoadDataset(file.FullName);
                        }
                    }
                }
            }
        }
        InitImageTarget();
    }
    public void LoadDataset(string name)
    {
        ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        DataSet m_Dataset = tracker.CreateDataSet();
        m_Dataset.Load(name, VuforiaUnity.StorageType.STORAGE_ABSOLUTE);
        tracker.Stop();
        tracker.ActivateDataSet(m_Dataset);
        tracker.Start();
    }

    private List<ARScanTrackableEventHandler> trackerList = new List<ARScanTrackableEventHandler>();

    /// <summary>
    /// 将每个ImageTarget改名并且挂上脚本
    /// </summary>
    public void InitImageTarget()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "New Game Object")
            {
                go.AddComponent<TurnOffBehaviour>();
                go.AddComponent<DefaultTrackableEventHandler>();
                go.AddComponent<ARScanTrackableEventHandler>();
                
                trackerList.Add(go.GetComponent<ARScanTrackableEventHandler>());
            }
        }
    }
    public void LoadScene(string scenename)
    {
        for (int i = 0; i < trackerList.Count; i++)
        {
            trackerList[i].UnloadAB();
        }
        CoroutineWrapper.EXES(0.2f, () => 
        {
            UnityHelper.LoadNextScene(scenename);
        });
       
    }


    public void ShowGameObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    
    private bool isShowHelp;
    public void ShowHelpPanel(bool value)
    {      
        isShowHelp = value;
        helpPanel.SetActive(value);
    }
    
    public void ShowScanGuide()
    {
        for (int i = 0; i < trackerList.Count; i++)
        {
            trackerList[i].HideInfo();
        }
        
        ARScanGuide.SetActive(true);
        guide.ReSetPos();
        
    }
    IEnumerator ScanGuide()
    {
        yield return new WaitForSeconds(2);
        ScanGuideImg.texture = ScanGuideTex[1];
    }

    public void HideScanGuide()
    {
        isGuide = false;
        ARScanGuide.SetActive(false);
    }

    private bool isShooting;
    public void ShotPic()
    {
        if (!isShooting)
        {
            isShooting = true;
            btnPanel.SetActive(false); 
            shotclip.Play();
            ScreenshotManager.SaveScreenshot("Scan");
            CoroutineWrapper.EXES(1.5f, () =>
            {
                showtoast("照片已保存");
                CoroutineWrapper.EXES(1.5f, () =>
                {
                    isShooting = false;
                    btnPanel.SetActive(true);
                });
            });
        }
    }

    public void showtoast(string content)
    {
        toastObj.text = content;
        toastObj.gameObject.SetActive(true);
        CoroutineWrapper.EXES(1.5f, () =>
        {
            toastObj.gameObject.SetActive(false);
        });
    }
} 