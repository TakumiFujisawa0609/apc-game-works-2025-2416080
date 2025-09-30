#include <DxLib.h>
#include "../Application.h"
#include "Resource.h"
#include "ResourceManager.h"

ResourceManager* ResourceManager::instance_ = nullptr;

void ResourceManager::CreateInstance(void)
{
	if (instance_ == nullptr)
	{
		instance_ = new ResourceManager();
	}
	instance_->Init();
}

ResourceManager& ResourceManager::GetInstance(void)
{
	return *instance_;
}

void ResourceManager::Init(void)
{

	Resource res;


	//ƒ^ƒCƒgƒ‹‰æ‘œ
	res = Resource(Resource::TYPE::IMG, Application::PATH_IMAGE + "Title.png");
	resourcesMap_.emplace(SRC::TITLE_IMG, res);

	//ƒQ[ƒ€‰æ‘œ
	res = Resource(Resource::TYPE::IMG, Application::PATH_IMAGE + "GameScene.png");
	resourcesMap_.emplace(SRC::GAME_IMG, res);

	//ƒQ[ƒ€‰æ‘œ
	res = Resource(Resource::TYPE::IMG, Application::PATH_IMAGE + "SelectScene.png");
	resourcesMap_.emplace(SRC::SELECT_IMG, res);

	////”wŒiX
	//res = Resource(Resource::TYPE::MODEL, Application::PATH_MODEL + "BackGround/Forest/Forest.mv1");
	//resourcesMap_.emplace(SRC::BACKGROUNDFOREST, res);

	//// ’e‚Ì”š”­
	//res = Resource(Resource::TYPE::EFFEKSEER, Application::PATH_EFFECT + "Blast.efkefc");
	//resourcesMap_.emplace(SRC::SHOT_EXPLOSION, res);

}

void ResourceManager::Release(void)
{
	for (auto& p : loadedMap_)
	{
		p.second->Release();
		delete p.second;
	}

	loadedMap_.clear();
}

void ResourceManager::Destroy(void)
{
	Release();
	resourcesMap_.clear();
	delete instance_;
}

Resource ResourceManager::Load(SRC src)
{
	Resource* res = _Load(src);
	if (res == nullptr)
	{
		return Resource();
	}
	Resource ret = *res;
	return *res;
}

int ResourceManager::LoadModelDuplicate(SRC src)
{
	Resource* res = _Load(src);
	if (res == nullptr)
	{
		return -1;
	}

	int duId = MV1DuplicateModel(res->handleId_);
	res->duplicateModelIds_.push_back(duId);

	return duId;
}

ResourceManager::ResourceManager(void)
{
}

Resource* ResourceManager::_Load(SRC src)
{
	const auto& lPair = loadedMap_.find(src);
	if (lPair != loadedMap_.end())
	{
		return lPair->second;
	}

	const auto& rPair = resourcesMap_.find(src);
	if (rPair == resourcesMap_.end())
	{
		// “o˜^‚³‚ê‚Ä‚¢‚È‚¢
		return nullptr;
	}

	rPair->second.Load();

	// ”O‚Ì‚½‚ßƒRƒs[ƒRƒ“ƒXƒgƒ‰ƒNƒ^
	Resource* ret = new Resource(rPair->second);
	loadedMap_.emplace(src, ret);

	return ret;
}
