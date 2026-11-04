using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    //int rotationSpeed = 10;
    int speed = 10;

    void Start()
    {
        
    }

    void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += new Vector3(0, 0, -1) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
        }
    }
}
