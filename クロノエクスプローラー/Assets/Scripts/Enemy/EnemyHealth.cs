using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHp = 1f;     
    private float currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        // ŠÔ’â~’†‚Íƒ_ƒ[ƒW‚ğó‚¯‚È‚¢
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
        Destroy(gameObject);
    }
}
