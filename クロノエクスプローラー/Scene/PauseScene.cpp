#include "PauseScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include "../Application.h"

PauseScene::PauseScene()
{
    selectNum_ = 0;
}

PauseScene::~PauseScene()
{
    Release();
}

void PauseScene::Init(void)
{
    selectNum_ = 0;
}

void PauseScene::Update(void)
{
    auto& input = InputManager::GetInstance();

    // 上下キーで選択移動
    if (input.IsTrgDown(KEY_INPUT_UP)) {
        selectNum_ = (selectNum_ - 1 + 2) % 2;
    }
    if (input.IsTrgDown(KEY_INPUT_DOWN)) {
        selectNum_ = (selectNum_ + 1) % 2;
    }

    // Spaceキーで決定
    if (input.IsTrgDown(KEY_INPUT_SPACE)) {
        if (selectNum_ == 0) {
            // Resume
            SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::GAME);
        }
        else if (selectNum_ == 1) {
            // Return to Title
            SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::TITLE);
        }
    }
}

void PauseScene::Draw(void)
{
    // 半透明の背景
    DrawBox(0, 0, Application::SCREEN_SIZE_X, Application::SCREEN_SIZE_Y,
        GetColor(0, 0, 0), TRUE);
    DrawBox(0, 0, Application::SCREEN_SIZE_X, Application::SCREEN_SIZE_Y,
        GetColor(255, 255, 255), FALSE);

    DrawString(200, 100, "PAUSE", GetColor(255, 255, 0));

    const char* menuStr[] = { "RESTART", "RETURN TO TITLE" };
    for (int i = 0; i < 2; i++) {
        int color = (i == selectNum_) ? GetColor(255, 0, 0) : GetColor(255, 255, 255);
        DrawString(250, 200 + i * 40, menuStr[i], color);
    }
}

void PauseScene::Release(void)
{
    // 今はリソースなし
}
