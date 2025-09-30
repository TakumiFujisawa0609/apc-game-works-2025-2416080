#include "TitleScene.h"
#include <DxLib.h>
#include "../Manager/InputManager.h"
#include "../Manager/SceneManager.h"
#include "../Application.h"

// コンストラクタ
TitleScene::TitleScene()
{
    selectNum_ = 0;

    titleImg_ = -1;   // 画像は未読み込み状態
}

// デストラクタ
TitleScene::~TitleScene()
{
}

// 初期化処理
// - タイトル画像を読み込む
void TitleScene::Init(void)
{
    selectNum_ = 0;

    // タイトル画像の読み込み
    titleImg_ = LoadGraph((Application::PATH_IMAGE + "title.png").c_str());
}

// 更新処理
// 入力を受け取ってメニュー選択を操作
// 決定キーでシーンを切り替える
void TitleScene::Update(void)
{
    auto& input = InputManager::GetInstance();

    // ↑キーで選択を上に移動
    if (input.IsTrgDown(KEY_INPUT_UP)) {
        selectNum_ = (selectNum_ - 1 + 3) % 3;  // 0→2にループするように
    }

    // ↓キーで選択を下に移動
    if (input.IsTrgDown(KEY_INPUT_DOWN)) {
        selectNum_ = (selectNum_ + 1) % 3;      // 2→0にループするように
    }

    // Spaceキーで決定
    if (input.IsTrgDown(KEY_INPUT_SPACE)) {
        if (selectNum_ == 0) {
            // ゲーム開始 → GameSceneへ遷移
            SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::GAME);
        }
        else if (selectNum_ == 1) {
            // オプション → OptionSceneへ遷移
            SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::OPTION);
        }
        else if (selectNum_ == 2) {
            // 終了 → DxLibを終了
            DxLib_End();
        }
    }
}

// 描画処理
// - タイトル画像を中央に表示
// - メニューをリスト表示し、選択中は色を変える
void TitleScene::Draw(void)
{
    // タイトル画像を中央に描画
    if (titleImg_ != -1) {
        int w, h;
        GetGraphSize(titleImg_, &w, &h);
        DrawGraph((Application::SCREEN_SIZE_X - w) / 2,   // 横中央に配置
            100,                                   // 上からの位置
            titleImg_, TRUE);
    }

    // メニュー文字列
    const char* menuStr[] = { "START GAME", "OPTION", "EXIT" };

    // 各メニューを縦に並べて描画
    for (int i = 0; i < 3; i++) {
        // 選択中は赤、それ以外は白
        int color = (i == selectNum_) ? GetColor(255, 0, 0) : GetColor(255, 255, 255);

        // 画面の下の方に並べる
        DrawString(250, 400 + i * 40, menuStr[i], color);
    }
}

// 解放処理
// - 読み込んだ画像を削除
void TitleScene::Release(void)
{
    if (titleImg_ != -1) {
        DeleteGraph(titleImg_);
        titleImg_ = -1;
    }
}