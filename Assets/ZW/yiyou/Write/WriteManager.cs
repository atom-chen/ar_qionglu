using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteManager : SingletonMono<WriteManager>
{
     Text inputText;
    Text txt1, txt2,txt3;
  public  Font tempfont = null;
    GameObject inputpanel;



     GameObject singlePanel;
     GameObject twoPanel;
     GameObject threePanel;
    [HideInInspector]
public     GoodsWriteEnum goodsEnum = GoodsWriteEnum.None;

     string writeText=string.Empty;
    public GameObject fontUIParent;
    public GameObject fontUIItem;


    private List<GameObject> fontGoList = new List<GameObject>();
    public UnityEngine.GameObject Inputpanel
    {
        get
        {

            if (inputpanel == null)
            {
                inputpanel = GameObject.Find("Canvas/InputPanel/FontPanel/Image/Input").gameObject;
            }
            return inputpanel;
        }
        set { inputpanel = value; }
    }

    public UnityEngine.GameObject SinglePanel
    {
        get {
            if (singlePanel==null)
            {
                singlePanel = transform.Find("WriteFontCanvas/SinglePanel").gameObject;
            }

            return singlePanel; }
        set { singlePanel = value; }
    }
 
    public UnityEngine.GameObject TwoPanel
    {
        get {
            if (twoPanel == null)
            {
                twoPanel = transform.Find("WriteFontCanvas/TwoPanel").gameObject;
            }
            return twoPanel; }
        set { twoPanel = value; }
    }

    public UnityEngine.GameObject ThreePanel
    {
        get {
            if (threePanel == null)
            {
                threePanel = transform.Find("WriteFontCanvas/ThreePanel").gameObject;
            }
            return threePanel; }
        set { threePanel = value; }
    }

    private void Start()
    {
            inputText = Inputpanel.transform.Find("Text").GetComponent<Text>();
        Inputpanel.transform.parent.parent.gameObject.SetActive(false);
        SinglePanel.gameObject.SetActive(false);
        TwoPanel.gameObject.SetActive(false);
        ThreePanel.gameObject.SetActive(false);
    }


    public    void SetGoodsEnum(GoodsWriteEnum  goodsEnum)
    {
        this.goodsEnum = goodsEnum;
        switch (this.goodsEnum)
        {
            case GoodsWriteEnum.None:
                break;
            case GoodsWriteEnum.Single:
                SinglePanel.gameObject.SetActive(true);
                break;
            case GoodsWriteEnum.Two:
                TwoPanel.gameObject.SetActive(true);
                break;
            case GoodsWriteEnum.Three:
                ThreePanel.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
 public   void SetText(string  sss)
    {
     
        writeText = sss;
        string str = "";
        string str2 = "";

        if (writeText==null)
        {
            return;
        }
        YiyouStaticDataManager.Instance.modelText = writeText;

        if (goodsEnum==GoodsWriteEnum.None)
        {
            return;
        }
        else if (goodsEnum == GoodsWriteEnum.Single)
        {
         
            txt1 = singlePanel.GetComponentInChildren<Text>();
               
                //一排的取前8个
                if (writeText.Length>=8)
                {
                    writeText = writeText.Substring(0, 8);

                }
                for (int i = 0; i < writeText.Length; i++)
                    {
                        str += writeText.Substring(i, 1).ToString() + "\n";
                        txt1.text = str;
                    }
                
              
            //Debug.Log("writeText========" + writeText);
            //Debug.Log("txt1========" + txt1.text);
        }

        else if (goodsEnum == GoodsWriteEnum.Two)
        {
            txt1 = TwoPanel.transform.Find("txt1").GetComponent<Text>();
            txt2 = TwoPanel.transform.Find("txt2").GetComponent<Text>();

          
            if (writeText.Length < 5)
            {
                for (int i = 0; i < writeText.Length; i++)
                {
                    str += " " + writeText.Substring(i, 1).ToString() + "\n";
                    txt1.text = str;
                }
                txt2.text = "";
            }
            else if (writeText.Length < 9)
            {
                for (int i = 0; i < 4; i++)
                {
                    str += " " + writeText.Substring(i, 1).ToString() + "\n";
                    txt1.text = str;
                }
                for (int i = 4; i < writeText.Length; i++)
                {
                    str2 += "   " + writeText.Substring(i, 1).ToString() + "\n";
                    txt2.text = str2;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    str += " " + writeText.Substring(i, 1).ToString() + "\n";
                    txt1.text = str;
                }
                for (int i = 4; i < 8; i++)
                {
                    str2 += "   " + writeText.Substring(i, 1).ToString() + "\n";
                    txt2.text = str2;
                }
            }
            //Debug.Log("writeText========" + writeText);
            //Debug.Log("txt1========" + txt1.text);
            //Debug.Log("txt2========" + txt2.text);

        }
        if (tempfont)
        {
            if (txt1 != null)
            {
                txt1.font = tempfont;

            }
            if (txt2 != null)
            {
                txt2.font = tempfont;
            }
        }

    }

    internal void SetFont(string name=null,Font  font=null)
    {
        //Font  tempfont=     fonts.Find(font => font.name == name);
        if (font!=null)
        {
            tempfont = font;
        }
        else if (name != null)
        {
            tempfont = Resources.Load<Font>("Fonts/" + name);
        }
        else
        {
            Debug.LogError("字体选择为空或者字体资源没找到====" + "\n名字="+name + "\n资源名字==" + font.name);
            return;
        }
        inputText.font = tempfont;
        YiyouStaticDataManager.Instance.currentFont = tempfont;
    }

    private void Update()
    {
 
        if (Inputpanel!=null&&Inputpanel.transform.parent.gameObject.activeSelf == true)
        {
            SetText(inputText.text);
        }

    }

    private void Init()
    {
        inputText.text = null;
        Inputpanel.transform.parent.gameObject.SetActive(true);
        writeText = null;
        Inputpanel.GetComponent<InputField>().text = null;
        Text[] childTransforms = transform.GetComponentsInChildren<Text>();
        if (childTransforms.Length != 0)
        {
            foreach (var item in childTransforms)
            {
                item.text = null;
            }
        }
    }

    public void ShowInputPanel(bool   flag)
    {

        Debug.Log(writeText);
        inputText.text = null;
        Inputpanel.transform.parent.parent.gameObject.SetActive(flag);
        if (flag)
        {
            Init();
        }

        if (flag&& fontUIParent!=null)
        {
            if (fontGoList.Count==0)
            {
                List<Sprite> fontSpriteList = YiyouStaticDataManager.Instance.GetSprites("ziti");
                List<Sprite> fontSpriteClickList = YiyouStaticDataManager.Instance.GetSprites("ziticlick");
                if (fontSpriteList != null)
                {
                    for (int i = 0; i < fontSpriteList.Count; i++)
                    {
                        GameObject obj = GameObject.Instantiate<GameObject>(fontUIItem);
                        obj.transform.parent = fontUIParent.transform;
                        obj.GetComponent<FontItemUI>().Init(fontSpriteList[i], fontSpriteClickList[i]);
                        fontGoList.Add(obj);
                    }
   
                }
            }

        }



    }


}
