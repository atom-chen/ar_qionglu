using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ScrollList : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public static ScrollList instance;

    /// <summary>
    /// 用于返回一个页码，-1说明page的数据为0
    /// </summary>
    public System.Action<int, int> OnPageChanged;

    float startime = 0f;
    float delay = 0.1f;

    public DOTweenAnimation dot;
    void Awake()
    {
        instance = this;
        //引导页调用
        DownloadProp.Instance.AutoCheckUpdateLocalComponent();
        //引导页调用
        DownloadProp.Instance.UpdatePreview();
    }
    // Use this for initialization
    void Start()
    {
        rect = transform.GetComponent<ScrollRect>();
        startime = Time.time;
        //GetImageList();
    }

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
        isDrag = true;
        pos1 = rect.horizontalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
            //OnPageChanged(pages.Count, currentPageIndex);
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
    public RectTransform[] ShowContent;
    public RectTransform Content;
    public Texture[] defaultTex;
    public ListItem pageItemPrefab;
    public int[] pageCount = new int[3];
    List<string> filename = new List<string>();
    /// <summary>
    /// 获取图片列表
    /// </summary>
    public void GetImageList(float type)
    {
        //景点加载
        if (type == 0)
        {
            if (JsonClass.Instance.TraitScenicSpotInfoS.Count == 0)
            {
                CoroutineWrapper.EXES(1f, () =>
                {
                    GetImageList(type);
                });
                return;
            }

            for (int i = 0; i < JsonClass.Instance.TraitScenicSpotInfoS.Count; i++)
            {
                pageCount[0]++;
                SetContent(pageCount[0]);
                ListItem item = GameObject.Instantiate<ListItem>(pageItemPrefab);
                item.transform.SetParent(ShowContent[0].transform);
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;

                item.id = JsonClass.Instance.TraitScenicSpotInfoS[i].id;
                item.name = JsonClass.Instance.TraitScenicSpotInfoS[i].name;
                item.thumbnail = JsonClass.Instance.TraitScenicSpotInfoS[i].thumbnail;
                item.address = JsonClass.Instance.TraitScenicSpotInfoS[i].address;

                HttpManager.Instance.Download(item.thumbnail, (() =>
                {
                    Loom.QueueOnMainThread((() =>
                    {
                        item.gameObject.SetActive(true);
                        item._init(item.thumbnail.localPath);
                    }));
                }));
            }
        }
        //特产加载
        if (type == 1)
        {
            if (JsonClass.Instance.LocalSpecialtyS.Count == 0 )
            {
                CoroutineWrapper.EXES(1f, () =>
                {
                    GetImageList(type);
                });
                return;
            }
            for (int i = 0; i < JsonClass.Instance.LocalSpecialtyS.Count; i++)
            {
                pageCount[1]++;
                //SetContent(pageCount);
                ListItem item = GameObject.Instantiate<ListItem>(pageItemPrefab);
                item.transform.SetParent(ShowContent[1].transform);
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;

                item.id = JsonClass.Instance.LocalSpecialtyS[i].id;
                item.name = JsonClass.Instance.LocalSpecialtyS[i].name;
                item.thumbnail = JsonClass.Instance.LocalSpecialtyS[i].thumbnail;
                item.address = JsonClass.Instance.LocalSpecialtyS[i].address;

                HttpManager.Instance.Download(item.thumbnail, (() =>
                {
                    Loom.QueueOnMainThread((() =>
                    {
                        item.gameObject.SetActive(true);
                        item._init(item.thumbnail.localPath);
                    }));
                }));
            }
        }
        //商家加载
        if (type == 2)
        {
            if (JsonClass.Instance.ShopInfoS.Count == 0)
            {
                CoroutineWrapper.EXES(1f, () =>
                {
                    GetImageList(type);
                });
                return;
            }
            for (int i = 0; i < JsonClass.Instance.ShopInfoS.Count; i++)
            {
                pageCount[2]++;
                //SetContent(pageCount);
                ListItem item = GameObject.Instantiate<ListItem>(pageItemPrefab);
                item.transform.SetParent(ShowContent[2].transform);
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;

                item.id = JsonClass.Instance.ShopInfoS[i].id;
                item.name = JsonClass.Instance.ShopInfoS[i].name;
                item.thumbnail = JsonClass.Instance.ShopInfoS[i].thumbnail;
                item.address = JsonClass.Instance.ShopInfoS[i].address;
                HttpManager.Instance.Download(item.thumbnail, (() =>
                {
                    Loom.QueueOnMainThread((() =>
                    {
                        item.gameObject.SetActive(true);
                        item._init(item.thumbnail.localPath);
                    }));
                }));

            }
        }
    }
    //offsetMin 是vector2(left, bottom);
    //offsetMax 是vector2(right, top)
    void SetContent(int Count)
    {
        if (Count <= 6)
        {
            Content.sizeDelta = new Vector2(1440, 1600);
        }
        else if (Count > 6 && Count <= 9)
        {
            Content.sizeDelta = new Vector2(1440, 1800);
        }
        else if (Count > 9)
        {
            if ((Count - 9) % 3 != 0)
            {
                Content.sizeDelta = new Vector2(1440, 1745 + 465 * (int)((Count - 9) / 3 + 1));
            }
            else
            {
                Content.sizeDelta = new Vector2(1440, 1745 + 465 * (int)((Count - 9) / 3));
            }
        }
        Content.anchoredPosition = new Vector2(-720, 0);
    }

    public void EnterMain()
    {
        SceneManager.LoadScene("main");
    }

    public void LoadWeb()
    {
        GameObject webObj = GameObject.Instantiate(Resources.Load("WebUI")) as GameObject;
        webObj.transform.SetParent(GameObject.Find("Canvas").transform);
        webObj.transform.localPosition = Vector3.zero;
        webObj.transform.localEulerAngles = Vector3.zero;
        webObj.GetComponent<RectTransform>().sizeDelta = new Vector2(1440, 2560);
        webObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    public Toggle[] tog;

    public void ChangeList(bool active)
    {
        if (active)
        {
            for (int i = 0; i < tog.Length; i++)
            {
                if (tog[i].isOn == true)
                {
                    GetImageList(i);
                    ShowContent[i].gameObject.SetActive(true);
                    SetContent(pageCount[i]);
                }
                else
                {
                    ShowContent[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
