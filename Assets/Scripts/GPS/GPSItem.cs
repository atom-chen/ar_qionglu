using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSItem : MonoBehaviour
{
    /// <summary>
    /// ʵ����ID
    /// </summary>
    public string id;

    /// <summary>
    /// ʵ��������
    /// </summary>
    public string name;

    /// <summary>
    /// ʵ���㾭��
    /// </summary>
    public string locationX;

    /// <summary>
    /// ʵ����γ��
    /// </summary>
    public string locationY;

    /// <summary>
    /// ʵ���㺣��
    /// </summary>
    public string height;
    /// <summary>
    /// ʵ��������
    /// </summary>
    public string content;

    /// <summary>
    /// ��ҳ�����ַ
    /// </summary>
    public string address;

    /// <summary>
    /// ������� 
    /// </summary>
    public string typeName;

    public IconFollow icon;

    public Thumbnail thumbnail;

    private void Start()
    {
       
    }
    public void ShowObj(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
