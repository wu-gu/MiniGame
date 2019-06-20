using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    #region 单例

    protected static T s_Instance;

    public static T Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<T>();

            if (s_Instance != null)
                return s_Instance;

            return s_Instance;
        }
    }

    #endregion
    // //可以重写的Awake虚方法，用于实例化对象
    // protected virtual void Awake()
    //{
    //    s_Instance = this as T;
    //}
}
