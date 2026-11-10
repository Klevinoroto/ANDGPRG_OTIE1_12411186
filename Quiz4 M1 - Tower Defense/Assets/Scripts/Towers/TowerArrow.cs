using Mono.Cecil;
using UnityEngine;

public class TowerArrow : Tower
{

    private float arrowBleed = 0.5f;

    public override void ApplyUniqueBonus(float value) { upgradeDescriptors += " (" + value.ToString() + "+ Arrow Bleed)"; arrowBleed += value; }
    public override void HitEffect(GameObject _target)
    {
        Enemy enemyScript = _target.transform.GetComponent<Enemy>();
        bool isKilled = enemyScript.TakeDamage(damage);
        enemyScript.ApplyArrowBleed(arrowBleed); // the gimmick of Arrow bleed is that it stacks up, so the more arrows the more bleed and eventual payoff

        RewardOnKill(isKilled, enemyScript.goldDropped);
    }
}
