using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeStopController : MonoBehaviour
{
    [Header("時間停止設定")]
    public KeyCode timeStopKey = KeyCode.J;
    public float stopDuration = 3f;
    public float cooldown = 2f;

    [Header("フェード設定")]
    public Image overlayImage;       // ← TimeStopOverlayを割り当て
    public float fadeSpeed = 3f;     // 暗転速度
    public float targetAlpha = 0.5f; // 暗くする度合い（0.0?1.0）

    private bool isTimeStopped = false;
    private bool isOnCooldown = false;

    void Update()
    {
        if (Input.GetKeyDown(timeStopKey) && !isTimeStopped && !isOnCooldown)
        {
            StartCoroutine(TimeStopRoutine());
        }
    }

    private IEnumerator TimeStopRoutine()
    {
        isTimeStopped = true;

        // フェードで画面を暗くする
        StartCoroutine(FadeOverlay(targetAlpha));

        // 時間停止
        Time.timeScale = 0f;
        Debug.Log("? 時間停止");

        // 停止時間（現実時間でカウント）
        yield return new WaitForSecondsRealtime(stopDuration);

        // フェードを戻す
        StartCoroutine(FadeOverlay(0f));

        // 再開
        Time.timeScale = 1f;
        Debug.Log("? 時間再開");

        isTimeStopped = false;
        isOnCooldown = true;

        yield return new WaitForSecondsRealtime(cooldown);
        isOnCooldown = false;
    }

    private IEnumerator FadeOverlay(float target)
    {
        if (overlayImage == null) yield break;

        Color c = overlayImage.color;
        float start = c.a;

        while (Mathf.Abs(c.a - target) > 0.01f)
        {
            c.a = Mathf.MoveTowards(c.a, target, fadeSpeed * Time.unscaledDeltaTime);
            overlayImage.color = c;
            yield return null;
        }
    }
}
