
using UnityEngine; 
using System;
using System.Collections;
using System.Collections.Generic;
namespace cn.sharesdk.unity3d
{
	/// <summary>
	/// Content type.
	/// </summary>
	public class ShareContent
	{	      		
		Hashtable shareParams = new Hashtable();
		Hashtable customizeShareParams = new Hashtable();

		/*Android Only- 隐藏九宫格的平台（平台id数组,例如：(int)PlatformType.Facebook）*/
		public void SetHidePlatforms(String[] hidePlatformList) {
			shareParams["hidePlatformList"] = String.Join (",", hidePlatformList);
		}

		/*iOS/Android*/
		public void SetTitle(String title) {
			shareParams["title"] = title;
		}

		/*iOS/Android*/
		public void SetText(String text) {
			shareParams["text"] = text;
		}

		/*iOS/Android*/
		public void SetUrl(String url) {
			shareParams["url"] = url;
		}

	
        /// <summary>
        /// 	/*iOS/Android - 本地图片路径*/
        /// </summary>
        /// <param name="imagePath"></param>
		public void SetImagePath(String imagePath) {
			#if UNITY_ANDROID
			shareParams["imagePath"] = imagePath;
			#elif UNITY_IPHONE
			shareParams["imageUrl"] = imagePath;
			#endif
		}

	
        /// <summary>
        /// 	/*iOS/Android - 网络图片路径*/
        /// </summary>
        /// <param name="imageUrl"></param>
		public void SetImageUrl(String imageUrl) {
			shareParams["imageUrl"] = imageUrl;
		}

		/*Android Only- 图片string数组（多图分享）*/
		public void SetImageArray(String[] imageArray) {
			shareParams["imageArray"] = String.Join(",", imageArray);
		}

		
        /// <summary>
        /// /*iOS/Android - 分享类型*/
        /// </summary>
        /// <param name="shareType"></param>
		public void SetShareType(int shareType) {
			#if UNITY_ANDROID
			if (shareType == 0) {
				shareType = 1;
			} else if(shareType == 10){
				shareType = 11;
			} 
			#endif
			shareParams["shareType"] = shareType;
		}
        /// <summary>
        /// 		/*Android Only*/
        /// </summary>
        /// <param name="titleUrl"></param>
		public void SetTitleUrl(String titleUrl) {
			shareParams["titleUrl"] = titleUrl;
		}
        /// <summary>
        ///         /*iOS/Android*/
        /// </summary>
        /// <param name="titleUrl"></param>
        public void SetComment(String comment) {
			shareParams["comment"] = comment;
		}
        /// <summary>
        ///         /*Android Only*/
        /// </summary>
        /// <param name="titleUrl"></param>
        public void SetSite(String site) {
			shareParams["site"] = site;
		}

        /// <summary>
        ///         /*Android Only*/
        /// </summary>
        /// <param name="titleUrl"></param>
        public void SetSiteUrl(String siteUrl) {
			shareParams["siteUrl"] = siteUrl;
		}
        /// <summary>
        ///         /*Android Only*/
        /// </summary>
        /// <param name="titleUrl"></param>
        public void SetAddress(String address) {
			shareParams["address"] = address;
		}
        /// <summary>
        ///         /*iOS/Android*/
        /// </summary>
        /// <param name="titleUrl"></param>
        public void SetFilePath(String filePath) {
			shareParams["filePath"] = filePath;
		}

		/*iOS/Android*/
		public void SetMusicUrl(String musicUrl) {
			shareParams["musicUrl"] = musicUrl;
		}

		/*iOS/Android - Sina/Tencent/Twitter/VKontakte*/
		public void SetLatitude(String latitude) {
			shareParams["latitude"] = latitude;
		}

