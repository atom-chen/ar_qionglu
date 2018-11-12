using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace ElviraFrame.ScrollView
{
    public class ScrollItem : MonoBehaviour
    {


        private Texture2D txt2D;
       
        public string path;

        public void Init(string path)
        {
            this.path = path;
            if (path != null)
            {
                StartCoroutine(LoadImage(path));
            }
        }
        private IEnumerator LoadImage(string iconName)
        {
#if UNITY_ANDROID
            //WWW www = new WWW("file://" + UnityHelper.LocalFilePath + "Web/" + iconName);
            WWW www = new WWW("file:///" + "sdcard/DCIM/Camera/" + iconName);
#elif UNITY_IOS || UNITY_IPHONE
 WWW www = new WWW(Application.persistentDataPath + "/"+iconName);
#endif
            yield return www;
            if (www.error != null)
            {
                Debug.LogError("www加载图片出错，请检查path");
            }
            else
            {
                txt2D = www.texture;

                Sprite sp = Sprite.Create(txt2D, new Rect(0, 0, txt2D.width, txt2D.height), new Vector2(0.5f, 0.5f));

                this.GetComponent<Image>().sprite = sp;
              
                
            }
        }
    }
}
