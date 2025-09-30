#include "Camera.h"
#include <algorithm>
#include <cmath>
#include "../Application.h"

// C++17 未満の場合、std::clamp を自前で実装
#ifndef __cpp_lib_clamp
namespace std {
    template <typename T>
    constexpr const T& clamp(const T& v, const T& lo, const T& hi) {
        return (v < lo) ? lo : (hi < v) ? hi : v;
    }
}
#endif

Camera::Camera()
    : cameraX_(0),
    targetX_(0),
    stageWidth_(0),
    followRate_(DEFAULT_FOLLOW_RATE) 
{
}

Camera::~Camera()
{
}

void Camera::Init(int stageWidth)
{
    stageWidth_ = stageWidth;
    cameraX_ = 0;
    targetX_ = 0;
    followRate_ = DEFAULT_FOLLOW_RATE; 
}

void Camera::Update(int playerX)
{
    // プレイヤーを画面中央に置く座標を目標にする
    targetX_ = playerX - Application::SCREEN_SIZE_X / 2;

    // 範囲外チェック（std::clampで制御）
    targetX_ = std::clamp(targetX_, 0, stageWidth_ - Application::SCREEN_SIZE_X);

    // Lerp（補間）で滑らかに追従
    cameraX_ = static_cast<int>(cameraX_ + (targetX_ - cameraX_) * followRate_);
}
