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
    playerX_ = Application::SCREEN_SIZE_X / 2;
    playerY_ = Application::SCREEN_SIZE_Y / 2;
}

// 更新処理
// - 入力を受け取ってプレイヤーを動かす
// - ESCでタイトルへ戻る
void GameScene::Update(void)
{
    auto& input = InputManager::GetInstance();

    if (input.IsNew(KEY_INPUT_RIGHT)) playerX_ += 5;
    if (input.IsNew(KEY_INPUT_LEFT))  playerX_ -= 5;
    if (input.IsNew(KEY_INPUT_UP))    playerY_ -= 5;
    if (input.IsNew(KEY_INPUT_DOWN))  playerY_ += 5;

    // ESCキーでタイトルへ戻る
    if (input.IsTrgDown(KEY_INPUT_ESCAPE)) {
        SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::TITLE);
    }
}

// 描画処理
// - プレイヤー画像を表示
void GameScene::Draw(void)
{
    if (playerImg_ != -1) {
        DrawGraph(playerX_, playerY_, playerImg_, TRUE);
    }
    else {
        // 画像がない場合は四角で代用
        DrawBox(playerX_, playerY_, playerX_ + 32, playerY_ + 32, GetColor(0, 255, 0), TRUE);
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
}
