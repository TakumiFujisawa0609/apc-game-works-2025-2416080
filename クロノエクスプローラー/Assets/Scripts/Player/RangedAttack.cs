using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [Header("発射設定")]
    public GameObject knifePrefab;   // 投げるナイフのPrefab
    public Transform firePoint;      // 発射位置
    public float throwSpeed = 8f;    // 投げるスピード

    [Header("所持数設定")]
    public int maxKnives = 3;        // 最大3本
    public float normalRefillTime = 3f;  // 1本でも残ってるときの回復時間
    public float emptyReloadTime = 12f;  // 0本になったときの特別リロード時間

    private int currentKnives;       // 現在の所持数
    private float normalRefillTimer; // 1～2本のときの3秒カウント

    private bool isEmptyReload = false; // 0本になってからの12秒モード中かどうか
    private float emptyReloadTimer = 0f; // 0本モードの経過時間

    void Start()
    {
        // ゲーム開始時はフルで持っている
        currentKnives = maxKnives;
    }

    void Update()
    {
        if (isEmptyReload)
        {
            emptyReloadTimer += Time.deltaTime;

            if (emptyReloadTimer >= emptyReloadTime)
            {
                // 12秒たったので一気に3本に戻す
                currentKnives = maxKnives;
                isEmptyReload = false;
                emptyReloadTimer = 0f;
                normalRefillTimer = 0f; 
                SfxPlayer.Play2D(SfxKey.KnifeRecharge);
            }

            return;
        }

        if (currentKnives < maxKnives)
        {
            normalRefillTimer += Time.deltaTime;

            if (normalRefillTimer >= normalRefillTime)
            {
                SfxPlayer.Play2D(SfxKey.KnifeRecharge);

                currentKnives++;
                normalRefillTimer = 0f;

                if (currentKnives > maxKnives)
                {
                    currentKnives = maxKnives;
                }
            }
        }
        else
        {
            normalRefillTimer = 0f;
        }
    }
    public void Shoot()
    {
        if (isEmptyReload) return;

        GetComponent<PlayerAnimDriver>()?.TriggerThrow();

        if (currentKnives <= 0)
        {
            SfxPlayer.Play2D(SfxKey.KnifeLocked);
            return;
        }

        if (knifePrefab == null || firePoint == null) return;

        // 1本消費する
        currentKnives--;

        SfxPlayer.Play2D(SfxKey.KnifeThrow);

        // ナイフを生成して発射する
        GameObject knifeObj = Instantiate(knifePrefab, firePoint.position, Quaternion.identity);

        // プレイヤーの向きに合わせて左右を決める
        float dir = transform.localScale.x >= 0 ? 1f : -1f;

        // ナイフのスクリプトに初速を渡す
        var knife = knifeObj.GetComponent<KnifeProjectile>();
        if (knife != null)
        {
            knife.Init(dir, throwSpeed);
        }

        // 今の発射でゼロになったら、ここから12秒ロックに入る
        if (currentKnives <= 0)
        {
            isEmptyReload = true;
            emptyReloadTimer = 0f;
            normalRefillTimer = 0f; 
        }
    }

    public int GetCurrentKnives() => currentKnives;
    public int GetMaxKnives() => maxKnives;

    public bool IsEmptyReload() => isEmptyReload;             
    public bool CanShoot() => !isEmptyReload && currentKnives > 0;

    public float GetNormalRefillTime() => normalRefillTime;      // 3s
    public float GetEmptyReloadTime() => emptyReloadTime;       // 12s

    public float GetNormalRefillProgress()
    {
        // 1～2本のときだけ進む。満タン/ロック中は0
        if (isEmptyReload || currentKnives >= maxKnives) return 0f;
        return Mathf.Clamp01(normalRefillTimer / normalRefillTime);
    }

    public float GetEmptyReloadProgress()
    {
        // ロック中だけ進む。通常時は0
        if (!isEmptyReload) return 0f;
        return Mathf.Clamp01(emptyReloadTimer / emptyReloadTime);
    }

    // 残り時間（秒）
    public float GetNormalRefillRemaining()
    {
        if (isEmptyReload || currentKnives >= maxKnives) return 0f;
        return Mathf.Max(0f, normalRefillTime - normalRefillTimer);
    }

    public float GetEmptyReloadRemaining()
    {
        if (!isEmptyReload) return 0f;
        return Mathf.Max(0f, emptyReloadTime - emptyReloadTimer);
    }
}
