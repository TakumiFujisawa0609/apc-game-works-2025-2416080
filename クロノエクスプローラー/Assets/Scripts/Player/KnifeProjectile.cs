using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class KnifeProjectile : MonoBehaviour
{
    [Header("// 寿命（距離＋保険の時間）")]
    public float lifeTime = 5f;
    public float maxDistance = 10f;

    [Header("// 攻撃")]
    public float damage = 0.5f;

    [Header("// デバッグ/堅牢化")]
    public bool maintainSpeed = true;    // // 他スクリプトが速度0にしても上書き維持
    public bool logSpawnState = true;    // // 生成直後の状態を数フレーム可視化

    Rigidbody2D rb;
    Vector3 startPos;
    Vector2 savedVelocity;
    float lifeTimer;
    bool initialized;
    int logFrames = 8;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        var col = GetComponent<Collider2D>();
        col.isTrigger = true; // // 物理衝突で止まらないように
    }

    void OnEnable()
    {
        // // 生成直後に外部から触られていても初期状態を強制
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
    }

    // dir: +1/-1, speed: 初速
    public void Init(float dir, float speed)
    {
        startPos = transform.position;
        savedVelocity = new Vector2(dir * speed, 0f);
        rb.velocity = savedVelocity;
        rb.gravityScale = 0f;
        initialized = true;

        if (dir < 0f)
        {
            var s = transform.localScale; s.x = -Mathf.Abs(s.x); transform.localScale = s;
        }

        if (TimeStopController.isStopped)
        {
            rb.simulated = false; // // 時止め中は完全停止
        }
    }

    void Update()
    {
        // // 外部から誤って変えられても無効化
        if (rb.gravityScale != 0f) rb.gravityScale = 0f;

        if (!TimeStopController.isStopped)
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeTime) { Destroy(gameObject); return; }
        }

        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        { Destroy(gameObject); return; }

        // 生成直後の状態ログ（原因特定用）
        if (logSpawnState && logFrames-- > 0)
        {
            Debug.Log($"[Knife] t={Time.time:F3} vel={rb.velocity} g={rb.gravityScale} sim={rb.simulated} kin?={rb.IsSleeping()}");
        }
    }

    void FixedUpdate()
    {
        if (!initialized) return;

        if (TimeStopController.isStopped)
        {
            if (rb.simulated && rb.velocity != Vector2.zero) savedVelocity = rb.velocity;
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }
        else
        {
            if (!rb.simulated) { rb.simulated = true; }

            // === ここが“即効パッチ” ===
            // 何かに0にされても、常に発射速度を維持する
            if (maintainSpeed && rb.velocity.sqrMagnitude < (savedVelocity.sqrMagnitude * 0.99f))
            {
                rb.velocity = savedVelocity;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !TimeStopController.isStopped)
        {
            var eh = other.GetComponent<EnemyHealth>();
            if (eh) eh.TakeDamage(damage);
            SfxPlayer.Play2D(SfxKey.KnifeHit);
            Destroy(gameObject);
        }
    }
}
