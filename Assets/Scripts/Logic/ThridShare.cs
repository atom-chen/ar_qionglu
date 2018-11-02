using cn.sharesdk.unity3d;
using UnityEngine;

public class ThridShare : MonoBehaviour
{
    private ShareSDK ssdk;

    private void Start()
    {
        ssdk = GameObject.Find("Root").GetComponent<ShareSDK>();
    }

    public void ShowShareMenu()
    {
        ShareContent content = new ShareContent();
        content.SetText("视觉美景+智能呈现  只留精彩，不留遗憾");
        content.SetImageUrl("https://gitee.com/visizencom/ar_game/raw/054c0ae483c0b9a65319815d5d76b92aea4668fc/Images/Logo.png");
        content.SetTitle("AR游");
        content.SetShareType(ContentType.Webpage);
        content.SetTitleUrl("http://download.vszapp.com");
        content.SetUrl("http://download.vszapp.com");

        
        // ShareContent content = new ShareContent();
        // content.SetText("视觉美景+智能呈现  只留精彩，不留遗憾");
        // content.SetTitle("AR游");
        // content.SetImageUrl("https://gitee.com/visizencom/ar_game/raw/054c0ae483c0b9a65319815d5d76b92aea4668fc/Images/Logo.png");
        // content.SetUrl("http://download.vszapp.com");
        // content.SetShareType(ContentType.Webpage);
        // ssdk.ShareContent (PlatformType.WeChatMoments, content);

        
        
        
        ShareContent SinaShareParams = new ShareContent();
        SinaShareParams.SetText("视觉美景+智能呈现  只留精彩，不留遗憾");
        SinaShareParams.SetImageUrl("https://gitee.com/visizencom/ar_game/raw/054c0ae483c0b9a65319815d5d76b92aea4668fc/Images/Logo.png");
        SinaShareParams.SetShareType(ContentType.Webpage);
        SinaShareParams.SetObjectID("SinaID");
        content.SetShareContentCustomize(PlatformType.SinaWeibo, SinaShareParams);

        //通过分享菜单分享
        ssdk.ShowPlatformList(null, content, 100, 100);
    }
}