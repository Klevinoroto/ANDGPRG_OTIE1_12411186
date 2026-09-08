using TMPro;
using UnityEngine;

public class Spaceship : MonoBehaviour
{

    [SerializeField] float speed = 5;
    [SerializeField] float rotationSpeed = 100;
    [SerializeField] TextMeshProUGUI uiText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Started!");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(new Vector3(0, 0, 1) * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(new Vector3(0, 0, -1) * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(new Vector3(1, 0, 0) * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            this.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        uiText.text = other.gameObject.name;
    }

    private void OnTriggerExit(Collider other)
    {
        uiText.text = "";
    }

}