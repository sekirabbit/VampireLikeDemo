using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreWeaponGrid : MonoBehaviour
{
    [SerializeField] StoreWeaponBase weapon;
    [SerializeField] Image weaponImage;
    [SerializeField] TMP_Text weaponName;
    [SerializeField] TMP_Text weaponCls;
    [SerializeField] WeaponAttrs weaponAttrs;
    [SerializeField] ConsumeGemBtn buyBtn;
    [SerializeField] Image background;

    int weaponLevel;

    private void OnEnable() 
    {
        RefeshGrid();
        buyBtn.Initialize(weapon.weaponData.weaponPrice);
        buyBtn.consumeGemBtn.onClick.AddListener(Buy);
    }

    private void OnDisable() 
    {
        buyBtn.consumeGemBtn.onClick.RemoveListener(Buy);
    }

    public void SetWeaponGrid(StoreWeaponBase weapon) 
    {
        this.weapon = weapon;
        this.weaponLevel = weapon.weaponLevel;
        RefeshGrid();
    }

    void RefeshGrid() 
    {
        background.color = new Color(
            GameManager.Instance.bgColors[weaponLevel].r, 
            GameManager.Instance.bgColors[weaponLevel].g, 
            GameManager.Instance.bgColors[weaponLevel].b, 
            background.color.a);

        weaponImage.sprite = weapon.weaponData.weaponSprite;
        weaponName.text = weapon.weaponData.weaponName;
        weaponCls.text = EnumAttrs.getWeaponCls(weapon.weaponData.weaponCls);
        
        weaponAttrs.ClearClauses();

        weaponAttrs.genClause(weapon.weaponData.damage, weaponLevel);
        weaponAttrs.genClause(weapon.weaponData.fireRate, weaponLevel);
        weaponAttrs.genClause(weapon.weaponData.range, weaponLevel);
        weaponAttrs.genClause(weapon.weaponData.otherEffects, weaponLevel);
        
        weaponAttrs.genSpecialInfo(weapon.weaponData.specialInfo);

        buyBtn.GetComponentInChildren<TMP_Text>().text = weapon.weaponData.weaponPrice.ToString();
    }

    public void Buy() 
    {
        if (PlayerAttr.Instance.GemNum < weapon.weaponData.weaponPrice) return;
        if (GameManager.Instance.playerWeapons.Count == 6 && !GameManager.Instance.CraftWeapon(weapon)) return;
        PlayerAttr.Instance.GemNum -= weapon.weaponData.weaponPrice;  // コストを支払う
        weapon.Buy();  // 購入する
        StoreObjectGridsArea.Instance.DeactivateLayout();  // レイアウトを無効にする
        gameObject.SetActive(false);  // 自身を無効にする
        Store.Instance.RefreshGem();  // ショップの宝石を更新する
    }

    public void RefreshBtn() 
    {
        buyBtn.IsGemEnough();
    }

}
