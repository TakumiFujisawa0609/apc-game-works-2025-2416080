#pragma once
#include "../Application.h"

// 2D横スクロール用カメラクラス
class Camera
{
public:
    Camera();
    ~Camera();

    void Init(int stageWidth);             // 初期化（ステージの横幅を指定）
    void Update(int playerX);              // プレイヤー座標に応じて更新
    int GetX() const { return cameraX_; }  // カメラのX座標を取得

private:
    int cameraX_;      // カメラ位置（左端座標）
    int targetX_;      // プレイヤーに基づいた目標カメラX
    int stageWidth_;   // ステージの全体幅
	float followRate_; // カメラの追従率
};