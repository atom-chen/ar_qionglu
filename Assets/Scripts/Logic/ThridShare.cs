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

        //(Android only) 隐藏九宫格里面不需要用到的平台（仅仅是不显示平台）
        //(Android only) 也可以把jar包删除或者把Enabl属性e改成false（对应平台的全部功能将用不了）
        //string[] platfsList = { ((int)PlatformType.QQ).ToString(), ((int)PlatformType.WeChat).ToString(), ((int)PlatformType.SinaWeibo).ToString() };
        //content.SetHidePlatforms(platfsList);

        //content.SetText("AR凉山游，带你玩转凉山！");
        //content.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
        //content.SetTitle("AR凉山游");

        ////(Android only) 针对Android绕过审核的多图分享，传图片String数组
        //string[] imageArray = { "/sdcard/test.jpg", "http://f1.webshare.mob.com/dimgs/1c950a7b02087bf41bc56f07f7d3572c11dfcf36.jpg", "/sdcard/test.jpg" };
        //content.SetImageArray(imageArray);

        //content.SetTitleUrl("http://www.visizen.com");
        //content.SetSite("威视真科技");
        //content.SetSiteUrl("http://www.visizen.com");
        //content.SetUrl("http://www.visizen.com");
        //content.SetComment("power by visizen");
        ////content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
        //content.SetShareType(ContentType.Image);

        //ShareContent QQShareParams = new ShareContent();
        //QQShareParams.SetTitle("AR凉山游");
        //QQShareParams.SetText("AR凉山游，带你玩转凉山");
        //QQShareParams.SetTitleUrl("http://www.visizen.com");
        //QQShareParams.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
        //QQShareParams.SetShareType(ContentType.Image);
        //content.SetShareContentCustomize(PlatformType.QQ, QQShareParams);

        //ShareContent WeChatShareParams = new ShareContent();
        //WeChatShareParams.SetTitle("AR凉山游");
        //WeChatShareParams.SetText("AR凉山游，带你玩转凉山");
        //WeChatShareParams.SetTitleUrl("http://www.visizen.com");
        //WeChatShareParams.SetUrl("http://www.visizen.com");
        //WeChatShareParams.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
        //WeChatShareParams.SetShareType(ContentType.Image);
        //content.SetShareContentCustomize(PlatformType.WechatPlatform, WeChatShareParams);

        content.SetText("只留精彩，不留遗憾。");
        content.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
        content.SetTitle("AR凉山游");
        content.SetTitleUrl("http://www.visizen.com");
        content.SetUrl("http://www.visizen.com");

        ShareContent SinaShareParams = new ShareContent();
        SinaShareParams.SetText("只留精彩，不留遗憾。");
        SinaShareParams.SetImageUrl("http://git.oschina.net/alexyu.yxj/MyTmpFiles/raw/master/kmk_pic_fld/small/107.JPG");
        SinaShareParams.SetShareType(ContentType.Text);
        SinaShareParams.SetObjectID("SinaID");
        content.SetShareContentCustomize(PlatformType.SinaWeibo, SinaShareParams);
        //优先客户端分享
        // content.SetEnableClientShare(true);

        //使用微博API接口应用内分享 iOS only
        // content.SetEnableSinaWeiboAPIShare(true);

        //通过分享菜单分享
        ssdk.ShowPlatformList(null, content, 100, 100);
    }
}