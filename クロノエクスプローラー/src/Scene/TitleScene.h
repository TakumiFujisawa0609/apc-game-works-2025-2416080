#pragma once
#include "../Scene/SceneBase.h"

// タイトル画面シーン
// - タイトルロゴを表示
// - メニュー選択（スタート / オプション / 終了）
// - 入力操作に応じてシーンを切り替える
class TitleScene : public SceneBase
{
public:
    static constexpr int MENU_COUNT = 3;   // メニューの数
    static constexpr int TITLE_Y = 100; // タイトル画像のY座標
    static constexpr int MENU_X = 250; // メニュー描画のX座標
    static constexpr int MENU_Y_START = 400; // メニュー描画の開始Y座標
    static constexpr int MENU_Y_STEP = 40;  // メニューの縦間隔
    static constexpr int MENU_COLOR_R = 255; // 非選択時の文字色（R）
    static constexpr int MENU_COLOR_G = 255; // 非選択時の文字色（G）
    static constexpr int MENU_COLOR_B = 255; // 非選択時の文字色（B）
    static constexpr int SELECT_COLOR_R = 255; // 選択中の文字色（R）
    static constexpr int SELECT_COLOR_G = 0;   // 選択中の文字色（G）
    static constexpr int SELECT_COLOR_B = 0;   // 選択中の文字色（B）

    TitleScene();
    ~TitleScene();

    void Init(void) override;    // シーン開始時の初期化処理
    void Update(void) override;  // 入力処理
    void Draw(void) override;    // 描画処理
    void Release(void) override; // リソース解放

private:
    int selectNum_;   // 現在選択中のメニュー番号（0=START,1=OPTION,2=EXIT）
    int titleImg_;    // タイトル画像のハンドル
};
