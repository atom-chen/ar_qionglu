/***
 *
 *   Title: "AssetBundle简单框架"项目
 *
 *   Description:
 *          功能： 对标记的资源进行打包输出
 *
 *   Author: ElviraFrame
 *
 *   Date: 2017.10
 *
 *   Modify：  
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;   //引入Unity编辑器，命名空间
using System.IO;     //引入的C#IO,命名空间

namespace ElviraFrame.AB
{
    public class BuildAssetBundle {

        /// <summary>
        /// 打包生成所有的AssetBundles(包)
        /// </summary>
        [MenuItem("AssetBundelTools/BuildAll According  BuildSettings ")]
        public static void BuildAllABAccordingBuildSettings()
        {
            //打包AB输出路径
            string strABOutPathDIR = string.Empty;

            //获取"StreamingAssets"数值
            strABOutPathDIR = PathTools.GetABOutPath();

            Debug.Log(strABOutPathDIR);
        //判断生成输出目录文件夹
            if (!Directory.Exists(strABOutPathDIR))
            {
                Directory.CreateDirectory(strABOutPathDIR);
            }
            Debug.Log(EditorUserBuildSettings.activeBuildTarget);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    BuildPipeline.BuildAssetBundles(strABOutPathDIR, 0, BuildTarget.Android);
                    break;
                case BuildTarget.iOS:
                    BuildPipeline.BuildAssetBundles(strABOutPathDIR, 0, BuildTarget.iOS);
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    BuildPipeline.BuildAssetBundles(strABOutPathDIR, 0, BuildTarget.StandaloneWindows64);
                    break;
                default:
                    //BuildPipeline.BuildAssetBundles(outPath, 0, BuildTarget.StandaloneWindows64);
                    break;
            }
        }

    }//Class_end
}
