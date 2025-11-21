using UnityEngine;

/// <summary>
/// 敵：左右往復移動（時間停止対応）
/// ・Rigidbody2D を Dynamic + 重力で地面を歩く
/// ・Player に触れたら PlayerDeath.Die() を呼ぶ
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 2.5f;            // // 地面上の水平移動スピード
    public float moveDistance = 3f;           // // 往復距離（中心からの距離）

    [Header("見た目")]
    [SerializeField] Transform visual;        // // 見た目（Sprite/Animator の子）
    public bool spriteFacesRight = true;      // // スプライトの素の向き（右＝true, 左＝false）

    // ==== 内部状態 ====
    Vector3 startPos;                         // // 往復の基準点
    int moveDir = 1;                          // // 1=右, -1=左
    Rigidbody2D rb;                           // // 物理本体
    float baseScaleX = 1f;                    // // 見た目の基準スケールX
    bool isDead = false;                      // // 将来拡張用（死亡フラグ）

    void Awake()
    {
        // // 物理本体を取得
        rb = GetComponent<Rigidbody2D>();

        // // Rigidbody2D の推奨設定（重力で歩かせる）
        rb.bodyType = RigidbodyType2D.Dynamic;         // // Dynamic にする
        rb.gravityScale = 2.0f;                        // // 好みで 1.5～3.0
        rb.freezeRotation = true;                      // // ひっくり返らない
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        // // Visual 自動取得＆基準スケール保持
        if (!visual) visual = transform.Find("Visual");
        if (visual) baseScaleX = Mathf.Abs(visual.localScale.x);
    }

    void Start()
    {
        // // 基準点を記録
        startPos = transform.position;

        // // 初期の向きを適用
        ApplyFacingByDir();
    }

    void Update()
    {
        // // 時止め or 死亡中は停止
        if (isDead || TimeStopController.isStopped)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        // // 水平移動：Y 速度は物理（重力/落下）に任せる
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);

        // // 往復距離に達したら向きを変える
        if (Vector2.Distance(transform.position, startPos) >= moveDistance)
        {
            moveDir *= -1;
            startPos = transform.position;
        }

        // // 速度から見た目の向きを合わせる
        ApplyFacingByVelocity();
    }

    /// <summary>
    /// // 目的：現在の速度/方向に基づいて見た目(Visual)のみ左右反転
    /// </summary>
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

    /// <summary>
    /// // 目的：開始直後など速度が0でも向きを適用
    /// </summary>
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
    /// // 目的：当たった相手が Player なら PlayerDeath.Die() を確実に呼ぶ
    /// </summary>
    void TryKillPlayer(Collider2D col)
    {
        if (!col) return;
        if (TimeStopController.isStopped) return;      // // 仕様：時止め中は無効

        // // まずタグ判定（ルートが Player でない可能性に備えて親も見る）
        bool isPlayerTag = col.CompareTag("Player") ||
                           (col.attachedRigidbody && col.attachedRigidbody.CompareTag("Player")) ||
                           (col.transform.root.CompareTag("Player"));

        if (!isPlayerTag) return;

        // // 子に当たっても親の PlayerDeath を拾えるよう「親まで」検索
        var death = col.GetComponentInParent<PlayerDeath>();
        if (death != null)
        {
            death.Die();                               // // ゲームオーバー演出へ
        }
        else
        {
            // // デバッグ補助：見つからない場合は一度ログで場所を確認
            Debug.LogWarning($"PlayerDeath が見つかりません: hit={col.name}", this);
        }
    }
}
