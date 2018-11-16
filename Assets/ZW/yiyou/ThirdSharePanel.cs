using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdSharePanel : SingletonMono<ThirdSharePanel>
{
    public GameObject ImageShare, VideoShare;

    public Button weShareBtn, weMomentShareBtn, qqShareBtn, sinaWeiboShareBtn, sinaWeiboShareVideoBtn;
    private string filePath = string.Empty;
    public override void Awake()
    {
        base.Awake();
   

    }
    private void Start()
    {

        weShareBtn.onClick.AddListener(() =>
        {
            
        ShareScriptsBase.Instance.    WEShare();
        });
        weMomentShareBtn.onClick.AddListener(() =>
        {

            ShareScriptsBase.Instance.WeMomentShare();
        });
        qqShareBtn.onClick.AddListener(() =>
        {
            ShareScriptsBase.Instance.QQShare();
        });
        sinaWeiboShareBtn.onClick.AddListener(() =>
        {

            ShareScriptsBase.Instance.SinaWeiboShare();
        });
        sinaWeiboShareVideoBtn.onClick.AddListener(() =>
        {

            ShareScriptsBase.Instance.SinaWeiboShareVideo(filePath);
        });
    }
    public void Init(string filePath = "", bool isShareImage = true)
    {
       
            ImageShare.gameObject.SetActive(isShareImage);
        VideoShare.gameObject.SetActive(!isShareImage);
        this.filePath = filePath;
    }
}
