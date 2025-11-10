using UnityEngine;

/// <summary>
/// 敵：左右往復移動（時間停止対応）
/// 「プレイヤーに触れたら PlayerDeath.Die() を呼ぶ」だけを担当
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 1.5f;     // 移動速度
    public float moveDistance = 3f;    // 往復距離

    Rigidbody2D rb;
    Vector3 pivotPos;
    int moveDir = 1;                   // 1=右, -1=左

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        pivotPos = transform.position;
    }

    void Update()
    {
        if (TimeStopController.isStopped)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // 左右移動
        rb.velocity = new Vector2(moveDir * moveSpeed, 0f);

        // 範囲到達で反転
        if (Vector2.Distance(transform.position, pivotPos) >= moveDistance)
        {
            moveDir *= -1;
            pivotPos = transform.position;
            FlipVisual();
        }
    }

    void FlipVisual()
    {
        var s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    // ---- プレイヤー接触で死亡依頼 ----
    void OnCollisionEnter2D(Collision2D c) { TryKillPlayer(c.collider); }
    void OnTriggerEnter2D(Collider2D c) { TryKillPlayer(c); }

    void TryKillPlayer(Collider2D col)
    {
        if (TimeStopController.isStopped) return;      // 時止め中は無効（仕様）
        if (!col || !col.CompareTag("Player")) return;

        var death = col.GetComponent<PlayerDeath>();
        if (death != null) death.Die();
    }
}
