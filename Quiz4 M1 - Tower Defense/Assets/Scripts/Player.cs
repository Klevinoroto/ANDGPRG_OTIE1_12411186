using Mono.Cecil;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject towerInfoUI;
    private TextMeshProUGUI goldTextLabel = null;

    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject towerPreview;
    public LayerMask layersToHit;

    private int gold = 20;
    private Tower selectedTower = null;
    private string upgradeKey;

    // UI STUFF
    GameObject infoTab;
    TextMeshProUGUI towerName;
    TextMeshProUGUI towerLevel;
    TextMeshProUGUI towerDescription;
    GameObject confirmUpgradeButton;

    GameObject upgradesTab;
    TextMeshProUGUI upgradeDescription;

    void Start()
    {

        goldTextLabel = playerUI.transform.Find("GoldCount").GetComponent<TextMeshProUGUI>();

        infoTab = towerInfoUI.transform.Find("Info").gameObject;
        towerName = infoTab.transform.Find("TowerName").gameObject.GetComponent<TextMeshProUGUI>();
        towerLevel = infoTab.transform.Find("Level").gameObject.GetComponent<TextMeshProUGUI>();
        towerDescription = infoTab.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();
        confirmUpgradeButton = infoTab.transform.Find("Confirmation").gameObject;

        GameObject upgradesTab = towerInfoUI.transform.Find("Upgrades").gameObject;
        upgradeDescription

        TowerStatsFunctionality();

        foreach (Transform child in playerUI.transform)
        {
            if (child.name.Contains("Tower") && child.name != "TowerInfo")
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
                AdjustGold(-towerScript.info.cost);
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

    void TowerStatsFunctionality()
    {
        // Exit the menu
        GameObject exitButton = towerInfoUI.transform.Find("ExitButton").gameObject;
        exitButton.GetComponent<Button>().onClick.AddListener(CloseTowerInfo);

        confirmUpgradeButton.GetComponent<Button>().onClick.AddListener(ConfirmUpgrade);

        // One Choice
        GameObject nextTierOption = upgradesTab.transform.Find("NextTier").gameObject;
        Button nextTierButton = nextTierOption.transform.Find("Button").gameObject.GetComponent<Button>();
        nextTierButton.onClick.AddListener(UpgradeSelected);


        // Three Choices
        void ChosenPathway(string path) // A or B or C
        {
            upgradeKey = selectedTower.towerType + (selectedTower.level + 1).ToString() + path;
            UpgradeSelected();
        }
        GameObject upgradePathsOption = upgradesTab.transform.Find("ThreeChoice").gameObject;
        foreach (Transform child in upgradePathsOption.transform)
        {
            string pathName = child.gameObject.name;
            if (pathName.Contains("Option"))
            {
                Button button = child.gameObject.GetComponent<Button>();
                button.onClick.AddListener(() => ChosenPathway(pathName.Replace("Option", "")));
            }
        }
    }

    public void OpenTowerInfo(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Deselect();
        }

        confirmUpgradeButton.SetActive(false);
        towerInfoUI.SetActive(true);
        selectedTower = tower;

        towerName.text = selectedTower.towerType + " Tower";
        towerLevel.text = "Level " + selectedTower.level.ToString();
        towerDescription.text = selectedTower.info.description;
        // set ui stuff here like its info an stuff
    }

    public void CloseTowerInfo()
    {
        towerInfoUI.SetActive(false);
    }

    void UpgradeSelected()
    {
        confirmUpgradeButton.SetActive(true);

        if ((selectedTower.level + 1) % 2 == 0) // Upgrade path
        {
            TowerUpgrades.Towers[upgradeKey];
            print("Hey");
        }
        else // Upgrade tier
        {

        }
    }

    public void ConfirmUpgrade()
    {
        if ((selectedTower.level + 1) % 2 == 0) // Upgrade path
        {
            TowerUpgrades.Towers[upgradeKey].Upgrade(selectedTower);
            print("Hey");
        }
        else // Upgrade tier
        {

        }
    }

}
