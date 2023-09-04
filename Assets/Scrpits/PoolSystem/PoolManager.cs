using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerBulletPools;
    [SerializeField] Pool[] vFXPools;
    [SerializeField] Pool[] textPools;
    [SerializeField] Pool[] lootsPools;
    static Dictionary<GameObject, Pool> prefab2Pool;  // プレハブを対応するプールにマッピングします。

    public void DeActivateAllLoots() 
    {
        foreach (var pool in lootsPools) 
        {
            pool.DeActivateAll();
        }
    }

    protected override void Awake() 
    {
        base.Awake();
        prefab2Pool = new Dictionary<GameObject, Pool>();
        Initialize(enemyPools);
        Initialize(playerBulletPools);
        Initialize(textPools);
        Initialize(vFXPools);
        Initialize(lootsPools);
    }

    #if UNITY_EDITOR
        // エディターの実行が停止したときに呼び出されます。デバッグに適しています。
        private void OnDestroy() 
        {
            checkPoolSize(enemyPools);
            checkPoolSize(playerBulletPools);
            checkPoolSize(textPools);
            checkPoolSize(vFXPools);
            checkPoolSize(lootsPools);
        }

    #endif

    void checkPoolSize(Pool[] pools) 
    {
        foreach (var pool in pools) 
        {
            if (pool.RuntimeSize > pool.Size) 
            {
                Debug.LogWarning(
                    $"Pool: {pool.Prefab.name} has a runtime size {pool.RuntimeSize} bigger " +
                    $"than its initial size {pool.Size}!"
                );
            }
        }
    }

    void Initialize(Pool[] pools) 
    {
        foreach (var pool in pools) 
        {
            // 条件付きコンパイル。Unity内でのみ有効にし、他のプラットフォームでの実行効率を確保します。
            #if UNITY_EDITOR
                if (prefab2Pool.ContainsKey(pool.Prefab)) {
                    Debug.LogError("Same prefab in multiple Pools! Prefab: " + pool.Prefab.name);
                    continue;  // 重複するキーを防ぐため
                }
            #endif
            prefab2Pool.Add(pool.Prefab, pool);
            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// 指定されたパラメータに基づいて、オブジェクトプールから準備されたオブジェクトを返します。
    /// </summary>
    /// <param name="prefab">指定されたオブジェクトのプレハブ</param>
    /// <returns>オブジェクトプールからの準備されたオブジェクト</returns>
    public static GameObject Release(GameObject prefab) 
    {
        #if UNITY_EDITOR
            if (!prefab2Pool.ContainsKey(prefab)) 
            {
                Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);
                return null;
            }
        #endif
        return prefab2Pool[prefab].preparedObject();
    }

    public static GameObject Release(GameObject prefab, Vector3 position) 
    {
        #if UNITY_EDITOR
            if (!prefab2Pool.ContainsKey(prefab)) 
            {
                Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);
                return null;
            }
        #endif
        return prefab2Pool[prefab].preparedObject(position);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation) 
    {
        #if UNITY_EDITOR
            if (!prefab2Pool.ContainsKey(prefab)) 
            {
                Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);
                return null;
            }
        #endif
        return prefab2Pool[prefab].preparedObject(position, rotation);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        #if UNITY_EDITOR
            if (!prefab2Pool.ContainsKey(prefab)) 
            {
                Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);
                return null;
            }
        #endif
        return prefab2Pool[prefab].preparedObject(position, rotation, localScale);
    }

}
