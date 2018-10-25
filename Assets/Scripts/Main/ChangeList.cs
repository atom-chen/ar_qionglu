using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ChangeList : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public static ChangeList instance;
    /// <summary>
    /// 用于返回一个页码，-1说明page的数据为0
    /// </summary>
    public System.Action<int, int> OnPageChanged;

    public DOTweenAnimation change_panel;
    public GameObject disWarn;
    float startime = 0f;
    float delay = 0.1f;

    void Awake()
    {
        instance = this;
        //引导页调用
        DownloadProp.Instance.AutoCheckUpdateLocalComponent();
        //引导页调用
        DownloadProp.Instance.UpdatePreview();
        rect = transform.GetComponent<ScrollRect>();
    }
    // Use this for initialization
    void Start()
    {
        startime = Time.time;
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
        if(pages == null)
            return;
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
        {
            index = lastpage;
        }
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
    public RectTransform Content;
    public Texture[] defaultTex;
    public ChangeItem pageItemPrefab;
    public int pageCount;
    private List<GameObject> itemObj = new List<GameObject>();
    /// <summary>
    /// 获取图片列表
    /// </summary>
    public void GetImageList()
    {
        foreach (var ChangeInfo in mainPageUI.curScenicInfo.ResourcesInfos)
        {
            if (ChangeInfo.ResourcesKey == "vsz-more-change")
            {
                if (ChangeInfo.DIS.Count <= 0)
                {
                    Invoke("GetImageList", 1);
                }
                else
                {
                    foreach (var fileitem in ChangeInfo.DIS)
                    {
                        HttpManager.Instance.Download(fileitem.PageThumbnail, (() =>
                        {
                            Loom.QueueOnMainThread((() =>
                            {
                                pageCount++;
                                SetContent(pageCount);
                                ChangeItem item = GameObject.Instantiate<ChangeItem>(pageItemPrefab);
                                item.transform.SetParent(Content.transform);
                                item.transform.localScale = Vector3.one;
                                item.transform.localPosition = Vector3.zero;

                                item.id = fileitem.baseEntity.id;
                                item.name = fileitem.baseEntity.name;
                                item.thumbnail = fileitem.PageThumbnail;
                                item.locationX = fileitem.baseEntity.locationX;
                                item.locationY = fileitem.baseEntity.locationY;
                                item.height = fileitem.baseEntity.height;
                                item.sceneryArea = fileitem.baseEntity.sceneryArea;
                                item.address = fileitem.baseEntity.address;
                                item.content = fileitem.description;
                                item.VersionFilesItems = fileitem.VersionFilesItems;

                                itemObj.Add(item.gameObject);
                                item.gameObject.SetActive(true);
                            }));
                        }));

                       
                    }
                }
            }
        }
    }
    //offsetMin 是vector2(left, bottom);
    //offsetMax 是vector2(right, top)
    void SetContent(int Count)
    {
        if (Count * 1000 > 2360)
            Content.sizeDelta = new Vector2(1440, Count * 1050);
        Content.anchoredPosition = new Vector2(-720, 0);
    }
    public void EnterMain()
    {
        SceneManager.LoadScene("main");
    }
    
    public void ShowCurPanorama(string id)
    {
        change_panel.DOPlayForward();
        foreach (var item in itemObj)
        {
            if (item.GetComponent<ChangeItem>().id==id)
            {
                item.GetComponent<ChangeItem>().BtnOnClick(disWarn);
                break;
            }
        }
    }
}