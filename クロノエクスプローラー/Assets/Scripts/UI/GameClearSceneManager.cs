using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameClearSceneManager : MonoBehaviour
{
    public RectTransform selector;
    public TextMeshProUGUI[] menuItems;
    private int selectedIndex = 0;

    private void Start()
    {
        UpdateSelectorPosition();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
            UpdateSelectorPosition();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuItems.Length;
            UpdateSelectorPosition();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteSelection();
        }
    }

    private void UpdateSelectorPosition()
    {
        // ? UI内でのローカル座標を取得
        RectTransform target = menuItems[selectedIndex].GetComponent<RectTransform>();
        selector.anchoredPosition = new Vector2(
            target.anchoredPosition.x - 50f, // ← 距離調整
            target.anchoredPosition.y
        );
    }

    private void ExecuteSelection()
    {
        if (SceneFader.Instance == null)
        {
            Debug.LogError("SceneFaderが見つかりません！");
            return;
        }

        switch (selectedIndex)
        {
            case 0: // Retry
                SceneFader.Instance.FadeOut("GameScene");
                break;
            case 1: // Titleへ
                SceneFader.Instance.FadeOut("TitleScene");
                break;
        }
    }
}
