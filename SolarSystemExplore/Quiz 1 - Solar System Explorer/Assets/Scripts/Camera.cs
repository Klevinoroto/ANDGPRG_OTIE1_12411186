using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] GameObject playerSpaceShip;

    void Update()
    {
        this.transform.position = playerSpaceShip.transform.position;
        this.transform.rotation = playerSpaceShip.transform.rotation;
    }
}
