using UnityEngine;

/// <summary>
/// プレイヤーが投げたナイフの動き
/// 時間停止中はその場で止まる
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class KnifeProjectile : MonoBehaviour
{
    public float lifeTime = 5f;     // 何秒で自動消滅するか
    public int damage = 1;          // 敵に与えるダメージ（あとで拡張）

    private Rigidbody2D rb;
    private float currentLife = 0f;

    // 停止前の速度を覚えておくための変数
    private Vector2 savedVelocity;
    private bool inited = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 発射時に呼ぶ初期化
    /// </summary>
    public void Init(float dir, float speed)
    {
        // 右向きなら正、左向きなら負の速度を与える
        rb.velocity = new Vector2(dir * speed, 0f);
        inited = true;
    }

    void Update()
    {
        // 生存時間で自動消滅
        currentLife += Time.deltaTime;
        if (currentLife >= lifeTime)
        {
            Destroy(gameObject);
        }

        // ← ここでは動かさない。実際の停止/再開はFixedUpdateでやる
    }

    void FixedUpdate()
    {
        if (!inited) return;

        // ?? 時間停止中 → 今の速度を保存しておいて止める
        if (TimeStopController.isStopped)
        {
            // 一度だけ保存するようにして不要な代入を防ぐ
            if (rb.velocity != Vector2.zero)
            {
                savedVelocity = rb.velocity;
            }
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;  // 物理を止めておくと安定する
        }
        else
        {
            // ?? 時間が動き出したら、保存していた速度で再開
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
                rb.velocity = savedVelocity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 敵に当たったら消す
        if (other.CompareTag("Enemy"))
        {
            // ここで敵にダメージを送るのも可
            Destroy(gameObject);
        }
        // 壁とかにも当たったら消したいならここに条件を追加
    }
}
