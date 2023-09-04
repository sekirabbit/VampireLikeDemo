using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AudioManagerはゲーム内のすべての音の再生を担当しています。
/// </summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;  // 効果音プレーヤー
    [SerializeField] int sFXPoolSize = 10;  // 効果音プールのサイズ

    private AudioSource musicSource;  // 音楽プレーヤー
    private AudioSource[] soundSources;  // 効果音プレーヤーの配列
    private int soundSourceIndex = 0;  // 現在の効果音プレーヤーのインデックス
    const float MIN_PITCH = 0.9f;  // ランダム効果音の最小ピッチ
    const float MAX_PITCH = 1.1f;  // ランダム効果音の最大ピッチ

    protected override void Awake() 
    {
        base.Awake();
        
        // Music
        GameObject newMusicSource = new GameObject {name = "Music Source"};
        musicSource = newMusicSource.AddComponent<AudioSource>();
        // 新しいオブジェクトを現在のオブジェクトの子オブジェクトに設定する
        newMusicSource.transform.SetParent(transform);  

        // Sound
        soundSources = new AudioSource[sFXPoolSize];
        for (int i = 0; i < sFXPoolSize; i++) 
        {
            GameObject newSoundSource = new GameObject("SoundSource" + i);
            soundSources[i] = newSoundSource.AddComponent<AudioSource>();
            newSoundSource.transform.parent = transform;
        }
    }

    public void PlayMusic(AudioData audioData) 
    {
        musicSource.clip = audioData.audioClip;
        musicSource.volume = audioData.volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public IEnumerator PlayMusic(AudioData audioData, float delay) 
    {
        yield return new WaitForSeconds(delay);
        PlayMusic(audioData);
    }

    public void PoolPlaySFX(AudioData audioData) 
    {
        soundSources[soundSourceIndex].PlayOneShot(audioData.audioClip, audioData.volume);
        soundSourceIndex = (soundSourceIndex + 1) % sFXPoolSize;
    }

    public void PoolPlayRandomSFX(AudioData audioData) 
    {
        soundSources[soundSourceIndex].pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PoolPlaySFX(audioData);
    }

    public void PoolPlayRandomSFX(AudioData[] audioData) 
    {
        PoolPlayRandomSFX(audioData[Random.Range(0, audioData.Length)]);
    }

    public IEnumerator PoolPlayRandomSFX(AudioData audioData, float interval) 
    {
        yield return new WaitForSeconds(interval);
        PoolPlayRandomSFX(audioData);
    }

}