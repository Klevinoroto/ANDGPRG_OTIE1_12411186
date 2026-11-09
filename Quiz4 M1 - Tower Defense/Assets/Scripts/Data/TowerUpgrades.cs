using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

[System.Serializable]
public struct UpgradeInfo
{
    public int cost;
    public Action<Tower> upgradeFunction; // Store a unique function here

    public UpgradeInfo(int _cost, Action<Tower> uniqueBehavior)
    {
        cost = _cost;
        upgradeFunction = uniqueBehavior;
    }

    public void Upgrade(Tower tower)
    {
        if (upgradeFunction != null)
        {
            upgradeFunction.Invoke(tower);
        }
    }
}

public static class TowerUpgrades
{
    public static Dictionary<string, UpgradeInfo> Towers = new Dictionary<string, UpgradeInfo>()
    {
        { "Arrow2a", new UpgradeInfo(5, (Tower tower) => { tower.bonusFirerate += 5; }) },
        { "Arrow2b", new UpgradeInfo(5, (Tower tower) => { tower.bonusDamage += 5; }) },
        { "Arrow2c", new UpgradeInfo(5, (Tower tower) => { tower.bonusRange += 5; }) },
    };
}