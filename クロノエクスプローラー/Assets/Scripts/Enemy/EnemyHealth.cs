using UnityEngine;

/// <summary>
/// 敵のHPを管理するクラス（小数ダメージ対応）
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    public float maxHp = 1f;     // 今回は1でOK（ナイフ0.5を2回で倒れる）
    private float currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        // 時間停止中はダメージを受けない
        if (TimeStopController.isStopped)
        {
            return;
        }

        currentHp -= damage;

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: エフェクトとか
        Destroy(gameObject);
    }
}
