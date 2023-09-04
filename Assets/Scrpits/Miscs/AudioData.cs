using UnityEngine;

/// <summary>
/// オーディオデータ。オーディオクリップとボリュームを含みます。
/// </summary>
[System.Serializable]  // このクラスがインスペクターパネルで表示されるようにします
public class AudioData 
{
    public AudioClip audioClip;
    public float volume;
}
