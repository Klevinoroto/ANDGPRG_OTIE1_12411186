using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    private Player player = null;
    private GameObject enemiesFolder = null;
    private bool isPlaced = false;
    private bool canFire = true;
    private float range = 7;

    public string towerType;
    private TowerInfo info;
    private GameObject towerModel;

    [SerializeField] private SphereCollider rangeCollision;

    void Start()
    {
        info = TowerData.Towers[towerType];
        
        towerModel = Instantiate(info.model, transform);
        towerModel.transform.localPosition = Vector3.zero;
        towerModel.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced) { return; }
        Shoot();
    }

    public void Place(GameObject _enemiesFolder, Player _player)
    {
        player = _player;
        enemiesFolder = _enemiesFolder;
        rangeCollision.radius = range;
        rangeCollision.enabled = true;
        isPlaced = true;
    }

    void Shoot()
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

                Enemy enemyScript = target.transform.GetComponent<Enemy>();
                bool isKilled = enemyScript.TakeDamage(TowerData.Towers[towerType].damage);

                if (isKilled)
                {
                    player.AddGold(enemyScript.goldDropped);
                }

                IEnumerator StartCooldown()
                {
                    canFire = false;
                    yield return new WaitForSeconds(0.5f);
                    canFire = true;
                }
                StartCoroutine(StartCooldown());
           }
        }
    }
}
