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
        { "Arrow1", new TowerInfo("TowerArrow", Resources.Load<GameObject>("TowerModels/Arrow/Tower1"), 5, 20, 7f, 0.5f) },
        { "Arrow2", new TowerInfo("TowerArrow", Resources.Load<GameObject>("TowerModels/Arrow/Tower2"), 5, 20, 7f, 0.5f) },
        { "Arrow3", new TowerInfo("TowerArrow", Resources.Load<GameObject>("TowerModels/Arrow/Tower3"), 5, 20, 7f, 0.5f) },

        { "Fire1", new TowerInfo("TowerFire", Resources.Load<GameObject>("TowerModels/Fire/Tower1"), 5, 5, 6f, 1f) },
        { "Fire2", new TowerInfo("TowerFire", Resources.Load<GameObject>("TowerModels/Fire/Tower2"), 5, 5, 6f, 1f) },
        { "Fire3", new TowerInfo("TowerFire", Resources.Load<GameObject>("TowerModels/Fire/Base"), 5, 5, 6f, 1f) },

        { "Ice1", new TowerInfo("TowerIce", Resources.Load<GameObject>("TowerModels/Ice/Tower1"), 5, 5, 6f, 5f) },
        { "Ice2", new TowerInfo("TowerIce", Resources.Load<GameObject>("TowerModels/Ice/Tower2"), 5, 5, 6f, 5f) },
        { "Ice3", new TowerInfo("TowerIce", Resources.Load<GameObject>("TowerModels/Ice/Tower3"), 5, 5, 6f, 5f) },

        { "Cannon1", new TowerInfo("TowerCannon", Resources.Load<GameObject>("TowerModels/Cannon/Tower1"), 5, 50, 15f, 2f) },
        { "Cannon2", new TowerInfo("TowerCannon", Resources.Load<GameObject>("TowerModels/Cannon/Tower2"), 5, 50, 15f, 2f) },
        { "Cannon3", new TowerInfo("TowerCannon", Resources.Load<GameObject>("TowerModels/Cannon/Tower3"), 5, 50, 15f, 2f) },
    };
}