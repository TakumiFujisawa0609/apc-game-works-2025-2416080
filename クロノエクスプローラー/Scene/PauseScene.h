#pragma once
#include "SceneBase.h"

// ポーズシーン
// - ゲーム中に ESC で開く
// - Resume (再開)、Return to Title (タイトルへ戻る)
class PauseScene : public SceneBase
{
public:
    PauseScene();
    ~PauseScene();

    void Init(void) override;
    void Update(void) override;
    void Draw(void) override;
    void Release(void) override;

private:
    int selectNum_; // メニュー選択番号
};
