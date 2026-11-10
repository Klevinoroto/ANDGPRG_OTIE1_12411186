using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    protected Player player = null;
    protected GameObject enemiesFolder = null;
    private bool isPlaced = false;
    protected bool canFire = true;

    public bool isPlaceable;
    public string towerType;
    public TowerInfo info;
    private GameObject towerModel = null;

    private GameObject baseHighlight;
    private GameObject upgradeIndicator;


    public int level;
    public int tier;
    public string upgradeDescriptors = "";
    protected float range;
    protected float damage;
    protected float firerate;
    private int nextUpgradeCost;

    private float bonusRange;
    private float bonusDamage;
    private float bonusFirerate;

    float RoundNearestTenth(float value) { return Mathf.Round(value * 10f) / 10f; }

    public virtual void ApplyUniqueBonus(float value) { }
    public void ApplyBonusRange(float value) { upgradeDescriptors += " (" + value.ToString() + "+ Range)"; bonusRange += value; range += value; }
    public void ApplyBonusDamage(float value) { upgradeDescriptors += " (" + value.ToString() + "+ Damage)"; bonusDamage += value; damage += value; }
    public void ApplyBonusFirerate(float value) { upgradeDescriptors += " (" + value.ToString() + "+ Range)"; bonusFirerate += value; firerate -= value; }

    void Start()
    {
        nextUpgradeCost = 5;

        level = 1;
        tier = 0;
        ApplyNextTier();

        baseHighlight = transform.Find("PreviewPlaceable").gameObject;
        upgradeIndicator = transform.Find("UpgradeReady").gameObject;
        upgradeIndicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced) {

            MeshRenderer preview = baseHighlight.GetComponent<MeshRenderer>();
            isPlaceable = RoundNearestTenth(transform.position.y) == 2.2f;

            if (isPlaceable) { preview.material.color = Color.green; }
            else             { preview.material.color = Color.red; }
        }
        else
        {
            
            if (player.GetGold() >= nextUpgradeCost) { upgradeIndicator.SetActive(true); }
            else                                     { upgradeIndicator.SetActive(false); }

            Shoot();
        }
    }

    public void Place(GameObject _enemiesFolder, Player _player)
    {
        player = _player;
        enemiesFolder = _enemiesFolder;
        baseHighlight.SetActive(false);
        isPlaced = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    public void OnMouseDown()
    {
        MeshRenderer preview = baseHighlight.GetComponent<MeshRenderer>();
        preview.material.color = Color.orange;
        baseHighlight.SetActive(true);

        player.OpenTowerInfo(this);
    }

    public void Deselect()
    {
        baseHighlight.SetActive(false);
    }

    public void UpgradePath(string upgradeKey) { 
        TowerUpgrades.Towers[upgradeKey].Upgrade(this);
        nextUpgradeCost = TowerData.Towers[towerType + (tier+1).ToString()].cost;
    }
    public void ApplyNextTier()
    {
        tier++;

        info = TowerData.Towers[towerType + tier.ToString()];
        range = info.range + bonusRange;
        damage = info.damage + bonusDamage;
        firerate = info.shootCooldown + bonusFirerate;

        if (towerModel != null) { Destroy(towerModel); }
        towerModel = Instantiate(info.model, transform);
        towerModel.transform.localPosition = Vector3.zero;
        towerModel.transform.localRotation = Quaternion.identity;

        if (level == 5) { nextUpgradeCost = 999999; }
        else            { nextUpgradeCost = 5; }
    }

    public IEnumerator StartCanFireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(firerate);
        canFire = true;
    }

    protected void RewardOnKill(bool targetKilled, int reward)
    {
        if (targetKilled) { player.AdjustGold(reward); }
    }

    public virtual void HitEffect(GameObject _target) {}

    public virtual void Shoot()
    {

        List<GameObject> currentEnemies = new List<GameObject>();
        foreach (Transform child in enemiesFolder.transform)
        {
            currentEnemies.Add(child.gameObject);
        }

        if (canFire && currentEnemies.Count > 0)
        {

            GameObject target = null;
            float distance = 9999;

            foreach (GameObject _target in currentEnemies)
            {
                Transform magnitudeCheck = _target.transform.Find("MagnitudeCheck");

                float targetDistance = Vector3.Distance(transform.position, magnitudeCheck.position);
                bool isWithinRange = targetDistance <= range;

                if (isWithinRange && targetDistance < distance)
                {
                    distance = targetDistance;
                    target = _target;
                }
            }

           if (target != null)
           {

                transform.LookAt(target.transform.position);
                HitEffect(target);

                StartCoroutine(StartCanFireCooldown());
           }
        }
    }
}
