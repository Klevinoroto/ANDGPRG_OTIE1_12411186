using UnityEngine;

public class Camera : MonoBehaviour
{

    int rotationSpeed = 10;
    int speed = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //this.transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime); // its local to its rotation typa movement
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
        //if (Input.GetKey(KeyCode.E))
        //{
        //    this.transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    this.transform.Rotate(new Vector3(0, -1, 0) * rotationSpeed * Time.deltaTime);
        //}
    }
}
