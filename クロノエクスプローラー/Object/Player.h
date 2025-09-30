#pragma once
#include "../Manager/Camera.h"
#include "../Object/Stage.h"

class Player
{
public:
    static constexpr int WIDTH = 32;                 // プレイヤー幅
    static constexpr int HEIGHT = 32;                // プレイヤー高さ
    static constexpr int SPEED = 5;                  // 横移動速度
    static constexpr float GRAVITY = 0.4f;           // 重力
    static constexpr float JUMP_POWER = 8.0f;        // ジャンプ力
    static constexpr float MAX_FALL_SPEED = 10.0f;   // 最大落下速度
    static constexpr int INIT_X = 200;               // 初期位置X
    static constexpr int INIT_Y_OFFSET = 200;        // 初期位置オフセット（画面下からの距離）


    Player();
    ~Player();

    void Init();
    void Update(const Stage& stage);
    void Draw(const Camera& cam);
    void Release();

    // getter
    int GetX() const { return x_; }
    int GetY() const { return y_; }
    int GetWidth() const { return WIDTH; }
    int GetHeight() const { return HEIGHT; }

private:
    // ==== メンバ変数 ====
    int x_, y_;
    int width_, height_;
    int img_;
    float velocityY_;
    bool onGround_;
};
