using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float dashSpeed = 6f;
    public float jumpPower = 12f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ? 時間停止中でも動けるように unscaledDeltaTime を使用
        float move = Input.GetAxisRaw("Horizontal");
        float speed = Input.GetKey(KeyCode.LeftShift) ? dashSpeed : moveSpeed;

        // Rigidbody の速度を直接設定する（時間の影響を受けない）
        Vector2 velocity = rb.velocity;
        velocity.x = move * speed;
        rb.velocity = velocity;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
