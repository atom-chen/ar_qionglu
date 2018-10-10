using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ElviraFrame
{


    /// <summary>
    /// ��ͨ���ĵ�������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : IDisposable where T : new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {

                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
            set { instance = value; }
        }

        public virtual void Dispose()
        {

        }
    }
}
