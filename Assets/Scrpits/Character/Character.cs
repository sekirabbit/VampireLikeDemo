using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// キャラクターの基本クラスで、ゲーム内のキャラクターの基本的な属性やメソッドを含みます。例えば、体力、死亡など
/// </summary>
public class Character : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected StatesBar onHeadHealthBar;
    [SerializeField] protected bool showOnHeadHealthBar = true;

    new protected Rigidbody2D rigidbody2D;  // キャラクターの
    protected Animator animator;  // キャラクターのアニメーターコントローラー
    protected SpriteRenderer spriteRenderer;  // キャラクターのスプライトレンダラー

    protected float health;  // キャラクターの体力
    protected Vector2 moveDirection;  // キャラクターの移動方向

    protected virtual void Awake() 
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        // キャラクターのモデルとアニメーションコントローラーはサブオブジェクト内にあります
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        rigidbody2D.gravityScale = 0f;
    }

    /// <summary>
    /// 体力と体力バーの表示を初期化します。
    /// </summary>
    protected virtual void OnEnable() 
    {
        health = maxHealth;
        if (showOnHeadHealthBar) ShowOnHeadHealthBar();
        else HideOnHeadHealthBar();
    }

    public virtual void FlipCharacter() 
    {
        spriteRenderer.flipX = moveDirection.x < 0 ? true : false;
    }

    /// <summary>
    /// キャラクターの頭上に体力バーを表示します。
    /// </summary>
    public void ShowOnHeadHealthBar() 
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }

    /// <summary>
    /// キャラクターの頭上の体力バーを非表示にします。
    /// </summary>
    public void HideOnHeadHealthBar() 
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage) 
    {
        if (health == 0) return;
        health -= damage;
        if (showOnHeadHealthBar) 
        {
            onHeadHealthBar.UpdateStates(health, maxHealth);
        }
        if (health <= 0f)
         {
            health = 0f;
            Die();
        }
    }

    public virtual void Die() 
    {
        gameObject.SetActive(false);
    }

}
