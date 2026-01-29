using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("参照")]
    public GameObject hitbox;            // 当たり判定
    public Transform playerVisual;       // Player/PlayerVisual
    public PlayerController player;      // 向き取得用
    [SerializeField] string attackTrigger = "Attack1";


    [Header("調整")]
    public float activeTime = 0.15f;     // フォールバック用の有効秒
    public Vector2 rightOffset = new Vector2(0.48f, 0f);
    public Vector2 leftOffset = new Vector2(-0.48f, 0f);
    public float attackCooldown = 0.2f;  // 連打抑制

    Animator anim;
    bool isAttacking = false;            // タイマー方式のフラグ
    float timer = 0f;
    bool canAttack = true;

    void Awake()
    {
        if (!playerVisual) playerVisual = transform.Find("PlayerVisual");
        if (!player) player = GetComponent<PlayerController>();
        if (playerVisual) anim = playerVisual.GetComponent<Animator>();
        if (hitbox) hitbox.SetActive(false);
    }

    void Update()
    {
        if (isAttacking)
        {
            timer += Time.deltaTime;
            if (timer >= activeTime) EndAttack_Internal();
        }
    }

    public void DoMelee()
    {
        if (!canAttack || anim == null) return;

        // 攻撃モーション（Attack Trigger）
        anim.ResetTrigger(attackTrigger);
        anim.SetTrigger(attackTrigger);

        // SFX（振り音）
        SfxPlayer.Play2D(SfxKey.MeleeSwing);

        if (hitbox && !HasAnimationEvents())
        {
            HitboxOn();                      
            timer = 0f;
            isAttacking = true;
        }

        // 連打抑制
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void HitboxOn()
    {
        if (!hitbox) return;

        // 向きに合わせてローカル位置をスワップ
        float dir = player ? player.GetFacingDir() : 1f;
        hitbox.transform.localPosition = (dir >= 0f) ? rightOffset : leftOffset;

        hitbox.SetActive(true);
        isAttacking = false;
    }

    public void HitboxOff()
    {
        EndAttack_Internal();
    }

    // 内部終了処理
    void EndAttack_Internal()
    {
        if (hitbox) hitbox.SetActive(false);
        isAttacking = false;
        timer = 0f;
    }

    bool HasAnimationEvents()
    {
        return false;
    }
}
