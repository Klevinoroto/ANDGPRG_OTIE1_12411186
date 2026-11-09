using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFire : Tower
{

    IEnumerator Burn(Enemy enemyScript)
    {
        for (int i = 0; i < 8; i++) // damage X 8 
        {
            if (enemyScript == null) { break; } // just incase they are already destroyed
            
            yield return new WaitForSeconds(1f); // 1 seconds apply time
            bool isKilled = enemyScript.TakeDamage(damage);
            RewardOnKill(isKilled, enemyScript.goldDropped);

            if (isKilled) { break; }
        }
    }

    public override void HitEffect(GameObject _target)
    {
        Enemy enemyScript = _target.transform.GetComponent<Enemy>();
        StartCoroutine(Burn(enemyScript));
    }
}
