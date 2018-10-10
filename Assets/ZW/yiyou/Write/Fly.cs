using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class Fly : MonoBehaviour
{
    int speed;
	// Use this for initialization
	void Start ()
    {
        YiyouStaticDataManager.Instance.OnDestroyGo += OnDestroyGo;
        speed = Random.Range(1, 5);
      
    }

    private void OnDestroyGo(GameObject obj)
    {
        if (this.gameObject)
        {
            Destroy(this.gameObject);

        }
    }

    // Update is called once per frame
    void Update ()
	{
        transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed/20);
	}
}