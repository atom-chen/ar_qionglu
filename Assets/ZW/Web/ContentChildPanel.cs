using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ContentChildPanel : MonoBehaviour
{
    public static ContentChildPanel _Instance;
    public Image titleImage;
    public Toggle titleImageToggle;
    public Text titleText;
    public Transform imagePanel;



    public int childCount = 0;

 public   int row;
    void Awake()
    {
        _Instance = this;
        //titleImage = this.transform.Find("titleImage").GetComponent<Image>();
        //titleImageToggle = this.transform.Find("titleImage/Toggle").GetComponent<Toggle>();


        //titleText = this.transform.Find("titleImage/titleText").GetComponent<Text>();
        //imagePanel = this.transform.Find("imagePanel");

        //titleImageToggle.onValueChanged.AddListener(TitleImageToggleClick);
    }

    private void TitleImageToggleClick(bool arg0)
    {
        if (arg0==true)
        {
            UpdateHeight();
        }
        else
        {
            this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1440f, 110f);
        }
        imagePanel.gameObject.SetActive(arg0);

        this.gameObject.transform.parent.GetComponent<ScrollContent>().Updateheight();

    }

    private void UpdateHeight()
    {
        if (childCount > GlobalParameter.maxCount)
        {
            row = childCount - GlobalParameter.maxCount;
            int num = row / GlobalParameter.maxCount;
            row = row % GlobalParameter.maxCount == 0 ? num : num + 1;
            this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1440f, 490f + row * GlobalParameter.rowOffset);
        }
        else
        {
            this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1440f, 490f);
        }

    }

    /// <summary>
    /// 添加ImagePrefabs
    /// </summary>
    /// <param name="path"></param>
    public void AddImageChild(string key,List<string> paths)
    {
        childCount = 0;
        Debug.Log("AddImageChildkey===" + key);
        foreach (var item in paths)
        {
            Debug.Log("AddImageChildpaths===" + item);
        }



       // string[] keyArray = key.Split('|');
        Debug.Log(key + "    "+paths.Count);
        
        //titleText.text =  keyArray[0] + "年" + keyArray[1] + "月" + keyArray[2] + "日";
        titleText.text = key;
        for (int i = 0; i < paths.Count; i++)
        {
            GameObject go = Instantiate<GameObject>(Resources.Load<GameObject>("Model/ImagePrefabs"), imagePanel);
            go.GetComponent<ImagePrefabs>().Init(paths[i]);
            childCount++;
        }
        UpdateHeight();

    }
}
