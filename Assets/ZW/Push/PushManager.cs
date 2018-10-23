using ElviraFrame;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices;
using ElviraFrame.Excel;
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


    public int pushState = 0;
#if UNITY_IOS || UNITY_IPHONE
    public MobPush mobPush;
#endif
    Dictionary<int, PushItem> pushPointIndexV2Dic = new Dictionary<int, PushItem>();
    /// <summary>
    /// 推送范围内的景点List
    /// </summary>
    List<PushItem> nearIndexList = new List<PushItem>();

    int lastPushId = -1;
    IEnumerator Start()
    {
        MobPushInit();
        AddPoint();
        yield return new WaitForSeconds(1f);
        //string reader = JsonManager.ReadJsonFromFilePath(UnityHelper.LocalFilePath + "Push/", "content.json");
        //if (reader != null)
        //{
        //    Debug.Log("reader不为空=====" + reader);
        //    pushPointClass = JsonMapper.ToObject<PushPointClass>(reader);
        //    AddToList(pushPointClass);
        //}
        //else
        //{
        //    Debug.Log("reader为空=====" + reader);
        //}


          InvokeRepeating("GetCurerntLocation", 0f, 60f);

       //GetCurerntLocation();
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

    //private void AddToList(PushPointClass pointDataParams)
    //{
    //    Debug.Log("AddToLinkList");
    //    if (pointDataParams == null)
    //    {
    //        return;
    //    }
    //    Debug.Log(pointDataParams.msgs.Count);
    //    for (int i = 0; i < pointDataParams.msgs.Count; i++)
    //    {
    //        PushMsg pushMsg = new PushMsg();
    //        pushMsg.id = pointDataParams.msgs[i].id;
    //        string x = pointDataParams.msgs[i].locationX;
    //        pushMsg.locationX = x;

    //        string y = pointDataParams.msgs[i].locationY;
    //        pushMsg.locationY = y;
    //        pushMsg.pos = new Vector2(float.Parse(x), float.Parse(y));

    //        pushMsg.height = pointDataParams.msgs[i].height;
    //        pushMsg.time = pointDataParams.msgs[i].time;
    //        pushMsg.url = pointDataParams.msgs[i].url;
    //        pushMsg.title = pointDataParams.msgs[i].title;
    //        pushMsg.msg = pointDataParams.msgs[i].msg;
    //        //pushMsg.distance = 10000f;
    //        pushPointIndexV2Dic.Add(int.Parse(pushMsg.id), pushMsg);

    //    }
    //    Debug.Log(pushPointIndexV2Dic.Count);
    //}
    public void AddPoint()
    {
        PushItemManager pointdata = Resources.Load<PushItemManager>("DataAssets/PushItem");
        Debug.Log("pointdata====" + pointdata.dataArray.Length);
        foreach (PushItem item in pointdata.dataArray)
        {
            PushItem pushItemMsg = new PushItem();
            pushItemMsg.id = item.id;

            pushItemMsg.locationX = item.locationX;


            pushItemMsg.locationY = item.locationY;
            pushItemMsg.pos = item.pos;

            pushItemMsg.height = item.height;
            pushItemMsg.time = item.time;
            pushItemMsg.url = item.url;
            pushItemMsg.title = item.title;
            pushItemMsg.msg = item.msg;
            pushItemMsg.distance =item.distance;



            Debug.Log("pointdata====" + pushItemMsg.title+"--"+ pushItemMsg.msg + "--" + pushItemMsg.pos);
            pushPointIndexV2Dic.Add(int.Parse(pushItemMsg.id), pushItemMsg);
        }
    }




    [DllImport("__Internal")]
    private static extern string GetLocation();//接收字符串



    /// <summary>
    /// 获取GPS经纬度信息
    /// 返回值
    /// {"altitude":460.1,"available":true,"latitude":30.597598,"longitude":104.066928}
    /// </summary>
    /// <returns></returns>
    public string GetBDGPSLocation()
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        String location = jo.Call<String>("getLocation");
        return location;
#endif
        return GetLocation();

    }

    public void GetCurerntLocation()
    {
#if UNITY_EDITOR
        Vector2 currentGps = new Vector2(104.067304f, 30.597963f);
#else
      String location = GetBDGPSLocation();
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
    public List<PushItem> GetNearEastPointList(Vector2 currentGps)
    {
        nearIndexList.Clear();
        if (pushPointIndexV2Dic.Count != 0)
        {
            foreach (KeyValuePair<int, PushItem> kt in pushPointIndexV2Dic)
            {
              kt.Value.distance = UnityHelper.GetDistance(currentGps, kt.Value.pos);

                Debug.Log(" kt.Value.distance ====" + (int)kt.Value.distance + "------");
                if ((int)kt.Value.distance <= 300)
                {
                    nearIndexList.Add(kt.Value);
                }
            }
        }
        if (nearIndexList.Count != 0)
        {
            Debug.Log("nearIndexList.Count==" + nearIndexList.Count);

            nearIndexList.Sort((x, y) => { return x.distance.CompareTo(y.distance); });
            foreach (var item in nearIndexList)
            {
                Debug.Log(item.distance);
            }
            return  nearIndexList ;
        }
        return null;
    }



    private void PushPointinformation()
    {
        if (nearIndexList.Count != 0)
        {

            PushItem newPushMsg = pushPointIndexV2Dic[int.Parse(nearIndexList[0].id)];
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

         pushState = 0;
			LocalNotifyStyle style = new LocalNotifyStyle ();
            Hashtable args = new Hashtable();
            args["pushId"] = newPushMsg.id;
   
			style.setContent (newPushMsg.msg);
			style.setTitle (newPushMsg.title);
            style.addHashParams(args);
      	mobPush.setMobPushLocalNotification (style);
             lastPushId = int.Parse(newPushMsg.id);
#endif
            }
        }
    }

#if   UNITY_IOS||UNITY_IPHONE
    	void OnNitifyHandler (int action, Hashtable resulte)
	{
		Debug.Log ("OnNitifyHandler");


        if (action==ResponseState.MessageRecvice)
        {
            switch (pushState)
            {
                case 0:
                    //自身的推送
                    Debug.Log("推送推送推送推送推送推送推送");
                    pushState = 1;
                    break;
                case 1:
                    Debug.Log("点击推送点击推送点击推送点击推送点击推送");
                    pushState = 0;
  
    JsonData jsonData=JsonMapper.ToObject(iOSMobPushImpl.reqJson);
    string  result=jsonData["pushId"]. ToString();
    if (string.IsNullOrEmpty(result))
	{
    Root root=GameObject.FindObjectOfType<Root>();
    root.Notify(result);
	}
                    break;
                default:
                    break;
            }
        }
   

    }
#endif
}
