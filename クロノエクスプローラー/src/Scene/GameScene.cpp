#include <string>
#include <DxLib.h>
#include "../Application.h"
#include "../Manager/ResourceManager.h"
#include "../Manager/SceneManager.h"
#include "../Manager/InputManager.h"
#include "../Manager/SoundManager.h"

#include "GameScene.h"

GameScene::GameScene(void)
{
}

GameScene::~GameScene(void)
{
}


void GameScene::Init(void)
{
	//非同期読み込みを有効にする
	SetUseASyncLoadFlag(false);

	stage_ = SceneManager::GetInstance().GetStage();
}

void GameScene::Update(void)
{
	//ロードが完了したか判断
	if (GetASyncLoadNum() != 0 || SceneManager::GetInstance().IsLoading())
	{
		return;
	}

	// シーン遷移
	InputManager& ins = InputManager::GetInstance();
	if (ins.IsTrgDown(KEY_INPUT_SPACE))
	{
		SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::RESULT);
		return;
	}

#pragma region Debug
	if (ins.IsTrgDown(KEY_INPUT_UP)) {
		SceneManager::GetInstance().SetIsWin(SceneManager::RESULT::WIN);
	}
	if (ins.IsTrgDown(KEY_INPUT_DOWN)) {
		SceneManager::GetInstance().SetIsWin(SceneManager::RESULT::LOSE);
	}
#pragma endregion

}

void GameScene::Draw(void)
{
	//ロードが完了したか判断
	if (GetASyncLoadNum() != 0 || SceneManager::GetInstance().IsLoading())
	{
		return;
	}

	DrawGraph(0, 0, ResourceManager::GetInstance().Load(ResourceManager::SRC::GAME_IMG).handleId_, false);

	if (SceneManager::GetInstance().GetIsWin() == SceneManager::RESULT::WIN) {
		DrawFormatString(0, 20, 0xff0000, "IsWin=WIN");
	}
	if (SceneManager::GetInstance().GetIsWin() == SceneManager::RESULT::LOSE) {
		DrawFormatString(0, 20, 0xff0000, "IsWin=LOSE");
	}

	DrawFormatString(0, 40, 0xff0000, "stage=%d", stage_);
}

void GameScene::Release(void)
{
}
