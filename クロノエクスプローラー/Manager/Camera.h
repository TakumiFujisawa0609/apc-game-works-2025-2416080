#pragma once
#include "../Application.h"

// 2D横スクロール用カメラクラス（プレイヤーに滑らかに追従）
class Camera
{
public:

    static constexpr float DEFAULT_FOLLOW_RATE = 0.15f; // デフォルト追従率
    static constexpr float MIN_FOLLOW_RATE = 0.05f; // 最低追従率
    static constexpr float MAX_FOLLOW_RATE = 0.3f;  // 最高追従率

    Camera();
    ~Camera();

    void Init(int stageWidth);              // 初期化
    void Update(int playerX);               // プレイヤー座標に基づいて更新
    int GetX() const { return cameraX_; }   // カメラ座標を取得

private:

    int cameraX_;       // 現在のカメラX
    int targetX_;       // プレイヤーに基づいた目標カメラX
    int stageWidth_;    // ステージ幅
    float followRate_;  // 追従率（0.1?0.2くらいが自然）
};
