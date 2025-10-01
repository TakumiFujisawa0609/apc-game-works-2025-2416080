#include "Player.h"
#include "../Manager/InputManager.h"
#include "Stage.h"
#include "Gimmick/GimmickManager.h"
#include "../Manager/Camera.h"
#include "../Common/Vector2.h"
#include <algorithm>
#include <DxLib.h>

Player::Player()
    : x_(0), y_(0),
    velocityX_(0.0f), velocityY_(0.0f),
    onGround_(false), isJumping_(false),
    jumpCharge_(0.0f), canJump_(true)
{
}

Player::~Player() {}

void Player::Init(float startX, float startY)
{
    x_ = startX;
    y_ = startY;

    velocityX_ = 0.0f;
    velocityY_ = 0.0f;
    onGround_ = false;
    isJumping_ = false;
    jumpCharge_ = 0.0f;
    canJump_ = true;
}

void Player::Update(const Stage& stage, GimmickManager& gimmickManager)
{
    auto& input = InputManager::GetInstance();

    // 横移動
    velocityX_ = 0.0f;
    if (input.IsPress(KEY_INPUT_D)) velocityX_ = MOVE_SPEED;
    if (input.IsPress(KEY_INPUT_A)) velocityX_ = -MOVE_SPEED;

    // ジャンプ開始
    if (onGround_ && canJump_ && input.IsNew(KEY_INPUT_SPACE)) {
        velocityY_ = -JUMP_POWER;
        onGround_ = false;
        isJumping_ = true;
        jumpCharge_ = 0.0f;
        canJump_ = false;
    }

    // 長押しジャンプ調整
    if (isJumping_ && input.IsPress(KEY_INPUT_SPACE)) {
        if (jumpCharge_ < MAX_JUMP_BOOST) {
            velocityY_ -= JUMP_CHARGE_RATE;
            jumpCharge_ += JUMP_CHARGE_RATE;
        }
    }

    // キーを離したらジャンプ再受付
    if (!input.IsPress(KEY_INPUT_SPACE)) {
        canJump_ = true;
    }

    // 重力
    velocityY_ += GRAVITY;
    if (velocityY_ > MAX_FALL_SPEED) velocityY_ = MAX_FALL_SPEED;

    float nextX = x_ + velocityX_;
    float nextY = y_ + velocityY_;
    onGround_ = false;

    // ステージブロックとの衝突判定
    for (auto& block : stage.GetBlocks())
    {
        RECT pRect = { (int)nextX, (int)nextY,
                       (int)(nextX + WIDTH), (int)(nextY + HEIGHT) };

        RECT bRect = { block.x, block.y, block.x + block.w, block.y + block.h };

        if (pRect.left < bRect.right && pRect.right > bRect.left &&
            pRect.top < bRect.bottom && pRect.bottom > bRect.top)
        {
            int overlapX1 = bRect.right - pRect.left;
            int overlapX2 = pRect.right - bRect.left;
            int overlapY1 = bRect.bottom - pRect.top;
            int overlapY2 = pRect.bottom - bRect.top;

            int overlaps[] = { overlapX1, overlapX2, overlapY1, overlapY2 };
            int minOverlap = *std::min_element(overlaps, overlaps + 4);

            if (minOverlap == overlapX1) {
                nextX = bRect.right;
                velocityX_ = 0;
            }
            else if (minOverlap == overlapX2) {
                nextX = bRect.left - WIDTH;
                velocityX_ = 0;
            }
            else if (minOverlap == overlapY1) {
                nextY = bRect.bottom;
                velocityY_ = 0;
            }
            else if (minOverlap == overlapY2) {
                nextY = bRect.top - HEIGHT;
                velocityY_ = 0;
                onGround_ = true;
                isJumping_ = false;
            }
        }
    }

    // 移動床の上に乗っていたら一緒に動く
    if (onGround_) {
        Vector2 floorMove = gimmickManager.CheckOnMoveFloor((int)x_, (int)y_, WIDTH, HEIGHT);
        x_ += floorMove.x;
        y_ += floorMove.y;
    }

    // 座標確定
    x_ = nextX;
    y_ = nextY;
}

void Player::Draw(const Camera& cam)
{
    int drawX = (int)(x_ - cam.GetX());
    int drawY = (int)(y_ - cam.GetY());

    DrawBox(drawX, drawY,
        drawX + WIDTH, drawY + HEIGHT,
        GetColor(0, 255, 0), TRUE);
}
