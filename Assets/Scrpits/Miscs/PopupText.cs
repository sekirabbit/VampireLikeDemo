using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{

    [Header("Move up")]
    [SerializeField] Vector3 moveUpVector = new Vector3(0, 1, 0);  // テキストが上に移動する方向
    [SerializeField] float moveUpSpeed = 2.0f;  // テキストの上昇速度
    [SerializeField] float scaleUpSpeed = 1.0f;  // テキストの拡大速度

    [Header("Move Down")]
    [SerializeField] Vector3 moveDownVector = new Vector3(-0.7f, 1, 0);  // テキストが下に移動する方向

    [Header("Disappear")]
    [SerializeField] float disappearSpeed = 3.0f;  // テキストの消える速度
    [SerializeField] float disappearTime = 0.2f;  // テキストが消え始める時間

    [Header("Damage Color")]
    [SerializeField] Color normalColor;
    [SerializeField] Color criticalColor;

    TextMeshPro textMesh;
    Color textColor;
    float disappearTimer;  // テキストが消えるタイマー

    private void Awake() 
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
    }

    private void Update() 
    {
        // 上昇
        if (disappearTimer > 0) 
        {
            transform.position += moveUpVector * Time.deltaTime;
            moveUpVector += moveUpVector * moveUpSpeed * Time.deltaTime;  // 加速漂浮速度
            transform.localScale += Vector3.one * scaleUpSpeed * Time.deltaTime;
        }

        // 消す
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) 
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) 
            {
                gameObject.SetActive(false);
            }
        }

    }

    public void SetText(int damage, bool isCritical) 
    {
        textMesh.text = damage.ToString();
        if (isCritical) 
        {
            textMesh.fontSize = 7;
            textColor = criticalColor;
        } 
        else 
        {
            textMesh.fontSize = 5;
            textColor = normalColor;
        }
        textMesh.color = textColor;
        disappearTimer = disappearTime;
    }

}
