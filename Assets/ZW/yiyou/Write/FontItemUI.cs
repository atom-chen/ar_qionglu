using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FontItemUI : SingletonMono<SelectItemUI>
{
    Image bgImage;
    Image tipImage;
    Button bgButton;
    public UnityEngine.UI.Button BgButton
    {
        get {
            if (bgButton == null)
            {

                bgButton = bgImage.GetComponent<Button>();
                bgButton.onClick.AddListener(BgImageClick);
            }
            return bgButton;

        }
        set { bgButton = value; }
    }
    public UnityEngine.UI.Image BgImage
    {
        get
        {
            if (bgImage == null)
            {
                bgImage = gameObject.transform.Find("BgImage").GetComponent<Image>();
                BgButton = bgImage.GetComponent<Button>();
             
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


    public void Init(Sprite sps,Sprite  clicksps)
    {
        BgButton.transition = Selectable.Transition.SpriteSwap;
        BgImage.sprite = sps;
        SpriteState spriteState = new SpriteState();
        spriteState.highlightedSprite = sps;
        spriteState.pressedSprite = clicksps;
        spriteState.disabledSprite = sps;
        BgButton.spriteState = spriteState;

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
