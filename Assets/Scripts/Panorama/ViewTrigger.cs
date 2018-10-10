using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewTrigger : MonoBehaviour
{
    /// <summary>
    /// 显示名称的物体
    /// </summary>
    public GameObject Names;

    /// <summary>
    /// 图片外框
    /// </summary>
    public  GameObject Kuang;
    /// <summary>
    /// 展示的图片
    /// </summary>
    public Sprite[] Sprites;
    /// <summary>
    /// 图片
    /// </summary>
    protected Image Photo;
    /// <summary>
    /// 热点介绍
    /// </summary>
    public AudioClip AC;


    public float activationTime = 0.8f;
    public Material focusedMaterial;
    public Material nonFocusedMaterial;
    public bool Focused { get; set; }
    private float mFocusedTime = 0;
    private bool mTriggered = false;

    private Vector3 initScale;
    //private MediaPlayerCtrl MPC;
    private  AudioSource audioS;


    private bool active;


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {

        active = false;

        //MPC = FindObjectOfType<MediaPlayerCtrl>();

        audioS = gameObject.AddComponent<AudioSource>();
        audioS.volume = 1;
        audioS.loop = false;
        audioS.maxDistance = 1000;

        initScale = transform.localScale;
        Names.SetActive(true);
        Kuang.SetActive(false);
        Photo = Kuang.transform.GetChild(0).GetComponent<Image>();
        mTriggered = false;
        mFocusedTime = 0;
        Focused = false;
        GetComponent<Renderer>().material = nonFocusedMaterial;
    }

    void Update()
    {
        bool startAction = false;
        if (Input.GetMouseButtonUp(0))
        {
            startAction = true;
        }
        if (Focused)
        {
            if (mTriggered)
                return;
            active = true;
            UpdateMaterials(Focused);

            this.transform.localScale = Vector3.Slerp(initScale, new Vector3(initScale.x + 0.0005f, initScale.y + 0.0005f, initScale.z + 0.0005f), 5f);

            mFocusedTime += Time.deltaTime;
            if ((mFocusedTime > activationTime) || startAction)
            {
                mTriggered = true;

                mFocusedTime = 0;
                //MPC.SetVolume(0.1f);

                StartCoroutine(DoSomeThing());
            }
        }
        else
        {

            //Debug.Log(active);

            if (!active)
            {
                return;
            }
            Stop();
        }
    }

    protected virtual IEnumerator DoSomeThing()
    {


        // StartCoroutine(ResetAfter(0.3f));
        Names.SetActive(false);
        Kuang.SetActive(true);
        Photo.sprite = Sprites[0];
        yield return new WaitForEndOfFrame();
        //Debug.Log(MPC.transform.name);
        audioS.clip = AC;
        audioS.Play();
    }

    protected virtual void Stop()
    {
        //Debug.Log("DDDDDDDDDDDDDDDDDDDDDDDDDDD");

        Names.SetActive(true);
        Kuang.SetActive(false);
        UpdateMaterials(Focused);
        mTriggered = false;
        active = false;
        mFocusedTime = 0;
        this.transform.localScale = initScale;
        audioS.Stop();

        //MPC.SetVolume(1f);

    }
    private void UpdateMaterials(bool focused)
    {
        Renderer meshRenderer = GetComponent<Renderer>();
        if (focused)
        {
            if (meshRenderer.material != focusedMaterial)
                meshRenderer.material = focusedMaterial;
        }
        else
        {
            if (meshRenderer.material != nonFocusedMaterial)
                meshRenderer.material = nonFocusedMaterial;
        }

        float t = focused ? Mathf.Clamp01(mFocusedTime / activationTime) : 0;
        foreach (var rnd in GetComponentsInChildren<Renderer>())
        {
            if (rnd.material.shader.name.Equals("Custom/SurfaceScan"))
            {
                rnd.material.SetFloat("_ScanRatio", t);
            }
        }
    }
    #endregion // PRIVATE_METHODS
}
