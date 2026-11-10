using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;

public class TowerCannon : Tower
{

    private float splashDistance = 4f;

    public override void ApplyUniqueBonus(float value) { upgradeDescriptors += " (" + value.ToString() + "+ Splash Range)"; splashDistance += value; }

    public override void HitEffect(GameObject _target)
    {
        Enemy enemyScript = _target.transform.GetComponent<Enemy>();
        bool isKilled = enemyScript.TakeDamage(damage);

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

            GameObject target = null;
            Transform targetMagnitude = null;
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

                targetMagnitude = target.transform.Find("MagnitudeCheck");

                // Cannon splash
                foreach (GameObject _splashTarget in currentEnemies)
                {
                    Transform magnitudeCheck = _splashTarget.transform.Find("MagnitudeCheck");
                    bool isWithinSplash = Vector3.Distance(targetMagnitude.position, magnitudeCheck.position) <= splashDistance;

                    if (isWithinSplash && target != _splashTarget)
                    {
                        HitEffect(_splashTarget);
                    }
                }


                StartCoroutine(StartCanFireCooldown());
            }
        }
    }
}
