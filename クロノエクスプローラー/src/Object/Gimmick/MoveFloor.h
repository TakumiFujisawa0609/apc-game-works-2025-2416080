#pragma once
#include "GimmickBase.h"
#include "../../Manager/Camera.h"

// 移動床（横方向の往復移動）
class MoveFloor : public GimmickBase
{
public:
    static constexpr int WIDTH = 64;   // 幅
    static constexpr int HEIGHT = 16;   // 高さ
    static constexpr int SPEED = 2;    // 移動速度

    MoveFloor(Vector2 pos, int range);
    ~MoveFloor();

    void SetParam() override;
    void ActionUpdate() override;
    void ActionDraw(const Camera& camera); // カメラ対応版

    Vector2 GetMoveDiff() const { return moveDiff_; }

private:
    int startX_;   // 移動開始位置
    int range_;    // 移動範囲
    int dir_;      // 移動方向（1 or -1）
    Vector2 moveDiff_; // 移動差分
};
