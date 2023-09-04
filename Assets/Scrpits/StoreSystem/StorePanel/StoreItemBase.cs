using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemBase : StoreObject
{
    [SerializeField] public ItemData itemData;

    public override bool isWeapon => false;

    public override void Buy() 
    {
        getItemBase();
        PlayerStatsPanel.Instance.RefreshStatsPanel();  // プレイヤーの属性パネルを更新する
        ItemPanel.Instance.RefreshItemPanel();  // アイテムパネルを更新する
    }

    private void getItemBase() 
    {
        foreach (ItemEffect effect in itemData.effects) 
        {
            PlayerAttr.GetChangePlayerAttrFunc(effect.attr).Invoke(effect.value);
        }

        if (GameManager.Instance.playerItems.ContainsKey(this)) 
        {
            GameManager.Instance.playerItems[this]++;
        } 
        else 
        {
            GameManager.Instance.playerItems.Add(this, 1);
        }
    }

}
