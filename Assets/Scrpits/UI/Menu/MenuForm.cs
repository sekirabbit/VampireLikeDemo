using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//  メインメニュー機能の実装
public class MenuForm : MonoBehaviour
{
    //========================= 選択と選択解除のための二重操作の実装 ====================//
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] AudioData mainMenuBGM;  // BGM
    [SerializeField] AudioData fingerSnapAudioData;  // 選択音効
    [SerializeField] AudioData selectAudioData;  // 選択音効

    // ここでは、=> はラムダ式により、メソッドの戻り値をプロパティ値として返します。
    private Toggle currentSelection => toggleGroup.GetFirstActiveToggle(); 
    private Toggle onToggle;  // 現在の選択トグル

    // Awake と Start のどちらかを使用する必要があります。Awake を使用すると、ヌル参照エラーが発生します。
    private void Start() 
    {
        var toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener( _ => OnToggleValueChanged(toggle));
        }

        currentSelection.onValueChanged?.Invoke(true);  // 画面に入ったときにイベントをトリガーする  
        StartCoroutine(AudioManager.Instance.PlayMusic(mainMenuBGM, 0.3f));
    }

    private void OnToggleValueChanged(Toggle toggle)
    {
        if (onToggle == currentSelection)
        {
            
            AudioManager.Instance.PoolPlaySFX(fingerSnapAudioData);  // 選択音を再生

            switch (toggle.name){
                case "GameStart":
                    SceneManager.LoadScene("Main");
                    break;
                case "Settings":
                    // TODO:音量調整と難易度調整
                    break;
                case "Sponsor":
                    // TODO:素材を提供する友達に感謝する
                    break;
                case "Quit":
                    Application.Quit();
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                    break;
                default:
                    throw new UnityException("Toggle name is Invalid!");
            }
            return;
        }
        if (toggle.isOn)
        {
            onToggle = toggle;
            // 選択されたトグルのテキストを黄色に設定
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.yellow;

            AudioManager.Instance.PoolPlaySFX(selectAudioData);  // 選択音を再生

        } 
        else
        {
            onToggle.transform.Find("Label").GetComponent<TMP_Text>().color = Color.white;
        }
    }
}