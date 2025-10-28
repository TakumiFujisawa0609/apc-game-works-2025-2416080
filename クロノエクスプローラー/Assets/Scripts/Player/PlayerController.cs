using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed;
    public float dashSpeed;
    public float jumpPower;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");
        float speed = Input.GetButton("Fire3") ? dashSpeed : moveSpeed;

        Vector2 velocity = rb.velocity;
        velocity.x = move * speed;
        rb.velocity = velocity;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButton("Jump"))
            if (isGrounded)
            {
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    isGrounded = false;
                }
            }
    }

    // -------------------------------------------------------------
    // 接地判定：横方向の当たりを除外する改良版
    // -------------------------------------------------------------
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("MoveFloor"))
        {
            bool grounded = false;

            foreach (var contact in col.contacts)
            {
                // 接触角度が下向き（真下から0.85以上）なら接地
                if (contact.normal.y > 0.85f)
                {
                    grounded = true;
                    break;
                }
            }

            isGrounded = grounded;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("MoveFloor"))
        {
            isGrounded = false;
        }
    }
}
