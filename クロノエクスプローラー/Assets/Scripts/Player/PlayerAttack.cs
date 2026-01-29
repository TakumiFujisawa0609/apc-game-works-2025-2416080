using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public MeleeAttack meleeAttack;   // 近接攻撃用コンポーネント
    public RangedAttack rangedAttack; // 遠距離攻撃用コンポーネント

    void Update()
    {
        // Kキー近接攻撃
        if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.K))
        {
            if (meleeAttack != null)
            {
                meleeAttack.DoMelee();
            }
        }

        // Lキー遠距離攻撃（ナイフ投げ）
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.L))
        {
            if (rangedAttack != null)
            {
                rangedAttack.Shoot();
            }
        }



    }
}
