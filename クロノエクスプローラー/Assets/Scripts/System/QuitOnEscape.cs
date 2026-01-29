using UnityEngine;

public class QuitOnEscape : MonoBehaviour
{
    void RestoreBeforeQuit()
    {
        Time.timeScale = 1f;        // 一時停止を解除
        AudioListener.pause = false; // 音声ポーズ解除
    }

    void Update()
    {
        // Esc キーが押された瞬間に終了を実行
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestoreBeforeQuit();

#if UNITY_EDITOR
            // 目的：エディタ再生中はプレイモードを終了
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}
