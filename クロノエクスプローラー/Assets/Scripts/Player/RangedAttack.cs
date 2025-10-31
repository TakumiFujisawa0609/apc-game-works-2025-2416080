using UnityEngine;

/// <summary>
/// プレイヤーから飛び道具を出すだけのクラス
/// </summary>
public class RangedAttack : MonoBehaviour
{
    public GameObject knifePrefab;   // 投げるナイフのプレファブ
    public Transform firePoint;      // 発射位置（Playerの手元に空オブジェクトをつける）
    public float throwSpeed = 8f;    // 投げる初速
    public bool faceWithPlayer = true; // プレイヤーの向きに合わせる場合

    public void Shoot()
    {
        if (knifePrefab == null || firePoint == null) return;

        // ナイフ生成
        GameObject knifeObj = Instantiate(knifePrefab, firePoint.position, Quaternion.identity);

        // プレイヤーの向きに合わせる
        float dir = transform.localScale.x >= 0 ? 1f : -1f;

        // ナイフに初速を渡す
        KnifeProjectile knife = knifeObj.GetComponent<KnifeProjectile>();
        if (knife != null)
        {
            knife.Init(dir, throwSpeed);
        }
    }
}
