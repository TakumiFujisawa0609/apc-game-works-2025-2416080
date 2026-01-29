
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KnifeHUD : MonoBehaviour
{
    [Header("// 参照")]
    public RangedAttack rangedAttack;
    public Image cooldownFill;
    public TMP_Text countText;
    public Image knifeIcon;
    public GameObject lockImage;

    [Header("// 色設定")]
    public Color normalColor = new Color(0f, 1f, 0.85f, 0.9f);
    public Color lockColor = new Color(0.6f, 0.6f, 0.6f, 0.9f);

    [Header("// Lock Pulse")]
    [Tooltip("ロック中の拡大縮小の中心倍率")]
    public float lockPulseBaseScale = 1.0f;
    [Tooltip("拡大縮小の振れ幅（例: 0.08 なら 1.0±0.08）")]
    public float lockPulseAmplitude = 0.08f;
    [Tooltip("1秒あたりの鼓動速度")]
    public float lockPulseSpeed = 1.2f;

    [Header("// Unlock Flash")]
    [Tooltip("ロック解除時にフラッシュさせるか")]
    public bool enableUnlockFlash = true;
    [Tooltip("フラッシュの時間(秒)")]
    public float unlockFlashDuration = 0.20f;
    [Tooltip("アイコンのポップ倍率（0.1 = 10%拡大）")]
    public float unlockIconPop = 0.12f;

    [Header("// Count Pop")]
    [Tooltip("残弾が増減した瞬間にCountTextをポップ表示")]
    public bool enableCountPop = true;
    [Tooltip("拡大率（0.18なら18%拡大）")]
    public float countPopScale = 0.18f;
    [Tooltip("拡大にかける時間")]
    public float countPopUpDuration = 0.08f;
    [Tooltip("元に戻す時間")]
    public float countPopDownDuration = 0.12f;


    int prevCount = -1;               // 前フレームの所持数
    Vector3 countOriginalScale = Vector3.one;
    Coroutine countPopCo;
    Vector3 lockImageOriginalScale;   // 元のスケールを保持
    Vector3 iconOriginalScale = Vector3.one;
    bool wasEmpty;                    // 前フレームの状態

    Coroutine flashCo;

    void Awake()
    {
        if (lockImage != null) lockImageOriginalScale = lockImage.transform.localScale;
        if (knifeIcon != null) iconOriginalScale = knifeIcon.transform.localScale;
        if (countText != null) countOriginalScale = countText.rectTransform.localScale;
    }

    void Reset()
    {
        if (!rangedAttack) rangedAttack = FindObjectOfType<RangedAttack>();
    }

    void Update()
    {
        if (rangedAttack == null) return;

        int cur = rangedAttack.GetCurrentKnives();     // 現在の本数を取得
        if (countText) countText.text = "×" + cur;

        if (enableCountPop && prevCount >= 0 && cur != prevCount)
        {
            if (countPopCo != null) StopCoroutine(countPopCo);
            countPopCo = StartCoroutine(CountPopCo()); // 変更時にポップ
        }


        // 残弾テキスト
        if (countText) countText.text = "×" + rangedAttack.GetCurrentKnives();

        bool isEmpty = rangedAttack.IsEmptyReload();

        if (isEmpty)
        {
            float p = rangedAttack.GetEmptyReloadProgress(); // 0→1
            if (cooldownFill)
            {
                cooldownFill.fillAmount = p;
                cooldownFill.color = lockColor;
            }
            if (lockImage)
            {
                if (!lockImage.activeSelf) lockImage.SetActive(true);

                float t = Time.unscaledTime * lockPulseSpeed;
                float s = lockPulseBaseScale + Mathf.Sin(t * Mathf.PI * 2f) * lockPulseAmplitude;
                lockImage.transform.localScale = lockImageOriginalScale * s;
            }
            if (knifeIcon) knifeIcon.color = Color.gray;
        }
        else
        {
            float p = rangedAttack.GetNormalRefillProgress(); // 0→1
            if (cooldownFill)
            {
                cooldownFill.fillAmount = p;
                cooldownFill.color = normalColor;
            }
            if (lockImage)
            {
                if (lockImage.activeSelf) lockImage.SetActive(false);

                if (wasEmpty) lockImage.transform.localScale = lockImageOriginalScale;
            }
            if (knifeIcon) knifeIcon.color = Color.white;
        }

        if (enableUnlockFlash && wasEmpty && !isEmpty)
        {
            if (flashCo != null) StopCoroutine(flashCo);
            flashCo = StartCoroutine(UnlockFlashCo());   // フラッシュ開始
        }

        prevCount = cur; // 最後に更新
        wasEmpty = isEmpty;
    }
    System.Collections.IEnumerator UnlockFlashCo()
    {
        if (cooldownFill) cooldownFill.fillAmount = 1f;

        float t = 0f;
        Color fromCol = normalColor;
        Color toCol = Color.white; // 白へフラッシュ
        Vector3 iconFrom = iconOriginalScale;
        Vector3 iconTo = iconOriginalScale * (1f + unlockIconPop);

        while (t < unlockFlashDuration)
        {
            float u = t / unlockFlashDuration;
            float k = 1f - (1f - u) * (1f - u); 

            if (cooldownFill)
                cooldownFill.color = Color.Lerp(fromCol, toCol, k);

            if (knifeIcon)
                knifeIcon.transform.localScale = Vector3.Lerp(iconFrom, iconTo, k);

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        if (cooldownFill) cooldownFill.color = normalColor;

        float back = unlockFlashDuration * 0.6f;
        t = 0f;
        while (t < back)
        {
            float u = t / back;
            float k = u * u;
            if (knifeIcon)
                knifeIcon.transform.localScale = Vector3.Lerp(iconOriginalScale * (1f + unlockIconPop), iconOriginalScale, k);

            t += Time.unscaledDeltaTime;
            yield return null;
        }
        if (knifeIcon) knifeIcon.transform.localScale = iconOriginalScale;

        flashCo = null;
    }
    System.Collections.IEnumerator CountPopCo()
    {
        if (countText == null) yield break;
        var rt = countText.rectTransform;

        float t = 0f;
        Vector3 from = countOriginalScale;
        Vector3 to = countOriginalScale * (1f + countPopScale);
        while (t < countPopUpDuration)
        {
            float u = t / countPopUpDuration;
            float k = 1f - (1f - u) * (1f - u);
            rt.localScale = Vector3.Lerp(from, to, k);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        rt.localScale = to;

        t = 0f;
        float down = countPopDownDuration;
        while (t < down)
        {
            float u = t / down;
            float k = u * u;
            rt.localScale = Vector3.Lerp(to, countOriginalScale, k);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        rt.localScale = countOriginalScale;
        countPopCo = null;
    }
}

