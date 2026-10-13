using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;




public class Spaceship : MonoBehaviour
{

    AudioSource attackSound;
    AudioSource hitSound;

    [SerializeField] Camera gameCamera;
    Rigidbody rigidBody;

    [SerializeField] float speed = 30;
    [SerializeField] TextMeshProUGUI uiText;
    [SerializeField] TextMeshProUGUI objectiveText;

    [SerializeField] GameObject level1Marker;
    [SerializeField] GameObject level3Marker;

    [SerializeField] GameObject arrrowSpawner;
    [SerializeField] GameObject theTower;


    [SerializeField] GameObject swordPrefab;

    private Plane groundPlane;


    int attackCooldown = 200;
    int currentAttackCooldown = 0;

    private bool isDead = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        AudioSource[] sounds = GetComponents<AudioSource>();
        attackSound = sounds[0];
        //reachedGoalSound = sounds[1];

        groundPlane = new Plane(Vector3.up, transform.position);

        currentAttackCooldown = 0;
        Debug.Log("Character initialized.");
    }

    // Update is called once per frame
    void Update()
    {

        if (isDead) { return; }

        Movement();
        MouseFollow();

        currentAttackCooldown--;
        //Debug.Log(currentAttackCooldown);
        if (currentAttackCooldown <= 0)
        {
            currentAttackCooldown = attackCooldown;
            Attack();
        }
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rigidBody.AddForce(new Vector3(0, 0, 1) * speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidBody.AddForce(new Vector3(0, 0, -1) * speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.AddForce(new Vector3(-1, 0, 0) * speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidBody.AddForce(new Vector3(1, 0, 0) * speed, ForceMode.Force);
        }
    }

    private void MouseFollow()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            Vector3 targetPosition = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
            transform.LookAt(targetPosition);
        }
    }

    private void Attack()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            attackSound.Play();
            
            GameObject swordObj = (GameObject)Instantiate(swordPrefab);
            swordObj.transform.position = transform.position;
            swordObj.transform.rotation = this.transform.localRotation * Quaternion.Euler(90, 0, 0);
            Destroy(swordObj, 3);
        }
    }


    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(5.0f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Arrow"))
        {
            isDead = true;

            AudioSource[] sounds = gameCamera.GetComponents<AudioSource>();
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Stop();
            sounds[3].Play();
            sounds[4].Play();
            objectiveText.text = "The Knight is Dead!";

            this.transform.position = new Vector3(17, -10f, -102);
            StartCoroutine(ResetGame());
        }
        if (other.gameObject.name.Contains("Level1Complete"))
        {

            AudioSource[] sounds = gameCamera.GetComponents<AudioSource>();
            sounds[0].Stop();
            sounds[1].Play();
            arrrowSpawner.SetActive(true);

            gameCamera.transform.position = new Vector3(34, 21.2f, -102);
            this.transform.position = new Vector3(17, 0.245f, -102);
            objectiveText.text = "Level 2 Objective: Reach through the other side!";
        }

        if (other.gameObject.name.Contains("Level2Complete"))
        {

            AudioSource[] sounds = gameCamera.GetComponents<AudioSource>();
            sounds[1].Stop();
            sounds[2].Play();
            arrrowSpawner.SetActive(false);
            theTower.SetActive(true);

            gameCamera.transform.position = new Vector3(34, 21.2f, -166);
            this.transform.position = new Vector3(17, 0.245f, -166);
            objectiveText.text = "Level 3 Objective: Destroy the Tower!";
        }

        if (other.gameObject.name.Contains("Level3Complete"))
        {
            AudioSource[] sounds = gameCamera.GetComponents<AudioSource>();
            sounds[2].Stop();
            sounds[5].Play();
            objectiveText.text = "GAME COMPLETE!";

            other.gameObject.SetActive(false);
        }
    }
}