using TMPro;
using UnityEngine;

public class CoreEndPoint : MonoBehaviour
{

    float health = 100;
    bool gameOver = false;

    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject healthUI;

    private TextMeshProUGUI healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthText = healthUI.GetComponent<TextMeshProUGUI>();
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
            healthText.text = "Health: " + health.ToString();
            Destroy(other.gameObject);

            if (health < 0 && !gameOver)
            {
                gameOver = true;
                gameManager.EndScreen(false);
            }
        }
    }
}
