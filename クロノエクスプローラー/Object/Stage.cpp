#include "Stage.h"
#include <DxLib.h>

Stage::Stage()
{
}

Stage::~Stage()
{
}

void Stage::Init()
{
    blocks_.clear();

    // --- サンプルステージ ---
    // 地面
    blocks_.push_back({ 0, Application::SCREEN_SIZE_Y - 50, 3000, 50 });

    // 足場ブロック
    blocks_.push_back({ 400, 400, 200, 30 });
    blocks_.push_back({ 800, 300, 200, 30 });
    blocks_.push_back({ 1200, 350, 200, 30 });
    blocks_.push_back({ 1600, 250, 200, 30 });
}

void Stage::Draw(const Camera& cam)
{
    for (auto& b : blocks_) {
        DrawBox(b.x - cam.GetX(), b.y,
            b.x + b.w - cam.GetX(), b.y + b.h,
            GetColor(150, 75, 0), TRUE);  // 茶色で床を描画
    }
}
