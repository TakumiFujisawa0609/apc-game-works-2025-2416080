// Assets/Scripts/Enemy/EnemyAnimDriver.cs
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimDriver : MonoBehaviour
{
    public Rigidbody2D rb;        // 親のRigidbody2D（Enemy本体）
    public Transform visualRoot;  // EnemyVisual（このスクリプトをEnemyVisualに付けるなら不要）

    Animator anim;
    bool grounded;

    [Header("接地判定")]
    public LayerMask groundMask;
    public float groundCheckRadius = 0.05f;
    public Vector2 groundCheckOffset = new Vector2(0f, -0.5f);

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (!rb) rb = GetComponentInParent<Rigidbody2D>();
        if (!visualRoot) visualRoot = transform;
    }

    void Update()
    {
        if (!rb || !anim) return;

        // 水平速度で向き反転
        float vx = rb.velocity.x;
        if (Mathf.Abs(vx) > 0.01f)
        {
            var s = visualRoot.localScale;
            s.x = Mathf.Sign(vx) * Mathf.Abs(s.x);
            visualRoot.localScale = s;
        }

        // 接地チェック（円）
        Vector2 c = (Vector2)rb.transform.position + groundCheckOffset;
        grounded = Physics2D.OverlapCircle(c, groundCheckRadius, groundMask);

        // パラメータ更新
        anim.SetFloat("Speed", Mathf.Abs(vx));
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("AirSpeed", rb.velocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (!rb) return;
        Gizmos.color = Color.yellow;
        Vector2 c = (Vector2)rb.transform.position + groundCheckOffset;
        Gizmos.DrawWireSphere(c, groundCheckRadius);
    }
}
