using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeStopController : MonoBehaviour
{
    [Header("キー設定")]
    public KeyCode timeStopKey = KeyCode.T;

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
        // ?? 入力受付
        if (Input.GetKeyDown(timeStopKey))
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

        // ?? フェード制御
        if (overlayImage != null)
        {
            overlayColor.a = Mathf.Lerp(overlayColor.a, targetAlpha, Time.unscaledDeltaTime * fadeSpeed);
            overlayImage.color = overlayColor;
        }

        // ?? ゲージ制御
        if (timeGauge != null)
        {
            if (isStopped)
            {
                timeGauge.value = Mathf.Max(0, stopDuration - currentTime);
            }
            else if (isOnCooldown)
            {
                // クールタイム中は灰色にしておく
                timeGauge.value = 0;
            }
            else
            {
                // 使用可能時は満タンに戻す
                timeGauge.value = Mathf.Lerp(timeGauge.value, stopDuration, Time.unscaledDeltaTime * 2f);
            }
        }
    }

    private void StartTimeStop()
    {
        isStopped = true;
        targetAlpha = darkAlpha;
        currentTime = 0f;

        if (timeRoutine != null)
            StopCoroutine(timeRoutine);
        timeRoutine = StartCoroutine(TimeStopRoutine());
    }

    private void StopTimeStop()
    {
        isStopped = false;
        targetAlpha = 0f;

        if (timeRoutine != null)
            StopCoroutine(timeRoutine);
        timeRoutine = StartCoroutine(CooldownRoutine());
    }

    private IEnumerator TimeStopRoutine()
    {
        // 停止時間経過
        while (currentTime < stopDuration)
        {
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // 自動解除
        if (isStopped)
            StopTimeStop();
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        float t = 0f;

        // クールタイム中の経過
        while (t < cooldownTime)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        isOnCooldown = false;
        timeRoutine = null;
    }
}
