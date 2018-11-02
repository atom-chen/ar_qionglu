using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
public class Loading : MonoBehaviour
{
    private float fps = 10.0f;
    private float time;

    private int nowFram;
    AsyncOperation async;




    private int progress = 0;

    public GameObject loadTipImage;
    // Use this for initialization
    void Start ()
    {

        if (string.IsNullOrEmpty(GlobalParameter.nextSceneName))
        {
            GlobalParameter.nextSceneName = "init";
        }
   StartCoroutine(LoadScenes());
    }
    IEnumerator LoadScenes()
    {
        int nDisPlayProgress = 0;
        async = SceneManager.LoadSceneAsync(GlobalParameter.nextSceneName);
        async.allowSceneActivation = false;
        while (async.progress < 0.9f)
        {
         
             
          
            progress = (int)async.progress * 100;
            while (nDisPlayProgress < progress)
            {
                ++nDisPlayProgress;
              
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
        progress = 100;
        while(nDisPlayProgress < progress)
        {
            ++nDisPlayProgress;
  
            yield return new WaitForEndOfFrame();
       }
        async.allowSceneActivation = true;
       // yield return async;
    }
    private void Update()
    {
        loadTipImage.transform.Rotate(Vector3.forward * 200f * Time.deltaTime, Space.World);
    }

}
