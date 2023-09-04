using UnityEngine;

[System.Serializable]  // Unityのインスペクターパネルで編集可能にするためのシリアライズタグ
public struct MapBound
{
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
}


// CameraFollowクラスは、カメラの追従機能を実装します。Cinemachineを代わりに使用することもできます。
public class CameraFollow : Singleton<CameraFollow>
{
    public Transform target;  // カメラが追従するプレイヤー
    public float smoothTime = 0.4f; 
    public MapBound mapBound;

    private float _offsetZ;  // Z軸のオフセット値
    private Vector3 _currentVelocity;

    private void Start() 
    {    
        if (target == null)
        {
            Debug.LogError("Can't find player, please check it!");
            return;
        }

        _offsetZ = (transform.position - target.position).z;  // Z轴的偏移值
    }

    private void FixedUpdate() 
    {
        if (target == null) return;
        // カメラ平面内での目標位置を計算し、カメラが移動すべき位置を求めます
        Vector3 targetPosition = target.position + Vector3.forward * _offsetZ;
        // refキーワードは変数の参照を関数に渡すため、関数が元の変数の値を変更できるようにします
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, 
        ref _currentVelocity, smoothTime);

        // カメラの追従を境界内に制限する
        newPosition.x = Mathf.Clamp(newPosition.x, mapBound.xMin, mapBound.xMax);
        newPosition.y = Mathf.Clamp(newPosition.y, mapBound.yMin, mapBound.yMax);

        // カメラの位置を更新
        transform.position = newPosition;
    }

    public void ResetCamera() 
    {
        transform.position = new Vector3(0f, 0f, -10f);
    }

}
