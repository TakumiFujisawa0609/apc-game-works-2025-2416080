using System.Collections;
using UnityEngine;

/// <summary>
/// 近接攻撃：AnimatorのAttackトリガーを叩く →
/// アニメイベント( HitboxOn/HitboxOff )で当たりON/OFF。
/// ※ イベント未設定なら activeTime のタイマー方式でフォールバック
/// </summary>
public class MeleeAttack : MonoBehaviour
{
    [Header("参照")]
    public GameObject hitbox;            // 当たり判定 (IsTrigger の Collider2D を付ける)
    public Transform playerVisual;       // Player/PlayerVisual
    public PlayerController player;      // 向き取得用（GetFacingDir）
    [SerializeField] string attackTrigger = "Attack1";


    [Header("調整")]
    public float activeTime = 0.15f;     // フォールバック用の有効秒
    public Vector2 rightOffset = new Vector2(0.48f, 0f);
    public Vector2 leftOffset = new Vector2(-0.48f, 0f);
    public float attackCooldown = 0.2f;  // 連打抑制

    Animator anim;
    bool isAttacking = false;            // タイマー方式のフラグ
    float timer = 0f;
    bool canAttack = true;

    void Awake()
    {
        if (!playerVisual) playerVisual = transform.Find("PlayerVisual");
        if (!player) player = GetComponent<PlayerController>();
        if (playerVisual) anim = playerVisual.GetComponent<Animator>();
        if (hitbox) hitbox.SetActive(false);
    }

    void Update()
    {
        // --- フォールバック（イベント未使用ならONからactiveTimeで自動OFF） ---
        if (isAttacking)
        {
            timer += Time.deltaTime;
            if (timer >= activeTime) EndAttack_Internal();
        }
    }

    /// <summary>入力から呼ぶ：攻撃開始</summary>
    public void DoMelee()
    {
        if (!canAttack || anim == null) return;

        // 攻撃モーション（Attack Trigger）
        anim.ResetTrigger(attackTrigger);
        anim.SetTrigger(attackTrigger);

        // SFX（振り音）
        SfxPlayer.Play2D(SfxKey.MeleeSwing);

        // もしアニメイベント未設定なら即ONしてタイマーでOFF
        if (hitbox && !HasAnimationEvents())
        {
            HitboxOn();                      // 位置合わせしてON
            timer = 0f;
            isAttacking = true;
        }

        // 連打抑制
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // ===== アニメーションイベントから呼ぶ関数 =====
    public void HitboxOn()
    {
        if (!hitbox) return;

        // 向きに合わせてローカル位置をスワップ
        float dir = player ? player.GetFacingDir() : 1f;
        hitbox.transform.localPosition = (dir >= 0f) ? rightOffset : leftOffset;

        hitbox.SetActive(true);
        isAttacking = false; // イベント駆動に切り替わったのでタイマーは無効
    }

    public void HitboxOff()
    {
        EndAttack_Internal();
    }

    // 内部終了処理（イベント/フォールバック共通）
    void EndAttack_Internal()
    {
        if (hitbox) hitbox.SetActive(false);
        isAttacking = false;
        timer = 0f;
    }

    // 現在のAttackクリップにイベントがあるかの簡易判定（無ければフォールバック）
    bool HasAnimationEvents()
    {
        // Animator から現在の再生ステート情報を見て Attack かどうか等を厳密判定してもOK。
        // ここでは簡易に true を返さず、フォールバック起動条件を
        // 「イベントが一度でも呼ばれたら isAttacking を false にする」で吸収しています。
        return false;
    }
}
