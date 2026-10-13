using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    int spawnCD = 60;
    int currentSpawnCD = 0;

    [SerializeField] GameObject arrowPrefab;

    void Start()
    {
        currentSpawnCD = spawnCD;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnCD--;
        if (currentSpawnCD <= 0)
        {
            SpawnArrow();
            currentSpawnCD = spawnCD;
        }
    }


    void SpawnArrow()
    {
        GameObject newArrow = (GameObject)Instantiate(arrowPrefab);
        newArrow.transform.position = transform.position + new Vector3(0, 0, Random.Range(-15.5f, 15.5f));
        newArrow.transform.rotation = this.transform.localRotation * Quaternion.Euler(90, 0, 0);
        Destroy(newArrow, 3);
    }
}
