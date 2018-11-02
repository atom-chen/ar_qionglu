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
        private IEnumerator LoadImage(string iconPath)
        {
            //WWW www = new WWW(UnityHelper.LocalFilePath+"Web/"+iconPath);
            WWW www = new WWW("file://" + UnityHelper.LocalFilePath + "Web/" + iconPath);
            Debug.Log("UnityHelper.LocalFilePath+iconPath===" + UnityHelper.LocalFilePath + "Web/" + iconPath);
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
