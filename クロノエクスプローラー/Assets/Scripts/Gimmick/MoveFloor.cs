using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    [Header("移動設定")]
    public Vector2 moveDirection = Vector2.right; // 移動方向
    public float moveRange;                  // 移動距離
    public float moveSpeed;                  // 移動速度

    private Vector3 startPos;
    private Vector3 dir3;
    private int dirSign = 1;
    private float traveled = 0f;

    public Vector3 DeltaMovement { get; private set; }

    private Vector3 lastPosition;

    void Start()
    {
        startPos = transform.position;
        lastPosition = startPos;
        dir3 = new Vector3(moveDirection.x, moveDirection.y, 0f).normalized;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (TimeStopController.isStopped) return;

        // 前フレーム位置を記録
        lastPosition = transform.position;

        // 移動処理
        transform.Translate(dir3 * moveSpeed * Time.deltaTime * dirSign);

        traveled = Vector3.Dot(transform.position - startPos, dir3);
        if (Mathf.Abs(traveled) >= moveRange)
        {
            float clamped = Mathf.Sign(traveled) * moveRange;
            transform.position = startPos + dir3 * clamped;
            dirSign *= -1;
        }
        // 今フレームの移動差分を記録
        DeltaMovement = transform.position - lastPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            // 接触点ごとにチェック
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 上方向からの接触のみ
                if (Vector2.Dot(contact.normal, Vector2.up) > 0.6f)
                {
                    // プレイヤーの位置が床より上にあるか
                    if (collision.transform.position.y > transform.position.y + 0.05f)
                    {
                        collision.transform.SetParent(transform);
                        return;
                    }
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
