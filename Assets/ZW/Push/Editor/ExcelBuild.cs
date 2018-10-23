using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace ElviraFrame.Excel
{
    public class ExcelBuild : Editor
    {


        [MenuItem("Elvira /ExcelEditor/CreateExcelItemAsset")]
        public static void CreateItemAsset()
        {
            PushItemManager manager = ScriptableObject.CreateInstance<PushItemManager>();
            //赋值
            manager.dataArray = ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "pushpoint.xlsx");

            //确保文件夹存在
            if (!Directory.Exists(ExcelConfig.assetPath))
            {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }

            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "PushItem");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            PushItemManager man = Resources.Load<PushItemManager>("DataAssets/PushItem");
            foreach (PushItem i in man.dataArray)
            {
                Debug.Log(i.id + "---" + i.title + "---" + i.msg);
            }
         



        }
    }

}