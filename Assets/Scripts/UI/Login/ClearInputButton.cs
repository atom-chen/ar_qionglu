using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





/// <summary>
/// 清除自身的所在的输入
/// </summary>
public class ClearInputButton : MonoBehaviour {


    private void Awake()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(()=> {


            ClearInput();

        });
    }


    public void ClearInput()
    {



        InputField inputText= transform.GetComponentInParent<InputField>();

        inputText.text = "";
    

    }
}
