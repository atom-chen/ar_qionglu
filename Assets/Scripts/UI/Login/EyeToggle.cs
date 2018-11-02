using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeToggle : MonoBehaviour
{
    private Toggle thisToggle;
    private InputField parentInputField;
    private void Awake()
    {
        thisToggle =GetComponent<Toggle>();
        parentInputField = GetComponentInParent<InputField>();

        thisToggle.onValueChanged.AddListener(((bool argo) =>
        {

            if (argo == true)
            {


                parentInputField.contentType = InputField.ContentType.Standard;
                parentInputField.enabled = false;
                parentInputField.enabled = true;
            }
            else
            {

                parentInputField.contentType = InputField.ContentType.Password;
                parentInputField.enabled = false;
                parentInputField.enabled = true;

            }

        }));
    }

}