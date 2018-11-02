using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;

public class bird3 : MonoBehaviour
{
    public static bird3 instance;

    private void Awake()
    {
        instance = this;
    }

    public Animator[] birdPaths;
    public Animator[] birds;

    public Animator women;
    private bool isLost = true;

    public GameObject food,FX,hand,food2;

    public Transform videoPanel;

    public VideoPlayer vp;

    private bool canEat;

    public AudioSource audFX;
    // Use this for initialization
    void Start ()
    {
        pr0 = PlayRandomAnim();
        pr1 = PlayRandomAnim1();
    }

    private IEnumerator fo,pb1,pb2,pr0,pr1;
    #region 1,2鸟
    public void OnTrackerFound()
    {
        if (isLost)
        {       
            food.SetActive(false);
            food2.SetActive(false);
            hand.SetActive(false);
            canEat = false;
            isLost = false;
            fo = FoundEvent();
            StartCoroutine(fo);
        }
    }
    public void OnTrackerLost()
    {
        isLost = true;
        canEat = false;
        
                     
        if (fo != null)
        {
            StopCoroutine(fo);
        }
                     
        if (pb1 != null)
        {
            StopCoroutine(pb1);
        }
                     
        if (pb2 != null)
        {
            StopCoroutine(pb2);
        }
                     
        if (pr0 != null)
        {
            StopCoroutine(pr0);
        }
                     
        if (pr1 != null)
        {
            StopCoroutine(pr1);
        }


        women.Play("idle");
        FX.SetActive(false);
        women.gameObject.SetActive(false);
        
        vp.Stop();
        
        videoPanel.DOKill();
        videoPanel.localPosition=new Vector3(-0.061f,0.01f,-0.197f);
        videoPanel.localScale=new Vector3(0.3f,0.3f,0.3f);
        
        birdPaths[0].Play("idle");
        birdPaths[1].Play("idle");
        birds[0].Play("FeiXing");
        birds[1].Play("FeiXing");
        birdPaths[0].gameObject.SetActive(false);
        birdPaths[1].gameObject.SetActive(false);
    }

    IEnumerator FoundEvent()
    {
        women.gameObject.SetActive(true);
        women.Play("start");
        FX.SetActive(true);
        audFX.Play();
        yield return new WaitForSeconds(2.0f);
        women.Play("speak");
        
        videoPanel.gameObject.SetActive(true);
        vp.url = ARScanManager.scan_Conjure_Path + "/hongzuiou.mp4";
        
        Debug.Log(vp.url);
        vp.Play();
        videoPanel.DOLocalMove(new Vector3(-0.01f, 0.01f, 0), 1);
        videoPanel.DOScale(Vector3.one, 1);
        
        birdPaths[0].gameObject.SetActive(true);
        birdPaths[1].gameObject.SetActive(true);
        birdPaths[0].Play("play");
        birdPaths[1].Play("play");
        
        pb1 = playBird1();
        pb2 = playBird2();
        StartCoroutine(pb1);
        StartCoroutine(pb2);
        
        yield return new WaitForSeconds(2.0f);
        FX.SetActive(false);
        yield return new WaitForSeconds(18.0f);
        videoPanel.gameObject.SetActive(false);
        women.Play("Idle");
        canEat = true;
        hand.SetActive(true);
    }
    
    IEnumerator playBird1()
    {
        birds[0].Play("FeiXing");
        birds[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        birds[0].Play("JiangLuo");
        yield return new WaitForSeconds(1.33f);
        birds[0].Play("DaiJi");
        yield return new WaitForSeconds(1.66f);
        birds[0].Play("ShuLi01");
        yield return new WaitForSeconds(3.33f);
        birds[0].Play("KaoJin");
        yield return new WaitForSeconds(2.1f);
        birds[0].Play("DaiJi");
        yield return new WaitForSeconds(1.66f);
        birds[0].Play("ShuLi03");
        yield return new WaitForSeconds(2.0f);
        
        pr0 = PlayRandomAnim();
        StartCoroutine(pr0);
    }
    
    IEnumerator playBird2()
    {

        birds[1].Play("FeiXing");
        birds[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        birds[1].Play("JiangLuo");
        yield return new WaitForSeconds(1.33f);
        birds[1].Play("XiuXian02");
        yield return new WaitForSeconds(4.0f);
        birds[1].Play("ShuLi02");
        yield return new WaitForSeconds(2.0f);
        birds[1].Play("DaiJi");
        yield return new WaitForSeconds(1.66f);
           
        pr1 = PlayRandomAnim1();
        StartCoroutine(pr1);
    }

    public IEnumerator PlayRandomAnim()
    {
        if (isLost)
        {
            yield break;;
        }
        int i = Random.Range(0, 3);
        switch (i)
        {
            case 0:
                birds[0].Play("DaiJi");
                yield return new WaitForSeconds(1.68f);
                break;
            case 1:
                birds[0].Play("ShuLi01");
                yield return new WaitForSeconds(3.12f);
                break;
            case 2:
                birds[0].Play("XiuXian02");
                yield return new WaitForSeconds(3.32f);
                break;
            case 3:
                birds[0].Play("XiuXian01");
                yield return new WaitForSeconds(4.32f);
                break;
        }
        StartCoroutine(pr0);
    }
    
    
    public IEnumerator PlayRandomAnim1()
    {
        if (isLost)
        {
            yield break;
        }
        int i = Random.Range(0, 3);
        switch (i)
        {
            case 0:
                birds[1].Play("DaiJi");
                yield return new WaitForSeconds(1.68f);
                break;
            case 1:
                birds[1].Play("ShuLi01");
                yield return new WaitForSeconds(3.12f);
                break;
            case 2:
                birds[1].Play("XiuXian02");
                yield return new WaitForSeconds(3.32f);
                break;
            case 3:
                birds[1].Play("XiuXian01");
                yield return new WaitForSeconds(4.32f);
                break;
        }
        StartCoroutine(pr1);
    }
    #endregion

    private bool isFeeding;
    private IEnumerator cc;
	public void ShowBird3()
    {
        if (!isFeeding && canEat)
        {        
            hand.GetComponent<SpriteRenderer>().color=new Color(1,1,1,0.3f);
            isFeeding = true;
            cc = EatFood();
            StartCoroutine(cc);
        }
        
    }
    IEnumerator	EatFood()
    {            
        food2.SetActive(false);
        food.SetActive(true);
        birdPaths[2].Play("play");
        birds[2].Play("FeiXing");
        birds[2].gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        birds[2].Play("JiangLuo");
        yield return new WaitForSeconds(1.66f);
        birds[2].Play("XiuXian01");
        yield return new WaitForSeconds(5.0f);
        birds[2].Play("DiaoMianBao");
        yield return new WaitForSeconds(1f);
        food2.SetActive(true);
        yield return new WaitForSeconds(1.33f);
        birds[2].Play("QiFei");
        yield return new WaitForSeconds(0.67f);
        birds[2].Play("FeiXing");
        yield return new WaitForSeconds(3.0f);
        birds[2].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        birdPaths[2].Play("idle");
    }


   public void HideBird3()
    {
        if (isFeeding)
        {
            if (cc!=null)
            {
                StopCoroutine(cc);
            }

            food2.SetActive(false);
            hand.GetComponent<SpriteRenderer>().color=new Color(1,1,1,1);
            isFeeding = false;
            food.SetActive(false);
            birds[2].Play("FeiXing");
            birds[2].gameObject.SetActive(false);
            birdPaths[2].Play("idle");                  
        }
    }
    
}
