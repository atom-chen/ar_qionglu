using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class ARScanManager : MonoBehaviour
{
    public static ARScanManager instance;

    public RawImage ScanGuideImg;
    public Texture[] ScanGuideTex;
    public GameObject ARScanGuide;

    public EdgeDetection ed;
    public DOTweenAnimation scanImg;
    public GameObject toastObj;

    public static string scan_more_Path = "";
    public static string scan_ticket_Path = "";
    public static string scan_native_product_Path = "";
    public static string scan_Conjure_Path = "";
    private void Awake()
    {
        instance = this;

        foreach (var ChangeInfo in mainPageUI.curScenicInfo.ResourcesInfos)
        {
            if (ChangeInfo.ResourcesKey == "vsz-scan-more")
            {
                scan_more_Path = ChangeInfo.LocalPath;
                Debug.Log(scan_more_Path);
            }
            else if (ChangeInfo.ResourcesKey == "vsz-scan-ticket")
            {
                scan_ticket_Path = ChangeInfo.LocalPath;
                Debug.Log(scan_ticket_Path);
            }
            else if (ChangeInfo.ResourcesKey == "vsz-scan-native-product")
            {
                scan_native_product_Path = ChangeInfo.LocalPath;
                Debug.Log(scan_native_product_Path);
            }
            else if (ChangeInfo.ResourcesKey == "vsz-scan-conjure")
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
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        //Invoke("LoadAllXML", 1);
        Invoke("GetImageList", 1);
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
        foreach (var ChangeInfo in mainPageUI.curScenicInfo.ResourcesInfos)
        {
            if (ChangeInfo.ResourcesKey == "vsz-scan-ticket" || ChangeInfo.ResourcesKey == "vsz-scan-native-product" || ChangeInfo.ResourcesKey == "vsz-scan-more" || ChangeInfo.ResourcesKey == "vsz-scan-conjure")
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
            }
        }
    }
    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void ShowGameObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void ShowScanGuide()
    {
        StartCoroutine(ScanGuide());
    }
    IEnumerator ScanGuide()
    {
        yield return new WaitForSeconds(2);
        ScanGuideImg.texture = ScanGuideTex[1];
    }

    public void HideScanGuide()
    {
        StopCoroutine(ScanGuide());
        ScanGuideImg.texture = ScanGuideTex[0];
        ARScanGuide.SetActive(false);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private bool isShooting;
    public void ShotPic()
    {
        if (!isShooting)
        {
            isShooting = true;
            ScreenshotManager.SaveScreenshot("Scan");
            toastObj.SetActive(true);
            CoroutineWrapper.EXES(1.5f, () =>
            {
                toastObj.SetActive(true);
                CoroutineWrapper.EXES(1.5f, () =>
                {
                    isShooting = false;
                    toastObj.SetActive(false);
                });
            });
        }
    }
}