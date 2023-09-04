using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component  // 型はComponentクラスを継承する必要があります。
{
    public static T Instance { get; private set; }

    protected virtual void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this as T;
        } 
        else 
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
