using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Video;

public class CiqiManager : MonoBehaviour
{
    #region PROTECTED_MEMBER_VARIABLES
    public static CiqiManager instance;
    public GameObject[] ciqi;
    private string womenstate;
    public Sprite[] idleTexture, speakTexture;
    public SpriteRenderer womenMat;
    public DOTweenAnimation[] uiDo;
    public VideoPlayer vp;
    public DOTweenAnimation[] ciqi1, ciqi2, ciqi3, ciqi4;
    public GameObject gaoguang;
    public Material[] panzi;
    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    private void Awake()
    {
        instance = this;
    }
    protected virtual void Start()
    {
        panzi[0].color = new Color(1, 1, 1, 1);
        panzi[1].color = new Color(1, 1, 1, 0);
        panzi[2].color = new Color(1, 1, 1, 0);
        panzi[3].color = new Color(1, 1, 1, 0);
    }
    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS
    int num;
    float timer = 0.125f;
    void WomenAnim()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 0.125f;
            switch (womenstate)
            {
                case "idle":
                    num++;
                    if (num > idleTexture.Length - 1)
                        num = 0;
                    womenMat.sprite = idleTexture[num];
                    break;
                case "speak":
                    num++;
                    if (num > speakTexture.Length - 1)
                        num = 0;
                    womenMat.sprite = speakTexture[num];
                    break;
                default:
                    womenstate = "idle";
                    break;
            }
        }
    }
    float ChangeTime = 25;
    IEnumerator FirstStep()
    {

        string scan_native_product = "";
        foreach (var ChangeInfo in mainPageUI.curScenicInfo.ResourcesInfos)
        {
            if (ChangeInfo.ResourcesKey == "scan_native_product")
            {
                scan_native_product = ChangeInfo.LocalPath;
                Debug.Log(scan_native_product);
            }
        }

        vp.url = scan_native_product + "/panzi.mp4";

        uiDo[0].DOPlayForward();
        womenstate = "speak";
        num = 0;
        yield return new WaitForSeconds(3);
        AudioManager.instance.PlayFX("1");
        yield return new WaitForSeconds(23f);
        womenstate = "idle";
        uiDo[1].DOPlayForward();
        vp.Play();
        yield return new WaitForSeconds(42);
        uiDo[1].DOPlayBackwards();
        vp.Stop();
        yield return new WaitForSeconds(1);
        num = 17;
        womenstate = "speak";
        AudioManager.instance.PlayFX("2");
        yield return new WaitForSeconds(18.2f);
        womenstate = "idle";
        uiDo[2].DOPlayForward();
        //瓷器1开始转
        for (int i = 0; i < ciqi1.Length; i++)
        {
            ciqi1[i].DOPlayForward();
        }
        yield return new WaitForSeconds(ChangeTime);
        //瓷器1淡出
        panzi[0].DOFade(0, 1).SetEase(Ease.Linear);
        gaoguang.SetActive(false);
        yield return new WaitForSeconds(1);
        ciqi[1].SetActive(true);
        ciqi[0].SetActive(false);
        //瓷器2淡入
        panzi[1].DOFade(1, 1).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1);
        gaoguang.SetActive(true);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < ciqi2.Length; i++)
        {
            ciqi2[i].DOPlayForward();
        }
        yield return new WaitForSeconds(ChangeTime);
        //瓷器2淡出
        panzi[1].DOFade(0, 1).SetEase(Ease.Linear);
        gaoguang.SetActive(false);
        yield return new WaitForSeconds(1);
        ciqi[2].SetActive(true);
        ciqi[1].SetActive(false);
        //瓷器3淡入
        panzi[2].DOFade(1, 1).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1);
        gaoguang.SetActive(true);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < ciqi3.Length; i++)
        {
            ciqi3[i].DOPlayForward();
        }
        yield return new WaitForSeconds(ChangeTime);
        //瓷器3淡出
        panzi[2].DOFade(0, 1).SetEase(Ease.Linear);
        gaoguang.SetActive(false);
        yield return new WaitForSeconds(1);
        ciqi[3].SetActive(true);
        ciqi[2].SetActive(false);
        //瓷器4淡入
        panzi[3].DOFade(1, 1).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1);
        gaoguang.SetActive(true);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < ciqi4.Length; i++)
        {
            ciqi4[i].DOPlayForward();
        }
    }
    private void Update()
    {
        WomenAnim();
    }
    public  void LostEvent()
    {
        gaoguang.SetActive(true);
        womenstate = "idle";
        num = 0;
        for (int i = 0; i < uiDo.Length; i++)
        {
            uiDo[i].DOPlayBackwards();
        }
        vp.Stop();
        for (int i = 0; i < ciqi1.Length; i++)
        {
            ciqi1[i].DOPause();
            ciqi1[i].transform.localEulerAngles = Vector3.zero;
        }
        ciqi[0].SetActive(true);
        ciqi[1].SetActive(false);
        ciqi[2].SetActive(false);
        ciqi[3].SetActive(false);
        AudioManager.instance.StopFX();

        panzi[0].DOKill();
        panzi[1].DOKill();
        panzi[2].DOKill();
        panzi[3].DOKill();

        panzi[0].color = new Color(0.95f, 0.95f, 0.95f, 1);
        panzi[1].color = new Color(0.95f, 0.95f, 0.95f, 0);
        panzi[2].color = new Color(0.95f, 0.95f, 0.95f, 0);
        panzi[3].color = new Color(0.95f, 0.95f, 0.95f, 0);
    }
    #endregion // PUBLIC_METHODS
    #region PROTECTED_METHODS
    public void OnTrackingFound()
    {


        StartCoroutine(FirstStep());
        KQCManager._Instance.OnTrackingLost();
        BDManager._Instance.OnTrackingLost();
    }
    public void OnTrackingLost()
    {
        StopAllCoroutines();
        LostEvent();
        OnTrackingLost();
    }
    #endregion // PROTECTED_METHODS
}