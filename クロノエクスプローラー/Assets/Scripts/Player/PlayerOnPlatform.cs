using UnityEngine;

/// <summary>
/// ・移動床に乗っている間は親子付けで追従
/// ・地面/移動床に接地しているかを IsGrounded で提供
/// </summary>
public class PlayerOnPlatform : MonoBehaviour
{
    public bool IsGrounded { get; private set; }  // ★ 追加

    private Transform originalParent;

    // 接地とみなすタグ（必要に応じて増やす）
    [SerializeField] string[] groundTags = { "Ground", "MoveFloor" };

    void Start()
    {
        originalParent = transform.parent;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MoveFloor"))
        {
            transform.SetParent(other.transform);
        }

        // ★ 上向きの接触（足元）なら接地ON
        if (IsGroundTag(other.gameObject) && HasUpwardContact(other))
            IsGrounded = true;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (IsGroundTag(other.gameObject) && HasUpwardContact(other))
            IsGrounded = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MoveFloor"))
        {
            transform.SetParent(originalParent);
        }

        // ★ 離れたら一旦OFF（複数接地がある場合は必要に応じて接地数カウントでも可）
        if (IsGroundTag(other.gameObject))
            IsGrounded = false;
    }

    bool IsGroundTag(GameObject go)
    {
        foreach (var t in groundTags) if (go.CompareTag(t)) return true;
        return false;
    }

    // 法線が上向きの接触点があるか（足元だけを接地とみなす）
    bool HasUpwardContact(Collision2D col)
    {
        foreach (var cp in col.contacts)
            if (cp.normal.y > 0.5f) return true;
        return false;
    }
}
