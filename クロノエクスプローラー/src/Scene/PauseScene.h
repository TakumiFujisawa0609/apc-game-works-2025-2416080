#pragma once
#include "SceneBase.h"

// ポーズシーン
// - TABキーで開く
// - RESTART（やり直し）、RETURN TO TITLE（タイトルへ戻る）
class PauseScene : public SceneBase
{
public:

    static constexpr int MENU_COUNT = 2;   // メニューの数
    static constexpr int TITLE_X = 200; // タイトル文字X座標
    static constexpr int TITLE_Y = 100; // タイトル文字Y座標
    static constexpr int MENU_X = 250; // メニュー文字X座標
    static constexpr int MENU_Y_START = 200; // メニュー開始Y座標
    static constexpr int MENU_Y_STEP = 40;  // メニュー間隔
    static constexpr int COLOR_BG_R = 0;   // 背景色（R）
    static constexpr int COLOR_BG_G = 0;   // 背景色（G）
    static constexpr int COLOR_BG_B = 0;   // 背景色（B）
    static constexpr int COLOR_TITLE_R = 255; // タイトル色（R）
    static constexpr int COLOR_TITLE_G = 255;
    static constexpr int COLOR_TITLE_B = 0;
    static constexpr int COLOR_SELECT_R = 255; // 選択中の色
    static constexpr int COLOR_SELECT_G = 0;
    static constexpr int COLOR_SELECT_B = 0;
    static constexpr int COLOR_NORMAL_R = 255; // 非選択時の色
    static constexpr int COLOR_NORMAL_G = 255;
    static constexpr int COLOR_NORMAL_B = 255;

    PauseScene();
    ~PauseScene();

    void Init(void) override;
    void Update(void) override;
    void Draw(void) override;
    void Release(void) override;

private:
    int selectNum_; // メニュー選択番号
};
