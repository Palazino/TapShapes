using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    [Header("Paramètres de progression")]
    public float timeToReachMaxDifficulty = 120f; // Temps pour atteindre la difficulté max
    public float relaxFactor = 0.8f;              // Facteur de relâchement temporaire
    public float relaxDuration = 5f;              // Durée de la phase relax
    public float intensityBoost = 5f;             // Réduction du tempsToReachMax par vague
    public float waveDuration = 30f;              // Durée entre chaque pic de tension

    private float waveTimer = 0f;
    private float timeElapsed = 0f;
    private int waveCount = 0;
    private bool isRelaxing = false;

    [Header("Paliers de difficulté (Tiers)")]
    public int currentTier = 1;
    public float[] difficultyThresholds = { 0.2f, 0.4f, 0.6f, 0.8f }; // pour Tier 1 → 5

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (!isRelaxing)
            waveTimer += Time.deltaTime;

        if (waveTimer >= waveDuration)
        {
            waveTimer = 0f;
            waveCount++;

            // Boost progressif (accélère un peu la montée de difficulté)
            timeToReachMaxDifficulty = Mathf.Max(10f, timeToReachMaxDifficulty - intensityBoost);

            // Lance une courte phase de répit
            StartCoroutine(RelaxPhase());

            // Feedback visuel / sonore
            GameManager.Instance?.TriggerDifficultyFlash(waveCount);
        }

        UpdateCurrentTier();
    }

    private System.Collections.IEnumerator RelaxPhase()
    {
        isRelaxing = true;

        // On diminue temporairement la tension (plus de marge)
        float originalTime = timeToReachMaxDifficulty;
        timeToReachMaxDifficulty *= (1f / relaxFactor); // augmentation temporaire du temps de montée

        yield return new WaitForSeconds(relaxDuration);

        // Retour à la progression normale
        timeToReachMaxDifficulty = originalTime;
        isRelaxing = false;
    }

    private void UpdateCurrentTier()
    {
        float difficulty = Mathf.Clamp01(timeElapsed / timeToReachMaxDifficulty);

        for (int i = 0; i < difficultyThresholds.Length; i++)
        {
            if (difficulty >= difficultyThresholds[i])
                currentTier = i + 1;
        }
    }

    public int GetCurrentTier()
    {
        return currentTier;
    }

    public float GetTrapChance()
    {
        return Mathf.Lerp(0.05f, 0.4f, GetDifficultyValue());
    }

    public int GetMaxShapesOnScreen()
    {
        return Mathf.RoundToInt(Mathf.Lerp(3f, 12f, GetDifficultyValue()));
    }

    public float GetCurrentShapeLifetime()
    {
        return Mathf.Lerp(4f, 1.2f, GetDifficultyValue());
    }

    private float GetDifficultyValue()
    {
        return Mathf.Clamp01(timeElapsed / timeToReachMaxDifficulty);
    }
}
