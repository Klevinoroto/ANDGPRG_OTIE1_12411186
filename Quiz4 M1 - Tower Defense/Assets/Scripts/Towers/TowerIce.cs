using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class TowerIce : Tower
{
    public override void HitEffect(GameObject _target)
    {
        Enemy enemyScript = _target.transform.GetComponent<Enemy>();
        bool isKilled = enemyScript.TakeDamage(info.damage);
        enemyScript.ApplyIceSlow();

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

                if (targetDistance <= info.range)
                {
                    isFired = true;
                    HitEffect(_target);
                }
            }

            if (isFired) { StartCoroutine(StartCanFireCooldown()); }
        }

    }
}
