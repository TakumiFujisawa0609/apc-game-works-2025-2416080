using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    public int damage = 1;

    // 
    public bool singleHitSfxPerSwing = true;
    bool playedThisSwing = false;

    void OnEnable()  // UŒ‚ŠJn‚ÉŒÄ‚Î‚ê‚é
    {
        playedThisSwing = false;

        //Trigger‚ğ•ÛØ
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
