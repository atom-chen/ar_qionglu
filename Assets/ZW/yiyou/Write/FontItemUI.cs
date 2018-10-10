using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FontItemUI : SingletonMono<SelectItemUI>
{
    Image bgImage;
    Image tipImage;
    public UnityEngine.UI.Image BgImage
    {
        get
        {
            if (bgImage == null)
            {
                bgImage = gameObject.transform.Find("BgImage").GetComponent<Image>();
                bgImage.GetComponent<Button>().onClick.AddListener(BgImageClick);
            }
            return bgImage;

        }
        set { bgImage = value; }
    }



    public UnityEngine.UI.Image TipImage
    {
        get
        {
            if (tipImage == null)
            {
                tipImage = gameObject.transform.Find("TipImage").GetComponent<Image>();
                tipImage.GetComponent<Button>().onClick.AddListener(TipImageClick);
            }
            return tipImage;

        }
        set { tipImage = value; }
    }


    public void Init(Sprite sps)
    {
        BgImage.sprite = sps;
        TipImage.gameObject.SetActive(false);
    }
    private void BgImageClick()
    {
        Debug.Log(BgImage.sprite.name);
        
            WriteManager.Instance.SetFont(BgImage.sprite.name);
    }

    private void TipImageClick()
    {

    }

}
