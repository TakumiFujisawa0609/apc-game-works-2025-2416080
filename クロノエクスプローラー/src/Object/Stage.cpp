#include "Stage.h"
#include <DxLib.h>
#include <fstream>
#include <sstream>
#include "../Common/Vector2.h"
#include "../Object/Player.h"

Stage::Stage() 
{
}

Stage::~Stage() 
{
}

void Stage::Init()
{
    blocks_.clear();

    // デフォルトの床（仮に置く場合、Tiled読み込みと併用しないならコメントアウト可）
    blocks_.push_back({
        0,
        Application::SCREEN_SIZE_Y - FLOOR_HEIGHT,
        STAGE_WIDTH,
        FLOOR_HEIGHT
    });
}

void Stage::LoadFromTiled(const std::string& filename)
{
    blocks_.clear();

    std::ifstream file(filename);
    if (!file.is_open()) return;

    std::string line;
    int y = 0;

    while (std::getline(file, line))
    {
        std::stringstream ss(line);
        std::string value;
        int x = 0;

        while (std::getline(ss, value, ',')) {
            if (value.empty()) {
                x++;
                continue; // 空白は無視
            }

            int tileId = std::stoi(value);

            if (tileId == -1) {
                x++;
                continue; // -1 も無視
            }


            if (tileId == 41) { 
                Block block;
                block.x = x * BLOCK_WIDTH;
                block.y = y * BLOCK_HEIGHT;
                block.w = BLOCK_WIDTH;
                block.h = BLOCK_HEIGHT;
                blocks_.push_back(block);
            }
            x++;

        }
        y++;


    }

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

Vector2 Stage::GetSpawnPoint() const
{
    if (blocks_.empty()) return { 64, 64 };

    const Block* spawnBlock = nullptr;

    // 一番下にあるブロックを探す（y が最大のもの）
    for (auto& block : blocks_) {
        if (!spawnBlock || block.y > spawnBlock->y) {
            spawnBlock = &block;
        }
    }

    return { spawnBlock->x + 200, spawnBlock->y - Player::HEIGHT };
}