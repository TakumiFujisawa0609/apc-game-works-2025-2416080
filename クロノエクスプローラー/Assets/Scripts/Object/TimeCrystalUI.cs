using TMPro;
using UnityEngine;

public class TimeCrystalUI : MonoBehaviour
{
    public TMP_Text countText;
    void OnEnable() { UpdateCount(); }

    public void UpdateCount()
    {
        var tm = FindObjectOfType<TimeManager>();
        if (tm && countText) countText.text = "×" + tm.CrystalCount;
    }
}
