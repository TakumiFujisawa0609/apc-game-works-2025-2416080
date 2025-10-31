using UnityEngine;

/// <summary>
/// ‹ßÚUŒ‚‚Ì“–‚½‚è”»’è‘¤‚É‚Â‚¯‚é
/// </summary>
public class MeleeHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // “G‚É“–‚½‚Á‚½‚ç
        if (other.CompareTag("Enemy"))
        {
            // ‚±‚±‚Å“G‚ÌHP‚ğŒ¸‚ç‚·ˆ—‚ğŒÄ‚Ô‘z’è
            // ¡‰ñ‚Í‚Æ‚è‚ ‚¦‚¸Á‚·‚¾‚¯
            Destroy(other.gameObject);
        }
    }
}
