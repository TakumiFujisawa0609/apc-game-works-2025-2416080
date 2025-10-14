using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 3f;
    public float dashSpeed = 6f;
    public float jumpPower = 12f;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 横移動
        float move = Input.GetAxisRaw("Horizontal");
        float speed = Input.GetKey(KeyCode.LeftShift) ? dashSpeed : moveSpeed;

        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面判定（Groundタグのオブジェクトに触れたら）
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
