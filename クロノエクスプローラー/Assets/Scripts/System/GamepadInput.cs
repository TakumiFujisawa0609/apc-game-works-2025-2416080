using UnityEngine;

public static class GamepadInput
{
    const string AXIS_RT = "RT";
    const string AXIS_LT = "LT";
    const float TH = 0.5f; // 閾値（必要なら 0.3〜0.6 で調整）

    static bool rtPrev, ltPrev;

    // RT：押した瞬間
    public static bool RTDown()
    {
        float v = Input.GetAxisRaw(AXIS_RT);
        bool now = v > TH;
        bool down = now && !rtPrev;
        rtPrev = now;
        return down;
    }
    // LT：押した瞬間
    public static bool LTDown()
    {
        float v = Input.GetAxisRaw(AXIS_LT);
        bool now = v > TH;
        bool down = now && !ltPrev;
        ltPrev = now;
        return down;
    }
    // 連続押下状態が欲しい場合
    public static bool RTHeld() => Input.GetAxisRaw(AXIS_RT) > TH;
    public static bool LTHeld() => Input.GetAxisRaw(AXIS_LT) > TH;
}
