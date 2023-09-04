using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内のすべての銃の基本クラス
/// </summary>
public class Gun : MonoBehaviour
{

    [Header("Bullets & Shells")]
    [SerializeField] GameObject bulletPrefab;  // 子弾のプレハブ
    [SerializeField] GameObject shellPrefab;  // 貝殻のプレハブ
    [SerializeField] Transform muzzlePoint;  // 銃口の位置
    [SerializeField] Transform shellPoint;  // 貝殻排出位置

    [Header("Weapon stats")]
    [SerializeField] LayerMask enemyLayer;  // 敵のレイヤー

    [Header("Audio")]
    [SerializeField] AudioData fireAudio;  // 射撃音

    Vector2 gunDirection;
    SpriteRenderer spriteRenderer;
    Animator animator;
    WaitForSeconds waitForFireInterval;
    GameObject bulletObject;
    Bullet bullet;
    Collider2D[] colliders;

    float damage;
    float gunRange;
    float fireRate;
    float fireTimer;

    private void Awake() 
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update() 
    {
        AutoFollowenemy();
        AutoFlip();
        fireTimer -= Time.deltaTime;
    }

    void SingleFire() 
    {
        animator.SetTrigger("Fire");
        StartCoroutine(AudioManager.Instance.PoolPlayRandomSFX(fireAudio, Random.Range(0.04f, 0.1f)));
        
        bulletObject = PoolManager.Release(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        bullet = bulletObject.GetComponent<Bullet>();
        bullet.SetSpeed(Quaternion.Euler(0f, 0f, Random.Range(-5f, 5f)) * gunDirection);
        bullet.SetDamage(damage);
        
        PoolManager.Release(shellPrefab, shellPoint.position, shellPoint.rotation);
    }

    void AutoFollowenemy() 
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, 
            DamageManager.Instance.GetFireRange(gunRange), enemyLayer);  // TODO: 発射直前だけfollowする
        if (colliders.Length == 0) 
        {
            gunDirection = Vector2.right;
            transform.right = gunDirection;
            return;
        }
        
        Collider2D randomEnemy = colliders[Random.Range(0, colliders.Length)];
        if (randomEnemy.TryGetComponent<Enemy>(out Enemy enemy) && !enemy.IsDead) 
        {
            gunDirection = ((Vector2)(enemy.transform.position - transform.position)).normalized;
            transform.right = gunDirection;  // 敵方向に向いて
            if (fireTimer <= 0f) 
            {
                SingleFire();
                fireTimer = DamageManager.Instance.GetFireRate(fireRate);
            }
        }
        
    }

    void AutoFlip() 
    {
        spriteRenderer.flipY = gunDirection.x < 0f;
    }

    public void SetGunAttrs(WeaponData weaponData, int weaponLevel) 
    {
        this.damage = weaponData.damage.value[weaponLevel];
        this.gunRange = weaponData.range.value[weaponLevel];
        this.fireRate = weaponData.fireRate.value[weaponLevel];
    }

    public void SetGunMaterial(Material material) 
    {
        spriteRenderer.material = material;
    }

}
