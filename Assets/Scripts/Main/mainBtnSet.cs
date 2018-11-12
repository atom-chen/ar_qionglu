using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace mainpage
{
	public class mainBtnSet : BaseUI
	{
		private mainUISet mainui;
		[Header("主界面")] 
		public Button btn_touser;
		public Button btn_tospot;
		public Button btn_tochange;
		public Button btn_togps;
		public Button btn_toarscan;
		public Button btn_toyiyou;
		
		[Header("景点列表")] 
		public Button btn_spotback;
		
		[Header("移空换景列表")] 
		public Button btn_changeback;
		public Button btn_changtogpscancel;
		public Button btn_changtogpssure;
		
		[Header("个人中心")] 
		public Button btn_centerback;
		public Button btn_centertofoot;
		public Button btn_centertoabout;
		public Button btn_centertoset;
		public Button btn_centerlogout;
		public Button btn_centertoeditor;
		public Button btn_centerenter;
		
		[Header("编辑界面")] 
		public Button btn_editorback;
		public Button btn_editor_nameback;
		public Button btn_editor_phoback;
		public Button btn_editor_passwordback;
		public Button btn_editor_headback;
		
		public Button btn_editortoname;
		public Button btn_editortopho;
		public Button btn_editortoword;
		public Button btn_editortohead;

		public Button[] headbtn;

		public Button namesure;
		public Text nametext;
		public Button emptyname;
		
		
		[Header("About界面")] 
		public Button btn_aboutback;
		public Button btn_yinsiback;
		public Button btn_xieyiback;
		public Button btn_showyinsi;
		public Button btn_showxieyi;
		public Button btn_update;
		
		[Header("Set界面")] 
		public Button btn_setback;
		public Button btn_fankuiback;
		public Button btn_shareapp;
		public Button btn_settofankui;
		public Button btn_fankuisure;
		public Text fankuitext;

		public GameObject[] btn_fankuitype;
		
		[Header("下载")] 
		public Button btn_down;
		
		
		[Header("换绑手机号页面")]  
		public LoginUIPopupPage PP;
		public InputField BindingPage_PhoneNoInputField;
		public InputField BindingPage_SmsCodeInputField;
		public Button BindingPage_BindingBtn;
		public Button BindingPage_GetSMSBtn;
		private bool isHit;

		// Use this for initialization
		void Start ()
		{
			mainui = GetComponent<mainUISet>();
			
			#region 主页
			btn_touser.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.user_center,0 );
			});
			btn_tospot.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.spot,0 );
			});
			btn_tochange.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.change,0 );
			});
			btn_togps.onClick.AddListener(delegate
			{
				mainui.LoadScene("gpsConvert");
			});
			btn_toarscan.onClick.AddListener(delegate
			{
				mainui.LoadScene("ARScan");
			});
			btn_toyiyou.onClick.AddListener(delegate
			{
				mainui.LoadScene("yiyou");
			});
			

			#endregion
			
			#region 景点列表

			btn_spotback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.spot,0);
			});

			#endregion
			
			#region 移空换景列表

			btn_changeback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.change,0);
			});
			btn_changtogpscancel.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.change,1);
			});
			btn_changtogpssure.onClick.AddListener(delegate
			{
				mainui.LoadScene("gpsConvert");
			});
			#endregion
			
			#region 个人中心

			btn_centerback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_center,0);
			});
			
			 btn_centertofoot.onClick.AddListener(delegate
			 {
				 mainui.LoadScene("Track");
			 });
			 btn_centertoabout.onClick.AddListener(delegate
			 {
				 mainui.ShowUI(mainUISet.UIname.user_about,0);
			 });
			 btn_centertoset.onClick.AddListener(delegate
			 {
				 mainui.ShowUI(mainUISet.UIname.user_set,0);
			 });
			 btn_centerlogout.onClick.AddListener(delegate
			 {
				 LogOut();
			 });
			 btn_centertoeditor.onClick.AddListener(delegate
			 {
				 mainui.ShowUI(mainUISet.UIname.user_editor,0);
			 });
			btn_centerenter.onClick.AddListener(delegate
			{
				LogOut();
			});

			#endregion
			
			#region 编辑界面

			btn_editorback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_editor,0);
			});
			btn_editor_nameback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_editor,1);
			});
			btn_editor_phoback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_editor,2);
			});
			btn_editor_passwordback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_editor,3);
			});
			btn_editor_headback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_editor,4);
			});
			
			
			
			btn_editortoname.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.user_editor,1);
			});
			btn_editortopho.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.user_editor,2);
			});
			btn_editortoword.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.user_editor,3);
			});
			btn_editortohead.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.user_editor,4);
			});

			headbtn[0].onClick.AddListener(delegate
			{			
				mainui.SetHeadID(0);
			});
			headbtn[1].onClick.AddListener(delegate
			{			
				mainui.SetHeadID(1);
			});
			headbtn[2].onClick.AddListener(delegate
			{			
				mainui.SetHeadID(2);
			});
			headbtn[3].onClick.AddListener(delegate
			{			
				mainui.SetHeadID(3);
			});
			headbtn[4].onClick.AddListener(delegate
			{			
				mainui.SetHeadID(4);
			});
			headbtn[5].onClick.AddListener(delegate
			{			
				mainui.SetHeadID(5);
			});
			
			
			namesure.onClick.AddListener(delegate
			{			
				SetUserName(nametext);
			});
			
			
			emptyname.onClick.AddListener(delegate
			{
				nametext.GetComponentInParent<InputField>().text = "";
			});

			#endregion
			
			#region About界面

			btn_aboutback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_about,0);
			});
			btn_yinsiback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_about,1);
			});
			btn_xieyiback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_about,2);
			});
			btn_showyinsi.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.user_about,1);
			});
			btn_showxieyi.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.user_about,2);
			});
			btn_update.onClick.AddListener(delegate
			{
				mainui.isTip = true;
				HttpManager.Instance.UpdateApp((b)=>
				{
					if (b.Contains("200"))
					{
						JsonData content = JsonMapper.ToObject(b);
						JsonData date = content["data"];
						if (date["appVersion"].ToString()==GlobalInfo.APPversion)
						{
							PP.ShowPopup("","当前已是最新版本");
						}
						else
						{
							PP.ShowUpdatePP();
						}
					}
					else
					{
						PP.ShowPopup("","网络出错！");
					}

				});
			});
			#endregion
			
			#region Set界面

			btn_setback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_set,0);
			});
			btn_fankuiback.onClick.AddListener(delegate
			{
				mainui.HideUI(mainUISet.UIname.user_set,1);
				for (int i = 0; i < btn_fankuitype.Length; i++)
				{
					btn_fankuitype[i].SetActive(false);
				}
			});
			btn_shareapp.onClick.AddListener(delegate
			{
				ShowShareMenu();
			});
			btn_settofankui.onClick.AddListener(delegate
			{
				mainui.ShowUI(mainUISet.UIname.user_set,1);
			});
			btn_fankuisure.onClick.AddListener(delegate
			{
				Suggest(fankuitext);
			});

			#endregion
			
			#region 编辑手机号
			BindingPage_BindingBtn.onClick.AddListener((() =>
			{
				if (VerifyPhoneNo(BindingPage_PhoneNoInputField.text) && VerifySMSCode(BindingPage_SmsCodeInputField.text))
				{
					if (!isHit)
					{
						isHit = true;
						HttpManager.Instance.ChangePhoneNo(BindingPage_PhoneNoInputField.text, BindingPage_SmsCodeInputField.text, (PopupInfo));
					}
              
				}
				else
				{
					PP.ShowPopup("格式不正确", "请输入正确的格式");
				}
			}));

			BindingPage_GetSMSBtn.onClick.AddListener((() =>
			{
				if (VerifyPhoneNo(BindingPage_PhoneNoInputField.text))
				{
					FreezeButton(BindingPage_GetSMSBtn);
					HttpManager.Instance.GetSMSS(BindingPage_PhoneNoInputField.text, (b =>
					{
						Debug.Log("获取短信验证码 " + b);
					}));
				}
				else
				{
					PP.ShowPopup("格式不正确", "请输入正确的手机号");
				}
			}));
			#endregion
			
			#region 下载
			btn_down.onClick.AddListener(delegate
			{
				mainui.panel_down.SetActive(false);
			});
			#endregion


		}
	
		// Update is called once per frame
		void Update () {
			
			mainui.loadingImg.fillAmount = mainui.fillamount;
			mainui.loadingText.text = ((int)(mainui.loadingImg.fillAmount * 100)).ToString() + "%";
			if (mainui.fillamount >= 1 && !mainui.isdown && mainui.isChangeScene)
			{
				mainui.isdown = true;
				mainui.panel_down.SetActive(false);
				UnityHelper.LoadNextScene(mainui.lastSceneName);
			}
			if (BindingPage_PhoneNoInputField.text.Length > 0 && BindingPage_SmsCodeInputField.text.Length > 0)
			{
				BindingPage_BindingBtn.interactable = true;
			}
			else
			{
				BindingPage_BindingBtn.interactable = false;
			}
		}

		public void ShowGameobject(GameObject obj)
		{
			obj.SetActive(!obj.activeSelf);
		}
		//修改名字
		public void SetUserName(Text name)
		{
			if (name.text.Length<=0)
			{
				PopupInfo("null");
			}
			else
			{

				HttpManager.Instance.ModifiUserNickName(name.text, (b =>
				{
					if (b)
					{
						for (int i = 0; i < mainui.UserName.Length; i++)
						{
							mainui.UserName[i].text = name.text;
						}
						PublicAttribute.UserInfo = new UserInfo()
						{
							PhoneNo = PublicAttribute.UserInfo.PhoneNo,
							NickName = name.text,
							UserIcon = null,
						};
						mainui.HideUI(mainUISet.UIname.user_editor,1);
					}
					else
					{
						for (int i = 0; i < mainui.UserName.Length; i++)
						{
							mainui.UserName[i].text = name.text;
						}
					}
    
				}));
			}            
		}

		public void Suggest(Text message)
		{
			HttpManager.Instance.Suggest(message.text, (b =>
			{
				if (b)
				{
					message.transform.parent.parent.parent.gameObject.SetActive(false);
					PopupInfo("300");
				}
				else
				{
					PopupInfo("Error");
				}
			}));
		}

		public void ReText(InputField txt)
		{
			txt.text = "";
		}
		
		/// <summary>
		/// 根据状态码执行
		/// </summary>
		/// <param name="status"></param>
		public void PopupInfo(string status)
		{
			Debug.Log(status);
			isHit = false;
			switch (status)
			{
				case "200":
					PP.ShowPopup("", "修改成功，请重新登录");
					PublicAttribute.UserInfo.PhoneNo = BindingPage_PhoneNoInputField.text;
					for (int i = 0; i < mainui.photonTxt.Length; i++)
					{
						mainui.photonTxt[i].text = PublicAttribute.UserInfo.PhoneNo;
					}
					BindingPage_PhoneNoInputField.transform.parent.parent.gameObject.SetActive(false);
					CoroutineWrapper.EXES(1f, () =>
					{
						mainUISet.lastpagename = mainUISet.UIname.main;
						mainUISet.lastpageid = 0;
						LogOut();
					});
					break;
				case "300":
					PP.ShowPopup("提交成功", "感谢你的宝贵意见!");
					break;
				case "500":
					PP.ShowPopup("", "请求失败，请稍后再试！");
					break;
				case "Error":
					PP.ShowPopup("请求失败", "请稍候重试");
					break;
				case "null":
					PP.ShowPopup("格式错误", "昵称不可为空");
					break;
				case "1002":
					PP.ShowPopup("号码出错", "号码已存在，请更换手机号再试");
					break;
				case "1004":
					PP.ShowPopup("验证码错误", "请输入正确的验证码");
					break;
				case "check":
					PP.ShowPopup("", "当前已是最新版本");
					break;
				default:
					PP.ShowPopup("请求失败", "请稍候重试");
					break;
			}
		}
	}

}
