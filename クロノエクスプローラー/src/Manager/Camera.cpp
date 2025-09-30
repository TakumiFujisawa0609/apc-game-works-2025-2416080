#include "Camera.h"
#include <algorithm>
#include <cmath>

// clamp 関数が C++17 以降で導入されたため、C++17 をサポートしていない場合は以下のように独自実装を追加します。
template <typename T>
constexpr const T& Clamp(const T& value, const T& min, const T& max) {
    return (value < min) ? min : (value > max) ? max : value;
}

Camera::Camera()
{
    cameraX_ = 0;
    targetX_ = 0;
    stageWidth_ = 0;
    followRate_ = 0.15f; // カメラの追従速度（0.0f?1.0f）
}

Camera::~Camera()
{
}

void Camera::Init(int stageWidth)
{
    stageWidth_ = stageWidth;
    cameraX_ = 0;
    targetX_ = 0;
}

void Camera::Update(int playerX)
{
    // プレイヤーを画面中央に置く座標を目標にする
    targetX_ = playerX - Application::SCREEN_SIZE_X / 2;

    // 範囲外チェック
    targetX_ = Clamp(targetX_, 0, stageWidth_ - Application::SCREEN_SIZE_X);

    // Lerp（補間）で滑らかに追従
    cameraX_ = static_cast<int>(cameraX_ + (targetX_ - cameraX_) * followRate_);
}
