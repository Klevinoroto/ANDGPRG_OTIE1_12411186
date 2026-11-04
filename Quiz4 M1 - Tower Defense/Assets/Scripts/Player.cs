using Mono.Cecil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{

    [SerializeField] private GameObject playerUI;
    private TextMeshProUGUI goldTextLabel = null;

    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject towerPreview;

    public LayerMask layersToHit;

    private int gold = 20;

    float RoundNearestTenth(float value)
    {
        return Mathf.Round(value * 10f) / 10f;
    }

    void Start()
    {

        goldTextLabel = playerUI.transform.Find("GoldCount").GetComponent<TextMeshProUGUI>();

        foreach (Transform child in playerUI.transform)
        {
            if (child.transform.name.Contains("Tower"))
            {

                void OnButtonClicked()
                {
                    SelectTower(child.transform.name.Replace("Tower", ""));
                }

                Button button = child.gameObject.GetComponent<Button>();
                button.onClick.AddListener(OnButtonClicked);
            }
        }
    }


    void Update()
    {
        HighlightedTower();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        goldTextLabel.text = "Gold: " + gold.ToString();
    }

    void SelectTower(string towerName)
    {
        //string path = "TowerModels/" + tower + "/Base";
        //GameObject towerModel = Resources.Load<GameObject>(path);

        towerPreview = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);
        towerPreview.transform.GetComponent<Tower>().towerType = towerName;

        //Instantiate(towerModel, Vector3.zero, Quaternion.identity, towerPreview.transform);
    }

    void HighlightedTower()
    {
        if (towerPreview)
        {
            bool isPlaceable = RoundNearestTenth(towerPreview.transform.position.y) == 2.2f;
            Vector3 worldPosition = new Vector3(0, 1000, 0);
            Vector3 screenPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            Transform previewTransform = towerPreview.transform.Find("PreviewPlaceable");
            GameObject previewGameObject = previewTransform.gameObject;
            MeshRenderer preview = previewGameObject.GetComponent<MeshRenderer>();

            if (Input.GetMouseButtonDown(0) && isPlaceable)
            {
                Destroy(previewGameObject);
                Tower towerScript = towerPreview.transform.GetComponent<Tower>();
                towerScript.Place(enemies, this);

                towerPreview = null;

                return;
            }

            if (Physics.Raycast(ray, out RaycastHit hitData, 1000, layersToHit))
            {
                worldPosition = hitData.point;
                if (isPlaceable)
                {
                    preview.material.color = Color.green;
                }
                else
                {
                    preview.material.color = Color.red;
                }
            }
            towerPreview.transform.position = worldPosition;
        }
    }
}
