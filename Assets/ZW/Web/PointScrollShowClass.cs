
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Text;
using UnityEngine.UI;
[System.Serializable]
public struct PointScrollShowClass
{
    public string id;
    public string jingdu;
    public string weidu;
    public string count;
    public Dictionary<string,Dictionary<string, List<string>>> paths;

}



