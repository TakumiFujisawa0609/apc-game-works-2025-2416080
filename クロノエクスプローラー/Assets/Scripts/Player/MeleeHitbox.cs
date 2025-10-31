using UnityEngine;

/// <summary>
/// 近接攻撃の当たり判定側につけるスクリプト
/// Triggerに入ってきたEnemyにダメージを送る
/// </summary>
public class MeleeHitbox : MonoBehaviour
{
    public int damage = 1;   // 今回は1固定でOK

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemyタグのオブジェクトに当たったか？
        if (other.CompareTag("Enemy"))
        {
            // EnemyHealthを持ってるか調べる
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                // ダメージを与える
                enemy.TakeDamage(damage);
            }
        }
    }
}
