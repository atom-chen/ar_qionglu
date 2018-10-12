using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class ScrollPage : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    /// <summary>
    /// 用于返回一个页码，-1说明page的数据为0
    /// </summary>
    public System.Action<int, int> OnPageChanged;

    float startime = 0f;
    float delay = 0.1f;

    void Awake()
    {
        //引导页调用
        DownloadProp.Instance.AutoCheckUpdateLocalComponent();
        //引导页调用
        DownloadProp.Instance.UpdatePreview();
    }
    // Use this for initialization
    void Start()
    {
        pageItemPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, 2560);
        HttpManager.Instance.GetGuids((b =>
        {
            if (b)
            {
                GetImageList();
            }
            else
            {
                for (int i = 0; i < defautPanel.Length; i++)
                {
                    defautPanel[i].SetActive(true);
                }
                SetContent(3);
            }
        }));
        rect = transform.GetComponent<ScrollRect>();
        startime = Time.time;
        OnPageChanged += ScrollPageMark.instance.OnScrollPageChanged;
    }

    private float autoChangeTime = 10;
    private float waiteTime = 5;
    private bool islast;
    void Update()
    {
        if (Time.time < startime + delay) return;
        UpdatePages();
        //如果不判断。当在拖拽的时候要也会执行插值，所以会出现闪烁的效果
        //这里只要在拖动结束的时候。在进行插值
        if (!isDrag && pages.Count > 0)
        {
            rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, Time.deltaTime * smooting);
        }

        autoChangeTime -= Time.deltaTime;
        if (autoChangeTime <= 0 && !islast)
        {
            autoChangeTime = waiteTime;

            index += 1;
            if (index >= pageCount)
            {
                islast = true;
                index--;
                return;
            }

            lastpage = index;
            currentPageIndex = index;
            OnPageChanged(pages.Count, currentPageIndex);
            targethorizontal = pages[index];
        }
    }

    #region 滑动
    private float page;
    ScrollRect rect;

    List<float> pages = new List<float>();
    private int currentPageIndex = -1;

    //滑动速度
    private float smooting = 4;

    //滑动的起始坐标
    private float targethorizontal = 0;

    //是否拖拽结束
    private bool isDrag = false;
    private float pos1, pos2;
    private int lastpage = 0;
    private int index = 0;
    public void OnBeginDrag(PointerEventData eventData)
    {
        autoChangeTime = waiteTime;
        isDrag = true;
        pos1 = rect.horizontalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        autoChangeTime = waiteTime;
        pos2 = rect.horizontalNormalizedPosition;
        isDrag = false;

        float posX = rect.horizontalNormalizedPosition;
        //假设离第一位最近
        float offset = Mathf.Abs(pages[index] - posX);

        if ((pos1 - pos2) > 0 && (pos1 - pos2) > 1 / ((page - 1) * 4))
        {
            index -= 1;
        }
        else if ((pos1 - pos2) < 0 && (pos1 - pos2) < -1 / ((page - 1) * 4))
        {
            index += 1;
        }
        else
            index = lastpage;

        if (index < 0 || index > pages.Count - 1)
        {
            index = lastpage;
        }
        if (index != currentPageIndex)
        {
            lastpage = index;
            currentPageIndex = index;
            OnPageChanged(pages.Count, currentPageIndex);
        }
        targethorizontal = pages[index];
    }

    void UpdatePages()
    {
        // 获取子对象的数量
        int count = rect.content.childCount;
        int temp = 0;
        for (int i = 0; i < count; i++)
        {
            if (this.rect.content.GetChild(i).gameObject.activeSelf)
            {
                temp++;
            }
        }
        count = temp;

        if (pages.Count != count)
        {
            if (count != 0)
            {
                pages.Clear();
                for (int i = 0; i < count; i++)
                {
                    float page = 0;
                    if (count != 1)
                        page = i / ((float)(count - 1));
                    pages.Add(page);
                    //Debug.Log(i.ToString() + " page:" + page.ToString());
                }
            }
            OnEndDrag(null);
        }
    }
    #endregion


    public RectTransform Content;
    public Texture[] defaultTex;
    public GuidePageItem pageItemPrefab;
    public int pageCount;
    public GameObject[] defautPanel;
    /// <summary>
    /// 获取图片列表
    /// </summary>
    void GetImageList()
    {
        for (int i = 0; i < defautPanel.Length; i++)
        {
            defautPanel[i].SetActive(true);
        }
        SetContent(3);
        return;
        if (JsonClass.Instance.GuidPages.Count <= 0)
        {
            Debug.LogError(" 不存在!");
            for (int i = 0; i < defautPanel.Length; i++)
            {
                defautPanel[i].SetActive(true);
            }
            SetContent(3);
            return;
        }
        else
        {
            int count = 0;
            foreach (var page in JsonClass.Instance.GuidPages)
            {
                if (File.Exists(page.Thumbnail.localPath))
                {
                    count++;
                }
            }

            if (count >= JsonClass.Instance.GuidPages.Count)
            {
                foreach (var page in JsonClass.Instance.GuidPages)
                {
                        HttpManager.Instance.Download(page.Thumbnail, (() =>
                        {
                            count++;
                            GuidePageItem item = GameObject.Instantiate<GuidePageItem>(pageItemPrefab);
                            item.transform.SetParent(Content.transform);
                            item.transform.localScale = Vector3.one;
                            item.transform.localPosition = Vector3.zero;
                            item.gameObject.SetActive(true);
                            //item.Url= page.
                            //item._init("Ads/" + ScenicDirectoryInfo.Name);
                            item._init(page.Thumbnail.localPath);
                            pageCount++;
                            SetContent(pageCount);
                        }));
                }
            }
            else
            {
                foreach (var page in JsonClass.Instance.GuidPages)
                {
                    HttpManager.Instance.Download(page.Thumbnail, (() =>
                    {

                    }));
                }
                for (int i = 0; i < defautPanel.Length; i++)
                {
                    defautPanel[i].SetActive(true);
                }
                SetContent(3);
            }
        }
    }
    //offsetMin 是vector2(left, bottom);
    //offsetMax 是vector2(right, top)
    void SetContent(int Count)
    {
        //Content.offsetMax = new Vector2(0,0);
        //Content.offsetMin = new Vector2(Screen.width*(pageCount-1), 0);
        Content.sizeDelta = new Vector2(1440 * Count, 2560);
        Content.anchoredPosition = new Vector2(-1440 / 2, 2560 / 2);
    }

    public void EnterMain()
    {
        HttpManager.Instance.GetUserInfoByToken((b =>
        {
            if (b)
            {
                ScenesManager.Instance.LoadMainScene();
            }
            else
            {
                ScenesManager.Instance.LoadLoginScene();
            }
        }));
    }
}
