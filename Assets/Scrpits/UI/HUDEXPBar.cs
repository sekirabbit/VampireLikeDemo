using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDEXPBar : StatesBar
{
    [SerializeField] TMP_Text text;

    public override void Initialize(float currentValue, float maxValue) 
    {
        base.Initialize(currentValue, maxValue);
        SetLevel();
    }

    public override void UpdateStates(float currentValue, float maxValue) 
    {
        if (currentValue >= maxValue) 
        {
            currentValue -= maxValue;
            GameManager.Instance.LevelUp();  // 経験値システムのレベルアップメソッドを呼び出す
            SetLevel();
            EmptyStates();  // 経験バーをリセットする
        }
        base.UpdateStates(currentValue, maxValue);
    }

    public void SetLevel() 
    {
        text.text = $"Lv {PlayerAttr.Instance.Level}";
    }

}
