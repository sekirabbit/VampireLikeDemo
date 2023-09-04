using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverForm : MonoBehaviour
{
    [SerializeField] private GameObject masks;
    [SerializeField] TMP_Text Title;
    [SerializeField] private Button menuButton; 
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    // シーンがロードされる際にAwakeメソッドが有効になります。
    private void Awake() 
    {
        masks.SetActive(false);
        menuButton.onClick.AddListener(OnMenuButtonClick);  // ボタンのリスナーを追加
        restartButton.onClick.AddListener(OnRestartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    // このスクリプトがアタッチされたコンポーネントがロードされる際、OnEnableが有効になり、繰り返し使用できます。
    private void OnEnable()
    {
        GameEvents.GameOver += GameOver;  // イベントリスナーを追加
        GameEvents.GameWin += GameWin;
    }

    private void OnDisable() 
    {
        GameEvents.GameOver -= GameOver;  // イベントリスナーを削除
        GameEvents.GameWin -= GameWin;
    }

    private void GameOver()
    {
        Title.text = "YOU DIED";
        Title.color = Color.red;
        masks.SetActive(true);
        EnemyManager.Instance.SlayAll();
    }

    private void GameWin()
    {
        EnemyManager.Instance.SlayAll();
        GameManager.Instance.playerInput.DisableAllInputs();
        WaveManager.Instance.gameObject.SetActive(false);
        EnemyManager.Instance.gameObject.SetActive(false);
        PoolManager.Instance.DeActivateAllLoots();
        StartCoroutine(WinCoroutine());
    }

    IEnumerator WinCoroutine() 
    {
        yield return 0.5f;
        Title.text = "YOU WIN !";
        Title.color = Color.green;
        masks.SetActive(true);
    }
    

    //==================== Button ==================//
    private void OnMenuButtonClick()
    {
        // シーンを切り替えてメインメニューに戻る
        SceneManager.LoadScene("Menu");
    }

    private void OnRestartButtonClick()
    {
        // ゲームのシーンを再読み込み
        SceneManager.LoadScene("Main");
    }

    private void OnQuitButtonClick()
    {
        // ゲーム終了
        Application.Quit();

        // Unityエディタモードであれば実行を停止
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


}
