using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID

using Wikitude;
#endif


public class Duijiao : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "yiyou")
        {
            CameraDevice.Instance.SetFocusMode(Vuforia.CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

        }
        else
        {
#if UNITY_ANDROID

            WikitudeCamera wikitudeCamera = GameObject.FindObjectOfType<WikitudeCamera>();

            wikitudeCamera.FocusMode = CaptureFocusMode.ContinuousAutoFocus;
#endif

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (SceneManager.GetActiveScene().name=="yiyou")
            {
                CameraDevice.Instance.SetFocusMode(Vuforia.CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

            }
            else
            {
#if UNITY_ANDROID

                WikitudeCamera wikitudeCamera = GameObject.FindObjectOfType<WikitudeCamera>();

                wikitudeCamera.FocusMode = CaptureFocusMode.ContinuousAutoFocus;
#endif

            }

      
    }

}