using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnifeProjectile : MonoBehaviour
{
    [Header("寿命")]
    public float lifeTime = 5f;        // 保険での寿命
    public float maxDistance = 10f;    // この距離を超えたら消す（時間停止を挟んでも位置で消える）

    [Header("攻撃")]
    public float damage = 0.5f;        // ← ここを0.5にする

    private Rigidbody2D rb;
    private float currentLife = 0f;
    private Vector3 startPos;
    private Vector2 savedVelocity;
    private bool initialized = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    public void Init(float dir, float speed)
    {
        rb.isKinematic = false;
        rb.velocity = new Vector2(dir * speed, 0f);
        initialized = true;

        startPos = transform.position;

        // 向き合わせ
        if (dir < 0f)
        {
            var s = transform.localScale;
            s.x = -Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

    void Update()
    {
        // 時間が動いてるときだけ時間寿命を進める
        if (!TimeStopController.isStopped)
        {
            currentLife += Time.deltaTime;
            if (currentLife >= lifeTime)
            {
                Destroy(gameObject);
                return;
            }
        }

        // 距離ベースの寿命
        float dist = Vector3.Distance(startPos, transform.position);
        if (dist >= maxDistance)
        {
            Destroy(gameObject);
            return;
        }
    }

    void FixedUpdate()
    {
        if (!initialized) return;

        if (TimeStopController.isStopped)
        {
            if (rb.velocity != Vector2.zero)
            {
                savedVelocity = rb.velocity;
            }
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        else
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
                rb.velocity = savedVelocity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        // 壁とかに当たったら消したいならここに追加
    }
}
