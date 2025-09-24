#pragma once
#include "../Scene/SceneBase.h"

// ゲーム本編のシーン
// - プレイヤーの操作テスト
// - ステージ背景や簡単な敵の雛形を後で追加予定
class GameScene : public SceneBase
{
public:
    GameScene();
    ~GameScene();

    void Init(void) override;    // 初期化
    void Update(void) override;  // 更新
    void Draw(void) override;    // 描画
    void Release(void) override; // 解放

private:

    int playerX_;   // プレイヤーのX座標
    int playerY_;   // プレイヤーのY座標
    int playerImg_; // プレイヤーの画像ハンドル
};
