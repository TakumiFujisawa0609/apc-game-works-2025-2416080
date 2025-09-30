#define NOMINMAX
#include "OptionScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include <algorithm>

OptionScene::OptionScene()
{
    selectNum_ = 0;
    bgmVolume_ = 50; // デフォルト音量
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
        selectNum_ = (selectNum_ - 1 + 3) % 3;
    }
    if (input.IsTrgDown(KEY_INPUT_DOWN)) {
        selectNum_ = (selectNum_ + 1) % 3;
    }

    // 左右キーで音量調整（選択が音量のときだけ）
    if (selectNum_ == 0) {
        if (input.IsNew(KEY_INPUT_LEFT)) {
            bgmVolume_ = std::max(0, bgmVolume_ - 5);
        }
        if (input.IsNew(KEY_INPUT_RIGHT)) {
            bgmVolume_ = std::min(100, bgmVolume_ + 5);
        }
        // DxLib の BGM 音量調整に反映
        ChangeVolumeSoundMem(bgmVolume_ * 255 / 100, 1);
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
    DrawString(200, 100, "OPTION", GetColor(255, 255, 0));

    // メニュー項目
    const char* menuStr[] = { "BGM VOLUME", "CONTROL INFO", "RETURN TO TITLE" };

    for (int i = 0; i < 3; i++) {
        int color = (i == selectNum_) ? GetColor(255, 0, 0) : GetColor(255, 255, 255);
        DrawString(250, 200 + i * 40, menuStr[i], color);

        // 音量バーの表示
        if (i == 0) {
            DrawBox(450, 200, 650, 220, GetColor(100, 100, 100), TRUE);
            DrawBox(450, 200, 450 + (bgmVolume_ * 2), 220, GetColor(0, 255, 0), TRUE);
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
