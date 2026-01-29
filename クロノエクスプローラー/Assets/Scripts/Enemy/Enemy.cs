using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 2.5f;          
    public float moveDistance = 3f;           

    [Header("見た目")]
    [SerializeField] Transform visual;       
    public bool spriteFacesRight = true;      

    [Header("物理モード")]
    public bool useGravity = true;            
    [Range(0f, 10f)]
    public float gravityScaleWhenGround = 2f; 
    public bool lockYWhenFloating = true;    
    public float floatingYOffset = 0f;        

    Vector3 startPos;                        
    int moveDir = 1;                          
    Rigidbody2D rb;                           
    float baseScaleX = 1f;                    
    bool isDead = false;                      
    float hoverY;                            

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;                              
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        if (!visual) visual = transform.Find("Visual");
        if (visual) baseScaleX = Mathf.Abs(visual.localScale.x);

        ApplyPhysicsModeImmediate();
    }

    void Start()
    {
        startPos = transform.position;
        hoverY = transform.position.y + floatingYOffset;

        ApplyFacingByDir();
    }

    void Update()
    {
        if (isDead || TimeStopController.isStopped)
        {
            if (useGravity)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
            else
            {
                rb.velocity = Vector2.zero;
                if (lockYWhenFloating) KeepHoverY();
            }
            return;
        }

        if (Vector2.Distance(transform.position, startPos) >= moveDistance)
        {
            moveDir *= -1;
            startPos = transform.position;
        }

        if (useGravity)
        {
            rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveDir * moveSpeed, 0f);
            if (lockYWhenFloating) KeepHoverY();
        }

        ApplyFacingByVelocity();
    }

    void OnValidate()
    {
        if (!Application.isPlaying)         
        {
            if (!rb) rb = GetComponent<Rigidbody2D>();
            ApplyPhysicsModeImmediate();
        }
    }

    void ApplyPhysicsModeImmediate()
    {
        if (!rb) return;

        if (useGravity)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = gravityScaleWhenGround;
            rb.interpolation = RigidbodyInterpolation2D.None;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.velocity = Vector2.zero;
        }
    }

    void KeepHoverY()
    {
        var p = rb.position;
        p.y = hoverY;
        rb.MovePosition(p);
    }

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

    void ApplyFacingByDir()
    {
        if (!visual) return;
        int artSign = spriteFacesRight ? 1 : -1;
        int wantSign = (moveDir >= 0) ? 1 : -1;
        var s = visual.localScale;
        s.x = baseScaleX * (wantSign * artSign);
        visual.localScale = s;
    }


    void OnCollisionEnter2D(Collision2D c) { TryKillPlayer(c.collider); }

    void OnTriggerEnter2D(Collider2D c) { TryKillPlayer(c); }

    void TryKillPlayer(Collider2D col)
    {
        if (!col) return;
        if (TimeStopController.isStopped) return; 

        if (col.GetComponent<MeleeHitbox>() != null) return;
        if (col.GetComponent<KnifeProjectile>() != null) return;
        int playerAttackLayer = LayerMask.NameToLayer("PlayerAttack");
        if (playerAttackLayer >= 0 && col.gameObject.layer == playerAttackLayer) return;

        PlayerController bodyOwner = col.GetComponent<PlayerController>();
        if (!bodyOwner && col.attachedRigidbody)
            bodyOwner = col.attachedRigidbody.GetComponent<PlayerController>();
        if (!bodyOwner) return;

        var death = bodyOwner.GetComponent<PlayerDeath>();
        if (death != null) death.Die();
    }
}
