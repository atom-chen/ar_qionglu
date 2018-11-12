#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

public class ScreenshotManager : MonoBehaviour {


	enum SaveStatus { NOTSAVED, SAVED, DENIED, TIMEOUT };

	public static event Action<Texture2D> OnScreenshotTaken;
    /// <summary>
    /// 截图完成，未保存，返回保存路径
    /// </summary>
    public static event Action<string> OnScreenshotFinished;
    /// <summary>
    /// 截图完成，已经保存，返回保存路径
    /// </summary>
    public static event Action<string> OnScreenshotSaved;
	public static event Action<string> OnImageSaved;

    /// <summary>
    /// 截图完成，已经保存，返回保存的名字
    /// </summary>
    public static event Action<string> OnImageSavedName;
    static ScreenshotManager instance = null;
	static GameObject go;
    /// <summary>
    /// 截图保存的名字
    /// </summary>
    [HideInInspector]
  public  string savedName = string.Empty;
    /// <summary>
    /// 截图保存的路径
    /// </summary>
    [HideInInspector]
    public string savedPath = string.Empty;
    byte[] bytes;
 static   string fileName;
    string path = string.Empty;
#if UNITY_IOS
	
	[DllImport("__Internal")]
    private static extern int saveToGallery( string path );
	
#elif UNITY_ANDROID

    static AndroidJavaClass obj;
	
	#endif
	
	
	//=============================================================================
	// Init singleton
	//=============================================================================
	
	public static ScreenshotManager Instance 
	{
		get {
			if(instance == null)
			{
				go = new GameObject();
				go.name = "ScreenshotManager";
				instance = go.AddComponent<ScreenshotManager>();
				
				#if UNITY_ANDROID
				
				if(Application.platform == RuntimePlatform.Android)
					obj = new AndroidJavaClass("com.secondfury.galleryscreenshot.MainActivity");
				
				#endif
			}
			
			return instance; 
		}
	}
	
	void Awake() 
	{
		if (instance != null && instance != this) 
		{
            Debug.Log("Destroy");
			Destroy(this.gameObject);
		}

    
    }

    /// <summary>
    /// Grab and save screenshot, , immediatelySave 是否在截图之后立即存储，默认为true
    /// </summary>
    /// <param name="shotfileName"></param>
    /// <param name="albumName"></param>
    /// <param name="fileType"></param>
    /// <param name="screenArea"></param>
    /// <param name="immediatelySave"></param>
    public static void SaveScreenshot(string shotfileName, string albumName = "sdcard/DCIM/Camera", string fileType = "jpg", Rect screenArea = default(Rect),bool immediatelySave=true)
	{
		Debug.Log("Save screenshot to gallery " + shotfileName);

        fileName = shotfileName;
		if(screenArea == default(Rect))
			screenArea = new Rect(0, 0, Screen.width, Screen.height);
		
		Instance.StartCoroutine(Instance.GrabScreenshot(shotfileName, albumName, fileType, screenArea,immediatelySave));
	}
	
	IEnumerator GrabScreenshot(string fileName, string albumName, string fileType, Rect screenArea, bool immediatelySave = true)
    {
		yield return new WaitForEndOfFrame();
		
		Texture2D texture = new Texture2D ((int)screenArea.width, (int)screenArea.height, TextureFormat.RGB24, false);
		texture.ReadPixels (screenArea, 0, 0);
		texture.Apply ();

        
        string fileExt;
 

        if (fileType == "png")
		{
			bytes = texture.EncodeToPNG();
			fileExt = ".png";
		}
		else
		{
			bytes = texture.EncodeToJPG();
			fileExt = ".jpg";
		}

		if(OnScreenshotTaken != null)
        {
            if (texture!=null)
            {
                OnScreenshotTaken(texture);
                Debug.Log("截图Texture不为空");
            }
            else
            {
                Debug.Log("截图Texture为空");
            }

        }
        else
			Destroy (texture);
		
		string date = System.DateTime.Now.ToString("hh-mm-ss_dd_MM_yyyy");
        date = date.Replace("-", "");
        date = date.Replace("_", "");
        string screenshotFilename = fileName  + date + fileExt;
        savedName = screenshotFilename;
		 path = Application.persistentDataPath + "/" + screenshotFilename;
      
#if UNITY_ANDROID

        if (Application.platform == RuntimePlatform.Android) 
		{
			 path = Path.Combine(albumName, screenshotFilename);

            savedPath = path;
            Debug.Log("androidPath==="+ path);
            string pathonly = Path.GetDirectoryName(path);
            Directory.CreateDirectory(pathonly);
        }

#endif

        if (OnScreenshotFinished != null)
            OnScreenshotFinished(path);
        if (immediatelySave)
        {
            Instance.StartCoroutine(Instance.Save(bytes, fileName, path));

        }
    }
    /// <summary>
    /// 存储截图缓存中的图片
    /// </summary>
    public  void SaveImage()
    {
        Instance.StartCoroutine(Instance.Save(bytes, fileName, path));
    }
    //=============================================================================
    // Save texture
    //=============================================================================

