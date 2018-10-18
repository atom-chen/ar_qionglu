using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IconFollow : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    private RectTransform recTransform;
    //private float lastTime = 0;
    //private float curtTime = 0;
    [SerializeField]
    private float rate = 0.1f;
    [SerializeField]
    private float normalScaleDis = 250.0f;
    [SerializeField]
    private string stateIconPrefab = "Prefabs/GoThere";
    private float maxScale = 8f;
    private float minScale = 4f;
    private float curScale;
    private float dis;
    public Text disText, nameText, infodisText, infonameText, describeText;
    private GPSItem item;
    private RawImage raw, iconraw;
    private Button btn, webbtn;
    public GameObject state;
    private Transform infobg, iconbg, namebg1, namebg2;
    private RectTransform txtbg;
    private string type;
    private Image other;
    private webrequest web;
    private void Awake()
    {
        item = transform.GetComponent<GPSItem>();
        Transform iconRoot = GameObject.Find("Canvas/Markers").transform;
        state = Instantiate(Resources.Load(stateIconPrefab)) as GameObject;
        GpsConvert.icon.Add(state);
        state.name = item.name;
        recTransform = state.GetComponent<RectTransform>();
        recTransform.SetParent(iconRoot);

        web = GameObject.Find("UniWebView").GetComponent<webrequest>();

        webbtn = state.transform.Find("infobg").GetComponent<Button>();
        webbtn.onClick.AddListener(delegate ()
        {
            web.LoadWeb(item.address);
        });

        raw = state.transform.Find("infobg/pic").GetComponent<RawImage>();

        infodisText = state.transform.Find("infobg/pic/bottom/distance").GetComponent<Text>();
        infonameText = state.transform.Find("infobg/pic/bottom/name").GetComponent<Text>();
        infonameText.text = item.name;
        describeText = state.transform.Find("infobg/info").GetComponent<Text>();
        describeText.text = item.content;

        infobg = state.transform.Find("infobg").GetComponent<Transform>();
        iconbg = state.transform.Find("iconbg").GetComponent<Transform>();
        namebg1 = state.transform.Find("namebg1").GetComponent<Transform>();
        namebg2 = state.transform.Find("namebg2").GetComponent<Transform>();
        LoadType(item.typeName);

        StartCoroutine(LoadImgFromCache(item.thumbnail.localPath, raw));
    }
    public void ChangeFade(float value)
    {
        if (iconraw != null)
            iconraw.color = new Color(1, 1, 1, value);
        if (txtbg != null)
            txtbg.GetComponent<Image>().color = new Color(1, 1, 1, value);
        if (nameText != null)
            nameText.color = new Color(1, 1, 1, value);
        if (disText != null)
            disText.color = new Color(1, 1, 1, value);
        if (other != null)
            other.color = new Color(1, 1, 1, value);

        if (value == 1)
        {
            infobg.gameObject.SetActive(false);
            switch (type)
            {
                case "vsz-facilities":
                case "vsz-building":
                case "vsz-business":
                case "vsz-sell-ticket":
                    iconbg.gameObject.SetActive(true);
                    break;
                case "vsz-special-scenery":
                    namebg1.gameObject.SetActive(true);
                    break;
                case "vsz-scenery-dot":
                    namebg2.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public void ShowDirectSpot()
    {
        infobg.gameObject.SetActive(true);
        iconbg.gameObject.SetActive(false);
        namebg1.gameObject.SetActive(false);
        namebg2.gameObject.SetActive(false);
    }

    private void LoadType(string spottype)
    {
        type = spottype;
        switch (spottype)
        {
            //公共设施
            case "vsz-facilities":
                iconraw = state.transform.Find("iconbg/icon").GetComponent<RawImage>();
                iconraw.texture = Resources.Load<Texture>("GPS/厕所");

                disText = state.transform.Find("iconbg/distance").GetComponent<Text>();
                btn = iconraw.transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate ()
                {
                    GpsConvert.instance.ShowDirectSpot(item.id);
                });

                infobg.gameObject.SetActive(false);
                iconbg.gameObject.SetActive(true);
                namebg1.gameObject.SetActive(false);
                namebg2.gameObject.SetActive(false);

                break;
            //市政建筑
            case "vsz-building":
                iconraw = state.transform.Find("iconbg/icon").GetComponent<RawImage>();
                iconraw.texture = Resources.Load<Texture>("GPS/自行车");

                disText = state.transform.Find("iconbg/distance").GetComponent<Text>();
                btn = iconraw.transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate ()
                {
                    GpsConvert.instance.ShowDirectSpot(item.id);
                });

                infobg.gameObject.SetActive(false);
                iconbg.gameObject.SetActive(true);
                namebg1.gameObject.SetActive(false);
                namebg2.gameObject.SetActive(false);

                break;
            //商家
            case "vsz-business":
                iconraw = state.transform.Find("iconbg/icon").GetComponent<RawImage>();
                iconraw.texture = Resources.Load<Texture>("GPS/餐饮");

                disText = state.transform.Find("iconbg/distance").GetComponent<Text>();
                btn = iconraw.transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate ()
                {
                    GpsConvert.instance.ShowDirectSpot(item.id);
                });

                infobg.gameObject.SetActive(false);
                iconbg.gameObject.SetActive(true);
                namebg1.gameObject.SetActive(false);
                namebg2.gameObject.SetActive(false);

                break;
            //售票处
            case "vsz-sell-ticket":
                iconraw = state.transform.Find("iconbg/icon").GetComponent<RawImage>();
                iconraw.texture = Resources.Load<Texture>("GPS/酒店");


                disText = state.transform.Find("iconbg/distance").GetComponent<Text>();
                btn = iconraw.transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate ()
                {
                    GpsConvert.instance.ShowDirectSpot(item.id);
                });

                infobg.gameObject.SetActive(false);
                iconbg.gameObject.SetActive(true);
                namebg1.gameObject.SetActive(false);
                namebg2.gameObject.SetActive(false);

                break;

            //普通景点
            case "vsz-scenery-dot":
                txtbg = state.transform.Find("namebg1/name").GetComponent<RectTransform>();
                int count = item.name.Length;
                if (count > 7)
                    count = 7;
                txtbg.sizeDelta = new Vector2(60, count * 60);

                nameText = state.transform.Find("namebg1/name/name").GetComponent<Text>();
                nameText.text = item.name;
                disText = state.transform.Find("namebg1/distance").GetComponent<Text>();
                btn = txtbg.transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate ()
                {
                    GpsConvert.instance.ShowDirectSpot(item.id);
                });

                infobg.gameObject.SetActive(false);
                iconbg.gameObject.SetActive(false);
                namebg1.gameObject.SetActive(true);
                namebg2.gameObject.SetActive(false);

                break;
            //特色景点
            case "vsz-special-scenery":
                other = state.transform.Find("namebg2/name/Image").GetComponent<Image>();

                txtbg = state.transform.Find("namebg2/name").GetComponent<RectTransform>();
                int count2 = item.name.Length;
                if (count2 > 7)
                    count2 = 7;
                txtbg.sizeDelta = new Vector2(60, count2 * 60 + 70);

                nameText = state.transform.Find("namebg2/name/name").GetComponent<Text>();
                nameText.text = item.name;
                disText = state.transform.Find("namebg2/distance").GetComponent<Text>();
                btn = txtbg.transform.GetComponent<Button>();
                btn.onClick.AddListener(delegate ()
                {
                    GpsConvert.instance.ShowDirectSpot(item.id);
                });

                infobg.gameObject.SetActive(false);
                iconbg.gameObject.SetActive(false);
                namebg1.gameObject.SetActive(false);
                namebg2.gameObject.SetActive(true);

                break;
        }
    }
    static public bool IsAPointInACamera(Camera cam, Vector3 worldPos)
    {
        // 是否在视野内
        bool result1 = false;
        Vector3 posViewport = cam.WorldToViewportPoint(worldPos);
        Rect rect = new Rect(0, 0, 1, 1);
        result1 = rect.Contains(posViewport);
        // 是否在远近平面内
        bool result2 = false;
        if (posViewport.z >= cam.nearClipPlane && posViewport.z <= cam.farClipPlane)
        {
            result2 = true;
        }
        // 综合判断
        bool result = result1 && result2;
        return result;
    }

    public static float showDistance = 99999;
    private void Update()
    {
        if (state != null)
        {
            Vector2 player2DPosition = Camera.main.WorldToScreenPoint(transform.position + offset);
            recTransform.position = player2DPosition;
            //recTransform.position = Vector2.Lerp(recTransform.position, player2DPosition, Time.deltaTime * 1f);
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            //物体不在屏幕中就不显示  
            if (showDistance < 300)
                showDistance = 99999;
            if (IsAPointInACamera(Camera.main, transform.position) && distance > 80 && distance <= showDistance)
            {
                recTransform.gameObject.SetActive(true);
                //disText.text = "距离：" + (int)(distance/1000)+ "km";
                if (distance < 1000)
                {
                    if (disText != null)
                    {
                        disText.text = (int)distance + "m";
                        infodisText.text = (int)distance + "m";
                    }
                }
                else
                {
                    disText.text = float.Parse((distance / 1000).ToString("#0.0")) + "km";
                    infodisText.text = float.Parse((distance / 1000).ToString("#0.0")) + "km";
                }
#if UNITY_ANDROID
                state.transform.localScale = Vector3.one * 1.2f;
#elif UNITY_IOS || UNITY_IPHONE
                state.transform.localScale = Vector3.one * 1.2f;
#endif

                if (distance > 0 && distance <= 500)
                {
                    iconbg.transform.localScale = Vector3.one * 1f;
                    namebg1.transform.localScale = Vector3.one * 1f;
                    namebg2.transform.localScale = Vector3.one * 1f;
                }
                else if (distance > 1000 && distance <= 1000)
                {
                    iconbg.transform.localScale = Vector3.one * 0.9f;
                    namebg1.transform.localScale = Vector3.one * 0.9f;
                    namebg2.transform.localScale = Vector3.one * 0.9f;
                }
                else if (distance > 2000 && distance <= 1500)
                {
                    iconbg.transform.localScale = Vector3.one * 0.8f;
                    namebg1.transform.localScale = Vector3.one * 0.8f;
                    namebg2.transform.localScale = Vector3.one * 0.8f;
                }
                else
                {
                    iconbg.transform.localScale = Vector3.one * 0.7f;
                    namebg1.transform.localScale = Vector3.one * 0.7f;
                    namebg2.transform.localScale = Vector3.one * 0.7f;
                }
            }
            else
            {
                recTransform.gameObject.SetActive(false);
            }
            //lastTime = curtTime;

            dis = Vector3.Distance(transform.position, Camera.main.transform.position);
            //curScale = (normalScaleDis - dis) * rate + 1;
            //curScale = Mathf.Clamp(curScale, minScale, maxScale);
            //recTransform.localScale = curScale * Vector3.one;
        }
    }

    private IEnumerator LoadImgFromCache(string imgURl, RawImage img)
    {
        if (CheckCacheUrlIsExit(imgURl))
        {
            Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
            img.texture = tex;
            HttpBase.Download(imgURl, ((request, downloaded, length) => { }), ((request, response)
                =>
            {
                if (response == null || !response.IsSuccess)
                {
                    DebugManager.Instance.LogError("请求失败！");
                    return;
                }
                tex.LoadImage(response.Data);
            }));
        }
        else
        {
            yield break;
        }
    }
    /// <summary>
    /// 检查文件是否存在
    /// </summary>
    /// <param name="imgURl"></param>
    /// <returns></returns>
    private bool CheckCacheUrlIsExit(string imgURl)
    {
        return true;
    }
}
