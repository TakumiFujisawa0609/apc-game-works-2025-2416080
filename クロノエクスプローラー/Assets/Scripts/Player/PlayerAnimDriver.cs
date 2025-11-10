using UnityEngine;

public class PlayerAnimDriver : MonoBehaviour
{
    [Header("Refs")]
    public Transform visual;      // PlayerVisual
    public Animator animator;     // PlayerVisual.Animator
    public Rigidbody2D rb;
    public PlayerOnPlatform onPlatform; // あれば

    [Header("Animator Parameters (rename here if needed)")]
    public string runBool = "Run";            // 速度でON/OFF
    public string groundedBool = "Grounded";  // 接地
    public string attackTrig = "Attack";      // 近接攻撃
    public string throwTrig = "";            // 使わなければ空でOK
    public string jumpTrig = "Jump";        // 任意
    public string rollTrig = "Roll";        // 任意

    [Header("Tuning")]
    public float runThreshold = 0.05f;        // 走り判定の速度しきい値
    public float faceFlipThreshold = 0.01f;   // 向き更新のしきい値

    void Reset()
    {
        visual = transform.Find("PlayerVisual");
        if (visual) animator = visual.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!animator || !rb) return;

        // 逆さ・回転を強制修正（子の見た目を常に正位置に）
        if (visual)
        {
            if (visual.localRotation != Quaternion.identity) visual.localRotation = Quaternion.identity;
            var vs = visual.localScale; if (vs.y < 0f) { vs.y = Mathf.Abs(vs.y); visual.localScale = vs; }
        }


        // Grounded
        bool grounded = (onPlatform && onPlatform.IsGrounded) || Mathf.Abs(rb.velocity.y) < 0.01f;
        SetBoolIfExists(groundedBool, grounded);

        // Run（速度でON/OFF）
        bool isRun = Mathf.Abs(rb.velocity.x) > runThreshold;
        SetBoolIfExists(runBool, isRun);

        // 向き（親のScale.xで統一）
        if (Mathf.Abs(rb.velocity.x) > faceFlipThreshold)
        {
            var s = transform.localScale;
            s.x = rb.velocity.x >= 0 ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

    // ---- public API（他スクリプトから呼ぶ用） ----
    public void PlayMelee() { SetTriggerIfExists(attackTrig); }
    public void TriggerThrow() { SetTriggerIfExists(throwTrig); }
    public void TriggerJump() { SetTriggerIfExists(jumpTrig); }
    public void TriggerRoll() { SetTriggerIfExists(rollTrig); }

    // ---- helpers ----
    void SetBoolIfExists(string name, bool v)
    {
        if (string.IsNullOrEmpty(name)) return;
        var ps = animator.parameters;
        for (int i = 0; i < ps.Length; i++)
            if (ps[i].name == name && ps[i].type == AnimatorControllerParameterType.Bool)
            { animator.SetBool(name, v); return; }
    }
    void SetTriggerIfExists(string name)
    {
        if (string.IsNullOrEmpty(name)) return;
        var ps = animator.parameters;
        for (int i = 0; i < ps.Length; i++)
            if (ps[i].name == name && ps[i].type == AnimatorControllerParameterType.Trigger)
            { animator.SetTrigger(name); return; }
    }
}
