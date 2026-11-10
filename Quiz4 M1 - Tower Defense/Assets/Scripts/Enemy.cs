using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{

    [SerializeField] private bool isFlying = false;

    private float arrowBleed = 0;

    private bool isIced = false;

    private float currentHealth;
    [SerializeField] private float _maxHealth = 100;
    public float maxHealth
    {
        get { return _maxHealth; }
        set {
            currentHealth = _maxHealth; 
            _maxHealth = value; 
        }
    }

    [SerializeField] private int _goldDropped = 1;
    public int goldDropped
    {
        get { return _goldDropped; }
        set { _goldDropped = value; }
    }

    int unitDamageToCore = 1;

    NavMeshAgent agent;


    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(GameManager.Instance.Core.transform.position);

        currentHealth = maxHealth;
    }


    // Actual events & functions
    public bool TakeDamage(float damage)
    {
        bool isKilled = false;
        float finalDamage = damage + arrowBleed;

        currentHealth -= finalDamage;
        //print(finalDamage);

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            isKilled = true;
        }

        return isKilled;
    }
    public void ApplyArrowBleed(float bleedAmount) { arrowBleed += bleedAmount; }

    IEnumerator IceSlow(float slowTime) // reduce to 20% movement speed
    {
        print("BRO GOT ICED");
        isIced = true;
        agent.speed *= 0.2f;
        yield return new WaitForSeconds(slowTime);
        agent.speed /= 0.2f;
        isIced = false;
    }
    public void ApplyIceSlow(float slowTime)
    {
        if (!isIced) { StartCoroutine(IceSlow(slowTime)); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("EndPoint"))
        {
            Destroy(this.gameObject);
        }
    }
}
