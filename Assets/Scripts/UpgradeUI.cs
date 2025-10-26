using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public Transform contentParent; // là où les boutons sont instanciés
    public GameObject upgradeButtonPrefab;

    void Start()
    {
        PopulateUI();
    }

    void PopulateUI()
    {
        foreach (var upgrade in UpgradeManager.Instance.allUpgrades)
        {
            GameObject go = Instantiate(upgradeButtonPrefab, contentParent, false);


            TMP_Text[] texts = go.GetComponentsInChildren<TMP_Text>();
            Button buyButton = go.GetComponentInChildren<Button>();

            texts[0].text = upgrade.displayName;
            texts[1].text = upgrade.description;
            texts[2].text = upgrade.isUnlocked ? "Déjà acheté" : upgrade.cost + " 💎";

            buyButton.interactable = !upgrade.isUnlocked;

            buyButton.onClick.AddListener(() =>
            {
                if (GameManager.Instance.SpendCurrency(upgrade.cost))
                {
                    UpgradeManager.Instance.UnlockUpgrade(upgrade.upgradeID);
                    texts[2].text = "Déjà acheté";
                    buyButton.interactable = false;
                    UpgradeEffects.Instance?.ApplyUnlockedUpgrades();

                }
            });
        }
    }
}
