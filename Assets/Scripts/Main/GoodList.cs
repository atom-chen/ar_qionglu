using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

public class GoodList : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public static GoodList instance;
    /// <summary>
    /// ���ڷ���һ��ҳ�룬-1˵��page������Ϊ0
    /// </summary>
    public System.Action<int, int> OnPageChanged;

    float startime = 0f;
    float delay = 0.1f;

    void Awake()
    {
        instance = this;
        //����ҳ����
        DownloadProp.Instance.AutoCheckUpdateLocalComponent();
        //����ҳ����
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
        //������жϡ�������ק��ʱ��ҪҲ��ִ�в�ֵ�����Ի������˸��Ч��
        //����ֻҪ���϶�������ʱ���ڽ��в�ֵ
        if (!isDrag && pages.Count > 0)
        {
            rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, Time.deltaTime * smooting);

        }
    }

    #region ����
    private float page;
    ScrollRect rect;

    List<float> pages = new List<float>();
    private int currentPageIndex = -1;

    //�����ٶ�
    private float smooting = 4;

    //��������ʼ����
    private float targethorizontal = 0;

    //�Ƿ���ק����
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
        if (pages == null)
            return;
        pos2 = rect.horizontalNormalizedPosition;
        isDrag = false;

        float posX = rect.horizontalNormalizedPosition;
        //�������һλ���
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
        // ��ȡ�Ӷ��������
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
    /// ��ȡͼƬ�б�
    /// </summary>
    public void CreateItems(VisitInfo info)
    {
        pageCount++;
        SetContent(pageCount);
        ChangeItem item = GameObject.Instantiate<ChangeItem>(pageItemPrefab);
        item.transform.SetParent(Content.transform);
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = Vector3.zero;

        item.id = info.id;
        item.name = info.name;
        item.thumbnail = info.Thumbnail;
        item.content = info.description;

        itemObj.Add(item.gameObject);
        item.gameObject.SetActive(true);
    }
    //offsetMin ��vector2(left, bottom);
    //offsetMax ��vector2(right, top)
    void SetContent(int Count)
    {
        if (Count * 1000 > 2360)
            Content.sizeDelta = new Vector2(1440, Count * 1000);
        Content.anchoredPosition = new Vector2(-720, 0);
    }
    public void EnterMain()
    {
        SceneManager.LoadScene("main");
    }
}