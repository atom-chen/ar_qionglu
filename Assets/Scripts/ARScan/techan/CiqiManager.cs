using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Video;

public class CiqiManager : MonoBehaviour
{
    #region PROTECTED_MEMBER_VARIABLES
    public static CiqiManager instance;
    public GameObject[] ciqi;
    public Animator women;
    public DOTweenAnimation[] uiDo;
    public VideoPlayer vp;
    public DOTweenAnimation[] ciqi1, ciqi2, ciqi3, ciqi4;
    public GameObject gaoguang;
    public Material[] panzi;

    public AudioSource aud1, aud2,audFX;
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

        vp.url = ARScanManager.scan_native_product_Path + "/qiqi.mp4";
    }
    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    float ChangeTime = 25;
    private IEnumerator ft;
    IEnumerator FirstStep()
    {
        women.Play("start");
        audFX.Play();
        yield return new WaitForSeconds(2);
        uiDo[0].DOPlayForward();
        women.Play("speak");
        aud1.Play();
        yield return new WaitForSeconds(17f);
        women.Play("Idle");
        uiDo[1].DOPlayForward();
        
        vp.url = ARScanManager.scan_native_product_Path + "/qiqi.mp4";
        Debug.Log(vp.url);
        vp.Play();
        yield return new WaitForSeconds(0.5f);
        women.Play("speak");
        yield return new WaitForSeconds(42);
        uiDo[1].DOPlayBackwards();
        vp.Stop();
        yield return new WaitForSeconds(1);
        women.Play("speak");
        aud2.Play();
        yield return new WaitForSeconds(15f);
        women.Play("Idle");
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

    public  void LostEvent()
    {
        aud1.Stop();
        aud2.Stop();
        gaoguang.SetActive(true);
        women.Play("Idle");
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
        ft = FirstStep();
        StartCoroutine(ft);
    }
    public void OnTrackingLost()
    {
        if (ft != null)
        {
            StopCoroutine(ft);
        }

        LostEvent();
    }
    #endregion // PROTECTED_METHODS
}