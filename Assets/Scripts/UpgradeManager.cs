using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Améliorations disponibles")]
    public List<UpgradeData> allUpgrades = new List<UpgradeData>();

    private const string SaveKey = "UPGRADE_";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (allUpgrades == null)
                allUpgrades = new List<UpgradeData>();
            LoadUpgrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void UnlockUpgrade(string id)
    {
        UpgradeData upgrade = allUpgrades.Find(u => u.upgradeID == id);
        if (upgrade != null && !upgrade.isUnlocked)
        {
            upgrade.isUnlocked = true;
            PlayerPrefs.SetInt(SaveKey + id, 1);
            PlayerPrefs.Save();
        }
    }

    public bool IsUnlocked(string id)
    {
        UpgradeData upgrade = allUpgrades.Find(u => u.upgradeID == id);
        return upgrade != null && upgrade.isUnlocked;
    }

    private void LoadUpgrades()
    {
        foreach (var upgrade in allUpgrades)
        {
            upgrade.isUnlocked = PlayerPrefs.GetInt(SaveKey + upgrade.upgradeID, 0) == 1;
        }
    }
}
