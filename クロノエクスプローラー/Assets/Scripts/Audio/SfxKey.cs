// SfxKey.cs
public enum SfxKey
{
    // Player
    MeleeSwing,
    MeleeHit,
    KnifeThrow,
    KnifeHit,
    Jump,
    Land,
    PlayerDeath,

    // Enemy
    EnemyDeath,
    EnemyHit,     // 使うなら

    // System / UI
    TimeStop,
    TimeResume,
    KnifeRecharge, // 3本復帰
    KnifeLocked,   // 0本ロック時
    TimeCrystalPickup,
}
