using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class Tower : MonoBehaviour
{
    protected Player player = null;
    protected GameObject enemiesFolder = null;
    private bool isPlaced = false;
    protected bool canFire = true;

    public bool isPlaceable;
    public string towerType;
    public TowerInfo info;
    protected GameObject towerModel = null;

    private GameObject baseHighlight;
    private GameObject upgradeIndicator;
    private GameObject upgradeEffect;

    protected AudioSource[] audioSources;
    private AudioSource levelUpSFX;
    private AudioSource buildingSFX;

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
    public void ApplyBonusRange(float value) { upgradeDescriptors += "\n(" + value.ToString() + "+ Range)"; bonusRange += value; range += value; }
    public void ApplyBonusDamage(float value) { upgradeDescriptors += "\n(" + value.ToString() + "+ Damage)"; bonusDamage += value; damage += value; }
    public void ApplyBonusFirerate(float value) { upgradeDescriptors += "\n(" + value.ToString() + "+ Range)"; bonusFirerate += value; firerate -= value; }

    protected virtual void Start()
    {
        audioSources = GetComponents<AudioSource>();
        levelUpSFX = audioSources[0];
        buildingSFX = audioSources[1];

        upgradeEffect = Resources.Load<GameObject>("TinyExplosion");

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
        buildingSFX.Play();
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

    public void UpgradeEffect()
    {
        if (tier == 0 ) { return; }

        levelUpSFX.Play();
        buildingSFX.Play();

        GameObject fx = Instantiate(upgradeEffect, transform);
        fx.transform.position = transform.position;
        fx.GetComponent<ParticleSystem>().Emit(1);
        Destroy(fx, 1.9f);
    }

    public void UpgradePath(string upgradeKey) {
        UpgradeEffect();

        TowerUpgrades.Towers[upgradeKey].Upgrade(this);
        nextUpgradeCost = TowerData.Towers[towerType + (tier+1).ToString()].cost;
    }
    public void ApplyNextTier()
    {
        UpgradeEffect();

        tier++;

        info = TowerData.Towers[towerType + tier.ToString()];
        range = info.range + bonusRange;
        damage = info.damage + bonusDamage;
        firerate = info.shootCooldown + bonusFirerate;

        if (towerModel != null) { Destroy(towerModel); }
        towerModel = Instantiate(info.model, transform);
        towerModel.transform.localPosition = Vector3.zero;
        towerModel.transform.localRotation = Quaternion.identity;

        if (tier == 3) { nextUpgradeCost = 999999; }
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

    public virtual void TowerAimAt(GameObject _target) { }

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

                TowerAimAt(target);
                HitEffect(target);

                StartCoroutine(StartCanFireCooldown());
           }
        }
    }
}
