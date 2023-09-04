using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject Prefab => prefab;  // { get {return prefab}; }と同じ
    public int Size => size;
    public int RuntimeSize => queue.Count;  // オブジェクトプールの実際のサイズ
    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;  // オブジェクトプールのサイズ
    Queue<GameObject> queue;
    Transform parent;

    // オブジェクトプールの初期化
    public void Initialize(Transform parent) 
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (var i = 0; i < size; i++) 
        {
            queue.Enqueue(Copy());
        }
    }

    // 新しいオブジェクトを生成
    GameObject Copy() 
    {
        var copy = GameObject.Instantiate(prefab, parent);  // 親クラスを設定する
        copy.SetActive(false);
        return copy;
    }

    // オブジェクトプールからオブジェクトを取得
    GameObject AvailableObject() 
    {
        GameObject availableObject = null;
        // キューが空でなく、先頭のオブジェクトが非アクティブの場合、先頭のオブジェクトを返し、それ以外の場合は新しいオブジェクトを生成
        availableObject = (queue.Count > 0 && !queue.Peek().activeSelf) ? queue.Dequeue() : Copy();
        queue.Enqueue(availableObject);  // アクティブにした後でキューに戻すが、重複してアクティブになるのを避ける必要があります
        return availableObject;
    }

    // オブジェクトプールから取得したオブジェクトをアクティブにする
    public GameObject preparedObject() 
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position, Quaternion rotation) 
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position, Quaternion rotation, Vector3 localScale) 
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;
        return preparedObject;
    }

    public void DeActivateAll() 
    {
        foreach (var obj in queue) 
        {
            obj.SetActive(false);
        }
    }

}
