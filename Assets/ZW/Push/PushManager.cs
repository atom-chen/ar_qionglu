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





public class PushManager : SingletonMono<PushManager>
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
    private PushPointClass pushPointClass=null;
    private int radiusDistance = 0;

    public void LoadPushJson()
    {


        string reader = JsonManager.ReadJsonFromFilePath(UnityHelper.LocalFilePath + "Push/"+PublicAttribute.AreaId+"/", "content.json");
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

    void Start()
    {
        MobPushInit();
       InvokeRepeating("GetCurerntLocation", 60f, 60f);

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

 
    public void AddToList(PushPointClass pointDataParams)
    {
        if (pointDataParams.msgs.Count!=0)
        {



            radiusDistance =int.Parse( pointDataParams.radius);
   foreach (PushMsg item in pointDataParams.msgs)
        {
            PushItem pushItemMsg = new PushItem();
            pushItemMsg.id = item.id;


                float XX = float.Parse(item.locationX);
                float yy = float.Parse(item.locationY);
           
                pushItemMsg.locationX = item.locationX;
            pushItemMsg.locationY = item.locationY;
            pushItemMsg.height = item.height;
            pushItemMsg.title = item.title;
            pushItemMsg.msg = item.msg;
            pushItemMsg.distance =10000;
                pushItemMsg.pos = new Vector2(XX,yy);
          
                pushItemMsg.dbid = item.dbid;
            pushItemMsg.type = item.type;
            //      Debug.Log("pointdata====" + pushItemMsg.title+"--"+ pushItemMsg.msg + "--" + pushItemMsg.pos);
            pushPointIndexV2Dic.Add(int.Parse(pushItemMsg.id), pushItemMsg);
        }
        }

     
    }






    public void GetCurerntLocation()
    {
#if UNITY_EDITOR
        Vector2 currentGps = new Vector2(104.067304f, 30.597963f);
#else
      String location =UnityHelper. GetBDGPSLocation();
   //     Debug.Log("GPS::::::::::" + location);
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

         //Debug.Log(" kt.Value.distance ====" + (int)kt.Value.distance + "------"+kt.Value.title+"----pos===="+ kt.Value.pos);
                if ((int)kt.Value.distance <= radiusDistance)
                {
                    nearIndexList.Add(kt.Value);
                }
            }
        }
        if (nearIndexList.Count != 0)
        {
         //   Debug.Log("nearIndexList.Count==" + nearIndexList.Count);

            nearIndexList.Sort((x, y) => { return x.distance.CompareTo(y.distance); });
      
            return  nearIndexList ;
        }


        return null;
    }



    private void PushPointinformation()
    {
        if (nearIndexList.Count != 0)
        {

            PushItem newPushMsg = pushPointIndexV2Dic[int.Parse(nearIndexList[0].id)];
            if (GlobalParameter.lastPushId != int.Parse(newPushMsg.id))
            {
                NotifyCallBack notifyCallBack = new NotifyCallBack()
                {
                    dbid = newPushMsg.dbid,
                    type = newPushMsg.type
                };
           
#if UNITY_ANDROID
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("notifySimpleMessage", newPushMsg.title, newPushMsg.msg, JsonMapper.ToJson(notifyCallBack));
                GlobalParameter.lastPushId = int.Parse(newPushMsg.id);
#elif UNITY_IOS || UNITY_IPHONE

         pushState = 0;
			LocalNotifyStyle style = new LocalNotifyStyle ();
            Hashtable args = new Hashtable();
            args["dbid"] = newPushMsg.dbid;
              args["type"] = newPushMsg.type;
			style.setContent (newPushMsg.msg);
			style.setTitle (newPushMsg.title);
            style.addHashParams(args);
      	mobPush.setMobPushLocalNotification (style);
             GlobalParameter.lastPushId = int.Parse(newPushMsg.id);
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
    if (string.IsNullOrEmpty(iOSMobPushImpl.reqJson))
	{
                Root root = GameObject.FindObjectOfType<Root>();
            JsonData  jsonData=JsonMapper.ToJson(iOSMobPushImpl.reqJson);
            NotifyCallBack notifyCallBack = new NotifyCallBack()
            {
                dbid = jsonData["dbid"].ToString(),
                type = jsonData["type"].ToString()
            };
      
            root.Notify(JsonMapper.ToJson(notifyCallBack));
	}


                    break;
                default:
                    break;
            }
        }
   

    }
#endif
}
