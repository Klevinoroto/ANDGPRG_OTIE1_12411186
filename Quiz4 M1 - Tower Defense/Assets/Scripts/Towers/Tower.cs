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
    protected TowerInfo info;
    private GameObject towerModel;

    private GameObject previewPlaceable;

    protected int level;
    protected float range;
    protected float damage;
    protected float firerate;

    public float bonusRange;
    public float bonusDamage;
    public float bonusFirerate;

    float RoundNearestTenth(float value)
    {
        return Mathf.Round(value * 10f) / 10f;
    }

    void Start()
    {
        level = 1;
        ApplyStats(level);
        
        towerModel = Instantiate(info.model, transform);
        towerModel.transform.localPosition = Vector3.zero;
        towerModel.transform.localRotation = Quaternion.identity;

        previewPlaceable = transform.Find("PreviewPlaceable").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced) {

            MeshRenderer preview = previewPlaceable.GetComponent<MeshRenderer>();
            isPlaceable = RoundNearestTenth(transform.position.y) == 2.2f;

            if (isPlaceable) { preview.material.color = Color.green; }
            else             { preview.material.color = Color.red; }
        }
        else
        {
            Shoot();
        }
    }

    public void Place(GameObject _enemiesFolder, Player _player)
    {
        player = _player;
        enemiesFolder = _enemiesFolder;
        Destroy(previewPlaceable);
        isPlaced = true;
    }

    private void ApplyStats(int _level)
    {
        level = _level;
        info = TowerData.Towers[towerType + _level.ToString()];
        range = info.range;
        damage = info.damage;
        firerate = info.shootCooldown;
    }

    public IEnumerator StartCanFireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(info.shootCooldown);
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
                bool isWithinRange = targetDistance <= info.range;

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
