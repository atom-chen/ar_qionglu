using ElviraFrame;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_IOS || UNITY_IPHONE
using com.mob.mobpush;
#endif




public class DistanceComparer : IComparer<PushMsg>
{
    public int Compare(PushMsg x, PushMsg y)
    {
        return x.distance.CompareTo(y.distance);
    }
}

public class PushManager : MonoBehaviour
{
#if UNITY_IOS || UNITY_IPHONE
    public MobPush mobPush;
#endif
    Dictionary<int, PushMsg> pushPointIndexV2Dic = new Dictionary<int, PushMsg>();
    /// <summary>
    /// 推送范围内的景点List
    /// </summary>
    List<PushMsg> nearIndexList = new List<PushMsg>();
    PushPointClass pushPointClass = new PushPointClass();
    int lastPushId = -1;
    IEnumerator Start()
    {
        MobPushInit();
        InvokeRepeating("GetCurerntLocation", 60f, 60f);
        yield return new WaitForSeconds(1f);
        string reader = JsonManager.ReadJsonFromFilePath(UnityHelper.LocalFilePath + "Push/", "content.json");
        if (reader != null)
        {
            Debug.Log("reader不为空=====" + reader);
            pushPointClass = JsonMapper.ToObject<PushPointClass>(reader);
            AddToList(pushPointClass);
        }
        else
        {
            Debug.Log("reader为空=====" + reader);
        }
    }

    private void MobPushInit()
    {
        // IPHONE 要想收到 APNs 和本地通知，必须先要 setCustom (only ios)
#if UNITY_IPHONE
        mobPush = gameObject.GetOrCreateComponent<MobPush>();
        mobPush.onNotifyCallback = OnNitifyHandler;
        // 真机调试 false , 上线 true
        mobPush.setAPNsForProduction(false);

        CustomNotifyStyle style = new CustomNotifyStyle();
        style.setType(CustomNotifyStyle.AuthorizationType.Badge | CustomNotifyStyle.AuthorizationType.Sound | CustomNotifyStyle.AuthorizationType.Alert);
        mobPush.setCustomNotification(style);

#endif
    }

    private void AddToList(PushPointClass pointDataParams)
    {
        Debug.Log("AddToLinkList");
        if (pointDataParams == null)
        {
            return;
        }
        Debug.Log(pointDataParams.msgs.Count);
        for (int i = 0; i < pointDataParams.msgs.Count; i++)
        {
            PushMsg pushMsg = new PushMsg();
            pushMsg.id = pointDataParams.msgs[i].id;
            string x = pointDataParams.msgs[i].locationX;
            pushMsg.locationX = x;

            string y = pointDataParams.msgs[i].locationY;
            pushMsg.locationY = y;
            pushMsg.pos = new Vector2(float.Parse(x), float.Parse(y));

            pushMsg.height = pointDataParams.msgs[i].height;
            pushMsg.time = pointDataParams.msgs[i].time;
            pushMsg.url = pointDataParams.msgs[i].url;
            pushMsg.title = pointDataParams.msgs[i].title;
            pushMsg.msg = pointDataParams.msgs[i].msg;
            pushMsg.distance = 10000f;
            pushPointIndexV2Dic.Add(int.Parse(pushMsg.id), pushMsg);

        }
        Debug.Log(pushPointIndexV2Dic.Count);
    }


    public void GetCurerntLocation()
    {
#if UNITY_EDITOR
        Vector2 currentGps = new Vector2(104.067304f, 30.597963f);
#else
      String location = GPSManager.Instance.GetBDGPSLocation();
        Debug.Log("GPS::::::::::" + location);
        JsonData zb = JsonMapper.ToObject(location);
        float x = float.Parse(zb["longitude"].ToString());
        float y = float.Parse(zb["latitude"].ToString());

     Vector2 currentGps = new Vector2(x, y);
#endif
        GetNearEastPointList(currentGps);
        if (nearIndexList.Count != 0)
        {
            PushPointinformation();
        }
    }

    /// <summary>
    /// 获取范围内的一个或者多个景点
    /// </summary>
    /// <param name="gps1"></param>
    /// <returns></returns>
    public List<int> GetNearEastPointList(Vector2 currentGps)
    {
        nearIndexList.Clear();
        if (pushPointIndexV2Dic.Count != 0)
        {
            foreach (KeyValuePair<int, PushMsg> kt in pushPointIndexV2Dic)
            {
                kt.Value.distance = UnityHelper.GetDistance(currentGps, kt.Value.pos);
                if (kt.Value.distance <= float.Parse(pushPointClass.radius))
                {
                    nearIndexList.Add(kt.Value);
                }
            }
        }
        if (nearIndexList.Count != 0)
        {
            Debug.Log("nearIndexList.Count==" + nearIndexList.Count);
            return nearIndexList.Sort(new DistanceComparer());
        }
        return null;
    }



    private void PushPointinformation()
    {
        if (nearIndexList.Count != 0)
        {
           
               PushMsg newPushMsg = pushPointIndexV2Dic[nearIndexList[0].id];
            if (lastPushId != int.Parse(newPushMsg.id))
            {
                NotifyCallBack notifyCallBack = new NotifyCallBack()
                {
                    id = newPushMsg.id
                };
#if UNITY_ANDROID
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("notifySimpleMessage", newPushMsg.title, newPushMsg.msg, JsonMapper.ToJson(notifyCallBack));
                lastPushId = int.Parse(newPushMsg.id);
#elif UNITY_IOS || UNITY_IPHONE

            LocalNotifyStyle style = new LocalNotifyStyle();
            style.setContent("Text");
            style.setTitle("title");
            lastPushId=newPushMsg.id;
#endif
            }
        }
    }

}
