using TMPro;
using UnityEngine;

public class ObjectivesHandler : MonoBehaviour
{

    [SerializeField] GameObject meteorPrefab;
    [SerializeField] GameObject spaceShip;
    [SerializeField] GameObject gameCamera;
    [SerializeField] TextMeshProUGUI objectiveText;

    private bool inLevelOne = true;
    private bool inLevelTwo = true;
    private int meteorSpawnCooldown = 35; // every 35 frames
    private int currentMeteorSpawnCooldown = 0;
    private int meteorsSent = 0;

    void Start()
    {
        
    }
    void Update()
    {

        currentMeteorSpawnCooldown++;
        if (inLevelOne)
        {
            if (meteorsSent <= 50 && currentMeteorSpawnCooldown >= meteorSpawnCooldown)
            {
                currentMeteorSpawnCooldown = 0;
                meteorsSent++;
                GameObject meteor = (GameObject)Instantiate(meteorPrefab);
                meteor.transform.position = spaceShip.transform.position + new Vector3(Random.Range(-50, 50), 0, 100 + Random.Range(-30, 30));
                meteor.transform.rotation = Quaternion.Euler(180, Random.Range(-30, 30), 0);

            }
            else if (currentMeteorSpawnCooldown >= 200)
            {
                inLevelOne = false;
            }
        }
        else if (inLevelTwo)
        {
            inLevelTwo = false; // trigger once just to update the text
            objectiveText.text = "Level 2 Objective: RUSH TO THE GREEN PORTAL";
            gameCamera.transform.position = new Vector3(34, 200, 16);
            for (int i = 0; i < 100; i++)
            {
                GameObject meteor = (GameObject)Instantiate(meteorPrefab);
                meteor.transform.position = spaceShip.transform.position + new Vector3(Random.Range(-100, 100), 0, -150 + Random.Range(0, 30));
                meteor.transform.rotation = Quaternion.Euler(0, Random.Range(-30, 30), 0);
            }
        }
    }
}
