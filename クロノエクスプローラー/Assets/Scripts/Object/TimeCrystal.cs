using UnityEngine;

public class TimeCrystal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddCrystal();
            Destroy(gameObject); // æ“¾‚µ‚½‚çÁ‚¦‚é
        }
    }
}
