using UnityEngine;
using System.Collections.Generic;
namespace ElviraFrame.ScrollView
{
    public class PageItem : MonoBehaviour
    {
        public ScrollItem prefabItem;
        List<ScrollItem> listItem = new List<ScrollItem>();

        public void Init(List<int> datas)
        {
            int count = datas.Count;
            for (int i = 0; i < count; i++)
            {
                listItem.Add(CreateItem(datas[i]));
            }
        }

        ScrollItem CreateItem(int data)
        {
            ScrollItem t = GameObject.Instantiate<ScrollItem>(prefabItem);
            t.gameObject.SetActive(true);
            t.transform.SetParent(prefabItem.transform.parent);
            t.transform.localScale = Vector3.one;
            t.transform.localPosition = Vector3.zero;
      //      t.Init(data);
            return t;
        }
    }

}
