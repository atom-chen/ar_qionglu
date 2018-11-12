using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElviraFrame.ScrollView {
    public class ScrollViewPanel : SingletonMono<ScrollViewPanel>
    {



        public GameObject scrollItem;
        public GameObject content;
        public void AddItem(List<string>   sps)
        {
            if (scrollItem!=null&&content!=null&& sps.Count!=0)
            {
                foreach (var item in sps)
                {
                    GameObject go = Instantiate<GameObject>(scrollItem);
                    go.name = scrollItem.name;
                    go.transform.parent = content.transform;
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = Vector3.zero;
                    go.GetComponent<ScrollItem>().Init(item);
                }

            }
        }




        public void Init()
        {
            GetComponentInChildren<ScrollView.ScrollPage>().Init();
        }
    }
}
