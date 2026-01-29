using UnityEngine;

public class PlayerAnimatorBridge : MonoBehaviour
{
    public Transform visual;    // PlayerVisual
    public Animator animator;   // PlayerVisual ÇÃ Animator
    public Rigidbody2D rb;      // ÉvÉåÉCÉÑÅ[ÇÃRB
    public PlayerOnPlatform onPlatform; 

    void Reset()
    {
        visual = transform.Find("PlayerVisual");
        if (visual) animator = visual.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!animator || !rb) return;

        float speedX = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", speedX);

        bool grounded = (onPlatform != null && onPlatform.IsGrounded)
                        || Mathf.Abs(rb.velocity.y) < 0.01f; 
        animator.SetBool("IsGrounded", grounded);

        // å¸Ç´
        if (speedX > 0.01f)
        {
            var s = transform.localScale;
            s.x = rb.velocity.x >= 0 ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

    public void PlayMelee() => animator?.SetBool("IsAttacking", true);
    public void EndMelee() => animator?.SetBool("IsAttacking", false);
    public void TriggerThrow() => animator?.SetTrigger("IsThrow");
}
