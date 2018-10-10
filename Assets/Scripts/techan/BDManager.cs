using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class BDManager : MonoBehaviour
{
    public static BDManager _Instance;
    void Awake()
    {
        _Instance = this;

    }
    private void Start()
    {
    }
    public void OnTrackingFound()
    {
        Found();
    }
    public void OnTrackingLost()
    {
        Lost();
    }

    void Found()
    {
        CiqiManager.instance.LostEvent();
        KQCManager._Instance.OnTrackingLost();
        AudioManager.instance.PlayFX("beidian");
    }
    void Lost()
    {
        AudioManager.instance.StopAll();
    }
    private bool Check(params Component[] com)
    {
        foreach (Component item in com)
        {
            if (item == null)
            {
                return false;
            }
        }
        return true;
    }

}
