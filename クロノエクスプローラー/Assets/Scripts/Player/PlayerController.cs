using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed;
    public float dashSpeed;
    public float jumpPower;

    [Header("入力（ダッシュ")]
    public KeyCode dashKey = KeyCode.LeftShift;             
    public KeyCode padDashKey = KeyCode.JoystickButton5;    

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private SpriteRenderer sr;

    [SerializeField] Animator anim;
    [SerializeField] Transform playerVisual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;             
        transform.localScale = Vector3.one;   
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        bool isDash = Input.GetKey(dashKey) || Input.GetKey(padDashKey);

        float speed = isDash ? dashSpeed : moveSpeed;

        Vector2 v = rb.velocity;
        v.x = move * speed;
        rb.velocity = v;
    }

    private void HandleJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || GamepadInput.RTDown()) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
            SfxPlayer.Play2D(SfxKey.Jump);
            isGrounded = false;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("MoveFloor"))
        {
            bool grounded = false;
            foreach (var c in col.contacts)
                if (c.normal.y > 0.85f) { grounded = true; break; }
            isGrounded = grounded;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("MoveFloor"))
            isGrounded = false;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!playerVisual) playerVisual = transform.Find("PlayerVisual");
        if (playerVisual) anim = playerVisual.GetComponent<Animator>();
    }

    public float GetFacingDir()
    {
        if (sr) return sr.flipX ? -1f : 1f;

        return transform.localScale.x >= 0f ? 1f : -1f;

    }

}