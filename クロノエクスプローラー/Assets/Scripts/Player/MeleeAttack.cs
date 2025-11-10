using UnityEngine;

/// <summary>
/// 近接攻撃：一瞬だけTriggerをONにして、当たった敵にダメージを与える
/// </summary>
public class MeleeAttack : MonoBehaviour
{
    [Header("攻撃判定")]
    public GameObject hitbox;     // 攻撃判定のオブジェクト（子に置く）
    public float activeTime = 0.15f; // 何秒間当たり判定を出すか

    private float timer = 0f;
    private bool isAttacking = false;

    void Start()
    {
        // 最初は非表示（当たり判定なし）
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
    }

    void Update()
    {
        // 攻撃中だけタイマーでOFFにする
        if (isAttacking)
        {
            timer += Time.deltaTime;
            if (timer >= activeTime)
            {
                EndAttack();
            }
        }
    }

    /// <summary>
    /// PlayerAttack から呼ばれる入口
    /// </summary>
    public void DoMelee()
    {
        if (isAttacking) return; // 連打防止

        isAttacking = true;
        timer = 0f;

        // ここで振り音（2D）
        SfxPlayer.Play2D(SfxKey.MeleeSwing);

        if (hitbox != null)
        {
            hitbox.SetActive(true);
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
    }
}
