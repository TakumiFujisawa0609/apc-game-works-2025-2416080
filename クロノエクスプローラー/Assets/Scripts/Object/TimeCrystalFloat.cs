using UnityEngine;
public class TimeCrystalFloat : MonoBehaviour
{
    public float bobAmp = 0.05f, bobSpeed = 2.2f;
    public float glowAmp = 0.15f, glowSpeed = 3.0f; 
    Vector3 basePos; SpriteRenderer sr; Color baseCol;

    void Awake() { basePos = transform.localPosition; sr = GetComponent<SpriteRenderer>(); baseCol = sr ? sr.color : Color.white; }
    void Update()
    {
        transform.localPosition = basePos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobAmp);
        if (sr) sr.color = baseCol * (1f + Mathf.Sin(Time.time * glowSpeed) * glowAmp);
    }
}
