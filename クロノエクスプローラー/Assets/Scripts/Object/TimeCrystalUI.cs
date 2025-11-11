// TimeCrystalUI.cs  — TextMeshPro で "×3" のように表示
using TMPro;
using UnityEngine;

public class TimeCrystalUI : MonoBehaviour
{
    public TMP_Text countText;
    void OnEnable() { UpdateCount(); }
    // 例：TimeManager から呼ぶ
    public void UpdateCount()
    {
        var tm = FindObjectOfType<TimeManager>();
        if (tm && countText) countText.text = "×" + tm.CrystalCount;
    }
}
