#pragma once
#include "../Scene/SceneBase.h"
#include "../Manager/Camera.h"
#include "../Object/Stage.h"
#include "../Object/Player.h"
#include "../Object/Gimmick/GimmickManager.h"

class GameScene : public SceneBase
{
public:
    static constexpr int STAGE_WIDTH = 3000; // ステージの横幅
    static constexpr int BG_COLOR_R = 0;
    static constexpr int BG_COLOR_G = 0;
    static constexpr int BG_COLOR_B = 255;

    GameScene();
    ~GameScene();

    void Init(void) override;
    void Update(void) override;
    void Draw(void) override;
    void Release(void) override;

private:
    int bgImg_;     // 背景画像
    int stageWidth_;

    Camera camera_;
    Stage stage_;
    Player player_;  // これだけで管理
    GimmickManager gimmickManager_;
};
