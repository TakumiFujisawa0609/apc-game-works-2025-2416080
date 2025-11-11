using UnityEngine;

/// <summary>
/// 近接攻撃の当たり判定側につけるスクリプト
/// Triggerに入ってきたEnemyにダメージを送る
/// </summary>
public class MeleeHitbox : MonoBehaviour
{
    public int damage = 1;   // 今回は1固定でOK

    // 1振りで音が鳴り過ぎないようにしたい場合の任意フラグ
    public bool singleHitSfxPerSwing = true;
    bool playedThisSwing = false;

    void OnEnable()  // 攻撃開始時に呼ばれる（hitboxをSetActive(true)しているため）
    {
        playedThisSwing = false;

        // 念のためTriggerを保証
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        SfxPlayer.Play2D(SfxKey.MeleeHit);

        var enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

        }
    }
}