    public static void SaveImage(Texture2D texture, string fileName, string fileType = "jpg")
	{
		Debug.Log("Save image to gallery " + fileName);
		
		Instance.Awake();
		
		byte[] bytes;
		string fileExt;
		
		if(fileType == "png")
		{
			bytes = texture.EncodeToPNG();
			fileExt = ".png";
		}
		else
		{
			bytes = texture.EncodeToJPG();
			fileExt = ".jpg";
		}
		
		string path = Application.persistentDataPath + "/" + fileName + fileExt;
		
		Instance.StartCoroutine(Instance.Save(bytes, fileName, path));
	}
	
	
	IEnumerator Save(byte[] bytes, string fileName, string path)
	{
		int count = 0;
		SaveStatus saved = SaveStatus.NOTSAVED;
        savedPath = path;
#if UNITY_IOS
		
		if(Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			System.IO.File.WriteAllBytes(path, bytes);
			
			while(saved == SaveStatus.NOTSAVED)
			{
				count++;
				if(count > 30) 
					saved = SaveStatus.TIMEOUT;
				else
					saved = (SaveStatus)saveToGallery(path);
			
				yield return Instance.StartCoroutine(Instance.Wait(.5f));
			}
			
			UnityEngine.iOS.Device.SetNoBackupFlag(path);
		}
		
		
#elif UNITY_ANDROID

        if (Application.platform == RuntimePlatform.Android) 
		{
			System.IO.File.WriteAllBytes(path, bytes);
			
			while(saved == SaveStatus.NOTSAVED) 
			{
				count++;
				if(count > 30) 
					saved = SaveStatus.TIMEOUT;
				else
					saved = (SaveStatus)obj.CallStatic<int>("addImageToGallery", path);
				    File.WriteAllBytes(path, bytes);
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("refreshMediaStore",path);
             //   refreshMediaStore(String filePath)
                yield return Instance.StartCoroutine(Instance.Wait(.5f));
			}
		}


#elif UNITY_WP8
		
		if(Application.platform == RuntimePlatform.WP8Player)
		{
			WP8Screenshot.Main.SaveImage(bytes, fileName);

			saved = SaveStatus.SAVED;

			yield return null;
		}
#elif UNITY_EDITOR
				  File.WriteAllBytes(path, bytes);
      			saved = SaveStatus.SAVED;

			yield return null;
#else
		
		yield return null;
			
		Debug.Log("Gallery Manager: Save file only available in iOS/Android/WP8 modes");
			
		saved = SaveStatus.SAVED;
		
#endif

        switch (saved)
		{
			case SaveStatus.DENIED:
				path = "DENIED";
				break;

			case SaveStatus.TIMEOUT:
				path = "TIMEOUT";
				break;
		}

        if (OnScreenshotSaved != null)
            OnScreenshotSaved(path);
        if (OnImageSavedName != null)
            OnImageSavedName(savedName);
    }
	
	
	IEnumerator Wait(float delay)
	{
		float pauseTarget = Time.realtimeSinceStartup + delay;
		
		while(Time.realtimeSinceStartup < pauseTarget)
		{
			yield return null;	
		}
	}


}