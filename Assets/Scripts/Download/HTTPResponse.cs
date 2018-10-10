using System;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// HTTP��������
/// </summary>
public class HTTPResponse
{
    // ״̬��
    private int statusCode;
    // ��Ӧ�ֽ�
    private byte[] responseBytes;
    // ��������
    private string error;
    /// <summary>
    /// Ĭ�Ϲ��캯��
    /// </summary>
    public HTTPResponse()
    {
    }
    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="content">��Ӧ����</param>
    public HTTPResponse(byte[] content)
    {
        this.responseBytes = content;
    }
    /// <summary>
    /// ��ȡ��Ӧ����
    /// </summary>
    /// <returns>��Ӧ�ı�����</returns>
    public string GetResponseText()
    {
        if (null == this.responseBytes)
        {
            return null;
        }
        return Encoding.UTF8.GetString(this.responseBytes);
    }
    /// <summary>
    /// ����Ӧ���ݴ洢���ļ�
    /// </summary>
    /// <param name="fileName">�ļ�����</param>
    public void SaveResponseToFile(string fileName)
    {
        if (null == this.responseBytes)
        {
            return;
        }
        // FIXME ·����ƽ̨����
        string path = Path.Combine(Application.dataPath + "/StreamingAssets", fileName);
        FileStream fs = new FileStream(path, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(fs);
        writer.Write(this.responseBytes);
        writer.Flush();
        writer.Close();
        fs.Close();
    }
    /// <summary>
    /// ��ȡ״̬��
    /// </summary>
    /// <value>״̬��</value>
    public int StatusCode
    {
        set
        {
            this.statusCode = value;
        }
        get
        {
            return this.statusCode;
        }
    }
    /// <summary>
    /// ��ȡ������Ϣ
    /// </summary>
    /// <value>������Ϣ</value>
    public string Error
    {
        set
        {
            this.error = value;
        }
        get
        {
            return this.error;
        }
    }
}