using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Spaceship"))
        {
            Destroy(this.gameObject);
        }
    }
}
