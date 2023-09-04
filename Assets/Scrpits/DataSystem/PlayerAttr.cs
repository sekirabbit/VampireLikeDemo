using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの各種属性をまとめ、属性を変更するメソッドを提供するクラスです。
/// </summary>
public class PlayerAttr : Singleton<PlayerAttr>
{
    [Header("Player Attributes")]
    [SerializeField] int maxHealth = 10;      // プレイヤーの最大体力
    [SerializeField] int healthRegeRate = 0;  // 体力回復速度、単位：/ 10s。値が10の場合、10秒ごとに10体力回復
    [SerializeField] int damageFactor = 0;    // ダメージ倍率、単位：%。最終ダメージ：damage * (1 + damageFactor / 100)
    [SerializeField] int attackRangeFactor = 0;  // 攻撃範囲倍率、単位：%。最終攻撃範囲：weaponAttackRange * (1 + attackRangeFactor / 100)
    [SerializeField] int armor = 0;           // 防御力、単位：%。最終ダメージ：damage * (1 - Armor / 100)
    [SerializeField] int criticalRate = 10;    // クリティカル率、単位：%。最終クリティカル率：criticalRate / 100
    [SerializeField] int criticalDamage = 50; // クリティカルダメージ、単位：%。最終クリティカルダメージ：damage * (1 + criticalDamage / 100)
    [SerializeField] int attackSpeed = 0;     // 攻撃速度、単位：%。最終武器の攻撃間隔：weaponAttackInterval * (1 - attackSpeed / 100)
    [SerializeField] int dodgeRate = 0;       // 回避率、単位：%。最終回避率：dodgeRate / 100
    [SerializeField] int moveSpeedFactor = 0; // 移動速度、単位：%。最終移動速度：moveSpeed * (1 + moveSpeedFactor / 100)
    [SerializeField] int pickUpRangeFactor = 0; // アイテム拾いの範囲、単位：%。最終アイテム拾いの範囲：pickUpRange * (1 + pickUpRangeFactor / 100)

    [Header("Other Attrs")]
    [SerializeField] int gemNum = 100;          // 宝石の数
    [SerializeField] int level = 1;             // プレイヤーレベル
    [SerializeField] float currentEXP = 0;      // 現在の経験値
    [SerializeField] float maxEXP = 100;        // 現在のレベルの最大経験値

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int HealthRegeRate { get => healthRegeRate; set => healthRegeRate = value; }
    public int DamageFactor { get => damageFactor; set => damageFactor = value; }
    public int AttackRangeFactor { get => attackRangeFactor; set => attackRangeFactor = value; }
    public int Armor { get => armor; set => armor = value; }
    public int CriticalRate { get => criticalRate; set => criticalRate = value; }
    public int CriticalDamage { get => criticalDamage; set => criticalDamage = value; }
    public int AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int DodgeRate { get => dodgeRate; set => dodgeRate = value; }
    public int MoveSpeedFactor { get => moveSpeedFactor; set => moveSpeedFactor = value; }
    public int PickUpRangeFactor { get => pickUpRangeFactor; set => pickUpRangeFactor = value; }

    public int GemNum { get => gemNum; set => gemNum = value; }
    public int Level { get => level; set => level = value; }
    public float CurrentEXP { get => currentEXP; set => currentEXP = value; }
    public float MaxEXP { get => maxEXP; set => maxEXP = value; }

    public List<int> GetPlayerAttrs() 
    {
        return new List<int> {
            maxHealth,
            healthRegeRate,
            damageFactor,
            attackRangeFactor,
            armor,
            criticalRate,
            criticalDamage,
            attackSpeed,
            dodgeRate,
            moveSpeedFactor,
            pickUpRangeFactor,
        };
    }

    public void ChangeMaxHealth(int value) => maxHealth += value;
    public void ChangeHealthRegeRate(int value) => healthRegeRate += value;
    public void ChangeDamageFactor(int value) => damageFactor += value;
    public void ChangeAttackRangeFactor(int value) => attackRangeFactor += value;
    public void ChangeArmor(int value) => armor += value;
    public void ChangeCriticalRate(int value) => criticalRate += value;
    public void ChangeCriticalDamage(int value) => criticalDamage += value;
    public void ChangeAttackSpeed(int value) => attackSpeed += value;
    public void ChangeDodgeRate(int value) => dodgeRate += value;
    public void ChangeMoveSpeedFactor(int value) => moveSpeedFactor += value;
    public void ChangePickUpRangeFactor(int value) => pickUpRangeFactor += value;
    
    public void ChangeGemNum(int value) => gemNum += value;
    public void ChangeCurrentEXP(int value) => currentEXP += value;


    public delegate void ChangePlayerAttr(int value);
    public static Dictionary<EnumAttrs.PlayerAttrs, ChangePlayerAttr> ChangePlayerAttrFuncDict;

    protected override void Awake() 
    {
        base.Awake();
        ChangePlayerAttrFuncDict = new Dictionary<EnumAttrs.PlayerAttrs, ChangePlayerAttr> 
        {
            {EnumAttrs.PlayerAttrs.MAXHEALTH, ChangeMaxHealth},
            {EnumAttrs.PlayerAttrs.HEALTHREGERATE, ChangeHealthRegeRate},
            {EnumAttrs.PlayerAttrs.DAMAGEFACTOR, ChangeDamageFactor},
            {EnumAttrs.PlayerAttrs.ATTACKRANGEFACTOR, ChangeAttackRangeFactor},
            {EnumAttrs.PlayerAttrs.ARMOR, ChangeArmor},
            {EnumAttrs.PlayerAttrs.CRITICALRATE, ChangeCriticalRate},
            {EnumAttrs.PlayerAttrs.CRITICALDAMAGE, ChangeCriticalDamage},
            {EnumAttrs.PlayerAttrs.ATTACKSPEED, ChangeAttackSpeed},
            {EnumAttrs.PlayerAttrs.DODGERATE, ChangeDodgeRate},
            {EnumAttrs.PlayerAttrs.MOVESPEEDFACTOR, ChangeMoveSpeedFactor},
            {EnumAttrs.PlayerAttrs.PICKUPRANGEFACTOR, ChangePickUpRangeFactor},
        };
    }

    private void OnEnable() 
    {
        GameEvents.LevelUp += LevelUp;
    }

    private void OnDisable()
     {
        ChangePlayerAttrFuncDict.Clear();
    }

    public static ChangePlayerAttr GetChangePlayerAttrFunc(EnumAttrs.PlayerAttrs playerAttr) 
        => ChangePlayerAttrFuncDict[playerAttr];

    public void LevelUp() 
    {
        currentEXP -= maxEXP;
        maxEXP *= 1.2f;
        level++;
    }

}
