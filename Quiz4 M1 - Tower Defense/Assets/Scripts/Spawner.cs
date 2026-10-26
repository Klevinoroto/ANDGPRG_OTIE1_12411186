using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{

    [SerializeField] GameObject knightPrefab;
    [SerializeField] GameObject theGeneralPrefab;

    [SerializeField] GameObject dragonPrefab;
    [SerializeField] GameObject dragonKingPrefab;

    [SerializeField] GameObject enemyFolder;
    [SerializeField] Transform spawnPoint;

    int currentWave = 0;
    int currentBatch = 0;
    int currentEnemyBatchAmount = 0;
    int currentEnemyBatchSpawned = 0;

    bool canSpawn = true;
    int spawnSpeed = 30;
    int currentSpawnSpeed;

    float enemyBonusHealth = 0;
    int enemyGoldBonus = 0;

    public Dictionary<int, WaveData[]> waves = new Dictionary<int, WaveData[]>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        currentSpawnSpeed = spawnSpeed;


        WaveData[] wave1 = new WaveData[]
        {
            new WaveData(knightPrefab, 3),
            new WaveData(dragonPrefab, 1),
            new WaveData(theGeneralPrefab, 1),
        };

        WaveData[] wave2 = new WaveData[]
        {
            new WaveData(knightPrefab, 5),
            new WaveData(dragonPrefab, 3),
            new WaveData(dragonKingPrefab, 1),
        };

        waves.Add(1, wave1);
        waves.Add(2, wave2);

        StartNewWave();

    }

    // Update is called once per frame
    void Update()
    {

        if (!canSpawn)
        {
            int numberOfEnemies = enemyFolder.transform.childCount;
            if (numberOfEnemies == 0)
            {
                canSpawn = true;
            }
            else
            {
                return;
            }
        }


        currentSpawnSpeed--;
        if (currentSpawnSpeed <= 0)
        {
            Spawn(waves[currentWave][currentBatch].enemyType);
            currentEnemyBatchSpawned++;

            if (currentEnemyBatchSpawned == currentEnemyBatchAmount)
            {
                currentEnemyBatchSpawned = 0;
                currentBatch++;
                if (currentBatch > waves[currentWave].Length-1)
                {
                    canSpawn = false;
                    StartNewWave();
                    return; // so it doesnt call UpdateBatchAmount again
                }
                UpdateBatchAmount();
            }


            currentSpawnSpeed = spawnSpeed;
        }
    }

    void UpdateBatchAmount()
    {
        currentEnemyBatchAmount = waves[currentWave][currentBatch].amountToSpawn;
    }

    void StartNewWave()
    {
        Debug.Log("New wave has been started!");

        if (currentWave + 1 > waves.Count)
        {
            canSpawn = false;
            Debug.Log("Waves Complete you won!");
            return;
        }

        enemyBonusHealth += 5;
        enemyGoldBonus += 1;

        currentWave++;
        currentBatch = 0;
        UpdateBatchAmount();
    }

    void Spawn(GameObject currentEnemyToSpawn)
    {
        GameObject enemyObj = Instantiate(currentEnemyToSpawn, spawnPoint.transform.position, Quaternion.identity);
        enemyObj.transform.parent = enemyFolder.transform;
        enemyObj.GetComponent<Enemy>().GoldDropped += enemyGoldBonus;
        enemyObj.GetComponent<Enemy>().MaxHealth += enemyBonusHealth;
        //enemyObj.SetMaxHealth(enemyObj.GetMaxHealth + enemyBonusHealth);
        //enemyObj.SetGoldDropped(enemyObj.GetGoldDropped + enemyGoldBonus);
    }
}
