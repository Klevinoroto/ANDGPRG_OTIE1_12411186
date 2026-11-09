using Mono.Cecil;
using UnityEngine;

public class TowerArrow : Tower
{
    public override void HitEffect(GameObject _target)
    {
        Enemy enemyScript = _target.transform.GetComponent<Enemy>();
        bool isKilled = enemyScript.TakeDamage(damage);
        enemyScript.ApplyArrowBleed(); // the gimmick of Arrow bleed is that it stacks up, so the more arrows the more bleed and eventual payoff

        RewardOnKill(isKilled, enemyScript.goldDropped);
    }
}
