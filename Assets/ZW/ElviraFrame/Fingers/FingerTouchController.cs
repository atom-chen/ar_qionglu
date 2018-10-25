using UnityEngine;
using System.Collections;
using System.IO;
/// <summary>
/// 点击屏幕实现缩放与旋转
/// </summary>
public class FingerTouchController : MonoBehaviour
{
    public static FingerTouchController instance;
    void Awake()
    {
        instance = this;
    }

    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2)  

    void Update()
    {
       
            //没有触摸  
            if (Input.touchCount <= 0)
            {
                return;
            }

        //单点触摸， x   z   移动
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            var deltaposition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-deltaposition.x * 0.1f, 0f, -deltaposition.y * 0.1f);
        }

        //多点触摸, 放大缩小  
        Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);

            //第2点刚开始接触屏幕, 只记录，不做处理  
            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }

            //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

            //两个距离之差，为正表示放大手势， 为负表示缩小手势  
            float offset = newDistance - oldDistance;

            //放大因子， 一个像素按 0.01倍来算(100可调整)  
            float scaleFactor = offset / 200f;
            Vector3 localScale = transform.localScale;
            Vector3 scale = new Vector3(Mathf.Abs(localScale.x + scaleFactor),
                                     Mathf.Abs(localScale.y + scaleFactor),
                                    Mathf.Abs(localScale.z + scaleFactor));

            //最小缩放到 1 倍  
            if ((scale.x >=1f && scale.y >= 1f && scale.z >= 1f) && (scale.x <= 1.3f && scale.y <= 1.3f && scale.z <= 1.3f))
            {
                transform.localScale = scale;
            }

            //记住最新的触摸点，下次使用  
            oldTouch1 = newTouch1;
            oldTouch2 = newTouch2;
        }
    }
