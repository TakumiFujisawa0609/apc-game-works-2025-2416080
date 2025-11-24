using UnityEngine;

/// <summary>
/// Esc でゲームを即終了するだけの単機能クラス
/// （Editor 再生中は再生停止、ビルド版ではアプリ終了）
/// </summary>
public class QuitOnEscape : MonoBehaviour
{
    // 目的：ポーズや時止めから復帰しないまま終了すると
    // 次回起動時に副作用が出るのを防ぐため、終了前に timeScale 等を戻す
    void RestoreBeforeQuit()
    {
        Time.timeScale = 1f;        // 目的：一時停止を解除
        AudioListener.pause = false; // 目的：音声ポーズ解除
    }

    void Update()
    {
        // 目的：Esc キーが押された瞬間に終了を実行
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestoreBeforeQuit();

#if UNITY_EDITOR
            // 目的：エディタ再生中はプレイモードを終了
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // 目的：ビルド版アプリを終了
            UnityEngine.Application.Quit();
#endif
        }
    }
}
