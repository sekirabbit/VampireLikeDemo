using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public bool IsDead => enemyIsDead;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 0.2f;
    [SerializeField] protected float damage = 1f;

    [Header("Raycast")]
    [SerializeField] float raycastDistance = 0.2f;
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] protected bool enemyIsDead = false;

    [Header("Hit")]
    [SerializeField] GameObject hitVFX;

    LootSpawner lootSpawner;
    new CircleCollider2D collider2D;
    WaitForSeconds waitForDie = new WaitForSeconds(1f);
    
    protected GameObject target;
    Ray ray;
    bool isBlocked;

    protected override void Awake()
     {
        base.Awake();
        collider2D = GetComponent<CircleCollider2D>();
        lootSpawner = GetComponent<LootSpawner>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnEnable() 
    {
        base.OnEnable();
        collider2D.enabled = true;
        rigidbody2D.drag = 100f;
        enemyIsDead = false;
    }

    protected virtual void FixedUpdate() {
        SimpleMove();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.TryGetComponent(out Player player)) 
        {
            player.TakeDamage(damage);
            PoolManager.Release(hitVFX, player.transform.position,
                Quaternion.Euler(0f, 0f, Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg));
        }
    }

    public void SetAttrs(float maxHealth, float damage, float moveSpeed) 
    {
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
    }

    public void SetAttrs(List<float> attrs)
    {
        this.maxHealth = attrs[0];
        this.damage = attrs[1];
        this.moveSpeed = attrs[2];
    }

#region Die

    public override void Die()
     {
        enemyIsDead = true;
        collider2D.enabled = false;
        rigidbody2D.drag = 5f;
        EnemyManager.Instance.RemoveEnemy(gameObject);

        lootSpawner.SpawnLoots(transform.position);

        StartCoroutine(DieCoroutine());
    }

    public void DieWithoutLoot() 
    {
        enemyIsDead = true;
        collider2D.enabled = false;
        rigidbody2D.drag = 5f;
        EnemyManager.Instance.RemoveEnemy(gameObject);

        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine() 
    {
        animator.SetTrigger("Die");
        yield return waitForDie;
        gameObject.SetActive(false);
    }

#endregion

#region Move

    protected virtual void SimpleMove() 
    {
        if (enemyIsDead || IsPathBlocked()) return;
        moveDirection = (target.transform.position - transform.position).normalized;
        FlipCharacter();
        transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    protected bool IsPathBlocked()
     {
        ray = new Ray(transform.position, moveDirection);
        collider2D.enabled = false;
        isBlocked = Physics2D.Raycast(ray.origin, ray.direction, raycastDistance, raycastLayerMask);
        collider2D.enabled = true;
        return isBlocked;
    }

#endregion

}
