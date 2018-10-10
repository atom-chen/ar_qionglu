using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToMain : MonoBehaviour
{

    private Button Btn;


    void Awake()
    {
        Btn = GetComponent<Button>();

        Btn.onClick.AddListener((() =>
        {
            ABManager.Instance.BackMainScene();
        }));
    }
}
