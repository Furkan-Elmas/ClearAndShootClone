using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            lock (Lock)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        var go = new GameObject($"[{typeof(T)}]");
                        instance = go.AddComponent<T>();
                    }

                    DontDestroyOnLoad(instance);
                }

                return instance; 
            }
        }
    }

    private static readonly object Lock = new object();
    private static T instance;


    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }
}