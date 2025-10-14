using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    [Header("フェード設定")]
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    private bool isFading = false;
    private bool firstLoad = true; // ← ★追加：初回起動判定

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            transform.SetParent(null);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 起動時は黒で覆わない
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    public void FadeOut(string sceneName)
    {
        if (!isFading && fadeImage != null)
        {
            StartCoroutine(FadeOutCoroutine(sceneName));
        }
    }

    private IEnumerator FadeOutCoroutine(string sceneName)
    {
        isFading = true;
        Color color = fadeImage.color;

        while (color.a < 1f)
        {
            color.a += Time.unscaledDeltaTime * fadeSpeed;
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
        isFading = false;
    }

    private IEnumerator FadeIn()
    {
        isFading = true;
        Color color = fadeImage.color;

        while (color.a > 0f)
        {
            color.a -= Time.unscaledDeltaTime * fadeSpeed;
            fadeImage.color = color;
            yield return null;
        }

        isFading = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ?? 初回ロード（起動時）はフェードしない
        if (firstLoad)
        {
            firstLoad = false;
            return;
        }

        if (fadeImage != null)
        {
            fadeImage.color = Color.black;
            StartCoroutine(FadeIn());
        }
    }
}
