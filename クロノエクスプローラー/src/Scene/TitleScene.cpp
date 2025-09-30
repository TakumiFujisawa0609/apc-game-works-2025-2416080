#include "TitleScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include "../Application.h"

TitleScene::TitleScene()
    : selectNum_(0),
    titleImg_(-1)
{
}

TitleScene::~TitleScene()
{
}

void TitleScene::Init(void)
{
    selectNum_ = 0;

    // タイトル画像の読み込み
    titleImg_ = LoadGraph((Application::PATH_IMAGE + "title.png").c_str());
}

void TitleScene::Update(void)
{
    auto& input = InputManager::GetInstance();

    // ↑キーで選択を上に移動
    if (input.IsTrgDown(KEY_INPUT_UP)) {
        selectNum_ = (selectNum_ - 1 + MENU_COUNT) % MENU_COUNT;
    }

    // ↓キーで選択を下に移動
    if (input.IsTrgDown(KEY_INPUT_DOWN)) {
        selectNum_ = (selectNum_ + 1) % MENU_COUNT;
    }

    // Spaceキーで決定
    if (input.IsTrgDown(KEY_INPUT_SPACE)) {
        if (selectNum_ == 0) {
            SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::GAME);
        }
        else if (selectNum_ == 1) {
            SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::OPTION);
        }
        else if (selectNum_ == 2) {
            DxLib_End(); // 終了
        }
    }
}

void TitleScene::Draw(void)
{
    // タイトル画像を中央に描画
    if (titleImg_ != -1) {
        int w, h;
        GetGraphSize(titleImg_, &w, &h);
        DrawGraph((Application::SCREEN_SIZE_X - w) / 2, TITLE_Y, titleImg_, TRUE);
    }

    // メニュー文字列
    const char* menuStr[MENU_COUNT] = { "START GAME", "OPTION", "EXIT" };

    // 各メニューを縦に並べて描画
    for (int i = 0; i < MENU_COUNT; i++) {
        int color = (i == selectNum_)
            ? GetColor(SELECT_COLOR_R, SELECT_COLOR_G, SELECT_COLOR_B)
            : GetColor(MENU_COLOR_R, MENU_COLOR_G, MENU_COLOR_B);

        DrawString(MENU_X, MENU_Y_START + i * MENU_Y_STEP, menuStr[i], color);
    }
}

void TitleScene::Release(void)
{
    if (titleImg_ != -1) {
        DeleteGraph(titleImg_);
        titleImg_ = -1;
    }
}
