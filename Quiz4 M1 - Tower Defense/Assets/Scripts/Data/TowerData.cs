using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct TowerInfo
{
    public string name;
    public string description;
    public GameObject model;
    public int cost;
    public int damage;
    public float range;
    public float shootCooldown;

    public TowerInfo(string _name, string _description, GameObject _model, int _cost, int _damage, float _range, float _shootCooldown)
    {
        name = _name;
        description = _description;
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
        { "Arrow1", new TowerInfo("TowerArrow", "A standard arrow tower.", Resources.Load<GameObject>("TowerModels/Arrow/Tower1"), 10, 20, 7f, 0.5f) },
        { "Arrow2", new TowerInfo("TowerArrow", "The next tier of the arrow tower.", Resources.Load<GameObject>("TowerModels/Arrow/Tower2"), 15, 25, 7f, 0.5f) },
        { "Arrow3", new TowerInfo("TowerArrow", "The final tier of the arrow tower.", Resources.Load<GameObject>("TowerModels/Arrow/Tower3"), 30, 25, 7f, 0.25f) },

        { "Fire1", new TowerInfo("TowerFire", "A standard fire tower.", Resources.Load<GameObject>("TowerModels/Fire/Tower1"), 10, 5, 6f, 1f) },
        { "Fire2", new TowerInfo("TowerFire", "The next tier of the fire tower.", Resources.Load<GameObject>("TowerModels/Fire/Tower2"), 15, 7, 6f, 1f) },
        { "Fire3", new TowerInfo("TowerFire", "The final tier of the fire tower.", Resources.Load<GameObject>("TowerModels/Fire/Base"), 30, 9, 6f, 1f) },

        { "Ice1", new TowerInfo("TowerIce", "A standard ice tower.", Resources.Load<GameObject>("TowerModels/Ice/Tower1"), 10, 5, 6f, 5f) },
        { "Ice2", new TowerInfo("TowerIce", "The next tier of the ice tower.", Resources.Load<GameObject>("TowerModels/Ice/Tower2"), 15, 6, 7f, 5f) },
        { "Ice3", new TowerInfo("TowerIce", "The final tier of the ice tower.", Resources.Load<GameObject>("TowerModels/Ice/Tower3"), 30, 7, 8f, 5f) },

        { "Cannon1", new TowerInfo("TowerCannon", "A standard cannon tower.", Resources.Load<GameObject>("TowerModels/Cannon/Tower1"), 10, 50, 15f, 4f) },
        { "Cannon2", new TowerInfo("TowerCannon", "The next tier of the cannon tower.", Resources.Load<GameObject>("TowerModels/Cannon/Tower2"), 15, 60, 15f, 4f) },
        { "Cannon3", new TowerInfo("TowerCannon", "The final tier of the cannon tower.", Resources.Load<GameObject>("TowerModels/Cannon/Tower3"), 30, 60, 20f, 3f) },
    };
}