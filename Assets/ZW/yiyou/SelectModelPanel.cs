using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectModelPanel : SingletonMono<SelectModelPanel>
{
    [Header("按钮Toggles")]
    public Toggle[] toggles;
    string toggleSceneName;

  public  Button hideButton;
 
    public GameObject selectItemUIPrefab;

    public GameObject selectItemParent;

    /// <summary>
    /// 具体的模型选择图标
    /// </summary>
    List<GameObject> selectUIObjList=new List<GameObject>();

    bool isCanClick = false;
    private void ShowModelPanel()
    {
        foreach (Toggle item in toggles)
        {
            item.onValueChanged.AddListener((flag) =>
            {

                toggleSceneName = item.gameObject.name;
                ShowSelectSceneModelUI(flag, toggleSceneName);

            });
        }
        toggles[0].isOn = true;

        ShowSelectSceneModelUI(true, toggles[0].gameObject.name);
    }

    void Start ()
    {
        hideButton.onClick.AddListener(HideClick);


        ShowModelPanel();
    }

    private void HideClick()
    {
        this.gameObject.SetActive(false);
    }

    private void ShowSelectSceneModelUI(bool flag, string toggleSceneName)
    {
        if (flag==true)
        {
            List<Sprite> spritesList = new List<Sprite>();
            spritesList.Clear();
            spritesList =YiyouStaticDataManager.Instance.GetSprites(toggleSceneName);
         
            if (spritesList!=null)
            {
                foreach (GameObject gos in selectUIObjList)
                {
                    Destroy(gos);
                }
     
                selectUIObjList.Clear();
               
                foreach (var item in spritesList)
                {
                    selectItemUIPrefab = Resources.Load<GameObject>("Model/SelectItem");
                       GameObject obj = GameObject.Instantiate<GameObject>(selectItemUIPrefab);
                    obj.transform.parent = selectItemParent.transform;
                    obj.GetComponent<SelectItemUI>().Init(item, isCanClick);
                    selectUIObjList.Add(obj);
                }
#if UNITY_ANDROID
                if (SceneManager.GetActiveScene().name == "wikiSLAM")
                {
                    WikiSLAMController.Instance.SetBtnList(selectUIObjList);
                }
#endif

            }
        }
    }

    public void SetItemInteractable(bool CanClick)
    {
   this.     isCanClick = true;
        foreach (var item in selectUIObjList)
        {
            item.GetComponent<SelectItemUI>().SetBtnInteractable(CanClick);


        }
    }
}
