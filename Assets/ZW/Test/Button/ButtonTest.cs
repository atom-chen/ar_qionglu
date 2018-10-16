using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour {


    public Transform cube1, cube2, cube3;
    public void buttonClcik()
    {
        Debug.Log("sdfgsdfgdf");
    }


    private void Update()
    {
        UnityHelper.FaceToGoal(cube1, cube2, 0.1f);
        UnityHelper.FaceToGoal(cube3, cube2, 0.1f);
    }
}
