using UnityEngine;

[CreateAssetMenu(menuName = "CE/SfxLibrary", fileName = "SfxLibrary")]
public class SfxLibrary : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public SfxKey key;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 0.9f;
        [Tooltip("ピッチの揺らぎ幅（±）")][Range(0f, 0.5f)] public float pitchJitter = 0.05f;
    }
    public Entry[] entries;

    public bool TryGet(SfxKey key, out Entry e)
    {
        foreach (var x in entries) if (x.key == key) { e = x; return true; }
        e = null; return false;
    }
}
