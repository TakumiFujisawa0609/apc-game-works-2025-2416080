#pragma once
#include "../Manager/Camera.h"
#include "Stage.h"

class Player
{
public:
    static constexpr int WIDTH = 32;

    static constexpr int HEIGHT = 32;

    static constexpr int SPEED = 5;

    static constexpr float GRAVITY = 0.4f;

    static constexpr float JUMP_POWER = 8.0f;

    static constexpr float JUMP_HOLD_BOOST = 0.2f;

    static constexpr float MAX_FALL_SPEED = 10.0f;

    Player();
    ~Player();

    void Init();                  // 初期化
    void Update(const Stage& stage);                // 入力処理・物理計算
    void Draw(const Camera& cam); // カメラ補正して描画
    void Release();               // 解放処理

    // getter
    int GetX() const { return x_; }
    int GetY() const { return y_; }
    int GetWidth() const { return width_; }
    int GetHeight() const { return height_; }

private:
    int x_, y_;        // プレイヤー座標
    int width_, height_;
    int img_;          // 画像ハンドル

    float velocityY_;    // Y方向の速度
    bool onGround_;      // 地面にいるかどうか

    int speed_;          // 横移動速度
    int gravity_;        // 重力
    int jumpPower_;      // ジャンプ力

};
