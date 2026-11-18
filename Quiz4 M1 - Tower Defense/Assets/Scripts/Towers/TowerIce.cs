using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class TowerIce : Tower
{
    private GameObject particles;
    private float slowTime = 3f;

    protected override void Start()
    {
        base.Start();
        particles = Resources.Load<GameObject>("TowerModels/Ice/BigSplash");
    }

    public override void ApplyUniqueBonus(float value) { upgradeDescriptors += "\n(" + value.ToString() + "+ Slow Time)"; slowTime += value; }

    public override void HitEffect(GameObject _target)
    {
        Enemy enemyScript = _target.transform.GetComponent<Enemy>();
        bool isKilled = enemyScript.TakeDamage(damage);
        enemyScript.ApplyIceSlow(slowTime);

        RewardOnKill(isKilled, enemyScript.goldDropped);
    }

    public override void Shoot()
    {

        List<GameObject> currentEnemies = new List<GameObject>();
        foreach (Transform child in enemiesFolder.transform)
        {
            currentEnemies.Add(child.gameObject);
        }

        if (canFire && currentEnemies.Count > 0)
        {

            bool isFired = false;
            foreach (GameObject _target in currentEnemies)
            {
                Transform magnitude = _target.transform.Find("MagnitudeCheck");
                float targetDistance = Vector3.Distance(transform.position, magnitude.position);

                if (targetDistance <= range)
                {
                    isFired = true;
                    HitEffect(_target);
                }
            }

            if (isFired) {

                audioSources[3].Play();

                GameObject effects = Instantiate(particles, transform);
                effects.transform.position = transform.position;
                effects.GetComponent<ParticleSystem>().Emit(1);
                Destroy(effects, 2f);

                StartCoroutine(StartCanFireCooldown());
            }
        }

    }
}
