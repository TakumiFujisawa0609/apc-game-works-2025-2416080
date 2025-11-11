using UnityEngine;

/// <summary>
/// プレイヤーの攻撃入力をまとめて処理するクラス
/// 移動はPlayerControllerに任せて、ここは「いつ攻撃するか」だけを見る
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    public MeleeAttack meleeAttack;   // 近接攻撃用コンポーネント
    public RangedAttack rangedAttack; // 遠距離攻撃用コンポーネント

    void Update()
    {
        // ※キーはとりあえずこうしておきます。あとで好きなキーに変えてOK
        // Kキー：近接攻撃
        if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.K))
        {
            if (meleeAttack != null)
            {
                meleeAttack.DoMelee();
            }
        }

        // Lキー：遠距離攻撃（ナイフ投げ）
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.L))
        {
            if (rangedAttack != null)
            {
                rangedAttack.Shoot();
            }
        }

        //// コントローラー対応（A,Bはジャンプに使ってるっぽいのでX/Y想定）
        //if (Input.GetButtonDown("Fire1") && meleeAttack != null)
        //{
        //    meleeAttack.DoMelee();
        //}
        //if (Input.GetButtonDown("Fire2") && rangedAttack != null)
        //{
        //    rangedAttack.Shoot();
        //}


    }
}
