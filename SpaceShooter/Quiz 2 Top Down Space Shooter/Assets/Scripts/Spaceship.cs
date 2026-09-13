using TMPro;
using UnityEngine;

public class Spaceship : MonoBehaviour
{

    [SerializeField] float speed = 5;
    [SerializeField] float rotationSpeed = 100;
    [SerializeField] TextMeshProUGUI uiText;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletPoint1;
    [SerializeField] GameObject bulletPoint2;
    [SerializeField] GameObject bulletPoint3;
    [SerializeField] GameObject bulletPoint4;
    private GameObject currentBulletPoint;

    [SerializeField] Material bullet1Material;
    [SerializeField] Material bullet2Material;
    [SerializeField] Material bullet3Material;
    [SerializeField] Material bullet4Material;
    private Material currentBulletMaterial;

    int bulletPoint1Cooldown = 50;
    int bulletPoint2Cooldown = 15;
    int bulletPoint3Cooldown = 110;
    int bulletPoint4Cooldown = 70;
    int currentSetBulletCooldown = 0;
    int currentBulletCooldown = 0;

    void Start()
    {
        currentBulletPoint = bulletPoint1;
        currentBulletMaterial = bullet1Material;
        currentSetBulletCooldown = bulletPoint1Cooldown;
        Debug.Log("Spaceship initialized.");
    }

    // Update is called once per frame
    void Update()
    {

        Movement();

        ChangeNozzle();
        currentBulletCooldown--;
        if (currentBulletCooldown <= 0)
        {
            ShootBullet();
            currentBulletCooldown = currentSetBulletCooldown;
        }
    }

    private void ChangeNozzle()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBulletPoint = bulletPoint1;
            currentBulletMaterial = bullet1Material;
            currentSetBulletCooldown = bulletPoint1Cooldown;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBulletPoint = bulletPoint2;
            currentBulletMaterial = bullet2Material;
            currentSetBulletCooldown = bulletPoint2Cooldown;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentBulletPoint = bulletPoint3;
            currentBulletMaterial = bullet3Material;
            currentSetBulletCooldown = bulletPoint3Cooldown;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentBulletPoint = bulletPoint4;
            currentBulletMaterial = bullet4Material;
            currentSetBulletCooldown = bulletPoint4Cooldown;
        }
    }

    private void ShootBullet()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GameObject bulletObj = (GameObject)Instantiate(bulletPrefab);
            Renderer renderer = bulletObj.GetComponent<Renderer>();
            bulletObj.transform.position = currentBulletPoint.transform.position;
            bulletObj.transform.rotation = this.transform.localRotation * Quaternion.Euler(90, 0, 0);
            renderer.material = currentBulletMaterial;

            if (currentBulletPoint == bulletPoint2)
            {
                bulletObj.transform.localScale = new Vector3(0.3f, 0.5f, 0.3f);
            }
            else if (currentBulletPoint == bulletPoint3)
            {
                bulletObj.transform.localScale = new Vector3(2f, 1f, 2f);
            }
            else if (currentBulletPoint == bulletPoint4)
            {
                bulletObj.transform.rotation = this.transform.localRotation * Quaternion.Euler(90, 0, 20);

                GameObject bulletObj2 = (GameObject)Instantiate(bulletPrefab);
                Renderer renderer2 = bulletObj2.GetComponent<Renderer>();
                bulletObj2.transform.position = currentBulletPoint.transform.position;
                bulletObj2.transform.rotation = this.transform.localRotation * Quaternion.Euler(90, 0, -20);
                renderer2.material = currentBulletMaterial;
            }

            Destroy(bulletObj, 5);
        }
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
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

    private void RigidBodyMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed);
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
}