#pragma once
#include "SceneBase.h"

// オプション画面シーン
// - 音量調整、操作説明、タイトルに戻る
class OptionScene : public SceneBase
{
public:
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
