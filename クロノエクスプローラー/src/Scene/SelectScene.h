#pragma once
#include "SceneBase.h"

class SelectScene : public SceneBase
{
public:
	SelectScene();
	~SelectScene();


	void Init(void) override;
	void Update(void) override;
	void Draw(void) override;
	void Release(void) override;

private:

	int stage_;

};

