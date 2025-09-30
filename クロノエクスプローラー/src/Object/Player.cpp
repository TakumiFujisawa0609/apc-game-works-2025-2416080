#include "Player.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"

Player::Player()
    : x_(INIT_X),
    y_(Application::SCREEN_SIZE_Y - INIT_Y_OFFSET),
    width_(WIDTH),
    height_(HEIGHT),
    img_(-1),
    velocityY_(0.0f),
    onGround_(false),
    isJumping_(false),
    jumpCharge_(0.0f)
{
}

Player::~Player() {}

void Player::Init()
{
    img_ = LoadGraph((Application::PATH_IMAGE + "player.png").c_str());
}

void Player::Update(const Stage& stage)
{
    auto& input = InputManager::GetInstance();

    // 横移動
    if (input.IsPress(KEY_INPUT_A)) x_ -= SPEED;
    if (input.IsPress(KEY_INPUT_D)) x_ += SPEED;

    // ジャンプ（長押し対応）
    if (onGround_ && input.IsPress(KEY_INPUT_SPACE)) {
        isJumping_ = true;
        jumpCharge_ = 0.0f;
        velocityY_ = -JUMP_POWER;
        onGround_ = false;
    }
    if (isJumping_ && input.IsPress(KEY_INPUT_SPACE)) {
        if (jumpCharge_ < MAX_JUMP_BOOST) {
            velocityY_ -= JUMP_CHARGE_RATE;
            jumpCharge_ += JUMP_CHARGE_RATE;
        }
    }
    if (!input.IsPress(KEY_INPUT_SPACE)) {
        isJumping_ = false;
    }

    // 重力適用
    velocityY_ += GRAVITY;
    if (velocityY_ > MAX_FALL_SPEED) velocityY_ = MAX_FALL_SPEED;
    y_ += static_cast<int>(velocityY_);

    // 簡易接地判定（Stageの地面を使う）
    for (auto& block : stage.GetBlocks()) {
        if (x_ + width_ > block.x && x_ < block.x + block.w &&
            y_ + height_ > block.y && y_ < block.y + block.h) {
            y_ = block.y - height_; // 上に乗せる
            velocityY_ = 0.0f;
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
        DrawBox(x_ - cam.GetX(), y_, x_ - cam.GetX() + width_, y_ + height_,
            GetColor(255, 255, 255), TRUE);
    }
}

void Player::Release()
{
    if (img_ != -1) {
        DeleteGraph(img_);
        img_ = -1;
    }
}
