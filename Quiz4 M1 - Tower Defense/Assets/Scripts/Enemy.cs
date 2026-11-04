using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private bool isFlying = false;

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
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            isKilled = true;
        }

        return isKilled;
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
