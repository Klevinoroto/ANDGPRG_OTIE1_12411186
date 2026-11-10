using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

[System.Serializable]
public struct UpgradeInfo
{
    public int cost;
    public string name;
    public string description;
    public Action<Tower> upgradeFunction; // Store a unique function here

    public UpgradeInfo(int _cost, string _name, string _description, Action<Tower> uniqueBehavior)
    {
        cost = _cost;
        name = _name;
        description = _description;
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
        { "Arrow2A", new UpgradeInfo(5, "Firerate", "Increases firerate of the tower by 5", (Tower tower) => { tower.bonusFirerate += 5; }) },
        { "Arrow2B", new UpgradeInfo(5, "Damage", "Increases damage of the tower by 5", (Tower tower) => { tower.bonusDamage += 5; }) },
        { "Arrow2C", new UpgradeInfo(5, "Range", "Increases range of the tower by 5", (Tower tower) => { tower.bonusRange += 5; }) },
    };
}