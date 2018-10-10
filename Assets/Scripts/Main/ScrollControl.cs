using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{
    public static ScrollControl instance;
    /// <summary>
    /// 横向滚动条
    /// </summary>
    public Scrollbar m_HScrollBar;

    /// <summary>
    /// 竖向滚动条
    /// </summary>
    public Scrollbar[] m_VScrollBars;

    /// <summary>
    /// 有竖向滚动的页面
    /// </summary>
    public int[] m_VScrollIndexs;

    /// <summary>
    /// 页面个数
    /// </summary>
    public int m_Num;

    /// <summary>
    /// 设置移动超过多少百分比之后向下翻页
    /// </summary>
    public float m_NextLimit;

    /// <summary>
    /// 滑动敏感值
    /// </summary>
    public float m_Sensitive;

    /// <summary>
    /// 鼠标上一次的位置
    /// </summary>
    private Vector3 mOldPosition;

    /// <summary>
    /// 记录上一次的value
    /// </summary>
    private float mOldValue;

    private float mTargetPosition = 0.5f;

    private int mCurrentIndex = 3;

    private int mTargetIndex = 3;

    /// <summary>
    /// 是否可以移动
    /// </summary>
    private bool mCanMove = false;

    /// <summary>
    /// 初始移动速度
    /// </summary>
    private float mMoveSpeed;

    /// <summary>
    /// 平滑移动参数
    /// </summary>
    private const float SMOOTH_TIME = 0.2F;

    private float mDragParam = 0;
    private float mPageWidth = 0;

    /// <summary>
    /// 是否需要进行滑动方向判定
    /// </summary>
    private bool mNeedCaculate = false;

    /// <summary>
    /// 是否进行竖向滚动
    /// </summary>
    public bool mIsScollV = false;

    /// <summary>
    /// 竖向临时滚动条
    /// </summary>
    private Scrollbar mVScrollBar;

    public void SetNextIndex(int pIndex)
    {
        mTargetIndex = pIndex;
        mTargetPosition = (mTargetIndex - 1) * mPageWidth;
        mIsScollV = false;
        mCanMove = true;
    }

    private void OnPointerDown(Vector2 mousePosition)
    {
        // 记录当前value
        mOldValue = m_HScrollBar.value;
        mOldPosition = Input.mousePosition;
        // mCanMove = false;
        mCurrentIndex = GetCurrentIndex(mOldValue);
        // 判断当前是否在可竖向滑动的页面上
        mNeedCaculate = true;
        mVScrollBar = m_VScrollBars[0];
        for (int i = 0; i < m_VScrollIndexs.Length; ++i)
        {
            if (m_VScrollIndexs[i] == mCurrentIndex)
            {
                mNeedCaculate = true;
                mVScrollBar = m_VScrollBars[i];
                break;
            }
        }
    }

    private void OnDrag(Vector2 mousePosition)
    {

        Vector2 dragVector = Input.mousePosition - mOldPosition;

        if (mNeedCaculate)
        {
            mNeedCaculate = false;

            if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y))
            {
                mIsScollV = false;
            }
            else
            {
                mIsScollV = true;
            }
        }

        DragScreen(dragVector);

        mOldPosition = Input.mousePosition;
    }

    private void OnPointerUp(Vector2 mousePosition)
    {
        Vector2 dragVector = Input.mousePosition - mOldPosition;
        DragScreen(dragVector);

        mOldPosition = Input.mousePosition;

        float valueOffset = m_HScrollBar.value - mOldValue;
        if (Mathf.Abs((valueOffset) / mPageWidth) > m_NextLimit)
        {
            mTargetIndex += valueOffset > 0 ? 1 : -1;
            mTargetPosition = (mTargetIndex - 1) * mPageWidth;
        }

        mCanMove = true;
    }

    private int GetCurrentIndex(float pCurrentValue)
    {
        return Mathf.RoundToInt(pCurrentValue / mPageWidth + 1);
    }

    private void DragScreen(Vector2 pDragVector)
    {
        if (mIsScollV)
        {
            float oldValue = mVScrollBar.value;
            mVScrollBar.value -= pDragVector.y / Screen.height * mVScrollBar.size;
            mMoveSpeed = mVScrollBar.value - oldValue;
        }
        else
        {
            //float oldValue = m_HScrollBar.value;
            //m_HScrollBar.value -= pDragVector.x / Screen.width * mDragParam;
            //mMoveSpeed = m_HScrollBar.value - oldValue;
        }
    }

    void Awake()
    {
        instance = this;
        if (m_Num <= 1)
        {
            Debug.LogError("参数错误:页面个数不对");
        }
        mDragParam = 1f / (m_Num - 1) * m_Sensitive;
        mPageWidth = 1f / (m_Num - 1);
        mCurrentIndex = GetCurrentIndex(m_HScrollBar.value);
        mTargetIndex = mCurrentIndex;
    }

    void Start()
    {
        InputControl.instance.EVENT_MOUSE_DOWN += OnPointerDown;
        InputControl.instance.EVENT_MOUSE_UP += OnPointerUp;
        InputControl.instance.EVENT_MOUSE_DRAG += OnDrag;
    }

    void OnDestory()
    {
        InputControl.instance.EVENT_MOUSE_DOWN -= OnPointerDown;
        InputControl.instance.EVENT_MOUSE_UP -= OnPointerUp;
        InputControl.instance.EVENT_MOUSE_DRAG -= OnDrag;
    }


    void Update()
    {
        if (mCanMove)
        {
            if (mIsScollV)
            {
                mVScrollBar.value += mMoveSpeed;
                float absValue = Mathf.Abs(mMoveSpeed);
                absValue -= 0.001f;
                if (absValue <= 0)
                {
                    mCanMove = false;
                }
                else
                {
                    mMoveSpeed = mMoveSpeed > 0 ? absValue : -absValue;
                }
            }
            else
            {
                //if (Mathf.Abs(m_HScrollBar.value - mTargetPosition) < 0.01f)
                //{
                //    m_HScrollBar.value = mTargetPosition;
                //    mCurrentIndex = mTargetIndex;
                //    mCanMove = false;
                //    return;
                //}
                //m_HScrollBar.value = Mathf.SmoothDamp(m_HScrollBar.value, mTargetPosition, ref mMoveSpeed, SMOOTH_TIME);
            }

        }
    }

    public void EnterPlay()
    {
        SceneManager.LoadScene("play");
    }
}