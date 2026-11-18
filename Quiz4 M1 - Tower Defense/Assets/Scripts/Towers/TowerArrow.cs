using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;

public class TowerArrow : Tower
{

    private GameObject arrowPrefab;
    private float arrowBleed = 0.5f;

    protected override void Start()
    {
        base.Start();
        arrowPrefab = Resources.Load<GameObject>("TowerModels/Arrow/Arrow");
    }
    public override void ApplyUniqueBonus(float value) { upgradeDescriptors += "\n(" + value.ToString() + "+ Arrow Bleed)"; arrowBleed += value; }
    public override void HitEffect(GameObject _target)
    {
        Enemy enemyScript = _target.transform.GetComponent<Enemy>();
        bool isKilled = enemyScript.TakeDamage(damage);
        enemyScript.ApplyArrowBleed(arrowBleed); // the gimmick of Arrow bleed is that it stacks up, so the more arrows the more bleed and eventual payoff

        GameObject arrow = Instantiate(arrowPrefab, _target.transform, false);
        arrow.transform.localPosition = _target.transform.Find("Target").transform.localPosition;
        arrow.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        RewardOnKill(isKilled, enemyScript.goldDropped);
    }

    public override void TowerAimAt(GameObject _target)
    {

        audioSources[2].Play();

        GameObject turret = towerModel.transform.Find("Base").Find("Turret").gameObject;

        Vector3 direction = (_target.transform.Find("Target").transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        LeanTween.cancel(turret);
        LeanTween.rotate(turret, targetRotation.eulerAngles, 0.1f)
                 .setEase(LeanTweenType.easeOutQuad);
    }
}
