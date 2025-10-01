#pragma once
#include "Stage.h"
#include "../Object/Gimmick/GimmickManager.h"

class Player
{
public:
    static constexpr int WIDTH = 32;
    static constexpr int HEIGHT = 32;

    static constexpr float GRAVITY = 0.4f;
    static constexpr float JUMP_POWER = 8.0f;
    static constexpr float MAX_FALL_SPEED = 12.0f;
    static constexpr float MOVE_SPEED = 3.0f;

    static constexpr float JUMP_CHARGE_RATE = 0.3f;
    static constexpr float MAX_JUMP_BOOST = 5.0f;

    static constexpr int DASH_SPEED = 6;

    Player();
    ~Player();

    // C³: ‰ŠúÀ•W‚ğˆø”‚Åó‚¯æ‚é
    void Init(float startX, float startY);

    void Update(const Stage& stage, GimmickManager& gimmickManager);
    void Draw(const Camera& cam);

    float GetX() const { return x_; }
    float GetY() const { return y_; }

private:
    float x_;
    float y_;
    float velocityX_;
    float velocityY_;
    bool onGround_;
    bool isJumping_;
    float jumpCharge_;
    bool canJump_;
};
