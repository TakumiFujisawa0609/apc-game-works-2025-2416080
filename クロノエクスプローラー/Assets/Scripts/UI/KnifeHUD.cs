using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KnifeHUD : MonoBehaviour
{
    [Header("参照")]
    public RangedAttack rangedAttack;   // プレイヤーのRangedAttack
    public Image cooldownFill;          // Type=Filled, Radial360
    public TMP_Text countText;          // 「×3」
    public Image knifeIcon;             // 中央のナイフ画像
    public GameObject lockImage;        // 0本時に表示

    [Header("色設定")]
    public Color normalColor = new Color(0f, 1f, 0.85f, 0.9f); // ミント
    public Color lockColor = new Color(0.6f, 0.6f, 0.6f, 0.9f); // グレー

    void Reset()
    {
        // シーンに1体だけの想定なら自動参照
        if (!rangedAttack) rangedAttack = FindObjectOfType<RangedAttack>();
    }

    void Update()
    {
        if (!rangedAttack) return;

        // 残弾テキスト
        if (countText)
            countText.text = "×" + rangedAttack.GetCurrentKnives();

        // 0本ロック中かどうか
        bool isEmpty = rangedAttack.IsEmptyReload();

        if (isEmpty)
        {
            // 0本時：12秒ロックの進捗（グレー）
            float p = rangedAttack.GetEmptyReloadProgress(); // 0→1
            if (cooldownFill)
            {
                cooldownFill.fillAmount = p;
                cooldownFill.color = lockColor;
            }
            if (lockImage) lockImage.SetActive(true);
            if (knifeIcon) knifeIcon.color = Color.gray;
        }
        else
        {
            // 1〜2本：3秒の通常クール（ミント）
            float p = rangedAttack.GetNormalRefillProgress(); // 0→1
            if (cooldownFill)
            {
                cooldownFill.fillAmount = p;
                cooldownFill.color = normalColor;
            }
            if (lockImage) lockImage.SetActive(false);
            if (knifeIcon) knifeIcon.color = Color.white;
        }
    }
}
