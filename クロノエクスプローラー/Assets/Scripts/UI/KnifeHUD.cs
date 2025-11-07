// KnifeHUD.cs

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

    // ==== 追加: ロック中パルス設定 ====
    [Header("// Lock Pulse")]
    [Tooltip("ロック中の拡大縮小の中心倍率")]
    public float lockPulseBaseScale = 1.0f;
    [Tooltip("拡大縮小の振れ幅（例: 0.08 なら 1.0±0.08）")]
    public float lockPulseAmplitude = 0.08f;
    [Tooltip("1秒あたりの鼓動速度")]
    public float lockPulseSpeed = 1.2f;

    // ==== 追加: 解除フラッシュ設定 ====
    [Header("// Unlock Flash")]
    [Tooltip("ロック解除時にフラッシュさせるか")]
    public bool enableUnlockFlash = true;
    [Tooltip("フラッシュの時間(秒)")]
    public float unlockFlashDuration = 0.20f;
    [Tooltip("アイコンのポップ倍率（0.1 = 10%拡大）")]
    public float unlockIconPop = 0.12f;

    // ==== 追加: カウントPOP設定 ====
    [Header("// Count Pop")]
    [Tooltip("残弾が増減した瞬間にCountTextをポップ表示")]
    public bool enableCountPop = true;
    [Tooltip("拡大率（0.18なら18%拡大）")]
    public float countPopScale = 0.18f;
    [Tooltip("拡大にかける時間")]
    public float countPopUpDuration = 0.08f;
    [Tooltip("元に戻す時間")]
    public float countPopDownDuration = 0.12f;


    int prevCount = -1;               // ★ 追加：前フレームの所持数
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

        // --- 残弾表示 & 変更検知 ---
        int cur = rangedAttack.GetCurrentKnives();     // ★ 現在の本数を取得
        if (countText) countText.text = "×" + cur;

        if (enableCountPop && prevCount >= 0 && cur != prevCount)
        {
            if (countPopCo != null) StopCoroutine(countPopCo);
            countPopCo = StartCoroutine(CountPopCo()); // ★ 変更時にポップ
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
                // === パルス ===
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
                // === 解除時にスケールを元へ ===
                if (wasEmpty) lockImage.transform.localScale = lockImageOriginalScale;
            }
            if (knifeIcon) knifeIcon.color = Color.white;
        }

        // ==== ここで遷移を検知：「直前はEmpty」→「今はEmptyでない」＝解除瞬間 ====
        if (enableUnlockFlash && wasEmpty && !isEmpty)
        {
            if (flashCo != null) StopCoroutine(flashCo);
            flashCo = StartCoroutine(UnlockFlashCo());   // ★ フラッシュ開始
        }

        prevCount = cur; // 最後に更新
        wasEmpty = isEmpty;
    }

    // ==== 追加: 解除フラッシュのコルーチン（unscaledTimeで駆動） ====
    System.Collections.IEnumerator UnlockFlashCo()
    {
        // 開始時に fill を一瞬だけ満タンにしてから（演出強調用、不要なら削除OK）
        if (cooldownFill) cooldownFill.fillAmount = 1f;

        float t = 0f;
        // 元の色/スケールを保持
        Color fromCol = normalColor;
        Color toCol = Color.white; // 白へフラッシュ
        Vector3 iconFrom = iconOriginalScale;
        Vector3 iconTo = iconOriginalScale * (1f + unlockIconPop);

        while (t < unlockFlashDuration)
        {
            float u = t / unlockFlashDuration;
            // イーズアウト（鋭く光ってすぐ戻る）
            float k = 1f - (1f - u) * (1f - u); // easeOutQuad

            if (cooldownFill)
                cooldownFill.color = Color.Lerp(fromCol, toCol, k);

            if (knifeIcon)
                knifeIcon.transform.localScale = Vector3.Lerp(iconFrom, iconTo, k);

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        // 戻す
        if (cooldownFill) cooldownFill.color = normalColor;

        // アイコンは少し気持ちよく戻す（短くリラックス）
        float back = unlockFlashDuration * 0.6f;
        t = 0f;
        while (t < back)
        {
            float u = t / back;
            // イーズイン（ふわっと戻る）
            float k = u * u; // easeInQuad
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

        // Up（鋭く）
        float t = 0f;
        Vector3 from = countOriginalScale;
        Vector3 to = countOriginalScale * (1f + countPopScale);
        while (t < countPopUpDuration)
        {
            float u = t / countPopUpDuration;
            // easeOutQuad
            float k = 1f - (1f - u) * (1f - u);
            rt.localScale = Vector3.Lerp(from, to, k);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        rt.localScale = to;

        // Down（ふわっと戻す）
        t = 0f;
        float down = countPopDownDuration;
        while (t < down)
        {
            float u = t / down;
            // easeInQuad
            float k = u * u;
            rt.localScale = Vector3.Lerp(to, countOriginalScale, k);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        rt.localScale = countOriginalScale;
        countPopCo = null;
    }
}

