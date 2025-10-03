#include "GameScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include "../Application.h"
#include "../Object/Gimmick/MoveFloor.h"


GameScene::GameScene()
    : 
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
    //bgImg_ = LoadGraph((Application::PATH_IMAGE + "game_bg.png").c_str());
    stage_.LoadFromTiled(Application::PATH_MAP_DATA + "stage1.csv");

    gimmickManager_.Add(new MoveFloor({ 200, 300 }, 100));

    // ステージからプレイヤー初期位置を決定
    Vector2 spawn = stage_.GetSpawnPoint();
    player_.Init(spawn.x, spawn.y);

    camera_.Init(Stage::STAGE_WIDTH);
}

void GameScene::Update(void)
{
    auto& input = InputManager::GetInstance();

    // プレイヤー更新
    player_.Update(stage_, gimmickManager_);

    // カメラ更新
    camera_.Update(player_.GetX());

    gimmickManager_.Update();

    if (input.IsNew(KEY_INPUT_TAB)) {
        SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::PAUSE);
    }
}

void GameScene::Draw(void)
{
    // 背景
    DrawBox(0, 0,
        Application::SCREEN_SIZE_X, Application::SCREEN_SIZE_Y,
        GetColor(BG_COLOR_R, BG_COLOR_G, BG_COLOR_B), TRUE);

     //ステージ
    stage_.Draw(camera_);

    // プレイヤー（カメラ座標対応済み）
    player_.Draw(camera_);

    // ギミック
    gimmickManager_.Draw(camera_);
}

void GameScene::Release(void)
{

    if (bgImg_ != -1) {
        DeleteGraph(bgImg_);
        bgImg_ = -1;
    }
}
