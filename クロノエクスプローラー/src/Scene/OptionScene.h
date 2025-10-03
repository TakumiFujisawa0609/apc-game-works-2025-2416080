#pragma once
#include "SceneBase.h"

// オプション画面シーン
// - 音量調整、操作説明、タイトルに戻る
class OptionScene : public SceneBase
{
public:

    static constexpr int MENU_COUNT = 3;   // メニュー数
    static constexpr int TITLE_X = 200; // タイトル文字のX座標
    static constexpr int TITLE_Y = 100; // タイトル文字のY座標
    static constexpr int MENU_X = 250; // メニューのX座標
    static constexpr int MENU_Y_START = 200; // メニュー開始位置のY座標
    static constexpr int MENU_Y_STEP = 40;  // メニューの縦間隔
    static constexpr int COLOR_TITLE_R = 255; // タイトル色 (R)
    static constexpr int COLOR_TITLE_G = 255; // タイトル色 (G)
    static constexpr int COLOR_TITLE_B = 0;   // タイトル色 (B)
    static constexpr int COLOR_SELECT_R = 255; // 選択中の色 (R)
    static constexpr int COLOR_SELECT_G = 0;   // 選択中の色 (G)
    static constexpr int COLOR_SELECT_B = 0;   // 選択中の色 (B)
    static constexpr int COLOR_NORMAL_R = 255; // 非選択色 (R)
    static constexpr int COLOR_NORMAL_G = 255; // 非選択色 (G)
    static constexpr int COLOR_NORMAL_B = 255; // 非選択色 (B)
    static constexpr int VOLUME_STEP = 5;   // 音量の増減幅
    static constexpr int VOLUME_MIN = 0;   // 音量の最小値
    static constexpr int VOLUME_MAX = 100; // 音量の最大値
    static constexpr int VOLUME_BAR_X1 = 450; // 音量バー左上X
    static constexpr int VOLUME_BAR_Y1 = 200; // 音量バー左上Y
    static constexpr int VOLUME_BAR_X2 = 650; // 音量バー右下X
    static constexpr int VOLUME_BAR_Y2 = 220; // 音量バー右下Y
    static constexpr int VOLUME_BAR_COLOR_R = 100; // 音量バー背景色
    static constexpr int VOLUME_BAR_COLOR_G = 100;
    static constexpr int VOLUME_BAR_COLOR_B = 100;
    static constexpr int VOLUME_FILL_COLOR_R = 0;  // 音量バー塗り色
    static constexpr int VOLUME_FILL_COLOR_G = 255;
    static constexpr int VOLUME_FILL_COLOR_B = 0;

    OptionScene();
    ~OptionScene();

    void Init(void) override;
    void Update(void) override;
    void Draw(void) override;
    void Release(void) override;

private:
    int selectNum_;     // 現在の選択項目
    int bgmVolume_;     // BGM音量（0〜100）
};
