using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private float bulletSpeed = 20;

    public void setBulletSpeed(float _speed)
    {
        bulletSpeed = _speed;
    }

    void Update()
    {
        this.transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
