using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : Singleton<WaveManager>
{
    [Header("Wave Texts")]
    [SerializeField] TMP_Text waveNumText;
    [SerializeField] TMP_Text waveTimerText;
    [SerializeField] GameObject waveCompleteText;

    [Header("Wave SFXs")]
    [SerializeField] AudioData wavecompleteSFX;

    [Header("Wave Settings")]
    [SerializeField] int waveNum, waveTime;

    public int WaveNum => waveNum;   
    int curWaveTimer;
    WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

    private void OnEnable() 
    {
        curWaveTimer = waveTime + waveNum * 5;
        waveNumText.text = ($"Wave {++waveNum}").ToString();
        waveTimerText.text = curWaveTimer.ToString();
        waveCompleteText.SetActive(false);
        StartCoroutine(nameof(WaveTimer));
    }

    private void OnDisable() 
    {
        StopAllCoroutines();
    }

    //各ウェーブのウェーブ番号の表示とカウントダウン
    IEnumerator WaveTimer() 
    {
        
        while (curWaveTimer > 0) 
        {
            yield return waitForOneSecond;
            curWaveTimer--;
            waveTimerText.text = curWaveTimer.ToString();
        }
        //カウントダウンが終了したら、すべての敵を撃破する。
        EnemyManager.Instance.SlayAll();
        PoolManager.Instance.DeActivateAllLoots();
        GameManager.Instance.playerInput.DisableAllInputs();

        waveCompleteText.SetActive(true);  // クリア表示を有効にする
        AudioManager.Instance.PoolPlaySFX(wavecompleteSFX);  // クリアの効果音を再生する
        
        yield return waitForOneSecond;  
        yield return waitForOneSecond;  
        
        GameManager.Instance.OnWaveEnd();
    }

}
