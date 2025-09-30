#pragma once
#include "../Manager/Camera.h"
#include "../Object/Stage.h"

class Player
{
public:
    static constexpr int WIDTH = 32;                 // プレイヤー幅

    static constexpr int HEIGHT = 32;                // プレイヤー高さ

    static constexpr int SPEED = 5;                  // 横移動速度

    static constexpr float GRAVITY = 0.25f;           // 重力

    static constexpr float JUMP_POWER = 6.5f;        // ジャンプ力

    static constexpr float MAX_FALL_SPEED = 5.0f;   // 最大落下速度

    static constexpr float JUMP_CHARGE_RATE = 0.25f;   // SPECE長押しによる加算率

    static constexpr float MAX_JUMP_BOOST = 7.0f;   // 長押しで追加できる最大値

    static constexpr int INIT_X = 200;               // 初期位置X

    static constexpr int INIT_Y_OFFSET = 200;        // 初期位置オフセット（画面下からの距離）


    Player();
    ~Player();

    void Init();
    void Update(const Stage& stage);
    void Draw(const Camera& cam);
    void Release();

    int GetX() const { return x_; }
    int GetY() const { return y_; }
    int GetWidth() const { return WIDTH; }
    int GetHeight() const { return HEIGHT; }

private:
    int x_, y_;
    int width_, height_;
    int img_;

    float velocityY_;   // 縦方向の速度
    bool onGround_;     // 接地判定
    bool isJumping_;    // ジャンプ中かどうか
    float jumpCharge_;  // ジャンプ長押し蓄積
};
