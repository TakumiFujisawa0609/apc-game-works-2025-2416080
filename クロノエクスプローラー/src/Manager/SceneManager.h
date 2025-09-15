#pragma once
#include <chrono>
class SceneBase;
class Fader;

class SceneManager
{

public:

	// シーン管理用
	enum class SCENE_ID
	{
		NONE,
		TITLE,
		SELECT,
		GAME,
		RESULT
	};

	enum class RESULT
	{
		NONE,
		WIN,
		LOSE
	};

	// インスタンスの生成
	static void CreateInstance(void);

	// インスタンスの取得
	static SceneManager& GetInstance(void);

	void Init(void);
	/*void Init3D(void);*/
	void Update(void);
	void Draw(void);

	// リソースの破棄
	void Destroy(void);

	// 状態遷移
	void ChangeScene(SCENE_ID nextId);

	//ゲームのリセット
	void ResetGame(void);
	//クリアしたか確認するための変数
	RESULT GetIsWin(void);
	void SetIsWin(RESULT result);

	//ステージを確認する変数
	int GetStage(void);
	void SetStage(int num);

	SCENE_ID GetSceneID(void);// シーンIDの取得

	// デルタタイムの取得
	float GetDeltaTime(void) const;

	// ロード中か調べる
	bool IsLoading(void) const;
	int LoadCunt(void) const;

private:

	// 静的インスタンス
	static SceneManager* instance_;

	SCENE_ID sceneId_;
	SCENE_ID waitSceneId_;

	// フェード
	Fader* fader_;

	// 各種シーン
	SceneBase* scene_;


	// シーン遷移中判定
	bool isSceneChanging_;

	// デルタタイム
	std::chrono::system_clock::time_point preTime_;
	float deltaTime_;


	//ゲームに使う変数
	//勝利したか判定する
	RESULT  isWin_;

	//プレイヤーの今いるステージ
	int stageNum_;

	// デフォルトコンストラクタをprivateにして、
	// 外部から生成できない様にする
	SceneManager(void);
	// コピーコンストラクタも同様
	SceneManager(const SceneManager&);
	// デストラクタも同様
	~SceneManager(void) = default;

	// デルタタイムをリセットする
	void ResetDeltaTime(void);

	// シーン遷移
	void DoChangeScene(SCENE_ID sceneId);

	// フェード
	void Fade(void);

};
