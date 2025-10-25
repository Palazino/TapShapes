using UnityEngine;
using System.Collections;

public class UpgradeEffects : MonoBehaviour
{
    public static UpgradeEffects Instance;

    // Valeurs actives
    public int bonusScorePerClick = 0;
    public float comboDurationBonus = 0f;
    public float baseMultiplierBonus = 0f;
    public int extraLives = 0;
    public bool trapShieldActive = false;
    public float gemDropBonus = 0f;
    public int startGems = 0;
    public float shapeLifetimeBonus = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    void Start()
    {
        StartCoroutine(WaitForUpgradeManager());
    }

    private IEnumerator WaitForUpgradeManager()
    {
        // Attend que le singleton UpgradeManager soit prêt
        while (UpgradeManager.Instance == null)
            yield return null;

        ApplyUnlockedUpgrades();
    }

    public void ApplyUnlockedUpgrades()
    {
        if (UpgradeManager.Instance == null || UpgradeManager.Instance.allUpgrades == null)
        {
            Debug.LogWarning("⚠️ UpgradeManager ou la liste allUpgrades n'est pas prête !");
            return;
        }

        // Reset avant recalcul
        bonusScorePerClick = 0;
        comboDurationBonus = 0f;
        baseMultiplierBonus = 0f;
        extraLives = 0;
        trapShieldActive = false;
        gemDropBonus = 0f;
        startGems = 0;
        shapeLifetimeBonus = 0f;

        foreach (var up in UpgradeManager.Instance.allUpgrades)
        {
            if (!up.isUnlocked) continue;

            switch (up.upgradeID)
            {
                case "UP_SCORE_PLUS": bonusScorePerClick += 1; break;
                case "UP_COMBO_TIME": comboDurationBonus += 1f; break;
                case "UP_MULTIPLIER_BASE": baseMultiplierBonus += 0.2f; break;
                case "UP_EXTRA_LIFE": extraLives += 1; break;
                case "UP_TRAP_SHIELD": trapShieldActive = true; break;
                case "UP_GEM_DROP": gemDropBonus += 0.10f; break;
                case "UP_GEM_START": startGems += 5; break;
                case "UP_SLOW_FADE": shapeLifetimeBonus += 0.2f; break;
            }
        }

        Debug.Log("✅ Améliorations appliquées avec succès.");
    }
}
