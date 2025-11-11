using UnityEngine;
using UnityEngine.Audio;

public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    [Header("参照")]
    public SfxLibrary library;
    public AudioMixerGroup mixerGroup;

    [Header("同時発音プール")]
    [Min(1)] public int poolSize = 8;

    AudioSource[] pool;
    int next;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // AudioSource プール生成
        pool = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            var go = new GameObject($"SFX_Source_{i}");
            go.transform.SetParent(transform, false);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            src.spatialBlend = 0f;               // 2D
            src.volume = 1f;
            src.pitch = 1f;
            if (mixerGroup) src.outputAudioMixerGroup = mixerGroup;
            pool[i] = src;
        }

        // 基本の安全チェック
        if (!Camera.main || !Camera.main.GetComponent<AudioListener>())
        {
            if (!FindObjectOfType<AudioListener>())
            {
                Debug.LogWarning("[SfxPlayer] AudioListener が見つかりません。Main Camera に AudioListener を追加してください。");
            }
        }
    }

    AudioSource GetSource()
    {
        var s = pool[next];
        next = (next + 1) % pool.Length;
        return s;
    }

    // ====== API ======
    public static void Play2D(SfxKey key)
    {
        if (!Instance) { Debug.LogWarning("[SfxPlayer] Instance がありません。SFX オブジェクトに SfxPlayer を置いてください。"); return; }
        if (!Instance.library) { Debug.LogWarning("[SfxPlayer] Library が未割り当てです。"); return; }

        if (!Instance.library.TryGet(key, out var ent) || !ent.clip)
        {
            Debug.LogWarning($"[SfxPlayer] ライブラリに {key} が無いか、Clip が未設定です。");
            return;
        }

        var src = Instance.GetSource();
        src.clip = ent.clip;
        src.volume = ent.volume;
        src.pitch = 1f + Random.Range(-ent.pitchJitter, ent.pitchJitter);
        src.spatialBlend = 0f;  // 2D
        src.transform.position = Vector3.zero;
        src.Play();
    }

    public static void PlayAt(SfxKey key, Vector3 worldPos)
    {
        if (!Instance) { Debug.LogWarning("[SfxPlayer] Instance がありません。"); return; }
        if (!Instance.library) { Debug.LogWarning("[SfxPlayer] Library が未割り当てです。"); return; }

        if (!Instance.library.TryGet(key, out var ent) || !ent.clip)
        {
            Debug.LogWarning($"[SfxPlayer] ライブラリに {key} が無いか、Clip が未設定です。");
            return;
        }

        var src = Instance.GetSource();
        src.clip = ent.clip;
        src.volume = ent.volume;
        src.pitch = 1f + Random.Range(-ent.pitchJitter, ent.pitchJitter);
        src.spatialBlend = 0f;      // 2D として鳴らす（3Dにしたいなら 1 に）
        src.transform.position = worldPos;
        src.Play();
    }
}
