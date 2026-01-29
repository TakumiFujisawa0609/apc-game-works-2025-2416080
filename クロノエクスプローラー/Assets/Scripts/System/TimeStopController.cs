using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class TimeStopController : MonoBehaviour
{
    [Header("キー設定")]
    public KeyCode timeStopKey;

    [Header("視覚演出")]
    [SerializeField] private Image overlayImage;
    [Range(0f, 1f)] public float darkAlpha = 0.75f;
    public float fadeSpeed = 3f;

    [Header("時間停止設定")]
    public float stopDuration = 4f;     // 停止時間
    public float cooldownTime = 5f;     // クールタイム
    public static bool isStopped = false;
    public static bool isOnCooldown = false;

    [Header("UI設定")]
    [SerializeField] private Slider timeGauge;
    private float currentTime = 0f;

    private float targetAlpha = 0f;
    private Color overlayColor;
    private Coroutine timeRoutine;

    // シーン跨ぎで確実に初期化するためのシングルトン参照
    private static TimeStopController _instance;

    //  どこからでも初期化できる静的API
    // 時止めの状態/演出/UIを即リセット（シーン遷移時用)
    public static void ForceClearStatic()
    {
        isStopped = false;
        isOnCooldown = false;
        // インスタンスがあれば見た目/UIも即時初期化
        if (_instance != null) _instance.ClearImmediate();
    }

    void Awake()
    {
        _instance = this;  //静的クリアがUIにも届くように
        //プレイ中のシーン再読み込み直後に必ずクリーン状態に
        ForceClearStatic();
    }

    void OnEnable()
    {
        //どんなロード経路でもロード後に必ず初期化されるように
        SceneManager.sceneLoaded += OnSceneLoaded_Reset;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded_Reset;
        //オブジェクト破棄時も状態持ち越しを防止
        ForceClearStatic();
    }

    private void OnSceneLoaded_Reset(Scene s, LoadSceneMode m)
    {
        ForceClearStatic();
    }

    void Start()
    {
        if (overlayImage != null)
        {
            overlayColor = overlayImage.color;
            overlayColor.a = 0f;
            overlayImage.color = overlayColor;
        }

        if (timeGauge != null)
        {
            timeGauge.maxValue = stopDuration;
            timeGauge.value = stopDuration;
        }
    }

    void Update()
    {
        // 入力受付
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(timeStopKey))
        {
            if (!isStopped && !isOnCooldown)
            {
                StartTimeStop();
            }
            else if (isStopped)
            {
                StopTimeStop();
            }
        }

        // フェード制御（unscaled）
        if (overlayImage != null)
        {
            overlayColor.a = Mathf.Lerp(overlayColor.a, targetAlpha, Time.unscaledDeltaTime * fadeSpeed);
            overlayImage.color = overlayColor;
        }

        // ゲージ制御（unscaled）
        if (timeGauge != null)
        {
            if (isStopped)
            {
                timeGauge.value = Mathf.Max(0, stopDuration - currentTime);
            }
            else if (isOnCooldown)
            {
                timeGauge.value = 0;
            }
            else
            {
                timeGauge.value = Mathf.Lerp(timeGauge.value, stopDuration, Time.unscaledDeltaTime * 2f);
            }
        }
    }

    private void StartTimeStop()
    {
        isStopped = true;
        targetAlpha = darkAlpha;
        currentTime = 0f;
        SfxPlayer.Play2D(SfxKey.TimeStop);

        if (timeRoutine != null) StopCoroutine(timeRoutine);
        timeRoutine = StartCoroutine(TimeStopRoutine());
    }

    private void StopTimeStop()
    {
        isStopped = false;
        targetAlpha = 0f;

        if (timeRoutine != null) StopCoroutine(timeRoutine);
        timeRoutine = StartCoroutine(CooldownRoutine());
    }

    private IEnumerator TimeStopRoutine()
    {
        // 停止時間経過（unscaled）
        while (currentTime < stopDuration)
        {
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // 自動解除
        if (isStopped) StopTimeStop();
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        float t = 0f;

        // クールタイム経過（unscaled）
        while (t < cooldownTime)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        isOnCooldown = false;
        timeRoutine = null;
    }

    //見た目＆内部状態を即初期化する実体処理
    private void ClearImmediate()
    {
        // 内部状態
        isStopped = false;
        isOnCooldown = false;
        currentTime = 0f;
        targetAlpha = 0f;
        if (timeRoutine != null) { StopCoroutine(timeRoutine); timeRoutine = null; }

        // 視覚/UIを即リセット
        if (overlayImage != null)
        {
            overlayColor = overlayImage.color;
            overlayColor.a = 0f;
            overlayImage.color = overlayColor;
        }
        if (timeGauge != null)
        {
            timeGauge.maxValue = stopDuration;
            timeGauge.value = stopDuration;
        }
    }
}
