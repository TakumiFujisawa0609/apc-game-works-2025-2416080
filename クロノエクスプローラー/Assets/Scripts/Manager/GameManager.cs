using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int crystalCount = 0;   // 現在取得数
    public int totalCrystals = 3;   // クリアに必要な数

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddCrystal()
    {
        crystalCount++;
        Debug.Log($"Crystal Collected! ({crystalCount}/{totalCrystals})");
    }

    public bool HasAllCrystals()
    {
        return crystalCount >= totalCrystals;
    }
}
