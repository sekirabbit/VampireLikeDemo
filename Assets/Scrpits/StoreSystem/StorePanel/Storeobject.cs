using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ショップの全てのアイテム基本クラス
/// </summary>
public abstract class StoreObject : MonoBehaviour
{
    public abstract bool isWeapon { get; }
    public abstract void Buy();

}
