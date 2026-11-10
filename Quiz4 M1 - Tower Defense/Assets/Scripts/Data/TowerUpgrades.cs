using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
        // Arrow
        { "Arrow2A", new UpgradeInfo(5, "Arrow Bleed", "Increases arrow bleed stack from by 0.5", (Tower tower) => { tower.ApplyUniqueBonus(0.5f); }) },
        { "Arrow2B", new UpgradeInfo(5, "Damage", "Increases damage of the tower by 2", (Tower tower) => { tower.ApplyBonusDamage(2); }) },
        { "Arrow2C", new UpgradeInfo(5, "Range", "Increases range of the tower by 4", (Tower tower) => { tower.ApplyBonusRange(4); }) },

        { "Arrow4A", new UpgradeInfo(5, "Arrow Bleed", "Increases arrow bleed stack from by 0.5", (Tower tower) => { tower.ApplyUniqueBonus(0.5f); }) },
        { "Arrow4B", new UpgradeInfo(5, "Damage", "Increases damage of the tower by 2", (Tower tower) => { tower.ApplyBonusDamage(2); }) },
        { "Arrow4C", new UpgradeInfo(5, "Range", "Increases range of the tower by 4", (Tower tower) => { tower.ApplyBonusRange(4); }) },

        // Fire
        { "Fire2A", new UpgradeInfo(5, "Apply Time", "Decreases the time it takes to apply damage by 0.3s", (Tower tower) => { tower.ApplyUniqueBonus(0.3f); }) },
        { "Fire2B", new UpgradeInfo(5, "Damage", "Increases damage of the tower by 2", (Tower tower) => { tower.ApplyBonusDamage(2); }) },
        { "Fire2C", new UpgradeInfo(5, "Range", "Increases range of the tower by 5", (Tower tower) => { tower.ApplyBonusRange(5); }) },

        { "Fire4A", new UpgradeInfo(5, "Apply Time", "Decreases the time it takes to apply damage by 0.3s", (Tower tower) => { tower.ApplyUniqueBonus(0.3f); }) },
        { "Fire4B", new UpgradeInfo(5, "Damage", "Increases damage of the tower by 2", (Tower tower) => { tower.ApplyBonusDamage(2); }) },
        { "Fire4C", new UpgradeInfo(5, "Range", "Increases range of the tower by 5", (Tower tower) => { tower.ApplyBonusRange(5); }) },

        // Ice
        { "Ice2A", new UpgradeInfo(5, "Freeze Time", "Increases slow down time by 0.3s", (Tower tower) => { tower.ApplyUniqueBonus(0.3f); }) },
        { "Ice2B", new UpgradeInfo(5, "Damage", "Increases damage of the tower by 5", (Tower tower) => { tower.ApplyBonusDamage(5); }) },
        { "Ice2C", new UpgradeInfo(5, "Range", "Increases range of the tower by 4", (Tower tower) => { tower.ApplyBonusRange(4); }) },

        { "Ice4A", new UpgradeInfo(5, "Freeze Time", "Increases slow down time by 0.3s", (Tower tower) => { tower.ApplyUniqueBonus(0.3f); }) },
        { "Ice4B", new UpgradeInfo(5, "Damage", "Increases damage of the tower by 5", (Tower tower) => { tower.ApplyBonusDamage(5); }) },
        { "Ice4C", new UpgradeInfo(5, "Range", "Increases range of the tower by 4", (Tower tower) => { tower.ApplyBonusRange(4); }) },

        // Cannon
        { "Cannon2A", new UpgradeInfo(5, "Firerate", "Increases splash damage range by 2", (Tower tower) => { tower.ApplyUniqueBonus(2f); }) },
        { "Cannon2B", new UpgradeInfo(5, "Damage", "Increases damage of the tower by 10", (Tower tower) => { tower.ApplyBonusDamage(10); }) },
        { "Cannon2C", new UpgradeInfo(5, "Range", "Increases range of the tower by 6", (Tower tower) => { tower.ApplyBonusRange(6); }) },

        { "Cannon4A", new UpgradeInfo(5, "Splash Range", "Increases splash damage range by 2", (Tower tower) => { tower.ApplyUniqueBonus(2f); }) },
        { "Cannon4B", new UpgradeInfo(5, "Damage", "Increases damage of the tower by 10", (Tower tower) => { tower.ApplyBonusDamage(10); }) },
        { "Cannon4C", new UpgradeInfo(5, "Range", "Increases range of the tower by 6", (Tower tower) => { tower.ApplyBonusRange(6); }) },
    };
}