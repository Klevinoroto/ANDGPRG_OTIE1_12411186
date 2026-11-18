using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class Spawner : MonoBehaviour
{

    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject knightPrefab;
    [SerializeField] GameObject knightHorsePrefab;
    [SerializeField] GameObject knightShieldPrefab;
    [SerializeField] GameObject ravenPrefab;
    [SerializeField] GameObject dragonPrefab;

    [SerializeField] GameObject theGeneralPrefab;
    [SerializeField] GameObject dragonKingPrefab;
    [SerializeField] GameObject grandGeneralPrefab;

    [SerializeField] GameObject enemyFolder;
    [SerializeField] Transform spawnPoint;

    [SerializeField] GameObject waveHeader;
    [SerializeField] GameObject waveDescription;

    int currentWave = 0;
    int currentBatch = 0;
    int currentEnemyBatchAmount = 0;
    int currentEnemyBatchSpawned = 0;

    bool canSpawn = false;
    bool isCountingDown = false;
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
        };

        WaveData[] wave2 = new WaveData[]
        {
            new WaveData(ravenPrefab, 3),
            new WaveData(knightPrefab, 7),
            new WaveData(ravenPrefab, 2),
        };

        WaveData[] wave3 = new WaveData[]
        {
            new WaveData(knightHorsePrefab, 2),
            new WaveData(knightPrefab, 8),
            new WaveData(theGeneralPrefab, 1),
        };

        WaveData[] wave4 = new WaveData[]
        {
            new WaveData(ravenPrefab, 4),
            new WaveData(knightPrefab, 4),
            new WaveData(knightHorsePrefab, 4),
            new WaveData(ravenPrefab, 4),
        };

        WaveData[] wave5 = new WaveData[]
        {
            new WaveData(knightHorsePrefab, 4),
            new WaveData(knightPrefab, 5),
        };

        WaveData[] wave6 = new WaveData[]
        {
            new WaveData(knightPrefab, 4),
            new WaveData(ravenPrefab, 4),
            new WaveData(knightShieldPrefab, 3),
        };

        WaveData[] wave7 = new WaveData[]
        {
            new WaveData(knightPrefab, 8),
            new WaveData(knightHorsePrefab, 3),
            new WaveData(knightShieldPrefab, 3),
            new WaveData(dragonPrefab, 2),
        };

        WaveData[] wave8 = new WaveData[]
        {
            new WaveData(knightShieldPrefab, 4),
            new WaveData(ravenPrefab, 4),
            new WaveData(knightHorsePrefab, 4),
            new WaveData(theGeneralPrefab, 2),
        };

        WaveData[] wave9 = new WaveData[]
        {
            new WaveData(theGeneralPrefab, 1),
            new WaveData(dragonPrefab, 3),
            new WaveData(ravenPrefab, 12),
            new WaveData(dragonPrefab, 5),
            new WaveData(dragonKingPrefab, 1),
        };

        WaveData[] wave10 = new WaveData[]
        {
            new WaveData(knightHorsePrefab, 6),
            new WaveData(dragonPrefab, 3),
            new WaveData(knightShieldPrefab, 5),
            new WaveData(theGeneralPrefab, 4),
            new WaveData(grandGeneralPrefab, 1),
        };

        waves.Add(1, wave1);
        waves.Add(2, wave2);
        waves.Add(3, wave3);
        waves.Add(4, wave4);
        waves.Add(5, wave5);
        waves.Add(6, wave6);
        waves.Add(7, wave7);
        waves.Add(8, wave8);
        waves.Add(9, wave9);
        waves.Add(10, wave10);

        StartNewWave();

    }

    // Update is called once per frame
    void Update()
    {

        if (!canSpawn)
        {
            int numberOfEnemies = enemyFolder.transform.childCount;
            if (numberOfEnemies == 0 && !isCountingDown)
            {
                StartCoroutine(WaveCountDown());
                return;
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
            currentWave++; // to trigger the if statement on the countdown thing and officially end the game
            Debug.Log("Waves Complete you won!");
            return;
        }

        enemyBonusHealth += 10;

        if (currentWave % 4 == 0) { enemyGoldBonus += 1; }

        currentWave++;
        currentBatch = 0;
        UpdateBatchAmount();
    }

    public void LostGame()
    {
        canSpawn = false;
    }

    IEnumerator WaveCountDown()
    {
        isCountingDown = true;
        canSpawn = false;

        if (currentWave == waves.Count + 1)
        {
            gameManager.EndScreen(true);
            yield return new WaitForSeconds(99999f);
        }

        GetComponent<AudioSource>().Play();

        TextMeshProUGUI wave = waveHeader.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = waveDescription.GetComponent<TextMeshProUGUI>();

        for (int i = 5; i >= 1; i--)
        {
            description.text = i.ToString() + "s until the next wave.";
            yield return new WaitForSeconds(1f);
        }
        description.text = "";
        wave.text = "Wave " + currentWave.ToString();

        canSpawn = true;
        isCountingDown = false;
    }

    void Spawn(GameObject currentEnemyToSpawn)
    {
        GameObject enemyObj = Instantiate(currentEnemyToSpawn, spawnPoint.transform.position, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemyObj.transform.parent = enemyFolder.transform;
        enemy.goldDropped += enemyGoldBonus;
        enemy.maxHealth += enemyBonusHealth;
    }
}
