using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  ヘッドアップ表示のステートバーの基本クラス。基本的な初期化と更新メソッドを提供します。
/// </summary>
public class StatesBar : MonoBehaviour
{
    [SerializeField] protected Image fillImageBack;
    [SerializeField] protected Image fillImageFront;
    [SerializeField] float fillSpeed = 0.1f;
    [SerializeField] bool delayFill = true;  // ステートのバッファが遅れて変化するかどうか
    [SerializeField] float fillDelay = 0.5f; 

    protected float currentFillAmount;
    protected float targetFillAmount;
    float t;  //頻繁な回収を防ぐため
    WaitForSeconds waitForDelayFill;  //  バッファ付きの遅延
    Coroutine bufferedfillingCoroutine;

    private void Awake() 
    {
        //  キャンバスコンポーネントがある場合のみ、カメラをメインカメラに設定
        if (TryGetComponent<Canvas>(out Canvas canvas)) 
        {
            canvas.worldCamera = Camera.main;  // キャンバスにメインカメラをバインド
        }
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    public virtual void UpdateStates(float currentValue, float maxValue) 
    {

        targetFillAmount = currentValue / maxValue;
        if (bufferedfillingCoroutine != null) 
        {
            StopCoroutine(bufferedfillingCoroutine);
        }
        if (currentFillAmount > targetFillAmount)
        {
            // if states reduce: 1. front -> target 2. back slowly reduce
            fillImageFront.fillAmount = targetFillAmount;
            bufferedfillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
            return;
        }
        if (currentFillAmount < targetFillAmount) 
        {
            // if states increase: 1. back -> target 2. front slowly increase
            fillImageBack.fillAmount = targetFillAmount;
            bufferedfillingCoroutine = StartCoroutine(
                BufferedFillingCoroutine(fillImageFront)
            );
        }
    }

    IEnumerator BufferedFillingCoroutine(Image image) 
    {
        if (delayFill)
        {
            yield return waitForDelayFill;
        }

        t = 0f;
        while (t < 1f) 
        {
            t += Time.deltaTime * fillSpeed;  // レンダリング関連のイベントなので deltaTime を使用
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;
            yield return null;  // 中断して、ステートのバッファの持続的な変化を実現する
        }
        if (currentFillAmount <= Mathf.Epsilon) 
        {
            gameObject.SetActive(false);
        }
    }

    public void EmptyStates() 
    {
        fillImageBack.fillAmount = 0;
        fillImageFront.fillAmount = 0;
        currentFillAmount = 0;
    }

}
