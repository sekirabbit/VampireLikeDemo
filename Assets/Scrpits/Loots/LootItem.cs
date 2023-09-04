using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LootItem : MonoBehaviour
{
    [SerializeField] float minSpeed = 0.2f;
    [SerializeField] float maxSpeed = 0.5f;
    [SerializeField] protected AudioData[] pickUpSFX;

    protected Player player;
    
    private void Awake() 
    {
        player = FindObjectOfType<Player>();
    }

    /// <summary>
    ///  プレイヤーがトリガーを検知した場合、アイテムを拾います。これにより、Lootsレイヤーと衝突するにはプレイヤーレイヤーのみが可能です。
    ///  プレイヤーであるかどうかのチェックは省略できます。
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) 
    {
        PickedUp();
    }

    protected virtual void PickedUp() 
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        AudioManager.Instance.PoolPlayRandomSFX(pickUpSFX);
    }

    public IEnumerator MoveCoroutine() 
    {
        float speed = Random.Range(minSpeed, maxSpeed);
        Vector2 direction = Vector2.left;
        while (true) 
        {
            if (player.isActiveAndEnabled) 
            {
                direction = (player.transform.position - transform.position).normalized;
            }
            transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }
    }
}
