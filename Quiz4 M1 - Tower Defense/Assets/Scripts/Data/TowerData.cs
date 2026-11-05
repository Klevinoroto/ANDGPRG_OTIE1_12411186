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
    public float range;
    public float shootCooldown;

    public TowerInfo(string _name, GameObject _model, int _cost, int _damage, float _range, float _shootCooldown)
    {
        name = _name;
        model = _model;
        cost = _cost;
        damage = _damage;
        range = _range;
        shootCooldown = _shootCooldown;
    }
}

public static class TowerData
{
    public static Dictionary<string, TowerInfo> Towers = new Dictionary<string, TowerInfo>()
    {
        { "Arrow", new TowerInfo("TowerArrow", Resources.Load<GameObject>("TowerModels/Arrow/Base"), 5, 20, 7f, 0.5f) },

        { "Fire", new TowerInfo("TowerFire", Resources.Load<GameObject>("TowerModels/Fire/Base"), 5, 5, 6f, 1f) },

        { "Ice", new TowerInfo("TowerIce", Resources.Load<GameObject>("TowerModels/Ice/Base"), 5, 5, 6f, 5f) },

        { "Cannon", new TowerInfo("TowerCannon", Resources.Load<GameObject>("TowerModels/Cannon/Base"), 5, 50, 15f, 2f) },
    };
}