#pragma once
#include "../Scene/SceneBase.h"

// タイトル画面シーン
// - タイトルロゴを表示
// - メニュー選択（スタート / オプション / 終了）
// - 入力操作に応じてシーンを切り替える
class TitleScene : public SceneBase
{
public:

    TitleScene();   // コンストラクタ
    ~TitleScene();  // デストラクタ

    // シーン開始時の初期化処理
    void Init(void) override;

    // 毎フレームの更新処理（入力チェックなど）
    void Update(void) override;

    // 毎フレームの描画処理
    void Draw(void) override;

    // シーン終了時のリソース解放
    void Release(void) override;

private:

    int selectNum_;   // 現在選択中のメニュー番号（0=START,1=OPTION,2=EXIT）

    int titleImg_;    // タイトル画像のハンドル
};