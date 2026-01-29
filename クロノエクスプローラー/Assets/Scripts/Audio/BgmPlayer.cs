using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class BgmEntry
{
    public BgmKey key;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}

public class BgmPlayer : MonoBehaviour
{
    public static BgmPlayer I;

    [Header("Clips")]
    public List<BgmEntry> clips = new();

    [Header("Mixer (”CˆÓ)")]
    public AudioMixerGroup musicGroup;     
    [Header("Fade")]
    public float defaultFade = 1.5f;

    AudioSource a;    
    AudioSource b;     
    Dictionary<BgmKey, BgmEntry> table = new();
    BgmKey currentKey = BgmKey.None;
    Coroutine fadeCo;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        a = CreateSource("BGM_A");
        b = CreateSource("BGM_B");

        foreach (var e in clips) if (e.clip) table[e.key] = e;
    }

    AudioSource CreateSource(string name)
    {
        var go = new GameObject(name);
        go.transform.SetParent(transform);
        var s = go.AddComponent<AudioSource>();
        s.playOnAwake = false;
        s.loop = true;
        s.spatialBlend = 0f; 
        s.outputAudioMixerGroup = musicGroup;
        return s;
    }

    public void Play(BgmKey key, float fade = -1f)
    {
        if (key == currentKey) return;
        if (!table.TryGetValue(key, out var e) || e.clip == null)
        {
            Stop(defaultFade);
            currentKey = BgmKey.None;
            return;
        }
        if (fade < 0f) fade = defaultFade;

        if (fadeCo != null) StopCoroutine(fadeCo);
        fadeCo = StartCoroutine(CoCrossFade(e, fade));
        currentKey = key;
    }

    public void Stop(float fade = -1f)
    {
        if (fade < 0f) fade = defaultFade;
        if (fadeCo != null) StopCoroutine(fadeCo);
        fadeCo = StartCoroutine(CoFadeOut(fade));
        currentKey = BgmKey.None;
    }

    IEnumerator CoCrossFade(BgmEntry next, float dur)
    {
        b.clip = next.clip;
        b.volume = 0f;
        b.Play();

        float from = a.isPlaying ? a.volume : 0f;
        float to = next.volume;

        float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / dur);
            a.volume = Mathf.Lerp(from, 0f, p);
            b.volume = Mathf.Lerp(0f, to, p);
            yield return null;
        }

        a.Stop();
        var tmp = a; a = b; b = tmp;
    }

    IEnumerator CoFadeOut(float dur)
    {
        float fromA = a.volume;
        float fromB = b.volume;
        float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / dur);
            a.volume = Mathf.Lerp(fromA, 0f, p);
            b.volume = Mathf.Lerp(fromB, 0f, p);
            yield return null;
        }
        a.Stop(); b.Stop();
    }
}
