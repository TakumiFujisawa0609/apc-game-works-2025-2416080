// TimeManager.cs  — 超最小の器
using UnityEngine;
public class TimeManager : MonoBehaviour
{
    public int CrystalCount { get; private set; }
    public void AddCrystals(int n)
    {
        CrystalCount += n;
        // UI更新通知（簡易版）
        var ui = FindObjectOfType<TimeCrystalUI>(); if (ui) ui.UpdateCount();
    }
}
