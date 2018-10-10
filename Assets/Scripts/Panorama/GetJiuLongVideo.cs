using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class GetJiuLongVideo : MonoBehaviour
{

    private VideoPlayer VP;


    void Awake()
    {
        Debug.Log("¾ÅÁúºº°Ø");
        VP = GetComponent<VideoPlayer>();
        foreach (var info in PublicAttribute.AreaResoucesDic["1"].ResourcesInfos)
        {
            if (info.ResourcesKey== "more_change")
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(info.LocalPath);
                foreach (var file in directoryInfo.GetFiles("*"))
                {
                    Debug.Log(file.Name);
                }
            }
        }
        foreach (FileInfo file in from info in PublicAttribute.AreaResoucesDic["1"].ResourcesInfos where info.ResourcesKey== "more_change" select new DirectoryInfo(info.LocalPath) into directoryInfo from file in directoryInfo.GetFiles("*") where file.Name=="JiuLong.mp4" select file)
        {
            Debug.Log(file.FullName);
            VP.url = file.FullName;
            VP.Play();
        }
    }
}
