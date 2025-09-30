#include "GameScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include "../Application.h"

GameScene::GameScene()
    : playerX_(0),
    playerY_(0),
    playerImg_(-1),
    bgImg_(-1),
    stageWidth_(STAGE_WIDTH)
{
}

GameScene::~GameScene()
{
    Release();
}

void GameScene::Init(void)
{
    // プレイヤー画像読み込み
    playerImg_ = LoadGraph((Application::PATH_IMAGE + "player.png").c_str());

    // 背景画像読み込み
    bgImg_ = LoadGraph((Application::PATH_IMAGE + "game_bg.png").c_str());

    // プレイヤー初期位置
    playerX_ = PLAYER_INIT_X;
    playerY_ = PLAYER_INIT_Y;

    // カメラ初期化
    camera_.Init(stageWidth_);

    // ステージ初期化
    stage_.Init();

    // プレイヤー初期化
    player_.Init();
}

void GameScene::Update(void)
{
    auto& input = InputManager::GetInstance();

    // 移動（仮：個別に座標更新）
    if (input.IsNew(KEY_INPUT_D)) playerX_ += MOVE_SPEED;
    if (input.IsNew(KEY_INPUT_A)) playerX_ -= MOVE_SPEED;
    if (input.IsNew(KEY_INPUT_W)) playerY_ -= MOVE_SPEED;
    if (input.IsNew(KEY_INPUT_S)) playerY_ += MOVE_SPEED;

    // ステージ範囲チェック
    if (playerX_ < 0) playerX_ = 0;
    if (playerX_ > stageWidth_ - PLAYER_SIZE) playerX_ = stageWidth_ - PLAYER_SIZE;

    // プレイヤー更新（当たり判定など内部処理）
    player_.Update(stage_);

    // カメラ更新（プレイヤーX座標に追従）
    camera_.Update(player_.GetX());

    // TAB → ポーズシーンへ
    if (input.IsNew(KEY_INPUT_TAB)) {
        SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::PAUSE);
    }
}

void GameScene::Draw(void)
{
    // 背景描画
    if (bgImg_ != -1) {
        int bgW, bgH;
        GetGraphSize(bgImg_, &bgW, &bgH);

        // 横スクロール対応のタイル描画
        int startX = -(camera_.GetX() % bgW);
        for (int x = startX; x < Application::SCREEN_SIZE_X; x += bgW) {
            DrawGraph(x, 0, bgImg_, TRUE);
        }
    }
    else {
        // 背景画像がない場合は青で塗る
        DrawBox(0, 0,
            Application::SCREEN_SIZE_X, Application::SCREEN_SIZE_Y,
            GetColor(BG_COLOR_R, BG_COLOR_G, BG_COLOR_B), TRUE);
    }

    // ステージ描画
    stage_.Draw(camera_);

    // プレイヤー描画
    player_.Draw(camera_);
}

void GameScene::Release(void)
{
    if (playerImg_ != -1) {
        DeleteGraph(playerImg_);
        playerImg_ = -1;
    }

    if (bgImg_ != -1) {
        DeleteGraph(bgImg_);
        bgImg_ = -1;
    }
}
