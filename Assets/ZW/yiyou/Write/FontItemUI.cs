using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FontItemUI : SingletonMono<SelectItemUI>
{
 public   Image bgImage;
    public Image tipImage;
    Button bgButton;

 

    private void Start()
    {

        bgImage = gameObject.transform.Find("BgImage").GetComponent<Image>();

        bgButton = bgImage.GetComponent<Button>();
       bgButton.onClick.AddListener(BgImageClick);
        tipImage = gameObject.transform.Find("TipImage").GetComponent<Image>();
        tipImage.GetComponent<Button>().onClick.AddListener(TipImageClick);
    }

    public void Init(Sprite sps,Sprite  clicksps)
    {
        //BgButton.transition = Selectable.Transition.SpriteSwap;
        this.transform.name = sps.name;
        bgImage.sprite = sps;
        //SpriteState spriteState = new SpriteState();
        //spriteState.highlightedSprite = sps;
        //spriteState.pressedSprite = clicksps;
        //spriteState.disabledSprite = sps;
        //BgButton.spriteState = spriteState;
        tipImage.sprite = clicksps;
        tipImage.gameObject.SetActive(false);
    }
    private void BgImageClick()
    {
        Debug.Log(bgImage.sprite.name);
     
            WriteManager.Instance.SetFont(bgImage.sprite.name);
        WriteManager.Instance.SetBtnClickState(this.gameObject);
    }

    private void TipImageClick()
    {

    }
    public void ShowTip(bool  flag)
    {
        tipImage.gameObject.SetActive(flag);
    }
}
