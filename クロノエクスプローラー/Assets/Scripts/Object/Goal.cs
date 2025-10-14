using UnityEngine;

public class Goal : MonoBehaviour
{
    private bool isCleared = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared) return;

        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.HasAllCrystals())
            {
                isCleared = true;
                Debug.Log("Stage Cleared!");
                SceneFader.Instance.FadeOut("GameClearScene");
            }
            else
            {
                Debug.Log("‚Ü‚¾Œ‹»‚ª‘«‚è‚È‚¢cI");
            }
        }
    }
}
