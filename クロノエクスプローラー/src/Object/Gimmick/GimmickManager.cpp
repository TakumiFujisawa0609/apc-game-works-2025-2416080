#include "GimmickManager.h"

GimmickManager::~GimmickManager()
{
    Release();
}

void GimmickManager::Init()
{
    for (auto g : gimmicks_) g->Init();
}

void GimmickManager::Update()
{
    for (auto it = gimmicks_.begin(); it != gimmicks_.end(); )
    {
        if (!(*it)->Update()) {
            delete* it;
            it = gimmicks_.erase(it);
        }
        else {
            ++it;
        }
    }
}

void GimmickManager::Draw(const Camera& cam)
{
    for (auto g : gimmicks_) g->Draw(cam);  // C³“_
}

void GimmickManager::Release()
{
    for (auto g : gimmicks_) delete g;
    gimmicks_.clear();
}

void GimmickManager::Add(GimmickBase* gimmick)
{
    gimmicks_.push_back(gimmick);
}

Vector2 GimmickManager::CheckOnMoveFloor(int px, int py, int pw, int ph)
{
    for (auto g : gimmicks_) {
        if (g->GetType() == GimmickBase::Type::MOVE_FLOOR) {
            if (g->Collision(px, py + 1, pw, ph)) {
                return dynamic_cast<MoveFloor*>(g)->GetMoveDiff();
            }
        }
    }
    return { 0, 0 };
}
