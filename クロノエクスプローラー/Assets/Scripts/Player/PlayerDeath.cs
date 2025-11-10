using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤー死亡演出の単一責務クラス
/// ・敵に触れた等で Die() を呼ぶ
/// ・入力/物理/当たりを止める → Deathモーション → フェードでGameOverへ
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] Transform playerVisual;   // Player/PlayerVisual をドラッグ
    [SerializeField] Animator anim;            // 自動取得
    [SerializeField] Rigidbody2D rb;           // 自動取得

    [Header("アニメ再生")]
    public string deathTrigger = "Death";    // Animator Trigger 名（なければ空に）
    public string deathStateName = "Death";    // 直接再生用のステート名（保険）

    [Header("遷移設定")]
    public float gameOverDelay = 1.0f;         // モーション後に待つ秒数
    public string gameOverSceneName = "GameOverScene";

    [Header("当たり停止")]
    public bool disableCollidersOnDeath = true;

    bool isDead;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!playerVisual) playerVisual = transform.Find("PlayerVisual");
        if (playerVisual && !anim) anim = playerVisual.GetComponent<Animator>();
    }

    // 外部（敵など）から呼ばれる
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // 入力/行動系を止める
        var pc = GetComponent<PlayerController>(); if (pc) pc.enabled = false;
        var pa = GetComponent<PlayerAttack>(); if (pa) pa.enabled = false;
        var ma = GetComponent<MeleeAttack>(); if (ma) ma.enabled = false;
        var ra = GetComponent<RangedAttack>(); if (ra) ra.enabled = false;
        var pop = GetComponent<PlayerOnPlatform>(); if (pop) pop.enabled = false;

        // 物理停止
        if (rb)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.simulated = false;
        }

        // これ以上の当たりを無効化
        if (disableCollidersOnDeath)
        {
            foreach (var col in GetComponentsInChildren<Collider2D>())
                col.enabled = false;
        }

        if (anim)
        {
            // 念のため：timeScaleに影響されないように（保険）
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;

            bool played = false;

            if (!string.IsNullOrEmpty(deathTrigger))
            {
                // パラメータが存在するかチェックしてから使う
                foreach (var p in anim.parameters)
                {
                    if (p.type == AnimatorControllerParameterType.Trigger && p.name == deathTrigger)
                    {
                        anim.ResetTrigger(deathTrigger);
                        anim.SetTrigger(deathTrigger);
                        played = true;
                        break;
                    }
                }
            }

            // トリガー名が無い/違う or 遷移が無い場合でも、ステート名で強制再生
            if (!played && !string.IsNullOrEmpty(deathStateName))
            {
                anim.CrossFade(deathStateName, 0f, 0); // レイヤー0へ即切替
            }
        }

        // SE等あればここで
        // SfxPlayer.Play2D(SfxKey.PlayerDeath);

        StartCoroutine(CoGameOver());
    }

    IEnumerator CoGameOver()
    {
        // ゲームの時間停止に影響されない待機
        float t = 0f;
        while (t < gameOverDelay) { t += Time.unscaledDeltaTime; yield return null; }

        // SceneFader優先（なければ直ロード）
        var fader = FindObjectOfType<SceneFader>();
        if (fader != null)
        {
            // あなたの実装に合わせて：FadeOut(string) がある想定
            fader.FadeOut(gameOverSceneName);
        }
        else
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}
