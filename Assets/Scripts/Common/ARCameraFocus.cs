using UnityEngine;
using System.Collections;

public class ARCameraFocus : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Vuforia.CameraDevice.Instance.SetFocusMode(Vuforia.CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Vuforia.CameraDevice.Instance.SetFocusMode(Vuforia.CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
