using UnityEngine;

/// <summary>
/// プレイヤーが移動床に乗っている間だけ、その床に追従する処理。
/// Transform.SetParent() により床の動きと完全同期。
/// </summary>
public class PlayerOnPlatform : MonoBehaviour
{
    private Transform originalParent; // 元の親（通常はシーンルート）

    void Start()
    {
        originalParent = transform.parent;
    }

    // 床に乗った時に親を切り替え
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MoveFloor"))
        {
            transform.SetParent(other.transform);
        }
    }

    // 床から離れた時に親を元に戻す
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MoveFloor"))
        {
            transform.SetParent(originalParent);
        }
    }
}
