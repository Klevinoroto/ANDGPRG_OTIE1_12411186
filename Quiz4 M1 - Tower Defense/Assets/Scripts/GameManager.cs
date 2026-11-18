using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject core;
    public GameObject Core { get => core; }

    private void Awake()
    {
        Instance = this;
    }

    public void EndScreen(bool hasWon)
    {
        endScreen.SetActive(true);

        TextMeshProUGUI endMessage = endScreen.transform.Find("Message").GetComponent<TextMeshProUGUI>();

        if (hasWon) { endMessage.text = "YOU WIN!"; }
        else { endMessage.text = "You Lose."; }

        void OnButtonClicked()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        Button button = endScreen.transform.Find("Restart").GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }
}