		/*iOS/Android - Sina/Tencent/Twitter/VKontakte*/
		public void SetLongitude(String longitude) {
			shareParams["longitude"] = longitude;
		}

		
		/*iOS/Android - WhatsApp/Youtube/ MeiPai/Sina(the path must be an assetUrl path in iOS)*/
		public void SetVideoPath(String videoPath){
			#if UNITY_ANDROID
			shareParams["filePath"] = videoPath;
			#elif UNITY_IPHONE
			shareParams ["videoPath"] = videoPath;
			#endif
		}
		

			
		/*iOS Only - Sina*/
		public void SetObjectID(String objectId) {
			shareParams["objectID"] = objectId;
		}



		/*iOS Only - Wechat*/
		public void SetEmotionPath(String emotionPath){
			shareParams["emotionPath"] = emotionPath;
		}

		/*iOS Only - Wechat/Yixin*/
		public void SetExtInfoPath(String extInfoPath){
			shareParams["extInfoPath"] = extInfoPath;
		}

		/*iOS Only - Wechat*/ 
		public void SetSourceFileExtension(String sourceFileExtension){
			shareParams["sourceFileExtension"] = sourceFileExtension;
		}

		/*iOS Only - Wechat*/
		public void SetSourceFilePath(String sourceFilePath){
			shareParams["sourceFilePath"] = sourceFilePath;
		}

		/*iOS Only - QQ/Wechat/Yixin*/
		public void SetThumbImageUrl(String thumbImageUrl){
			shareParams["thumbImageUrl"] = thumbImageUrl;
		}



		public void SetEnableClientShare(bool enalble){
			shareParams ["clientShare"] = enalble;
		}

		//iOS Only 用于启用新浪微博的api分享
		public void SetEnableSinaWeiboAPIShare(bool enalble){
			shareParams ["apiShare"] = enalble;
		}

		//iOS Only 应用内分享时使用微博高级接口 v3.6.3  v4.0.1 弃用
		public void SetEnableAdvancedInterfaceShare(bool enalble){
			shareParams ["advancedShare"] = enalble;
		}

		// iOS v4.0.8 新浪微博 分享到Story开关
		public void SetSinaShareEnableShareToStory(bool enalble){
			shareParams ["isShareToStory"] = enalble;
		}

		// iOS/Android 分享小程序的ID
		public void SetMiniProgramUserName(String userName){
			shareParams ["wxUserName"] = userName;
		}

		// iOS/Android 微信小程序的页面路径
		public void SetMiniProgramPath(String path){
			shareParams ["wxPath"] = path;
		}

		// iOS/Android 微信小程序 withTicket开关
		public void SetMiniProgramWithShareTicket(bool enalble){
			shareParams ["wxWithShareTicket"] = enalble;
		}

		// iOS/Android 分享小程序的版本（0-正式，1-开发，2-体验）
		public void SetMiniProgramType(int type){
			shareParams ["wxMiniProgramType"] = type;
		}

		// iOS only 高清缩略图，建议长宽比是 5:4 ,6.5.9及以上版本微信客户端小程序类型分享使用 要求图片数据小于128k
		public void SetMiniProgramHdThumbImage(string hdThumbImage){
			shareParams ["wxMiniProgramHdThumbImage"] = hdThumbImage;
		}


        /// <summary>
        ///  不同平台分享不同内容,platform:平台；content： 自定义的分享内容
        /// </summary>
        /// <param name="imagePath"></param>
        public void SetShareContentCustomize(PlatformType platform, ShareContent content)
        {
			customizeShareParams [(int)platform] = content.GetShareParamsStr();
		}

		public String GetShareParamsStr() {
			if (customizeShareParams.Count > 0) {
				shareParams["customizeShareParams"] = customizeShareParams;
			}
			String jsonStr = MiniJSON.jsonEncode (shareParams);
			Debug.Log("ParseShareParams  ===>>> " + jsonStr );
			return jsonStr;
		}

		public Hashtable GetShareParams() {
			if (customizeShareParams.Count > 0) {
				shareParams["customizeShareParams"] = customizeShareParams;
			}
			String jsonStr = MiniJSON.jsonEncode (shareParams);
			Debug.Log("ParseShareParams  ===>>> " + jsonStr );
			return shareParams;
		}
	}

}

