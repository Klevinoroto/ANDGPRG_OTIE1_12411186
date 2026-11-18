using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;

public class TowerFire : Tower
{

    private GameObject particles;
    private float applyTime = 0.5f;

    protected override void Start()
    {
        base.Start();
        particles = Resources.Load<GameObject>("TowerModels/Fire/OnFire");
    }
    IEnumerator Burn(GameObject _target, Enemy enemyScript)
    {

        audioSources[4].Play();

        GameObject effects = Instantiate(particles, _target.transform, false);
        effects.transform.localPosition = _target.transform.Find("Target").transform.localPosition;

        for (int i = 0; i < 8; i++) // damage X 8 
        {
            if (enemyScript == null) { break; } // just incase they are already destroyed
            
            yield return new WaitForSeconds(applyTime); // 1 seconds apply time
            bool isKilled = enemyScript.TakeDamage(damage);
            RewardOnKill(isKilled, enemyScript.goldDropped);

            if (isKilled) { break; }
        }

        Destroy(effects, 2f);
    }

    public override void ApplyUniqueBonus(float value) { upgradeDescriptors += "\n(" + value.ToString() + "- Apply Time)"; applyTime -= value; }

    public override void HitEffect(GameObject _target)
    {
        Enemy enemyScript = _target.transform.GetComponent<Enemy>();
        StartCoroutine(Burn(_target, enemyScript));
    }

    public override void TowerAimAt(GameObject _target)
    {
        GameObject turret = towerModel.transform.Find("Turret").gameObject;

        Vector3 direction = (_target.transform.Find("Target").transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        LeanTween.cancel(turret);
        LeanTween.rotate(turret, targetRotation.eulerAngles, 0.1f)
                 .setEase(LeanTweenType.easeOutQuad);
    }
}
