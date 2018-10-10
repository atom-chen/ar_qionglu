/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     ConverterTypeToBoolean.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-25 
 *Description:    
 *History: 
*/

using System;
using UnityEngine;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// 类型转换
/// </summary>
public class ConvertType
{

    /// <summary>
    /// 转换为Bool类型
    /// </summary>
    /// <param name="text"> 输入true/false </param>
    /// <returns></returns>
    public static bool? ToBool(string text)
    {
        bool result;
        if (bool.TryParse(text, out result))
        {
            return result;
        }
        return null;
    }

    /// <summary>
    /// 转换为Bool类型
    /// </summary>
    /// <param name="number">输入1返回True 其余返回False 输入Null返回Null</param>
    /// <returns></returns>
    public static bool? ToBool(int number)
    {
        bool result;
        result = number == 1;
        return result;
    }



    /// <summary>
    /// 字符串转为颜色  "0.1,0.2,0.1,0.5"
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static Color? ToColor(string text)
    {

        text = text.Replace("RGBA(", "");
        text = text.Replace(")", "");

        float r = float.Parse(text.Split(',')[0]);
        float g = float.Parse(text.Split(',')[1]);
        float b = float.Parse(text.Split(',')[2]);
        float a = float.Parse(text.Split(',')[3]);
        Color color = new Color(r, g, b, a);
        return color;
    }


    public static string UnicodeToString(string source)
    {
        //关键也就是这句了
        byte[] bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, Encoding.Unicode.GetBytes(source));
        string utf8 = Encoding.UTF8.GetString(bytes);
        return utf8;
    }
}