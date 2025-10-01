#pragma once
#include "../../Manager/Camera.h"
#include "../../Common/Vector2.h"

class GimmickBase
{
public:
    enum class Type {
        NONE = -1,
        MOVE_FLOOR,
        SPIKE,
        CRYSTAL,
    };

    enum class State {
        STAY,
        ACTION,
        END,
    };

    struct Data {
        Vector2 pos;   // 座標
        Type type;     // ギミック種類
        int handle;    // 画像ハンドル
        bool isAlive;  // 生存フラグ
    };

    static constexpr int SIZE = 32;
    static constexpr int SIZE_HALF = SIZE / 2;

    GimmickBase();
    virtual ~GimmickBase();

    void Init();
    bool Update();
    void Draw(const Camera& cam);   // Camera対応
    void Release();

    void Create(Vector2 pos);

    // --- 継承先で実装する ---
    virtual void SetParam() = 0;
    virtual void ActionUpdate() {}
    virtual void ActionDraw(const Camera& cam) {}
    virtual void StayUpdate() {}
    virtual void StayDraw(const Camera& cam);

    // 衝突判定（必要に応じてオーバーライド）
    virtual bool Collision(int px, int py, int pw, int ph);

    // Getter
    Type GetType() const { return data_.type; }
    Data GetData() const { return data_; }

    void ChangeState(State s) { state_ = s; }

protected:
    State state_;
    Data data_;
};
