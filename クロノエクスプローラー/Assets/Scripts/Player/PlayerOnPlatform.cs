using UnityEngine;

public class PlayerOnPlatform : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    private Transform originalParent;

    // Ú’n‚Æ‚İ‚È‚·ƒ^ƒO
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

        // ãŒü‚«‚ÌÚG‚È‚çÚ’n
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

        // —£‚ê‚½‚çˆê’UOFF
        if (IsGroundTag(other.gameObject))
            IsGrounded = false;
    }

    bool IsGroundTag(GameObject go)
    {
        foreach (var t in groundTags) if (go.CompareTag(t)) return true;
        return false;
    }

    // –@ü‚ªãŒü‚«‚ÌÚG“_‚ª‚ ‚é‚©
    bool HasUpwardContact(Collision2D col)
    {
        foreach (var cp in col.contacts)
            if (cp.normal.y > 0.5f) return true;
        return false;
    }
}
