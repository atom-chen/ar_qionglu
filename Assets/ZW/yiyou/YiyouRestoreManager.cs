using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.moblink.unity3d;
using UnityEngine.UI;
using System;

public class YiyouRestoreManager : SingletonMono<YiyouRestoreManager>
{


    string modelName = string.Empty;
    Transform modelTransform;

    string modelText = string.Empty;
    Light mainLight;
    Font modelFont;
    Hashtable customRestore = new Hashtable();
    public override void Awake()
    {
        base.Awake();
        customRestore = Root.tempScene.customParams;

        modelName = customRestore["modelName"].ToString();
        modelText = customRestore["modelText"].ToString();
        modelTransform = customRestore["modelTransform"] as Transform;
        mainLight = customRestore["Light"] as Light;
        modelFont = customRestore["modelTextFont"] as Font;
    }

    internal Light GetLight()
    {
        return mainLight;

    }
    internal GameObject GetModel()
    {
        return Resources.Load<GameObject>("Model/" + modelName);

    }
    internal string GetModelText()
    {
        return modelText;

    }
    internal string GetModelName()
    {
        return modelName;

    }
    internal Transform GetModelTranform()
    {
        return modelTransform;

    }

    internal Font GetModelFont()
    {
        return modelFont;

    }
}
