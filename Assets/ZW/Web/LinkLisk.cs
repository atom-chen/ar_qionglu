
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
/// <summary>  
/// 结点类  
/// longitude：经度
/// latitude： 纬度
/// </summary>  
public class ListNode
{
    public string id;
    /// <summary>
    /// longitude：经度
    /// </summary>
    public string longitude;
    /// <summary>
    /// latitude： 纬度
    /// </summary>
    public string latitude;

    public string path;
    public string count;
    
    public ListNode() { }  // 构造函数  
    public ListNode next;
}

/// <summary>  
/// 链表类  
/// 
/// 
/// longitude：经度
/// latitude： 纬度
/// </summary>  
public class LinkList
{

    private ListNode first=null;   
    private static LinkList _Instance = null;

    public static LinkList GetInstance()
    {
        if (_Instance == null)
            _Instance = new LinkList();
        return _Instance;
    }
    public LinkList()
    {
        first = null;
    }

    public bool IsEmpty()
    {
        return first == null;
    }



    /// <summary>
    /// 插入，1、表中有就更新节点，2、表中没有就生成节点并添加到末尾  ,已经有了返回false，新建插入返回true
    /// </summary>
    public bool InsertAtLast(string longitude, string latitude, string imagepath,string  count,string  id)
    {
        ListNode resultNode = Find(longitude, latitude);
        if (resultNode == null)
        {
            //没找到就重新生成
            ListNode insertNode = new ListNode
            {
                longitude = longitude,
                latitude = latitude,
                path = imagepath,
                count = count,
                id = id
            };
            ListNode node = first;
            if (node==null)
            {
                first = insertNode;
            }
            else
            {
                while (node.next != null)
                {
                    node = node.next;
                }
                //在Node之后插入  
                node.next = insertNode;
            }

        }
        else
        {
            //找到
            resultNode.path=imagepath;
            resultNode.count = count;
            return false;
        }

        return true;
    }

    public int Length()
    {
        ListNode current = first;
        int length = 0;
        while (current != null)
        {
            length++;
            current = current.next;
        }
        return length;
    }

    /// <summary>  
    /// 返回元素是否在链表中 ,在就返回结点，不在返回null
    /// </summary>  
    /// <param name="longitude"></param>  
    /// <param name="latitude"></param>  
    /// <returns></returns>  
    public ListNode Find(string longitude, string latitude)
    {

        ListNode current = first;
        if (first==null)
        {
            return null;
        }
        while (current != null)
        {
            if (current.longitude.Equals(longitude) && current.latitude.Equals(latitude))
            {
                return current;
            }
            else
            {
                current = current.next;
            }
        }
        return null;
    }
    /// <summary>
    /// 查找节点，有了就返回true，没有就返回false
    /// </summary>
    /// <param name="longitude"></param>
    /// <param name="latitude"></param>
    /// <returns></returns>
    public bool FindNode(string longitude, string latitude)
    {

        ListNode current = first;
        if (first == null)
        {
            return false;
        }
        while (current != null)
        {
            if (current.longitude.Equals(longitude) && current.latitude.Equals(latitude))
            {
                return true;
            }
            else
            {
                current = current.next;
            }
        }
        return false;
    }
    /// <summary>
    /// 根据ID查找
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ListNode Find(string id)
    {

        ListNode current = first;
        if (first == null)
        {
            return null;
        }
        while (current != null)
        {
            if (current.id==id)
            {
                return current;
            }
            else
            {
                current = current.next;
            }
        }
        return null;
    }
    /// <summary>  
    /// 返回x所在的位置  
    /// </summary>  
    /// <param name="x"></param>  
    /// <returns>如果x不在表中则返回0</returns>  
    public int Search(string jindu, string latitude)
    {
        ListNode current = first;
        int index = 1;
        while (current != null && !current.longitude.Equals(jindu) && !current.latitude.Equals(latitude))
        {
            current = current.next;
            index++;
        }
        if (current != null)
            return index;
        return 0;
    }

    /// <summary>  
    /// 删除(longitude, latitude)元素  
    /// </summary>  
    /// <param name="longitude"></param>  
    /// <param name="latitude"></param>  
    /// <returns></returns>  
    public void Delete(string longitude, string latitude)
    {
        ListNode pNode = first;
        ListNode qNode = first;
        while (qNode.next != null)
        {
            if (qNode.longitude.Equals(longitude) && qNode.latitude.Equals(latitude))
            {
                pNode.next = qNode.next;
                break;
            }
            else
            {
                if (qNode.next != null)
                {

                    pNode = qNode;
                    qNode = qNode.next;
                }
            }
        }
    }
    /// <summary>
    ///  清空链表  
    /// </summary>
    public void Clear()
    {
        if (first==null)
        {
            return;
        }
        else
        {
            ListNode node = first.next;
            while (first.next!=null)
            {
                first = null;
                GC.Collect();
                first = node;
                node = first.next;
            }
        }
    }
    /// <summary>
    /// 返回第几个节点
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ListNode GetNode(int index)
    {
        int n = 1;
        ListNode currentNode = first;
        if (currentNode==null)
        {
            return null;
        }
        else
        {
            while (currentNode!=null)
            {
                if (n==index)
                {
                    return currentNode;
                }
                else
                {
                    n++;
                    currentNode = currentNode.next;
                }
            }
        }
        return null;
    }
}
