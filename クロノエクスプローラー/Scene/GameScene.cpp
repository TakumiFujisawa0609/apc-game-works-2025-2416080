#include "GameScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include "../Application.h"

GameScene::GameScene()
{
    playerX_ = 0;
    playerY_ = 0;
    playerImg_ = -1;
    bgImg_ = -1;
    stageWidth_ = 3000; // ステージの横幅
}

GameScene::~GameScene()
{
    Release();
}

// 初期化処理
// - プレイヤー画像を読み込み
// - プレイヤーの初期位置をセット
void GameScene::Init(void)
{
    playerImg_ = LoadGraph((Application::PATH_IMAGE + "player.png").c_str());

    bgImg_ = LoadGraph((Application::PATH_IMAGE + "game_bg.png").c_str());

    playerX_ = Application::SCREEN_SIZE_X / 2;
    playerY_ = Application::SCREEN_SIZE_Y / 2;

    camera_.Init(stageWidth_);

}

// 更新処理
// - 入力を受け取ってプレイヤーを動かす
// - ESCでタイトルへ戻る
void GameScene::Update(void)
{
    auto& input = InputManager::GetInstance();

    if (input.IsNew(KEY_INPUT_D)) playerX_ += 5;
    if (input.IsNew(KEY_INPUT_A))  playerX_ -= 5;
    if (input.IsNew(KEY_INPUT_W))    playerY_ -= 5;
    if (input.IsNew(KEY_INPUT_S))  playerY_ += 5;

    // ステージ範囲チェック
    if (playerX_ < 0) playerX_ = 0;
    if (playerX_ > stageWidth_ - 32) playerX_ = stageWidth_ - 32;

    // カメラを更新
    camera_.Update(playerX_);

    // ESCキーでタイトルへ戻る
    if (input.IsTrgDown(KEY_INPUT_ESCAPE)) {
        SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::TITLE);
    }
}

// 描画処理
// - プレイヤー画像を表示
void GameScene::Draw(void)
{
    if (bgImg_ != -1) {
        int bgW, bgH;
        GetGraphSize(bgImg_, &bgW, &bgH);

        // 背景をタイル状に並べて描画（横スクロール対応）
        int startX = -(camera_.GetX() % bgW);
        for (int x = startX; x < Application::SCREEN_SIZE_X; x += bgW) {
            DrawGraph(x, 0, bgImg_, TRUE);
        }
    }
    else {
        // 画像がない場合は四角で代用
        DrawBox(0, 0, Application::SCREEN_SIZE_X, Application::SCREEN_SIZE_Y, GetColor(0, 0, 255), TRUE);
    }

    // プレイヤー描画（カメラ補正）
    if (playerImg_ != -1) {
        DrawGraph(playerX_ - camera_.GetX(), playerY_, playerImg_, TRUE);
    }
    else {
        DrawBox(playerX_ - camera_.GetX(), playerY_,
            playerX_ - camera_.GetX() + 32, playerY_ + 32,
            GetColor(0, 255, 0), TRUE);
    }
}

// 解放処理
// - 読み込んだ画像を削除
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
