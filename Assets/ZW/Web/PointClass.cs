/*******
* Copyright (C)2018    Administrator 
* 创建人:              Administrator  
* 创建时间:            2018/6/15 星期五 15:38:15    
****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[Serializable]
public class PointClass
{

    public List<Data> data;

    public PointClass()
    {
        data = new List<Data>();
    }
}



[Serializable]
public  class Data
{
   
    public string id;
    public string jingdu;
    public string weidu;
    public string count;
    public List<Paths>  paths;

    public Data()
    {
        
    }
    public Data(string  id,string  jingdu,string weidu,string count, List<Paths> paths)
    {

        this.id = id;
        this.jingdu = jingdu;
        this.weidu = weidu;
        this.count = count;
        this.paths = paths;
    }
}

[Serializable]
public class Paths
{
    public string time;
    public List<Timepaths> timepaths;

    public Paths()
    {
        
    }
    public Paths(string  time, List<Timepaths> timepaths)
    {
        this.time = time;
        this.timepaths = timepaths;
    }
}


[Serializable]
public class Timepaths
{
    public string imageindex;
    public string imagepath;

    public Timepaths()
    {
        
    }
    public Timepaths(string imageindex,string imagepath)
    {
        this.imageindex = imageindex;
        this.imagepath = imagepath;

    }
}