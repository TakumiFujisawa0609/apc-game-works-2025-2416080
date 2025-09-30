#include "Player.h"
#include <DxLib.h>
#include "../Application.h"
#include "../Manager/InputManager.h"

Player::Player()
{
}

Player::~Player()
{
    Release();
}

void Player::Init()
{
    // プレイヤー画像を読み込み
    img_ = LoadGraph((Application::PATH_IMAGE + "player.png").c_str());

    x_ = 200;
    y_ = Application::SCREEN_SIZE_Y - 200;
    width_ = WIDTH;

	height_ = HEIGHT;

    velocityY_ = 0.0f;
    onGround_ = false;

    speed_ = SPEED;
    gravity_ = GRAVITY;      // 毎フレーム下に落ちる加速度
    jumpPower_ = JUMP_POWER;   // ジャンプ力
}

void Player::Update(const Stage& stage)
{
    auto& input = InputManager::GetInstance();

    // 横移動
    if (input.IsNew(KEY_INPUT_D)) x_ += speed_;
    if (input.IsNew(KEY_INPUT_A)) x_ -= speed_;

    // ジャンプ
    if (input.IsNew(KEY_INPUT_SPACE) && onGround_) {
        velocityY_ = -jumpPower_;
        onGround_ = false;
    }

    // 重力を適用
    velocityY_ += gravity_;
    y_ += static_cast<int>(velocityY_);

    // 地面との当たり判定
    onGround_ = false;
    for (auto& block : stage.GetBlocks()) {
        // プレイヤー矩形
        int px1 = x_;
        int py1 = y_;
        int px2 = x_ + width_;
        int py2 = y_ + height_;

        // ブロック矩形
        int bx1 = block.x;
        int by1 = block.y;
        int bx2 = block.x + block.w;
        int by2 = block.y + block.h;

        // 足元がブロックに当たっているか？
        if (px2 > bx1 && px1 < bx2 && py2 > by1 && py1 < by2) {
            // 床の上に乗る
            y_ = block.y - height_;
            velocityY_ = 0;
            onGround_ = true;
        }
    }

}

void Player::Draw(const Camera& cam)
{
    if (img_ != -1) {
        DrawGraph(x_ - cam.GetX(), y_, img_, TRUE);
    }
    else {
        DrawBox(x_ - cam.GetX(), y_,
            x_ - cam.GetX() + width_, y_ + height_,
            GetColor(0, 255, 0), TRUE);
    }
}

void Player::Release()
{
    if (img_ != -1) {
        DeleteGraph(img_);
        img_ = -1;
    }
}
