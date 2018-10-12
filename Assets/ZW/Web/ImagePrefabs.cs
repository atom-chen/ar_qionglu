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
        Debug.Log(this.gameObject.name);
    TrackUIManager.Instance.ShowBigImage(sprite);
    }


    public void Init(string path)
    {
        this.path = path;
        if (path != null)
        {
            StartCoroutine(LoadImage(path));
        }
    }
    private IEnumerator LoadImage(string iconPath)
    {
        //WWW www = new WWW(UnityHelper.LocalFilePath+"Web/"+iconPath);
        WWW www = new WWW("file://"+ UnityHelper.LocalFilePath + "Web/" + iconPath);
        Debug.Log("UnityHelper.LocalFilePath+iconPath==="+ UnityHelper.LocalFilePath + "Web/" + iconPath);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError("www加载图片出错，请检查path");
        }
        else
        {
             txt2D = www.texture;

            Sprite sp = Sprite.Create(txt2D, new Rect(0, 0, txt2D.width, txt2D.height), new Vector2(0.5f, 0.5f));

            sprite = sp;
            icon.sprite = sp;

        }
    }
}
