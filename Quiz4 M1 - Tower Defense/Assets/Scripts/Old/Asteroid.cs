using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] bool isStationary = false;
    [SerializeField] float speed;

    void Start()
    {

        speed = Random.Range(15f, 30f);
        
        if (!isStationary)
        {
            Destroy(this.gameObject, 20);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Spaceship"))
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (!isStationary)
        {
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
