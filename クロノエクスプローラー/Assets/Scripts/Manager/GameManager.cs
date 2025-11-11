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


    /// <summary>
    /// 結晶を1つ取得
    /// </summary>
    public void AddCrystal()
    {
        crystalCount++;
        Debug.Log($"Crystal Collected! ({crystalCount}/{totalCrystals})");
    }

    /// <summary>
    /// すべての結晶を集めたか？
    /// </summary>
    public bool HasAllCrystals()
    {
        return crystalCount >= totalCrystals;
    }
}
