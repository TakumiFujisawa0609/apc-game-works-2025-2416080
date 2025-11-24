using UnityEngine;

/// <summary>
/// 敵：左右往復移動（時間停止対応）
/// ・歩行型（重力あり）/ 浮遊型（重力なし）をスイッチ
/// ・Player に触れたら PlayerDeath.Die() を呼ぶ
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    // ====== 基本移動設定 ======
    [Header("移動設定")]
    public float moveSpeed = 2.5f;            // // 目的：水平移動スピード
    public float moveDistance = 3f;           // // 目的：往復距離（中心からの距離）

    // ====== 見た目（左右反転） ======
    [Header("見た目")]
    [SerializeField] Transform visual;        // // 目的：Sprite/Animator の子を割当
    public bool spriteFacesRight = true;      // // 目的：スプライトの素の向き（右= true）

    // ====== 物理モード切替 ======
    [Header("物理モード")]
    public bool useGravity = true;            // // 目的：true=地上歩行 / false=浮遊
    [Range(0f, 10f)]
    public float gravityScaleWhenGround = 2f; // // 目的：歩行時の重力
    public bool lockYWhenFloating = true;     // // 目的：浮遊時にYを固定するか
    public float floatingYOffset = 0f;        // // 目的：開始位置からの微調整

    // ====== 内部状態 ======
    Vector3 startPos;                         // // 目的：往復の基準点（いま居る位置）
    int moveDir = 1;                          // // 目的：1=右, -1=左
    Rigidbody2D rb;                           // // 目的：物理本体
    float baseScaleX = 1f;                    // // 目的：見た目の基準スケールX
    bool isDead = false;                      // // 目的：将来拡張用（死亡フラグ）
    float hoverY;                             // // 目的：浮遊時に維持する高さ（開始時に確定）

    void Awake()
    {
        // // 物理本体を取得
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;                              // // 目的：ひっくり返らない
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        // // Visual 自動取得＆基準スケール保持
        if (!visual) visual = transform.Find("Visual");
        if (visual) baseScaleX = Mathf.Abs(visual.localScale.x);

        // // 目的：Inspectorの設定に応じて Rigidbody を初期化
        ApplyPhysicsModeImmediate();
    }

    void Start()
    {
        // // 基準点と浮遊高さを記録
        startPos = transform.position;
        hoverY = transform.position.y + floatingYOffset;

        // // 初期の向きを適用
        ApplyFacingByDir();
    }

    void Update()
    {
        // // 目的：時止め/死亡時は水平速度だけ止める（落下は維持）
        if (isDead || TimeStopController.isStopped)
        {
            if (useGravity)
            {
                // // 目的：歩行型は水平だけ止めて、重力のYは生かす
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
            else
            {
                // // 目的：浮遊型は完全停止＋位置固定
                rb.velocity = Vector2.zero;
                if (lockYWhenFloating) KeepHoverY();
            }
            return;
        }

        // // 目的：共通の往復制御（範囲到達で方向反転）
        if (Vector2.Distance(transform.position, startPos) >= moveDistance)
        {
            moveDir *= -1;
            startPos = transform.position;
        }

        if (useGravity)
        {
            // // 目的：歩行型＝重力に任せつつ水平だけ与える
            rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
        }
        else
        {
            // // 目的：浮遊型＝Y速度は常に0、Xだけ等速で動く
            rb.velocity = new Vector2(moveDir * moveSpeed, 0f);
            if (lockYWhenFloating) KeepHoverY();
        }

        // // 目的：速度または意図方向に見た目を合わせる
        ApplyFacingByVelocity();
    }

    // ----------------------------------------------------------------------
    // 目的：Inspector 変更を即 Rigidbody に反映（エディタ上でも便利）
    // ----------------------------------------------------------------------
    void OnValidate()
    {
        if (!Application.isPlaying)          // ← UnityEngine.Application を使用
        {
            if (!rb) rb = GetComponent<Rigidbody2D>();
            ApplyPhysicsModeImmediate();
        }
    }

    /// <summary>目的：Rigidbody2D を歩行型/浮遊型に切替える</summary>
    void ApplyPhysicsModeImmediate()
    {
        if (!rb) return;

        if (useGravity)
        {
            // // 目的：歩行型＝Dynamic + 重力
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = gravityScaleWhenGround;
            rb.interpolation = RigidbodyInterpolation2D.None;
        }
        else
        {
            // // 目的：浮遊型＝Kinematic + 重力0（衝突はする）
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.velocity = Vector2.zero;
        }
    }

    /// <summary>目的：浮遊型でY位置を固定する</summary>
    void KeepHoverY()
    {
        var p = rb.position;
        p.y = hoverY;
        rb.MovePosition(p);
    }

    /// <summary>目的：現在の速度/方向に基づいて見た目(Visual)のみ左右反転</summary>
    void ApplyFacingByVelocity()
    {
        if (!visual) return;

        // // 右を向かせたいか？（停止時は moveDir を採用）
        bool wantRight = Mathf.Abs(rb.velocity.x) > 0.01f ? (rb.velocity.x > 0f) : (moveDir > 0);
        int artSign = spriteFacesRight ? 1 : -1;
        int wantSign = wantRight ? 1 : -1;

        var s = visual.localScale;
        s.x = baseScaleX * (wantSign * artSign);
        visual.localScale = s;
    }

    /// <summary>目的：開始直後など速度が0でも向きを適用</summary>
    void ApplyFacingByDir()
    {
        if (!visual) return;
        int artSign = spriteFacesRight ? 1 : -1;
        int wantSign = (moveDir >= 0) ? 1 : -1;
        var s = visual.localScale;
        s.x = baseScaleX * (wantSign * artSign);
        visual.localScale = s;
    }

    // ================= 衝突でプレイヤーを Kill =================

    // // 目的：非Trigger の物理衝突で検知
    void OnCollisionEnter2D(Collision2D c) { TryKillPlayer(c.collider); }

    // // 目的：Trigger で検知（どちらかが IsTrigger の場合）
    void OnTriggerEnter2D(Collider2D c) { TryKillPlayer(c); }

    /// <summary>
    /// 目的：当たった相手が Player なら PlayerDeath.Die() を確実に呼ぶ
    ///        （攻撃コライダーは除外）
    /// </summary>
    void TryKillPlayer(Collider2D col)
    {
        if (!col) return;
        if (TimeStopController.isStopped) return;  // // 仕様：時止め中は無効

        // --- 攻撃コライダーの早期リターン -----------------------
        if (col.GetComponent<MeleeHitbox>() != null) return;
        if (col.GetComponent<KnifeProjectile>() != null) return;
        int playerAttackLayer = LayerMask.NameToLayer("PlayerAttack");
        if (playerAttackLayer >= 0 && col.gameObject.layer == playerAttackLayer) return;
        // -------------------------------------------------------

        // --- プレイヤー本体かどうか判定 ------------------------
        PlayerController bodyOwner = col.GetComponent<PlayerController>();
        if (!bodyOwner && col.attachedRigidbody)
            bodyOwner = col.attachedRigidbody.GetComponent<PlayerController>();
        if (!bodyOwner) return;
        // -------------------------------------------------------

        // --- キル実行 ------------------------------------------
        var death = bodyOwner.GetComponent<PlayerDeath>();
        if (death != null) death.Die();
        // -------------------------------------------------------
    }
}
