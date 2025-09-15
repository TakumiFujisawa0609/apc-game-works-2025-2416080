#include <string>
#include <DxLib.h>
#include "../Application.h"
#include "../Manager/ResourceManager.h"
#include "../Manager/SceneManager.h"
#include "../Manager/InputManager.h"
#include "../Manager/SoundManager.h"

#include "SelectScene.h"

SelectScene::SelectScene()
{
}

SelectScene::~SelectScene()
{
}


void SelectScene::Init(void)
{
	stage_ = SceneManager::GetInstance().GetStage();

	//非同期読み込みを有効にする
	SetUseASyncLoadFlag(false);
}

void SelectScene::Update(void)
{
	//ロードが完了したか判断
	//if (GetASyncLoadNum() != 0 || SceneManager::GetInstance().IsLoading())
	//{
	//	return;
	//}

	//// シーン遷移
	InputManager& ins = InputManager::GetInstance();
	if (ins.IsTrgDown(KEY_INPUT_SPACE))
	{
		SceneManager::GetInstance().ResetGame();
		SceneManager::GetInstance().SetStage(stage_);
		SceneManager::GetInstance().ChangeScene(SceneManager::SCENE_ID::GAME);
		return;
	}

	if (ins.IsTrgDown(KEY_INPUT_LEFT)) {
		stage_--;
	}
	if (ins.IsTrgDown(KEY_INPUT_RIGHT)) {
		stage_++;
	}
}

void SelectScene::Draw(void)
{
	////ロードが完了したか判断
	//if (GetASyncLoadNum() != 0 || SceneManager::GetInstance().IsLoading())
	//{
	//	return;
	//}

	DrawGraph(0, 0, ResourceManager::GetInstance().Load(ResourceManager::SRC::SELECT_IMG).handleId_, false);
	DrawFormatString(0, 40, 0xff0000, "stage=%d", stage_);
}

void SelectScene::Release(void)
{
}
