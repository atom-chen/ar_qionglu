using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using System;
using UnityEngine.SceneManagement;

public class HaiouItem : MonoBehaviour
{

    float time = 5f;
    float timer = 0;

    AudioSource m_audioSource;

    Animation anim;
	void Start ()
    {

      YiyouStaticDataManager.Instance.OnDestroyGo += OnDestroyGo;

        YiyouStaticDataManager.Instance.OnSilenceGo += OnSilenceGo;
        m_audioSource = GetComponent<AudioSource>();
           time = Random.Range(5, 40);
        anim = transform.GetComponentInChildren<Animation>();
        anim.CrossFade("flap");
        Move();


    
	}

    private void OnSilenceGo(float  value )
    {
        m_audioSource.volume = value;
    }

    private void OnDestroyGo(GameObject obj)
    {
        Destroy(this.gameObject);
    }

    private void Move()
    {
        Vector3 targetPos = new Vector3(Random.Range(-8f,8f), Random.Range(1f, 5f), Random.Range(-8f, 8f));
        transform.DOMove(targetPos, Random.Range(1.5f, 3f)).OnComplete(Move).SetEase(Ease.Linear).SetSpeedBased(true) ;
        transform.DOLookAt(targetPos, 0.1f);
       
    }

    // Update is called once per frame
    void Update ()
    {
        if (this.gameObject.activeSelf)
        {

            timer += Time.deltaTime;
            if (timer>=time)
            {
                m_audioSource.Play();
                switch ((int)timer%2)
                {
                    case 0:
                        anim.CrossFade("flap");
                        break;
                    case 1:
                        anim.CrossFade("soar");
                        break;
                    default:
                        break;
                }
                timer = 0;
                time = Random.Range(5, 40);
            }
        }
	}
}
