using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using cn.sharesdk.unity3d;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace mainpage
{
	public class BaseUI : MonoBehaviour
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
      
			ShareContent SinaShareParams = new ShareContent();
			SinaShareParams.SetText("视觉美景+智能呈现  只留精彩，不留遗憾");
			SinaShareParams.SetImageUrl("https://gitee.com/visizencom/ar_game/raw/054c0ae483c0b9a65319815d5d76b92aea4668fc/Images/Logo.png");
			SinaShareParams.SetShareType(ContentType.Webpage);
			SinaShareParams.SetObjectID("SinaID");
			content.SetShareContentCustomize(PlatformType.SinaWeibo, SinaShareParams);
			ssdk = GameObject.Find("Root").GetComponent<ShareSDK>();
			//通过分享菜单分享
			ssdk.ShowPlatformList(null, content, 100, 100);
		}
		/// <summary>
		/// 设置是否显示隐藏
		/// </summary>
		/// <param name="state"></param>
		/// <param name="action"></param>
		public  void SetState(GameObject obj,bool state,Action<bool> callback)
		{
			if (obj!=null)
			{
				obj.SetActive(state);
				if (callback != null)
				{
					callback(true);
				}
			}
			else
			{
				if (callback != null)
				{
					callback(false);
				}
			}
		}

		public void Debuger(string message)
		{
			// Debug.Log(message);
		}
		
		/// <summary>
		/// 验证手机号是否符合格式
		/// </summary>
		public bool VerifyPhoneNo(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			if (str.Length != 11)
			{
				return false;
			}
			return true;
		} 
		/// <summary>
		/// 验证验证码是否符合格式
		/// </summary>
		public bool VerifySMSCode(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			if (str.Length != 6)
			{
				return false;
			}
			return true;
		}
		public void FreezeButton(Button btn, float time = 60)
		{
			Text text = btn.gameObject.GetComponentInChildren<Text>();
			string oldText = text.text;

			StartCoroutine(changeTime(btn, text, time, oldText));
		}

		IEnumerator changeTime(Button btn, Text text, float time, string oldText)
		{
			btn.interactable = false;
			while (time > 0)
			{
				text.text = time + "s";
				//暂停一秒
				yield return new WaitForSeconds(1);
				time--;
			}
			btn.interactable = true;
			text.text = oldText;
		}
		//退出登录
		public void LogOut()
		{
			HttpManager.Instance.Logout((b =>
			{
				if (!b)
				{

				}
			}));
			mainUISet.lastpagename = mainUISet.UIname.main;
			mainUISet.lastpageid = 0;
			File.Delete(PublicAttribute.LocalFilePath + "APP/Token.json");
			SceneManager.LoadScene("Login");
		}
	}
}