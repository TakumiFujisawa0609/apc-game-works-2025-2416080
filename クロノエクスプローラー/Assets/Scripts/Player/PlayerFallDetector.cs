using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFallDetector : MonoBehaviour
{
    [Header("—‰º”»’èYÀ•W")]
    public float fallThresholdY = -10f;

    void Update()
    {
        if (transform.position.y < fallThresholdY)
        {
            SceneFader.Instance.FadeOut("GameOverScene");
        }
    }
}
