using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer I { get; private set; }

    [Header("参照")]
    public SfxLibrary library;
    public AudioMixerGroup mixerGroup;     // 任意（未設定でもOK）

    [Header("同時発音プール")]
    [SerializeField] int poolSize = 8;

    readonly List<AudioSource> pool = new();
    int idx;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < poolSize; i++)
        {
            var go = new GameObject($"SFX_{i}");
            go.transform.SetParent(transform);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f;              // デフォは2D
            src.outputAudioMixerGroup = mixerGroup;
            pool.Add(src);
        }
    }

    AudioSource Next() { idx = (idx + 1) % pool.Count; return pool[idx]; }

    // === 2D再生（画面内どこでも同じ音量） ===
    public static void Play2D(SfxKey key, float volumeMul = 1f)
    {
        if (I == null || I.library == null) return;
        if (!I.library.TryGet(key, out var e) || e.clip == null) return;

        var src = I.Next();
        src.spatialBlend = 0f;
        src.pitch = 1f + Random.Range(-e.pitchJitter, e.pitchJitter);
        src.volume = e.volume * Mathf.Clamp01(volumeMul);
        src.transform.position = Vector3.zero;
        src.clip = e.clip;
        src.Play();
    }

    // === 3D再生（位置つき。2Dゲームでも近接だけ位置付けしたい時に） ===
    public static void PlayAt(SfxKey key, Vector3 worldPos, float volumeMul = 1f)
    {
        if (I == null || I.library == null) return;
        if (!I.library.TryGet(key, out var e) || e.clip == null) return;

        var src = I.Next();
        src.spatialBlend = 1f;
        src.rolloffMode = AudioRolloffMode.Linear;
        src.minDistance = 2f; src.maxDistance = 25f;
        src.pitch = 1f + Random.Range(-e.pitchJitter, e.pitchJitter);
        src.volume = e.volume * Mathf.Clamp01(volumeMul);
        src.transform.position = worldPos;
        src.clip = e.clip;
        src.Play();
    }
}
