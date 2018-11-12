using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Vuforia;
public class ARScanTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    private bool isLoading;
    public GameObject loadingImg;
    string name;
    GameObject child;
    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
  
    protected virtual void Start()
    {
        loadingImg = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/LoadingImg"));
        loadingImg.transform.SetParent(transform);
        loadingImg.transform.localEulerAngles = Vector3.zero;
        loadingImg.transform.localPosition = Vector3.zero;
        loadingImg.transform.localScale = Vector3.one *0.03f;
        AssetBundle.UnloadAllAssetBundles(false);
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS
    public  bool istracking,isloaded;
    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED )
        {
            OnTrackingFound();
            istracking = true;
            name = mTrackableBehaviour.TrackableName;
            transform.name = name;
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            if (!isLoading && !isloaded && !ARScanManager.instance.isGuide)
            {
                loadingImg.transform.localScale = Vector3.one *0.03f;
                LoadAssetbundle();
                isLoading = true;
            }
            else if (isloaded)
            {
                ARScanManager.instance.isScan = true;
                ShowInfo();
            }
            // ARScanManager.instance.ShowShotBtn();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE ||
                 newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingLost();
            istracking = false;
            if (child != null)
                if (child.GetComponent<Targets>() != null)
                {          
                    Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
                    ARScanManager.instance.isScan = false;
                    HideInfo();
                }
            
            // ARScanManager.instance.HideShotBtn();
        }
        else
        {
            istracking = false;
            OnTrackingLost();
            if (child != null)
                if (child.GetComponent<Targets>() != null)
                {
                    ARScanManager.instance.isScan = false;
                    HideInfo();
                }

            // ARScanManager.instance.HideShotBtn();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS
    protected virtual void LoadAssetbundle()
    {
        StartCoroutine(LoadAssets());
    }
    private IEnumerator LoadAssets()
    {
        string path = PublicAttribute.LocalFilePath + "scan_Ticket/1/Target" + name + ".vsz";
        switch (transform.name)
        {
            case "1":
                path = PublicAttribute.LocalFilePath + "scan_More/1/Target" + name + ".vsz";
                break;
            case "2":
                path = PublicAttribute.LocalFilePath + "scan_More/1/Target" + name + ".vsz";
                break;
            case "3":
                path = PublicAttribute.LocalFilePath + "scan_More/1/Target" + name + ".vsz";
                break;
            case "4":
                path = PublicAttribute.LocalFilePath + "scan_More/1/Target" + name + ".vsz";
                break;
            case "5":
                path = PublicAttribute.LocalFilePath + "scan_More/1/Target" + name + ".vsz";
                break;
            case "6":
                path = PublicAttribute.LocalFilePath + "scan_More/1/Target" + name + ".vsz";
                break;
            case "7":
                path = PublicAttribute.LocalFilePath + "scan_More/1/Target" + name + ".vsz";
                break;
            case "8":
                path = PublicAttribute.LocalFilePath + "scan_More/1/Target" + name + ".vsz";
                break;
            case "beidian":
                path = PublicAttribute.LocalFilePath + "scan_ProductPath/1/Target" + name + ".vsz";
                break;
            case "panzi":
                path = PublicAttribute.LocalFilePath + "scan_ProductPath/1/Target" + name + ".vsz";
                break;
            case "kqc":
                path = PublicAttribute.LocalFilePath + "scan_ProductPath/1/Target" + name + ".vsz";
                break;
            case "lushan":
                path = PublicAttribute.LocalFilePath + "scan_Ticket/1/Target" + name + ".vsz";
                break;
            case "shuixiang":
                path = PublicAttribute.LocalFilePath + "scan_Ticket/1/Target" + name + ".vsz";
                break;
            case "zhaohuan":
                path = PublicAttribute.LocalFilePath + "scan_Conjure/1/Target" + name + ".vsz";
                break;

        }
        WWW bundle = WWW.LoadFromCacheOrDownload("file:///" + path,0);
        yield return bundle;
        Debug.Log(bundle.bytesDownloaded);
        Debug.Log("Target" + name);
        var data = bundle.assetBundle;
        yield return new WaitForEndOfFrame();
        Object obj = data.LoadAsset("Target"+ name);
        child = GameObject.Instantiate(obj) as GameObject;
        child.transform.SetParent(transform);
        child.transform.localEulerAngles = Vector3.zero;
        child.transform.localPosition = Vector3.zero;
        child.transform.localScale = Vector3.one;
        loadingImg.transform.localScale = Vector3.zero;
        isLoading = false;
        isloaded = true;
        if (transform.name=="zhaohuan")
        {
            transform.GetComponent<VirtualButtonEventHandler>().bbb = child.GetComponent<bird3>();
        }
        yield return new WaitForEndOfFrame();
        if (istracking)
            ShowInfo();
    }
    public void ShowInfo()
    {
        if (child != null)
        {  
            if (child.GetComponent<Targets>() != null)
                child.GetComponent<Targets>().ShowInfo();
        } 
    }
    public void HideInfo()
    {
        istracking = false;
        if (child != null)
        {  
            if (child.GetComponent<Targets>() != null)
                child.GetComponent<Targets>().HideInfo();
        }
       
    }
    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }
    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PROTECTED_METHODS
}

