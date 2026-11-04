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
        if (other.gameObject.layer != LayerMask.NameToLayer("Tower"))
        {
            health -= 10;
            Destroy(other.gameObject);
        }
    }
}
