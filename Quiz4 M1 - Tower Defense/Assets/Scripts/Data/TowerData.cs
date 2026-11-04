using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct TowerInfo
{
    public string name;
    public GameObject model;
    public int cost;
    public int damage;

    public TowerInfo(string _name, GameObject _model, int _cost, int _damage)
    {
        name = _name;
        model = _model;
        cost = _cost;
        damage = _damage;
    }
}

public static class TowerData
{
    public static Dictionary<string, TowerInfo> Towers = new Dictionary<string, TowerInfo>()
    {
        { "Arrow", new TowerInfo("Arrow Tower", Resources.Load<GameObject>("TowerModels/Arrow/Base"), 5, 20) },

        { "Fire", new TowerInfo("Fire Tower", Resources.Load<GameObject>("TowerModels/Fire/Base"), 5, 20) },

        { "Ice", new TowerInfo("Ice Tower", Resources.Load<GameObject>("TowerModels/Ice/Base"), 5, 20) },

        { "Cannon", new TowerInfo("Cannon Tower", Resources.Load<GameObject>("TowerModels/Cannon/Base"), 5, 20) },
    };
}