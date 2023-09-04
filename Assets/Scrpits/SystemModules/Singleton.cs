using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component  // 型はComponentクラスを継承する必要があります。
{
    public static T Instance { get; private set; }

    /// <summary>
    /// シングルトンを取得する
    /// </summary>
    protected virtual void Awake() 
    {
        Instance = this as T; 
    }
}
