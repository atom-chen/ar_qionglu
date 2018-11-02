using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class BDManager : MonoBehaviour
{
    public static BDManager _Instance;
    public AudioSource aud;
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
        aud.Play();
    }
    void Lost()
    {
        aud.Stop();
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
