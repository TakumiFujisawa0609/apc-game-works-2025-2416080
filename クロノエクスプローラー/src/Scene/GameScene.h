#pragma once
#include "../Scene/SceneBase.h"

class GameScene : public SceneBase
{
public:

    GameScene();
    ~GameScene();

    void Init(void) override;
    void Update(void) override;
    void Draw(void) override;
    void Release(void) override;

private:
    int bgImg_;     // ”wŒi‰æ‘œ
    int stageWidth_;

};
