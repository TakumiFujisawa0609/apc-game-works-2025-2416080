#include "GimmickBase.h"
#include <DxLib.h>

// コンストラクタ
GimmickBase::GimmickBase()
{
    state_ = State::STAY;
    data_.pos = { 0, 0 };
    data_.type = Type::NONE;
    data_.handle = -1;
    data_.isAlive = true;
}

// デストラクタ
GimmickBase::~GimmickBase()
{
    Release();
}

// 初期化
void GimmickBase::Init()
{
    SetParam();
}

// 更新処理
bool GimmickBase::Update()
{
    if (!data_.isAlive) return false;

    switch (state_)
    {
    case State::STAY:   StayUpdate();   break;
    case State::ACTION: ActionUpdate(); break;
    case State::END:    data_.isAlive = false; break;
    }
    return data_.isAlive;
}

// 描画処理
void GimmickBase::Draw(const Camera& cam)
{
    if (!data_.isAlive) return;

    switch (state_)
    {
    case State::STAY:   StayDraw(cam);   break;
    case State::ACTION: ActionDraw(cam); break;
    default: break;
    }
}

// 解放処理
void GimmickBase::Release()
{
    if (data_.handle != -1) {
        DeleteGraph(data_.handle);
        data_.handle = -1;
    }
}

// 生成
void GimmickBase::Create(Vector2 pos)
{
    data_.pos = pos;
    data_.isAlive = true;
    state_ = State::STAY;
    Init();
}

// デフォルトのStay描画（今は無描画 or デバッグ用色に変更）
void GimmickBase::StayDraw(const Camera& cam)
{
    // 例: デバッグ表示したい場合だけ有効化
    // DrawBox((int)(data_.pos.x - cam.GetX()), (int)(data_.pos.y - cam.GetY()),
    //         (int)(data_.pos.x + SIZE - cam.GetX()), (int)(data_.pos.y + SIZE - cam.GetY()),
    //         GetColor(200, 0, 200), TRUE);
}

// デフォルトの当たり判定
bool GimmickBase::Collision(int px, int py, int pw, int ph)
{
    int gx = static_cast<int>(data_.pos.x);
    int gy = static_cast<int>(data_.pos.y);

    return (px < gx + SIZE &&
        px + pw > gx &&
        py < gy + SIZE &&
        py + ph > gy);
}
