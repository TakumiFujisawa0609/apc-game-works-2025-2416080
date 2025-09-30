#include "Player.h"
#include <DxLib.h>
#include "../Application.h"
#include "../Manager/InputManager.h"

Player::Player()
    : x_(0), y_(0), width_(WIDTH), height_(HEIGHT),
    img_(-1), velocityY_(0.0f), onGround_(false)
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

    // 初期位置（画面下から INIT_Y_OFFSET 上）
    x_ = INIT_X;
    y_ = Application::SCREEN_SIZE_Y - INIT_Y_OFFSET;

    velocityY_ = 0.0f;
    onGround_ = false;
}

void Player::Update(const Stage& stage)
{
    auto& input = InputManager::GetInstance();

    // 横移動
    if (input.IsNew(KEY_INPUT_D)) x_ += SPEED;
    if (input.IsNew(KEY_INPUT_A)) x_ -= SPEED;

    // ジャンプ
    if (input.IsNew(KEY_INPUT_SPACE) && onGround_) {
        velocityY_ = -JUMP_POWER;
        onGround_ = false;
    }

    // 重力を適用
    velocityY_ += GRAVITY;
    if (velocityY_ > MAX_FALL_SPEED) velocityY_ = MAX_FALL_SPEED;
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
