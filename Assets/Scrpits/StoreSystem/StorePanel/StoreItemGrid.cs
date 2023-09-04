using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreItemGrid : MonoBehaviour
{
    [SerializeField] StoreItemBase storeItem;
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text itemCls;
    [SerializeField] ItemAttrs itemAttrs;
    [SerializeField] ConsumeGemBtn buyBtn;
    [SerializeField] Image background;

    int itemLevel;

    private void OnEnable() 
    {
        RefeshGrid();
        buyBtn.Initialize(storeItem.itemData.itemPrice);
        buyBtn.consumeGemBtn.onClick.AddListener(Buy);
    }

    private void OnDisable() 
    {
        buyBtn.consumeGemBtn.onClick.RemoveListener(Buy);
    }

    public void SetItemGrid(StoreItemBase item) 
    {
        storeItem = item;
        itemLevel = storeItem.itemData.itemLevel;
        RefeshGrid();
    }

    void RefeshGrid() 
    {
        background.color = new Color(
            GameManager.Instance.bgColors[itemLevel].r, 
            GameManager.Instance.bgColors[itemLevel].g, 
            GameManager.Instance.bgColors[itemLevel].b, 
            background.color.a);

        itemImage.sprite = storeItem.itemData.itemSprite;
        itemName.text = storeItem.itemData.itemName;
        itemCls.text = EnumAttrs.getItemCls(storeItem.itemData.itemCls);

        itemAttrs.ClearClauses();

        itemAttrs.genClause(storeItem.itemData.effects);
        itemAttrs.genSpecialInfo(storeItem.itemData.specialInfo);
        buyBtn.GetComponentInChildren<TMP_Text>().text = storeItem.itemData.itemPrice.ToString();
    }

    public void Buy() 
    {
        if (PlayerAttr.Instance.GemNum < storeItem.itemData.itemPrice) return;
        PlayerAttr.Instance.GemNum -= storeItem.itemData.itemPrice;  // コストを支払う
        storeItem.Buy();  // 購入する
        StoreObjectGridsArea.Instance.DeactivateLayout();  // レイアウトを無効にする
        gameObject.SetActive(false);  // 自身を無効にする
        Store.Instance.RefreshGem();  // ショップの宝石を更新する
    }

    public void RefreshBtn() 
    {
        buyBtn.IsGemEnough();
    }

}
