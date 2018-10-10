using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{

    private Camera camera;
    private Vector3 relativePos;
    private Quaternion rotation;
    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {

        //transform.LookAt(camera.transform, camera.transform.forward);

        relativePos = camera.transform.position - transform.position;
        rotation = Quaternion.LookRotation(Vector3.forward,relativePos);

        transform.rotation = rotation;
    }
}
