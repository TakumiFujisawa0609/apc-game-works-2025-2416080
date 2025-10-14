using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    private static SceneFader instance;
    private bool isFirstScene = true;

    private void Awake()
    {
        // シングルトン（重複しないように）
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // フェードイメージ初期化
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            // ? 起動時（TitleScene）は最初から透明にする
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float t = 0f;
        Color c = fadeImage.color;
        fadeImage.raycastTarget = true;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

        // ? フェード用オブジェクトを削除（残らないようにする）
        Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ? TitleSceneではフェードインしない
        if (isFirstScene)
        {
            isFirstScene = false;
            return;
        }

        // ? 2回目以降（GameSceneなど）ではフェードイン
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = fadeDuration;
        Color c = fadeImage.color;
        fadeImage.raycastTarget = false;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }
}
