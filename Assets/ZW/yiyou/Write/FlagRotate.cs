﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagRotate : MonoBehaviour {


	void Update ()
    {

        transform.Rotate(Vector3.forward*60f, Space.Self);
	}
}
