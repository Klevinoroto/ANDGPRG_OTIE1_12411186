using Mono.Cecil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
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

    public void AdjustGold(int amount)
    {
        gold += amount;
        goldTextLabel.text = "Gold: " + gold.ToString();
    }

    void SelectTower(string towerName)
    {

        TowerInfo info = TowerData.Towers[towerName + "1"];

        if (info.cost <= gold)
        {
            
            Type towerClass = Type.GetType(info.name);


            towerPreview = Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);
            towerPreview.AddComponent(towerClass);
            towerPreview.transform.GetComponent<Tower>().towerType = towerName;
        }
    }

    void HighlightedTower()
    {
        if (towerPreview)
        {
            Tower towerScript = towerPreview.transform.GetComponent<Tower>();

            Vector3 worldPosition = new Vector3(0, 1000, 0);
            Vector3 screenPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            if (Input.GetMouseButtonDown(0) && towerScript.isPlaceable)
            {
                AdjustGold(-TowerData.Towers[towerScript.towerType].cost);
                towerScript.Place(enemies, this);
                towerPreview = null;
                return;
            }

            if (Physics.Raycast(ray, out RaycastHit hitData, 1000, layersToHit))
            {
                worldPosition = hitData.point;
            }
            towerPreview.transform.position = worldPosition;
        }
    }
}
