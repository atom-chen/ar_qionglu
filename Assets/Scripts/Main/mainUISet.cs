using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace mainpage
{
	public class mainUISet : BaseUI
	{
		public static mainUISet _inst;
		public enum UIname
		{
			/// <summary>
			/// 主界面
			/// </summary>
			main = 0,
			/// <summary>
			/// 景点列表
			/// </summary>
			spot = 1,
			/// <summary>
			/// 移空换景
			/// </summary>
			change = 2,
			/// <summary>
			/// 个人中心首页
			/// </summary>
			user_center = 3,
			/// <summary>
			/// 个人信息编辑界面
			/// </summary>
			user_editor = 4,
			/// <summary>
			/// 个人关于界面
			/// </summary>
			user_about = 5,
			/// <summary>
			/// 个人设置界面
			/// </summary>
			user_set = 6,
			/// <summary>
			/// 网页
			/// </summary>
			webpage = 7,
		};

		public static UIname lastpagename = UIname.main;
		public static int lastpageid = 0;
		[Header("主界面")] 
		public GameObject panel_mainpage;
		private GameObject scroll_banner;
		private GameObject scroll_view;
		private Text[] view_titles;
		private GameObject[] scroll_view_contents;
		
		[Header("景点列表")] 
		public GameObject panel_spotlist;
		
		[Header("移空换景列表")] 
		public GameObject panel_changelist;
		public GameObject panel_changetogps;
		
		[Header("个人中心")] 
		public GameObject panel_user_center;
		public GameObject panel_user_center_normal;
		public GameObject panel_user_center_visiter;
		
		[Header("编辑界面")] 
		public GameObject panel_editor;
		public GameObject panel_editor_name;
		public GameObject panel_editor_pho;
		public GameObject panel_editor_password;
		public GameObject panel_editor_head;
		
		public Image[] HeadImg;
		public Sprite[] HeadSpr;
		
		[Header("About界面")] 
		public GameObject panel_about;
		public GameObject panel_about_yinsi;
		public GameObject panel_about_xieyi;
		
		[Header("Set界面")] 
		public GameObject panel_set;
		public GameObject panel_fankui;
		
		[Header("下载界面")] 
		public GameObject panel_down;
		public Image loadingImg;
		public Text loadingText;
		public bool isdown;
		
		[Header("列表栏")]
		public RectTransform[] ChildPanel;
		public static float scale;
		/// <summary>
		/// 显示指定界面
		/// </summary>
		/// <param name="pagename">UIname </param>
		/// <param name="id">
		///	main 0主页
		/// spot 0列表
		/// change 0列表
		/// user_center  0 背景 1 用户 2 游客
		/// user_editor 0 主页 1 修改名称 2 修改手机 3 修改密码
		/// user_about 0 主页 1 隐私政策 2 用户协议
		/// user_set 0 主页 1 反馈
		/// webpage 0 网页
		/// </param>
		public void ShowUI(UIname pagename,int pageid)
		{
			lastpagename = pagename;
			lastpageid = pageid;
			switch (pagename)
			{
				case UIname.main:
					toMainPage();
					break;
				case UIname.spot:
					toSpotPage(true);
					break;
				case UIname.change:
					toChangePage(pageid,true);
					break;
				case UIname.user_center:
					toUser_CenterPage(pageid,true);
					break;
				case UIname.user_editor:
					toUser_Editor(pageid,true);
					break;
				case UIname.user_about:
					toUser_About(pageid,true);
					break;
				case UIname.user_set:
					toUser_Set(pageid,true);
					break;
				case UIname.webpage:
					toLastWebpage();
					break;
			}
		}
		public void HideUI(UIname pagename,int pageid)
		{
			switch (pagename)
			{
				case UIname.main:
					break;
				case UIname.spot:
					toSpotPage(false);
					if (pageid==0)
					{
						lastpagename = UIname.main;
					}
					break;
				case UIname.change:
					toChangePage(pageid,false);
					if (pageid==0)
					{
						lastpagename = UIname.main;
					}
					break;
				case UIname.user_center:
					toUser_CenterPage(pageid,false);
					if (pageid==0)
					{
						lastpagename = UIname.main;
					}
					break;
				case UIname.user_editor:
					toUser_Editor(pageid,false);
					if (pageid==0)
					{
						lastpagename = UIname.user_center;
					}
					break;
				case UIname.user_about:
					toUser_About(pageid,false);
					if (pageid==0)
					{
						lastpagename = UIname.user_center;
					}
					break;
				case UIname.user_set:
					toUser_Set(pageid,false);
					if (pageid==0)
					{
						lastpagename = UIname.user_center;
					}
					break;
				case UIname.webpage:
					break;
			}
		}

		#region 界面跳转

		private void toMainPage()
		{
			panel_mainpage.SetActive(true);

			UIname name = lastpagename;
			int id = lastpageid;
			
			HideUI(UIname.spot, 0);
			HideUI(UIname.change, 0);
			HideUI(UIname.user_center, 0);
			HideUI(UIname.user_editor, 0);
			HideUI(UIname.user_about, 0);
			HideUI(UIname.user_set, 0);
			HideUI(UIname.webpage, 0);
			lastpagename = name;
			lastpageid = id;
		}
		/// spot 0列表
		private void toSpotPage(bool state)
		{
			SetState(panel_spotlist,state, (b=>
			{
				if (b)
				{
					Debuger("显示景点列表");
				}
				else
				{
					Debuger("没找到景点列表");
				}
			}));
		}

		/// change 0 列表 1 toGPS
		private void toChangePage(int pageid,bool state)
		{
			switch (pageid)
			{
				case 0:
					SetState(panel_changelist, state, (b =>
					{
						if (b)
						{
							Debuger("显示移空换景列表");
						}
						else
						{
							Debuger("没找到移空换景列表");
						}
					}));
					break;
				case 1:
					SetState(panel_changetogps, state, (b =>
					{
						if (b)
						{
							Debuger("显示移空换景列表");
						}
						else
						{
							Debuger("没找到移空换景列表");
						}
					}));
					break;
			}
		}

		/// user_center  0 背景 1 用户 2 游客
		private void toUser_CenterPage(int pageid,bool state)
		{
			if (GlobalParameter.isVisitor)
			{
				pageid = 1;
			}
			else
			{
				pageid = 0;
			}
			
			SetState(panel_user_center,state, (b=>
			{
				if (b)
				{
					Debuger("显示个人中心");
				}
				else
				{
					Debuger("没找到个人中心");
				}
			}));
			switch (pageid)
			{
				case 0:
					SetState(panel_user_center_normal,state, (b=>
					{
						if (b)
						{
							Debuger("显示个人中心");
						}
						else
						{
							Debuger("没找到个人中心");
						}
					}));
					break;
				case 1:
					SetState(panel_user_center_visiter,state, (b=>
					{
						if (b)
						{
							Debuger("显示游客界面");
						}
						else
						{
							Debuger("没找到个人中心");
						}
					}));
					break;
			}
		}
		///user_editor 0 主页 1 修改名称 2 修改手机 3 修改密码 4 修改头像
		private void toUser_Editor(int pageid,bool state)
		{
			switch (pageid)
			{
				case 0:
					if (state==false)
					{
						SetState(panel_editor_password,state, (b=>
						{
						}));
						SetState(panel_editor_name,state, (b=>
						{
						}));
						SetState(panel_editor_pho,state, (b=>
						{
						}));
						SetState(panel_editor_head,state, (b=>
						{
						}));
					}
					SetState(panel_editor,state, (b=>
					{
						if (b)
						{
							Debuger("显示编辑界面");
						}
						else
						{
							Debuger("没找到编辑界面");
						}
					}));
					break;
				case 1:
					SetState(panel_editor_name,state, (b=>
					{
						if (b)
						{
							Debuger("编辑姓名");
						}
						else
						{
							Debuger("没找到编辑姓名");
						}
					}));
					break;
				case 2:
					SetState(panel_editor_pho,state, (b=>
					{
						if (b)
						{
							Debuger("修改手机");
						}
						else
						{
							Debuger("没找到修改手机");
						}
					}));
					break;
				case 3:
					SetState(panel_editor_password,state, (b=>
					{
						if (b)
						{
							Debuger("修改密码");
						}
						else
						{
							Debuger("没找到修改密码");
						}
					}));
					break;
				case 4:
					SetState(panel_editor_head,state, (b=>
					{
						if (b)
						{
							Debuger("修改头像");
						}
						else
						{
							Debuger("没找到修改头像");
						}
					}));
					break;
			}
		}
		/// user_about 0 主页 1 隐私政策 2 用户协议
		private void toUser_About(int pageid,bool state)
		{
			switch (pageid)
			{
				case 0:
					if (state==false)
					{
						SetState(panel_about_yinsi,state, (b=>
						{
							if (b)
							{
								Debuger("显示隐私界面");
							}
							else
							{
								Debuger("没找到隐私界面");
							}
						}));
						SetState(panel_about_xieyi,state, (b=>
						{
							if (b)
							{
								Debuger("显示用户协议界面");
							}
							else
							{
								Debuger("没找到用户协议界面");
							}
						}));
					}
					SetState(panel_about,state, (b=>
					{
						if (b)
						{
							Debuger("显示about界面");
						}
						else
						{
							Debuger("没找到about界面");
						}
					}));
					break;
				case 1:
					SetState(panel_about_yinsi,state, (b=>
					{
						if (b)
						{
							Debuger("显示隐私界面");
						}
						else
						{
							Debuger("没找到隐私界面");
						}
					}));
					break;
				case 2:
					SetState(panel_about_xieyi,state, (b=>
					{
						if (b)
						{
							Debuger("显示用户协议界面");
						}
						else
						{
							Debuger("没找到用户协议界面");
						}
					}));
					break;
			}
		}
		/// user_set 0 主页 1 反馈	
		private void toUser_Set(int pageid,bool state)
		{
			switch (pageid)
			{
				case 0:
					if (state==false)
					{
						SetState(panel_fankui,state, (b=>
						{
							if (b)
							{
								Debuger("编辑反馈");
							}
							else
							{
								Debuger("没找到反馈");
							}
						}));
					}
					SetState(panel_set,state, (b=>
					{
						if (b)
						{
							Debuger("显示set界面");
						}
						else
						{
							Debuger("没找到set界面");
						}
					}));
					break;
				case 1:
					SetState(panel_fankui,state, (b=>
					{
						if (b)
						{
							Debuger("编辑反馈");
						}
						else
						{
							Debuger("没找到反馈");
						}
					}));
					break;
			}
		}
		///loadlastweb
		private void toLastWebpage()
		{
			webrequest.instance.LoadLastWeb();
		}


		#endregion

		#region 信息获取属性

		private mainBtnSet mainbtn;
		private static bool[] firstDown = new bool[]{false,false, false, false, false, false, false, false, false };
		/// 当前景区ID
		public static AreaInfo SceneID;
		/// 当前景区资源信息
		public static DynamicResoucesInfos curScenicInfo;
		/// 动态资源下载进度
		public float fillamount;
		/// 是否在下载动态资源
		public bool ischecking;
		
		//跳转场景，判断资源是否完整
		[HideInInspector]
		public string lastSceneName;
		[HideInInspector]
		public bool isChangeScene;
		
		public static bool ResisDown;
		
		[Header("个人中心电话和姓名")]
		public Text[] photonTxt, UserName;
		
		public static int HeadId
		{
			set {PlayerPrefs.SetInt("HeadId", value);}
			get { return PlayerPrefs.GetInt("HeadId");}
		}

		public bool isTip;
		public GameObject quitTip;
		private int hit = 0;
		#endregion

		private void Awake()
		{
			_inst = this;
			toMainPage();
		}

		private void Start()
		{
			AssetBundle.UnloadAllAssetBundles(false);
			Resources.UnloadUnusedAssets();
			
			mainbtn = GetComponent<mainBtnSet>();
			scale = (((float) Screen.height / (float)Screen.width) * 1080f)/1920f;
			for (int i = 0; i < ChildPanel.Length; i++)
			{
				// ChildPanel[i].sizeDelta=new Vector2(1080,1920 * scale);
			}
			
			//获取景区信息
			GetSceneInfo();
			
			if (GlobalParameter.isVisitor)
			{

			}
			else
			{
				HttpManager.Instance.GetUserInfoByToken((b =>
				{
					if (b)
					{
						for (int i = 0; i < UserName.Length; i++)
						{
							UserName[i].text = PublicAttribute.UserInfo.NickName;
						}

						for (int i = 0; i < photonTxt.Length; i++)
						{
							photonTxt[i].text = PublicAttribute.UserInfo.PhoneNo;
						}
					}
					else
					{
						for (int i = 0; i < UserName.Length; i++)
						{
							UserName[i].text = PublicAttribute.UserInfo.NickName;
						}

						for (int i = 0; i < photonTxt.Length; i++)
						{
							photonTxt[i].text = PublicAttribute.UserInfo.PhoneNo;
						}
					}
				}));
			}

			if (ResisDown)
			{
				fillamount = 1;
				CoroutineWrapper.EXES(0.5f, () =>
				{
					ChangeList.instance.GetImageList();
				});
			}
			else
			{
				//获取动态资源信息
				CoroutineWrapper.EXES(1.5f, () =>
				{
					PublicAttribute.AreaResoucesDic.TryGetValue(SceneID.id, out curScenicInfo);
					ChangeList.instance.GetImageList();
				});
				//自动下载动态资源
				CoroutineWrapper.EXES(5f, () =>
				{
					if (!ischecking)
					{
						Debug.LogError("自动下载");
						ischecking = true;
						HttpManager.Instance.DynamicCheekUpdateByArea(SceneID.id);
					}
				});
				HttpManager.Instance.DownloadPercent = f =>
				{
					Debug.LogError("进度: " + f.ToString("#0.000"));
					fillamount = f;
				};
			}

			ReLoadPage();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Debug.Log(lastpagename+"  "+lastpageid);
				if (isTip)
				{
					isTip = false;
					mainbtn.PP.Hide();
				}
				else if(webrequest.isLoadedWeb)
				{
					if (GameObject.Find("webpage") != null)
					{            			
						webrequest.isLoadedWeb = false;
						if (GameObject.Find("webpage").GetComponent<UniWebView>()!=null)
						{
							GameObject.Find("webpage").GetComponent<UniWebView>().Stop();
						}
					    Destroy(GameObject.Find("webpage").gameObject);
					}
				}
				else if (lastpagename!=UIname.main)
				{
					HideUI(lastpagename,lastpageid);
					if (lastpageid!=0)
					{
						lastpageid = 0;
					}
				}
				else if (lastpagename == UIname.main)
				{
					hit++;
					quitTip.SetActive(true);
					CoroutineWrapper.EXES(1f, () =>
					{
						hit = 0;
						quitTip.SetActive(false);
					});
				}
				
				if (hit >= 2)
				{
					Application.Quit();
				}
			}
		}

		/// <summary>
		/// 修改头像
		/// </summary>
		/// <param name="id"></param>
		public void SetHeadID(int id)
		{
			HeadId = id;
			for (int i = 0; i < HeadImg.Length; i++)
			{
				HeadImg[i].sprite = HeadSpr[id];
			}
			HideUI(UIname.user_editor,4);
		}
		//初始化获取界面数据
		private void GetSceneInfo()
		{
			//获取景区列表信息
			if (!firstDown[0])
			{
				HttpManager.Instance.GetAreaInfo((b =>
				{
					if (b)
					{
						firstDown[0] = true;
						HttpManager.Instance.DynamicResources();
						SceneID = JsonClass.Instance.AreaInfoS[0];
					}
				}));
			}	
			//获取景点信息
			if (!firstDown[1])
			{
				HttpManager.Instance.GetScenicSpotInfo((b =>
				{
					if (b)
					{
						firstDown[1] = true;
						///如果获取信息成功，开始下载缩略图
						foreach (var info in JsonClass.Instance.ScenicSpotInfoS)
						{
							HttpManager.Instance.Download(info.thumbnail, (() =>
							{
								SpotList.instance.CreateItems(info);
							}));
						}
					}
				}));
			}
			else
			{
				foreach (var info in JsonClass.Instance.ScenicSpotInfoS)
				{
					HttpManager.Instance.Download(info.thumbnail, (() =>
					{
						SpotList.instance.CreateItems(info);
					}));
				}
			}
			//获取主页特色景点缩略图
			if (!firstDown[2])
			{
				HttpManager.Instance.GetTraitScenicSpotInfo((b =>
				{
					if (b)
					{
						firstDown[2] = true;
						Loom.QueueOnMainThread((() =>
						{
							ScrollList.instance.GetImageList(0);
						}));
					}
				}));
			}
			else
			{
				ScrollList.instance.GetImageList(0);
			}	
			//获取主页商家缩略图
			if (!firstDown[3])
			{
				HttpManager.Instance.GetShopSInfo((b =>
				{
					if (b)
					{
						firstDown[3] = true;
						///如果获取信息成功，开始下载缩略图
						foreach (var info in JsonClass.Instance.ShopInfoS)
						{
							HttpManager.Instance.Download(info.thumbnail, (() => { }));
						}
					}
				}));
			}
			else
			{
				
			}
			//获取特产缩略图
			if (!firstDown[4])
			{
				HttpManager.Instance.GetLocalSpecialtyInfo((b =>
				{
					if (b)
					{
						firstDown[4] = true;
						///如果获取信息成功，开始下载缩略图
						foreach (var info in JsonClass.Instance.LocalSpecialtyS)
						{
							HttpManager.Instance.Download(info.thumbnail, (() => {  }));
						}
					}
				}));
			}
			else
			{
				
			}		
			if (!firstDown[5])
			{
	
			//首次获取全部到此一游道具
				HttpManager.Instance.Visit_GetAll((b =>
				{
					//获取信息成功后
					if (b)
					{
						firstDown[5] = true;
						//下载道具的缩略图信息
						foreach (var info in JsonClass.Instance.VisitInfoS)
						{
							HttpManager.Instance.Download(info.Thumbnail, (() => { GoodList.instance.CreateItems(info); }));
						}
					}
				}));
				//获取到此一游道具类型列表
				HttpManager.Instance.Visit_GetType((b =>
				{
					//获取信息成功后
					if (b)
					{
						firstDown[5] = true;
						////下载道具的缩略图信息
						//foreach (var info in JsonClass.Instance.VisitInfoS)
						//{
						//    HttpManager.Instance.Download(info.Thumbnail, (() => { GoodList.instance.CreateItems(info); }));
						//}
					}
				}));
			}
			else
			{
				foreach (var info in JsonClass.Instance.VisitInfoS)
				{
					GoodList.instance.CreateItems(info);
				}
	
			}
			//获取主页商家缩略图
			if (!firstDown[6])
			{
				HttpManager.Instance.GetHotelsInfo((b =>
				{
					if (b)
					{
						firstDown[6] = true;
						///如果获取信息成功，开始下载缩略图
						foreach (var info in JsonClass.Instance.HotelInfoS)
						{
							HttpManager.Instance.Download(info.thumbnail, (() => { }));
						}
					}
				}));
			}
			else
			{
				
			}
		}
		public void LoadScene(string scenename)
		{
			isChangeScene = true;
			lastSceneName = scenename;
			if (fillamount >= 1)
			{
				UnityHelper.LoadNextScene(scenename);
			}
			else
			{
				panel_down.SetActive(true);
				if (!ischecking)
				{
					ischecking = true;
					HttpManager.Instance.DynamicCheekUpdateByArea(SceneID.id);
				}
			}
		}

		public void ReLoadPage()
		{
			if (lastpageid!=0)
			{
				int id = lastpageid;
				ShowUI(lastpagename,0);
				ShowUI(lastpagename,id);
			}
			else
			{
				ShowUI(lastpagename,0);
			}
			if (webrequest.isLoadedWeb)
			{
				webrequest.instance.LoadLastWeb();
			}
		}
	}
}
