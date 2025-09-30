#pragma once
#include "../Scene/SceneBase.h"
#include "../Manager/Camera.h"
#include "../Object/Stage.h"
#include "../Object/Player.h"

// ゲーム本編のシーン
// - プレイヤー操作とステージ管理
class GameScene : public SceneBase
{
public:
    static constexpr int STAGE_WIDTH = 3000; // ステージの横幅
    static constexpr int PLAYER_INIT_X = Application::SCREEN_SIZE_X / 2; // 初期位置X
    static constexpr int PLAYER_INIT_Y = Application::SCREEN_SIZE_Y / 2; // 初期位置Y
    static constexpr int MOVE_SPEED = 5;    // プレイヤー移動速度（仮）
    static constexpr int PLAYER_SIZE = 32;   // プレイヤーの幅（判定用）
    static constexpr int BG_COLOR_R = 0;    // 背景代替色（R）
    static constexpr int BG_COLOR_G = 0;    // 背景代替色（G）
    static constexpr int BG_COLOR_B = 255;  // 背景代替色（B）

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

    int bgImg_;     // 背景画像のハンドル

    int stageWidth_;

    Camera camera_; // カメラ
    Stage stage_;   // ステージ
    Player player_; // プレイヤー
};
