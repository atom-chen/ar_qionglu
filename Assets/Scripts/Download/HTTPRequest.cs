using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Http����
/// </summary>
public class HTTPRequest
{
  private string url;
  private int timeout;
  private Action<HTTPResponse> callback;
  private HttpWebRequest request;
  private string method;
  private string contentType;
  private KeyValuePair<string, int> proxy;
  protected int range = -1;
  // post����
  private StringBuilder postBuilder;
  /// <summary>
  /// �������
  /// </summary>
  public const int ERR_EXCEPTION = -1;
  /// <summary>
  /// ���캯��, ����GET����
  /// </summary>
  /// <param name="url">url��ַ</param>
  /// <param name="timeout">��ʱʱ��</param>
  /// <param name="callback">�ص�����</param>
  public HTTPRequest (string url, string method, int timeout, Action<HTTPResponse> callback)
  {
      this.url = url;
      this.timeout = timeout;
      this.callback = callback;
      this.method = method.ToUpper();
  }
  /// <summary>
  /// ����Post����
  /// </summary>
  /// <param name="data">����</param>
  public void SetPostData(string data) {
      if (postBuilder == null) {
          postBuilder = new StringBuilder (data.Length);
      }
      if (postBuilder.Length > 0) {
          postBuilder.Append ("&");
      }
      postBuilder.Append (data);
  }
  /// <summary>
  /// ���Post����
  /// </summary>
  /// <param name="key">keyֵ</param>
  /// <param name="value">valueֵ</param>
  public void AddPostData(string key, string value) {
      if (postBuilder == null) {
          postBuilder = new StringBuilder ();
      }
      if (postBuilder.Length > 0) {
          postBuilder.Append ("&");
      }
      postBuilder.Append (key).Append ("=").Append (UrlEncode (value));
  }
  /// <summary>
  /// ���ô���
  /// </summary>
  /// <param name="ip">ip��ַ</param>
  /// <param name="port">�˿ں�</param>
  public void SetProxy(string ip, int port) {
      this.proxy = new KeyValuePair<string, int> (ip, port);
  }
  /// <summary>
  /// ����ContentType
  /// </summary>
  /// <value>ContentType value</value>
  public string ContentType {
      set {
          this.contentType = value;
      }
  }
  /// <summary>
  /// ��������
  /// </summary>
  public void Start() {
      Debug.Log ("Handle Http Request Start");
      this.request = WebRequest.Create (url) as HttpWebRequest;
      this.request.Timeout = timeout;
      this.request.Method = method;
      if (this.proxy.Key != null) {
          this.request.Proxy = new WebProxy(this.proxy.Key, this.proxy.Value);
      }
      if (this.contentType != null) {
          this.request.ContentType = this.contentType;
      }
      if (this.range != -1) {
          this.request.AddRange (this.range);
      }
      // POSTдPOST����
      if (method.Equals ("POST")) {
          WritePostData ();
      }
      try {
          AsyncCallback callback = new AsyncCallback (OnResponse);
          this.request.BeginGetResponse (callback, null);
      } catch (Exception e) {
          CallBack (ERR_EXCEPTION, e.Message);
          if (request != null) {
              request.Abort ();
          }
      }
  }
  /// <summary>
  /// �����ȡResponse
  /// </summary>
  /// <param name="result">�첽�ص�result</param>
  protected void OnResponse(IAsyncResult result) {
      //Debug.Log ("Handle Http Response");
      HttpWebResponse response = null;
      try {
          // ��ȡResponse
          response = request.EndGetResponse (result) as HttpWebResponse;
          if (response.StatusCode == HttpStatusCode.OK) {
              if ("HEAD".Equals(method)) {
                  // HEAD����
                  long contentLength = response.ContentLength;
                  CallBack((int)response.StatusCode, contentLength + "");
                  return;
              }
              // ��ȡ��������
              Stream responseStream = response.GetResponseStream();
              byte[] buff = new byte[2048];
              MemoryStream ms = new MemoryStream();
              int len = -1;
              while ((len = responseStream.Read(buff, 0, buff.Length)) > 0) {
                  ms.Write(buff, 0, len);
              }
              // �������
              responseStream.Close();
              response.Close();
              request.Abort();
              // ���ûص�
              CallBack ((int)response.StatusCode, ms.ToArray());
          } else {
              CallBack ((int)response.StatusCode, "");
          }
      } catch (Exception e) {
          CallBack (ERR_EXCEPTION, e.Message);
          if (request != null) {
              request.Abort ();
          }
          if (response != null) {
              response.Close ();
          }
      }
  }
  /// <summary>
  /// �ص�
  /// </summary>
  /// <param name="code">����</param>
  /// <param name="content">����</param>
  private void CallBack(int code, string content) {
      Debug.LogFormat ("Handle Http Callback, code:{0}", code);
      if (callback != null) {
          HTTPResponse response = new HTTPResponse ();
          response.StatusCode = code;
          response.Error = content;
          callback (response);
      }
  }
  /// <summary>
  /// �ص�
  /// </summary>
  /// <param name="code">����</param>
  /// <param name="content">����</param>
  private void CallBack(int code, byte[] content) {
      Debug.LogFormat ("Handle Http Callback, code:{0}", code);
      if (callback != null) {
          HTTPResponse response = new HTTPResponse (content);
          response.StatusCode = code;
          callback (response);
      }
  }
  /// <summary>
  /// дPost����
  /// </summary>
  private void WritePostData() {
      if (null == postBuilder || postBuilder.Length <= 0) {
          return;
      }
      byte[] bytes = Encoding.UTF8.GetBytes (postBuilder.ToString ());
      Stream stream = request.GetRequestStream ();
      stream.Write (bytes, 0, bytes.Length);
      stream.Close ();
  }
  /// <summary>
  /// URLEncode
  /// </summary>
  /// <returns>encode value</returns>
  /// <param name="value">Ҫencode��ֵ</param>
  private string UrlEncode(string value) {
      StringBuilder sb = new StringBuilder();
      byte[] byStr = System.Text.Encoding.UTF8.GetBytes(value);
      for (int i = 0; i < byStr.Length; i++)
      {
          sb.Append(@"%" + Convert.ToString(byStr[i], 16));
      }
      return (sb.ToString());
  }
}