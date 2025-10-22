using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI retryText;
    public TextMeshProUGUI titleText;
    public RectTransform selector; // ← 矢印などのUIを設定

    private int selectedIndex = 0;
    private readonly string[] options = { "Retry", "Title" };

    private Vector3 retryPos;
    private Vector3 titlePos;

    void Start()
    {
        Time.timeScale = 1f; // 念のためリセット

        // 初期位置を記録
        retryPos = retryText.rectTransform.localPosition;
        titlePos = titleText.rectTransform.localPosition;

        UpdateSelectorPosition();
    }

    void Update()
    {
        // --- 選択移動 ---
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
            UpdateSelectorPosition();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % options.Length;
            UpdateSelectorPosition();
        }

        // --- 決定 ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (selectedIndex)
            {
                case 0: // Retry
                    SceneManager.LoadScene("GameScene");
                    break;
                case 1: // Title
                    SceneManager.LoadScene("TitleScene");
                    break;
            }
        }
    }

    void UpdateSelectorPosition()
    {
        if (selector == null) return;

        switch (selectedIndex)
        {
            case 0:
                selector.localPosition = retryPos + new Vector3(-150f, 0, 0); // ←矢印を左側に少し
                break;
            case 1:
                selector.localPosition = titlePos + new Vector3(-150f, 0, 0);
                break;
        }

        // 色の切り替え（選択中は黄色）
        retryText.color = (selectedIndex == 0) ? Color.yellow : Color.white;
        titleText.color = (selectedIndex == 1) ? Color.yellow : Color.white;
    }
}
