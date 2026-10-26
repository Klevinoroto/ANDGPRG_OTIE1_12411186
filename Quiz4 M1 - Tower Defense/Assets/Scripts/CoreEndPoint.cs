using UnityEngine;

public class CoreEndPoint : MonoBehaviour
{

    float health = 100;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Crystal created");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hello, Unity Console!");
        Destroy(other.gameObject);
    }
}
