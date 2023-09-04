using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPLoot : LootItem
{
    [SerializeField] int exp = 8;  // 提供された経験値

    protected override void PickedUp() 
    {
        GameManager.Instance.OnGemChangedInGame(Random.Range(1, 6));
        GameManager.Instance.ChangeEXP(exp);
        base.PickedUp();
    }
}
