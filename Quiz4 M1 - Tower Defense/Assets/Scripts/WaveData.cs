using UnityEngine;

public class WaveData : MonoBehaviour
{
    public GameObject enemyType;
    public int amountToSpawn;

    public WaveData(GameObject _enemyType, int _amountToSpawn)
    {
        enemyType = _enemyType;
        amountToSpawn = _amountToSpawn;
    }
}
