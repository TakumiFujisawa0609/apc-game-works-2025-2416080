#pragma once
#include <vector>
#include "../Application.h"
#include "../Manager/Camera.h"
#include "../Common/Vector2.h"

// ステージのブロック定義
struct Block {
    int x, y;   // 左上座標（ステージ座標）
    int w, h;   // 幅と高さ
};

// ステージ全体を管理するクラス
class Stage
{
public:

    static constexpr int STAGE_WIDTH = 3000; // ステージ全体の幅
    static constexpr int FLOOR_HEIGHT = 50;   // 地面の高さ
    static constexpr int BLOCK_WIDTH = 200;  // 足場ブロックの幅
    static constexpr int BLOCK_HEIGHT = 30;   // 足場ブロックの高さ
    static constexpr int GROUND_Y_OFFSET = 50; // 画面下からのオフセット
    static constexpr int BLOCK_COLOR_R = 100;  // ブロック色（R）
    static constexpr int BLOCK_COLOR_G = 100;   // ブロック色（G）
    static constexpr int BLOCK_COLOR_B = 100;    // ブロック色（B）

    Stage();
    ~Stage();

    void Init();                   // ステージ生成
    void Draw(const Camera& cam);  // カメラに合わせて描画

    const std::vector<Block>& GetBlocks() const { return blocks_; }

    void LoadFromTiled(const std::string& filename);

    Vector2 GetSpawnPoint() const;

private:
    std::vector<Block> blocks_;    // ステージのブロック配列


};
