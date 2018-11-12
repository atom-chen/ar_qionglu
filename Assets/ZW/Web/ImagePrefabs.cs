using Assets.ZW.ElviraFrame.TextureUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePrefabs : MonoBehaviour {
    public Image icon;
    private Button btn;
    private Texture2D txt2D;
    private Sprite sprite;
    public string path;
    void Awake()
    {
        icon = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(BtnClick);

    }

    private void BtnClick()
    {
      
    TrackUIManager.Instance.ShowBigImage();
    }


    public void Init(string path)
    {
        this.path = path;
        if (path != null)
        {
            StartCoroutine(LoadImage(path));
        }
    }
    private IEnumerator LoadImage(string iconName)
    {
        //WWW www = new WWW(UnityHelper.LocalFilePath+"Web/"+iconPath);

#if UNITY_ANDROID
//WWW www = new WWW("file://"+ UnityHelper.LocalFilePath + "Web/" + iconName);
        WWW www = new WWW("file:///" + "sdcard/DCIM/Camera/" + iconName);
#elif  UNITY_IOS||UNITY_IPHONE
 WWW www = new WWW(Application.persistentDataPath + "/"+iconName);
#endif

        yield return www;
        if (www.error != null)
        {
            Debug.LogError("www加载图片出错，请检查path");
        }
        else
        {
            txt2D = TextureUtility.ScalePoint(www.texture, 290, 250);
           Sprite sp = Sprite.Create(txt2D, new Rect(0, 0, txt2D.width, txt2D.height), new Vector2(0.5f, 0.5f));
           // Sprite sp = Sprite.Create(txt2D, new Rect(0, 0, 250f, 250f), new Vector2(0.5f, 0.5f));
            sprite = sp;
            icon.sprite = sp;
            TrackUIManager.Instance.AddChildSprite(iconName);
        }
    }
}
