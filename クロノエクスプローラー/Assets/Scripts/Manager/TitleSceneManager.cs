using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI startText;
    public TextMeshProUGUI exitText;
    public RectTransform selector; 

    private int selectedIndex = 0;

    void Start()
    {
        UpdateSelectorPosition();
        UpdateTextColor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            UpdateSelectorPosition();
            UpdateTextColor();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = Mathf.Min(1, selectedIndex + 1);
            UpdateSelectorPosition();
            UpdateTextColor();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedIndex == 0)
                StartGame();
            else
                QuitGame();
        }
    }

    void UpdateSelectorPosition()
    {
        if (selector == null) return;
        if (selectedIndex == 0)
            selector.position = new Vector3(selector.position.x, startText.transform.position.y, 0);
        else
            selector.position = new Vector3(selector.position.x, exitText.transform.position.y, 0);
    }

    void UpdateTextColor()
    {
        Color normal = Color.white;
        Color selected = Color.yellow;

        startText.color = (selectedIndex == 0) ? selected : normal;
        exitText.color = (selectedIndex == 1) ? selected : normal;
    }

    void StartGame()
    {
        SceneFader fader = FindObjectOfType<SceneFader>();
        if (fader != null)
        {
            fader.FadeOut("GameScene");
        }
        else
        {
            SceneManager.LoadScene("GameScene"); 
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
