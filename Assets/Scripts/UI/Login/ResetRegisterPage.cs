using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResetRegisterPage : MonoBehaviour {

    public InputField[] IFs;

	void OnDisable()
    {
        foreach (var item in IFs)
        {
            if (item)
            {
                item.text = "";
            }
        }
    }
}
