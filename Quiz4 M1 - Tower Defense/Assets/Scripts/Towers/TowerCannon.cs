using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerCannon : Tower
{

    private GameObject particles;
    private float splashDistance = 4f;

    protected override void Start()
    {
        base.Start();
        particles = Resources.Load<GameObject>("TowerModels/Cannon/BigExplosion");
    }

    public override void ApplyUniqueBonus(float value) { upgradeDescriptors += "\n(" + value.ToString() + "+ Splash Range)"; splashDistance += value; }

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

                TowerAimAt(target);
                HitEffect(target);

                targetMagnitude = target.transform.Find("MagnitudeCheck");

                audioSources[5].Play();

                GameObject effects = Instantiate(particles, transform);
                effects.transform.position = targetMagnitude.position;
                effects.GetComponent<ParticleSystem>().Emit(1);
                Destroy(effects, 1.9f);


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

    public override void TowerAimAt(GameObject _target)
    {
        Vector3 baseAimAt = _target.transform.Find("Target").transform.position - transform.position;
        baseAimAt.y = 0f;
        Quaternion lookRotationY = Quaternion.LookRotation(baseAimAt);

        LeanTween.cancel(towerModel);
        LeanTween.rotate(towerModel, Quaternion.Euler(0f, lookRotationY.eulerAngles.y, 0f).eulerAngles, 0.1f)
                 .setEase(LeanTweenType.easeOutQuad);
    }
}
