using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollPageMark : MonoBehaviour
{
    public static ScrollPageMark instance;
    public ToggleGroup toggleGroup;
    public Toggle togglePrefab;

    public List<Toggle> toggleList = new List<Toggle>();
    private Scene sc;
    void Awake()
    {
        instance = this;
       
    }

    private void Start()
    {
        sc = SceneManager.GetActiveScene();
    }

    public void OnScrollPageChanged(int pageCount, int currentPageIndex)
    {
        if(pageCount!=toggleList.Count)
        {
            if(pageCount>toggleList.Count)
            {
                int cc = pageCount - toggleList.Count;
                for(int i=0; i< cc; i++)
                {
                    toggleList.Add(CreateToggle());
                }
            }
            else if(pageCount < toggleList.Count)
            {
                while(toggleList.Count > pageCount)
                {
                    Toggle t = toggleList[toggleList.Count - 1];
                    toggleList.Remove(t);
                    DestroyImmediate(t.gameObject);
                }
            }
        }

        if(currentPageIndex>=0)
        {
            toggleList[currentPageIndex].isOn = true;
        }
        if(currentPageIndex==0)
        {
            toggleList[toggleList.Count-1].isOn = false;
        }

        if (sc.name == "ARScan")
        {
            if (currentPageIndex==pageCount-1)
            {
                for(int i=0; i< toggleList.Count; i++)
                {              
                    toggleList[i].isOn = false;
                    toggleList[i].gameObject.SetActive(false);
                }
            }
            else
            {
                for(int i=0; i< toggleList.Count; i++)
                {
                    toggleList[i].gameObject.SetActive(true);
                }
            }
        }
       
    }

    Toggle CreateToggle()
    {
        Toggle t = GameObject.Instantiate<Toggle>(togglePrefab);
        t.gameObject.SetActive(true);
        t.transform.SetParent(toggleGroup.transform);
        t.transform.localScale = Vector3.one;
        t.transform.localPosition = Vector3.zero;
        return t;
    }
}
