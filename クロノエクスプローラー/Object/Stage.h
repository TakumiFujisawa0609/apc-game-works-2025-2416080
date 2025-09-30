#pragma once
#include <vector>
#include "../Application.h"
#include "../Manager/Camera.h"

// ステージのブロック定義
struct Block {
    int x, y;   // 左上座標（ステージ座標）
    int w, h;   // 幅と高さ
};

// ステージ全体を管理するクラス
class Stage
{
public:
    Stage();
    ~Stage();

    void Init();                   // ステージ生成
    void Draw(const Camera& cam);  // カメラに合わせて描画

    const std::vector<Block>& GetBlocks() const { return blocks_; }

private:
    std::vector<Block> blocks_;    // ステージのブロック配列
};
