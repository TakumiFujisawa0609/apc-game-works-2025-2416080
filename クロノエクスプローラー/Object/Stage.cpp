#include "Stage.h"
#include <DxLib.h>

Stage::Stage() {}
Stage::~Stage() {}

void Stage::Init()
{
    blocks_.clear();

    // --- サンプルステージ ---

    // 地面（画面下に横長の床）
    blocks_.push_back({
        0,
        Application::SCREEN_SIZE_Y - FLOOR_HEIGHT,
        STAGE_WIDTH,
        FLOOR_HEIGHT
        });

    // 足場ブロック
    blocks_.push_back({ 400, 400, BLOCK_WIDTH, BLOCK_HEIGHT });
    blocks_.push_back({ 800, 300, BLOCK_WIDTH, BLOCK_HEIGHT });
    blocks_.push_back({ 1200, 350, BLOCK_WIDTH, BLOCK_HEIGHT });
    blocks_.push_back({ 1600, 250, BLOCK_WIDTH, BLOCK_HEIGHT });
}

void Stage::Draw(const Camera& cam)
{
    for (auto& b : blocks_) {
        DrawBox(b.x - cam.GetX(), b.y,
            b.x + b.w - cam.GetX(), b.y + b.h,
            GetColor(BLOCK_COLOR_R, BLOCK_COLOR_G, BLOCK_COLOR_B),
            TRUE);
    }
}
