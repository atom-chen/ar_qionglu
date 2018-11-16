using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Panorama : MonoBehaviour {
	public GameObject UIpanel,UIpanel2,toastObj,helppanel;
	public VideoPlayer vp;
	public VideoPlayer vp2;
	public AudioSource aud,bgm;
	public Button backBtn,helpbtn,shottbtn,shottbtn2,helphidebtn,videoback;
	private bool isHelp,once;
	// Use this for initialization
	void Start () {
		
		if (PlayerPrefs.GetInt("Panorama") == 0)
		{
			PlayerPrefs.SetInt("Panorama", 1);
			helppanel.SetActive(true);
			isHelp = true;
			if (bgm!=null)
			{
				bgm.gameObject.SetActive(false);
			}
		}
		else
		{
			isHelp = false;
		}
		
		backBtn.onClick.AddListener((() =>
		{
			UnityHelper.LoadNextScene("main");
		}));
		helpbtn.onClick.AddListener((() =>
		{
			isHelp = true;
			helppanel.SetActive(true);
		}));
		shottbtn.onClick.AddListener((() =>
		{
			ShotPic(UIpanel);
		}));
		helphidebtn.onClick.AddListener((() =>
		{
			isHelp = false;
			helppanel.SetActive(false);
			if (bgm!=null)
			{
				bgm.gameObject.SetActive(true);
			}
			if (!once)
			{
				once = true;
				
				if (vp2 != null)
				{  
					if (Screen.width!=750)
					{
						vp2.GetComponent<RectTransform>().sizeDelta=new Vector2(Screen.height,Screen.width);
					}
					vp2.url = GlobalInfo.VideoURL2D;
					vp2.Play();
					CoroutineWrapper.EXES(37f, () =>
					{
						if (vp != null)
						{    
							vp2.Stop();
							vp.url = GlobalInfo.VideoURL360;
							vp.Play();
							vp2.gameObject.SetActive(false);
						}
					});
				}
				else
				{
					if (vp != null)
					{
						vp.url = GlobalInfo.VideoURL360;
						vp.Play();
					}
				}
			}
		}));

		if (shottbtn2!=null)
		{
			shottbtn2.onClick.AddListener((() =>
			{
				ShotPic(UIpanel2);
			}));
		}
		
		if (videoback!=null)
		{
			videoback.onClick.AddListener((() =>
			{
				UnityHelper.LoadNextScene("main");
			}));
		}

		if (!isHelp)
		{
			if (vp2 != null)
			{
				if (Screen.width!=750)
				{
					vp2.GetComponent<RectTransform>().sizeDelta=new Vector2(Screen.height,Screen.width);
				}
			
				
				vp2.url = GlobalInfo.VideoURL2D;
				vp2.Play();
				CoroutineWrapper.EXES(37f, () =>
				{
					if (vp != null)
					{    
						vp2.Stop();
						vp.url = GlobalInfo.VideoURL360;
						vp.Play();
						vp2.gameObject.SetActive(false);
					}
				});
			}
			else
			{
				if (vp != null)
				{
					vp.url = GlobalInfo.VideoURL360;
					vp.Play();
				}
			}
		}
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (isHelp)
			{
				isHelp = false;
				helppanel.SetActive(false);
				if (!once)
				{
					once = true;
					if (bgm!=null)
					{
						bgm.gameObject.SetActive(true);
					}
					if (vp2 != null)
					{
						if (Screen.width!=750)
						{
							vp2.GetComponent<RectTransform>().sizeDelta=new Vector2(Screen.height,Screen.width);
						}
						vp2.url = GlobalInfo.VideoURL2D;
						vp2.Play();
						CoroutineWrapper.EXES(37f, () =>
						{
							if (vp != null)
							{
								vp2.Stop();
								vp.url = GlobalInfo.VideoURL360;
								vp.Play();
								vp2.gameObject.SetActive(false);
							}
						});
					}
					else
					{
						if (vp != null)
						{
							vp.url = GlobalInfo.VideoURL360;
							vp.Play();
						}
					}
				}
			}
			else
			{
				UnityHelper.LoadNextScene("main");
			}
		}
	}

	private bool isShooting;
	private string path;
	public void ShotPic(GameObject obj)
	{
		if (!isShooting)
		{
			obj.SetActive(false);
			isShooting = true;
// #if UNITY_ANDROID
            
            // string destination = "/sdcard/DCIM/AR游";
            // //判断目录是否存在，不存在则会创建目录  
            // if (!Directory.Exists(destination))
            // {
            //     Directory.CreateDirectory(destination);
            // }
            //
            // path = destination + "/Panorama"+  GlobalInfo.ShotCount +".png";
            // GlobalInfo.ShotCount++;
            // StartCoroutine(shot());
            //
            // #elif UNITY_IOS || UNITY_IPHONE
			ScreenshotManager.SaveScreenshot("Panorama");
			if (aud!=null)
			{
				aud.Play();
			}
			CoroutineWrapper.EXES(0.2f, () =>
			{
				toastObj.SetActive(true);
			});
			CoroutineWrapper.EXES(1.5f, () =>
			{
				isShooting = false;
				toastObj.SetActive(false);
				obj.SetActive(true);
			});
// #endif
		}
	}

	IEnumerator shot()
	{
		yield return new WaitForEndOfFrame();
          
		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		texture.Apply();
		yield return new WaitForEndOfFrame();
		toastObj.SetActive(true);
		byte[] bytes = texture.EncodeToPNG();
		File.WriteAllBytes(path, bytes);
		if (aud!=null)
		{
			aud.Play();
		}
		yield return new WaitForSeconds(1.5f);
		isShooting = false;
		toastObj.SetActive(false);
		UIpanel.SetActive(true);
	}
}
