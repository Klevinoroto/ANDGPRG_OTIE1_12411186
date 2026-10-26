using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] bool isFlying = false;

    [SerializeField] float maxHealth = 100;
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    float currentHealth;

    [SerializeField] int goldDropped = 1;
    public int GoldDropped
    {
        get { return goldDropped; }
        set { goldDropped = value; }
    }

    int unitDamageToCore = 1;

    NavMeshAgent agent;


    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(GameManager.Instance.Core.transform.position);

        currentHealth = maxHealth;


    }

    // Setters & Getters
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public void SetMaxHealth(float _maxHealth)
    {
        maxHealth = _maxHealth;
        currentHealth = maxHealth;
    }
    public float GetGoldDropped()
    {
        return goldDropped;
    }
    public void SetGoldDropped(int _goldDropped)
    {
        goldDropped = _goldDropped;
    }


    // Actual events & functions
    private void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.name.Contains("EndPoint"))
        {
            Destroy(this.gameObject);
        }
    }
}
