using System;
using System.Threading;

/// <summary>
/// 简单的线程类
/// </summary>
public class EasyThread
{
    Thread m_Thread;
    /// <summary>
    /// 线程方法
    /// </summary>
    public Action m_CallBack;
    /// <summary>
    /// 线程id
    /// </summary>
    static int workerId = 0;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="cb">线程方法</param>
    /// <param name="priority">线程优先级</param>
    public EasyThread(Action cb, ThreadPriority priority = ThreadPriority.Normal)
    {
        m_Thread = new Thread(Run);
        m_CallBack = cb;
        m_Thread.Priority = priority;

        workerId++;
        //UnityEngine.Debug.LogFormat("开启进程 ThreadId is {0}", workerId);
    }

    /// <summary>
    /// 是否活着
    /// </summary>
    /// <returns></returns>
    public bool IsAlive()
    {
        return m_Thread.IsAlive;
    }

    /// <summary>
    /// 启动线程
    /// </summary>
    public void Start()
    {
        m_Thread.Start();
    }

    /// <summary>
    /// 执行线程方法
    /// </summary>
    void Run()
    {
        m_CallBack();
    }


    public void Stop()
    {
        UnityEngine.Debug.LogWarning("结束线程");
        m_Thread.Abort();
    }
}
