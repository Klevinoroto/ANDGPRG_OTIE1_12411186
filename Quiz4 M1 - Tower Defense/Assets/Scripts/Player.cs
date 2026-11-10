using Mono.Cecil;
using System;
using System.IO;
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

    private int gold = 0;
    private Tower selectedTower = null;
    private int selectedUpgradeCost;
    private string upgradeKey;

    // UI STUFF
    GameObject infoTab;
    TextMeshProUGUI towerName;
    TextMeshProUGUI towerLevel;
    TextMeshProUGUI towerDescription;
    TextMeshProUGUI towerUpgradeSelection;
    TextMeshProUGUI towerUpgradeCost;
    GameObject confirmUpgradeButton;

    GameObject upgradesTab;
    TextMeshProUGUI upgradeDescription;
    TextMeshProUGUI upgradeSubheader;

    GameObject upgradePathsTab;
    GameObject upgradeTierTab;

    void Start()
    {

        goldTextLabel = playerUI.transform.Find("GoldCount").GetComponent<TextMeshProUGUI>();

        infoTab = towerInfoUI.transform.Find("Info").gameObject;
        towerName = infoTab.transform.Find("TowerName").gameObject.GetComponent<TextMeshProUGUI>();
        towerLevel = infoTab.transform.Find("Level").gameObject.GetComponent<TextMeshProUGUI>();
        towerDescription = infoTab.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();
        towerUpgradeSelection = infoTab.transform.Find("Selection").gameObject.GetComponent<TextMeshProUGUI>();
        towerUpgradeCost = infoTab.transform.Find("Cost").gameObject.GetComponent<TextMeshProUGUI>();
        confirmUpgradeButton = infoTab.transform.Find("Confirmation").gameObject;

        upgradesTab = towerInfoUI.transform.Find("Upgrades").gameObject;
        upgradeDescription = upgradesTab.transform.Find("Description").gameObject.GetComponent<TextMeshProUGUI>();
        upgradeSubheader = upgradesTab.transform.Find("Subheader").gameObject.GetComponent<TextMeshProUGUI>();

        upgradePathsTab = upgradesTab.transform.Find("ThreeChoice").gameObject;
        upgradeTierTab = upgradesTab.transform.Find("NextTier").gameObject;

        gold = 0;
        AdjustGold(20);
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

    public int GetGold() { return gold; }
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

    private bool IsUpgradePath() { return (selectedTower.level + 1) % 2 == 0; }

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
        foreach (Transform child in upgradePathsTab.transform)
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

        selectedTower = tower;
        upgradesTab.transform.Find("Header").gameObject.SetActive(true);
        upgradesTab.transform.Find("Subheader").gameObject.SetActive(true);
        upgradePathsTab.SetActive(false);
        upgradeTierTab.SetActive(false);
        confirmUpgradeButton.SetActive(false);
        towerInfoUI.SetActive(true);

        upgradeSubheader.text = "Lv. " + tower.level.ToString() + " -> Lv. " + (tower.level + 1).ToString();

        towerName.text = selectedTower.towerType + " Tower";
        towerLevel.text = "Level " + selectedTower.level.ToString();
        towerDescription.text = selectedTower.info.description + selectedTower.upgradeDescriptors;
        towerUpgradeSelection.text = "";
        towerUpgradeCost.text = "";

        if (selectedTower.level == 5)
        {
            upgradesTab.transform.Find("Header").gameObject.SetActive(false);
            upgradesTab.transform.Find("Subheader").gameObject.SetActive(false);
            upgradeDescription.text = "Tower has reached max level!";
            return;
        }

        if (IsUpgradePath()) // Upgrade path
        {
            upgradePathsTab.SetActive(true);
            upgradeDescription.text = "Select an upgrade!";
            foreach (Transform child in upgradePathsTab.transform)
            {
                string pathName = child.gameObject.name;
                if (pathName.Contains("Option"))
                {
                    string upgradeKey = selectedTower.towerType + (selectedTower.level + 1).ToString() + pathName.Replace("Option", "");
                    UpgradeInfo upgradeInfo = TowerUpgrades.Towers[upgradeKey];

                    TextMeshProUGUI optionText = child.Find("Label").GetComponent<TextMeshProUGUI>();
                    optionText.text = upgradeInfo.name;
                }
            }
        }
        else // Upgrade tier
        {
            upgradeTierTab.SetActive(true);
            upgradeDescription.text = "Upgrade the to the next tier of the tower! Generally boosting all stats.";
        }
        // set ui stuff here like its info an stuff
    }

    public void CloseTowerInfo()
    {
        towerInfoUI.SetActive(false);
        selectedTower.Deselect();
        selectedTower = null;
    }

    void UpgradeSelected()
    {
        confirmUpgradeButton.SetActive(true);

        if (IsUpgradePath()) // Upgrade path
        {
            UpgradeInfo upgradeInfo = TowerUpgrades.Towers[upgradeKey];
            upgradeDescription.text = upgradeInfo.description;
            towerUpgradeSelection.text = upgradeInfo.name;

            selectedUpgradeCost = upgradeInfo.cost;
        }
        else // Upgrade tier
        {
            TowerInfo nextLevelInfo = TowerData.Towers[selectedTower.towerType + (selectedTower.tier+1).ToString()];
            towerUpgradeSelection.text = "Next Tier Level";

            selectedUpgradeCost = nextLevelInfo.cost;
        }

        towerUpgradeCost.text = "Cost: " + selectedUpgradeCost.ToString() + " Gold";
    }

    public void ConfirmUpgrade()
    {
        if (gold >= selectedUpgradeCost)
        {
            AdjustGold(-selectedUpgradeCost);

            if (IsUpgradePath()) { selectedTower.UpgradePath(upgradeKey); }// Upgrade path
            else                 { selectedTower.ApplyNextTier(); }

            selectedTower.level++;
            CloseTowerInfo();
        }
    }

}
