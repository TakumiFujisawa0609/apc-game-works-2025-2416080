#include "MoveFloor.h"
#include <DxLib.h>

MoveFloor::MoveFloor(Vector2 pos, int range)
    : startX_((int)pos.x), range_(range), dir_(1)
{
    Create(pos);
    data_.type = Type::MOVE_FLOOR;
    moveDiff_ = { 0, 0 };
}

MoveFloor::~MoveFloor() {}

void MoveFloor::SetParam()
{
    data_.handle = -1; // 今は画像なし
}

void MoveFloor::ActionUpdate()
{
    int oldX = (int)data_.pos.x;

    // 横方向往復移動
    data_.pos.x += dir_ * SPEED;
    if (data_.pos.x > startX_ + range_ || data_.pos.x < startX_) {
        dir_ *= -1;
    }

    // 移動差分を保存（プレイヤー追従用などに使う）
    moveDiff_.x = (int)data_.pos.x - oldX;
    moveDiff_.y = 0;
}

void MoveFloor::ActionDraw(const Camera& camera)
{
    DrawBox(
        (int)(data_.pos.x - camera.GetX()),
        (int)(data_.pos.y - camera.GetY()),
        (int)(data_.pos.x + WIDTH - camera.GetX()),
        (int)(data_.pos.y + HEIGHT - camera.GetY()),
        GetColor(150, 75, 0), // 茶色
        TRUE
    );
}
