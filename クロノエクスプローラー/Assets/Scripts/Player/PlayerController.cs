using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;    // 通常移動速度
    public float dashSpeed;    // ダッシュ速度
    public float jumpPower;    // ジャンプ力

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ?? 水平入力（時間停止中でも動けるように unscaledDeltaTime は不要）
        float move = Input.GetAxisRaw("Horizontal");
        float speed = Input.GetKey(KeyCode.LeftShift) ? dashSpeed : moveSpeed;

        // ?? 水平方向の速度だけ更新
        Vector2 velocity = rb.velocity;
        velocity.x = move * speed;
        rb.velocity = velocity;

        // ?? Space でジャンプ（地面にいる時のみ）
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            foreach (var contact in col.contacts)
            {
                // ? 壁張り付き防止：接触角度が下方向（ほぼ真下）だけ有効にする
                if (contact.normal.y > 0.7f)
                {
                    isGrounded = true;
                    return;
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            bool grounded = false;

            foreach (var contact in col.contacts)
            {
                // ? 接触面が下方向（地面）なら接地中
                if (contact.normal.y > 0.7f)
                {
                    grounded = true;
                }

                // ? 横方向に押しつけている場合は、X方向の速度を少し減らす
                if (Mathf.Abs(contact.normal.x) > 0.7f)
                {
                    Vector2 v = rb.velocity;
                    v.x *= 0.5f; // ← 壁に押しつけたときの粘りを減らす
                    rb.velocity = v;
                }
            }

            isGrounded = grounded;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
