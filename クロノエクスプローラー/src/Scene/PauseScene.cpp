#include "PauseScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include "../Application.h"

PauseScene::PauseScene()
    : selectNum_(0)
{
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
        selectNum_ = (selectNum_ - 1 + MENU_COUNT) % MENU_COUNT;
    }
    if (input.IsTrgDown(KEY_INPUT_DOWN)) {
        selectNum_ = (selectNum_ + 1) % MENU_COUNT;
    }

    // Spaceキーで決定
    if (input.IsTrgDown(KEY_INPUT_SPACE)) {
        if (selectNum_ == 0) {
            // RESTART（ゲーム再開）
            SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::GAME);
        }
        else if (selectNum_ == 1) {
            // RETURN TO TITLE
            SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::TITLE);
        }
    }
}

void PauseScene::Draw(void)
{
    // 半透明背景
    DrawBox(0, 0,
        Application::SCREEN_SIZE_X, Application::SCREEN_SIZE_Y,
        GetColor(COLOR_BG_R, COLOR_BG_G, COLOR_BG_B), TRUE);

    // 外枠（白）
    DrawBox(0, 0,
        Application::SCREEN_SIZE_X, Application::SCREEN_SIZE_Y,
        GetColor(COLOR_NORMAL_R, COLOR_NORMAL_G, COLOR_NORMAL_B), FALSE);

    // タイトル描画
    DrawString(TITLE_X, TITLE_Y, "PAUSE",
        GetColor(COLOR_TITLE_R, COLOR_TITLE_G, COLOR_TITLE_B));

    // メニュー文字列
    const char* menuStr[MENU_COUNT] = { "RESTART", "RETURN TO TITLE" };
    for (int i = 0; i < MENU_COUNT; i++) {
        int color = (i == selectNum_)
            ? GetColor(COLOR_SELECT_R, COLOR_SELECT_G, COLOR_SELECT_B)
            : GetColor(COLOR_NORMAL_R, COLOR_NORMAL_G, COLOR_NORMAL_B);

        DrawString(MENU_X, MENU_Y_START + i * MENU_Y_STEP, menuStr[i], color);
    }
}

void PauseScene::Release(void)
{
    // 今はリソースなし
}
