using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 敵キャラクター（左右往復移動 + 時間停止対応）
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed;        // 移動速度
    public float moveDistance;     // 移動距離（左右往復範囲）

    private Vector3 startPos;           // 初期位置
    private int moveDir = 1;            // 1=右, -1=左
    private Rigidbody2D rb;
    private bool isDead = false;         // 死亡フラグ（将来の拡張用）

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;            // 落下しない敵なら0に
        rb.freezeRotation = true;       // 回転固定
        startPos = transform.position;
    }

    void Update()
    {
        // ? 時間停止中は動かない
        if (isDead || TimeStopController.isStopped)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // 左右移動処理
        rb.velocity = new Vector2(moveDir * moveSpeed, 0f);

        // 移動範囲チェック（範囲超えたら方向反転）
        if (Vector2.Distance(transform.position, startPos) >= moveDistance)
        {
            moveDir *= -1;
            startPos = transform.position;
            Flip(); // 向きを変える（SpriteRenderer用）
        }
    }

    // -------------------------------------------------------------
    // 向きを反転させる（見た目）
    // -------------------------------------------------------------
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // -------------------------------------------------------------
    // プレイヤー接触処理（GameOverに遷移）
    // -------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && TimeStopController.isStopped == false)
        {
            isDead = true;

            Time.timeScale = 0f;

            // ここでGameOverシーンへ遷移
            StartCoroutine(WaitAndFadeOut());
        }
    }

    private IEnumerator WaitAndFadeOut()
    {
        // Time.timeScale = 0 の状態でも動作するように unscaledDeltaTime 使用
        float timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // フェードアウト開始（GameOverSceneへ）
        SceneFader.Instance.FadeOut("GameOverScene");

        // 時間を戻しておく（シーン遷移後に正常化）
        Time.timeScale = 1f;
    }
}

