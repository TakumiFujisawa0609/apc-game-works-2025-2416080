#include "GameScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include "../Application.h"


GameScene::GameScene()
{
}

GameScene::~GameScene()
{
    Release();
}

void GameScene::Init(void)
{

}

void GameScene::Update(void)
{
    auto& input = InputManager::GetInstance();


    if (input.IsNew(KEY_INPUT_TAB)) {
        SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::PAUSE);
    }
}

void GameScene::Draw(void)
{
    // îwåi

}

void GameScene::Release(void)
{

    if (bgImg_ != -1) {
        DeleteGraph(bgImg_);
        bgImg_ = -1;
    }
}
