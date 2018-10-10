using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPoint : MonoBehaviour {


    public ViewTrigger[] viewTriggers;

    void Update ()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * 6000, out hit);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward)*6000,Color.red );
        foreach (var trigger in viewTriggers)
        {
            trigger.Focused = hit.collider && (hit.collider.gameObject == trigger.gameObject);
        }
    }
}
