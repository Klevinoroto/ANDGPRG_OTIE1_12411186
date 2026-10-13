using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{

    [SerializeField] float maxHealth = 30;
    [SerializeField] float currentHealth;

    [SerializeField] GameObject arrowPrefab;

    Vector3 originalPosition;

    AudioSource hitSound;
    AudioSource dieSound;

    GameObject targetCharacter;

    int attackCooldown = 60;
    int currentAttackCooldown = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Character"))
            {
                targetCharacter = obj;
                break;
            }
        };

        AudioSource[] sounds = GetComponents<AudioSource>();
        hitSound = sounds[0];
        dieSound = sounds[1];

        currentAttackCooldown = attackCooldown;

        originalPosition = transform.position;

        currentHealth = maxHealth;
    }
    void Update()
    {
        if (name.Contains("Tower"))
        {
            currentAttackCooldown--;
            if (currentAttackCooldown <= 0 && targetCharacter != null)
            {
                GameObject newArrow = (GameObject)Instantiate(arrowPrefab);
                newArrow.transform.position = transform.position;
                newArrow.transform.LookAt(targetCharacter.transform);
                newArrow.transform.Rotate(0, 90f, 90f);

                Destroy(newArrow, 3);

                currentAttackCooldown = attackCooldown;
            }
        }
    }

    IEnumerator WaitAndRevert(float waitTime) { 
        yield return new WaitForSeconds(waitTime);
        transform.position = originalPosition;
    }

    IEnumerator WaitAndDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }


    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        hitSound.Play();

        transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        StartCoroutine(WaitAndRevert(0.08f));

        if (currentHealth <= 0)
        {

            if (name.Contains("Dummy"))
            {
                GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                GameObject level1Target = null;
                int count = 0;

                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.Contains("TargetDummy"))
                    {
                        count++;
                    }
                    else if (obj.name.Contains("Level1Complete"))
                    {
                        level1Target = obj;
                    }
                };

                if (count == 1)
                {
                    level1Target.SetActive(true);
                }
            }
            else
            {
                GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.Contains("Level3Complete"))
                    {
                        obj.SetActive(true);
                        break;
                    }
                };
            }

            MeshRenderer mesh = GetComponent<MeshRenderer>();
            mesh.enabled = false;

            CapsuleCollider myCollider = GetComponent<CapsuleCollider>();
            myCollider.enabled = false;

            dieSound.Play();
            StartCoroutine(WaitAndDestroy(0.5f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Sword"))
        {
            TakeDamage(10);
        }
    }
}
