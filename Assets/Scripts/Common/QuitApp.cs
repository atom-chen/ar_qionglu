using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApp : MonoBehaviour
{

	public GameObject obj;
	// Use this for initialization
	void Start () {
		
	}

	private int hit = 0;
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			hit++;
			obj.SetActive(true);
			CoroutineWrapper.EXES(1f, () =>
			{
				hit = 0;
				obj.SetActive(false);
			});
		}

		if (hit >= 2)
		{
			Application.Quit();
		}
	}
}
