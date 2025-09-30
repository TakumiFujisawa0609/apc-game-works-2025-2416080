#define NOMINMAX
#include "OptionScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include <algorithm>

OptionScene::OptionScene()
    : selectNum_(0),
    bgmVolume_(50) // デフォルト音量
{
}

OptionScene::~OptionScene()
{
    Release();
}

void OptionScene::Init(void)
{
    selectNum_ = 0;
}

void OptionScene::Update(void)
{
    auto& input = InputManager::GetInstance();

    // 上下でメニュー移動
    if (input.IsTrgDown(KEY_INPUT_UP)) {
        selectNum_ = (selectNum_ - 1 + MENU_COUNT) % MENU_COUNT;
    }
    if (input.IsTrgDown(KEY_INPUT_DOWN)) {
        selectNum_ = (selectNum_ + 1) % MENU_COUNT;
    }

    // 左右キーで音量調整（選択が音量のときだけ）
    if (selectNum_ == 0) {
        if (input.IsNew(KEY_INPUT_LEFT)) {
            bgmVolume_ = std::max(VOLUME_MIN, bgmVolume_ - VOLUME_STEP);
        }
        if (input.IsNew(KEY_INPUT_RIGHT)) {
            bgmVolume_ = std::min(VOLUME_MAX, bgmVolume_ + VOLUME_STEP);
        }
        // DxLib の BGM 音量調整に反映
        ChangeVolumeSoundMem(bgmVolume_ * 255 / VOLUME_MAX, 1);
    }

    // Spaceキーで決定
    if (input.IsTrgDown(KEY_INPUT_SPACE)) {
        if (selectNum_ == 2) {
            // 「Return to Title」で戻る
            SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::TITLE);
        }
    }
}

void OptionScene::Draw(void)
{
    // タイトル表示
    DrawString(TITLE_X, TITLE_Y, "OPTION",
        GetColor(COLOR_TITLE_R, COLOR_TITLE_G, COLOR_TITLE_B));

    // メニュー項目
    const char* menuStr[MENU_COUNT] = { "BGM VOLUME", "CONTROL INFO", "RETURN TO TITLE" };

    for (int i = 0; i < MENU_COUNT; i++) {
        int color = (i == selectNum_)
            ? GetColor(COLOR_SELECT_R, COLOR_SELECT_G, COLOR_SELECT_B)
            : GetColor(COLOR_NORMAL_R, COLOR_NORMAL_G, COLOR_NORMAL_B);

        DrawString(MENU_X, MENU_Y_START + i * MENU_Y_STEP, menuStr[i], color);

        // 音量バーの表示（BGM VOLUME のときだけ）
        if (i == 0) {
            DrawBox(VOLUME_BAR_X1, VOLUME_BAR_Y1,
                VOLUME_BAR_X2, VOLUME_BAR_Y2,
                GetColor(VOLUME_BAR_COLOR_R, VOLUME_BAR_COLOR_G, VOLUME_BAR_COLOR_B),
                TRUE);

            DrawBox(VOLUME_BAR_X1, VOLUME_BAR_Y1,
                VOLUME_BAR_X1 + (bgmVolume_ * 2),
                VOLUME_BAR_Y2,
                GetColor(VOLUME_FILL_COLOR_R, VOLUME_FILL_COLOR_G, VOLUME_FILL_COLOR_B),
                TRUE);
        }
    }

    // 操作説明（選択が CONTROL INFO のとき）
    if (selectNum_ == 1) {
        DrawString(200, 400, "Arrow Keys : Move", GetColor(255, 255, 255));
        DrawString(200, 430, "Space Key  : Action/Select", GetColor(255, 255, 255));
        DrawString(200, 460, "ESC Key    : Pause/Back", GetColor(255, 255, 255));
    }
}

void OptionScene::Release(void)
{
    // 今はリソースなし
}
