using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class txtLimit : MonoBehaviour
{

	private Text txt;
	// Use this for initialization
	void Start ()
	{
		txt = GetComponent<Text>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (txt!=null)
		{
			if (txt.text.Length > 50)
			{
				string connect = txt.text;
				txt.text = connect.Substring(0, 50) + "......";
			}
		}
	}
}
