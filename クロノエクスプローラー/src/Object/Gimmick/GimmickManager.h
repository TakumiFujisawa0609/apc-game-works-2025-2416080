#pragma once
#include <vector>
#include "GimmickBase.h"
#include "MoveFloor.h"
#include "../../Manager/Camera.h"
#include "../../Common/Vector2.h"

class GimmickManager
{
public:
    GimmickManager() {}
    ~GimmickManager();

    void Init();
    void Update();
    void Draw(const Camera& cam);   // Åö CameraëŒâû
    void Release();

    void Add(GimmickBase* gimmick);
    Vector2 CheckOnMoveFloor(int px, int py, int pw, int ph);

private:
    std::vector<GimmickBase*> gimmicks_;
};
