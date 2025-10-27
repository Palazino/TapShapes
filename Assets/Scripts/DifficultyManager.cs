using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public float timeElapsed = 0f;

    [Header("Réglages de difficulté")]
    public float minLifeTime = 0.8f;
    public float maxLifeTime = 3f;

    public int maxShapesOnScreen = 1;
    public float timeToReachMaxDifficulty = 60f; // temps en secondes pour atteindre la difficulté maximale

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
    }

    // Appelé par les formes pour savoir leur durée de vie
    public float GetCurrentShapeLifetime()
    {
        float t = Mathf.Clamp01(timeElapsed / timeToReachMaxDifficulty);
        return Mathf.Lerp(maxLifeTime, minLifeTime, t);
    }

    // Appelé par le spawner pour savoir combien de formes max doivent être présentes
    public int GetMaxShapesOnScreen()
    {
        float t = Mathf.Clamp01(timeElapsed / timeToReachMaxDifficulty);
        return Mathf.FloorToInt(Mathf.Lerp(1, maxShapesOnScreen, t));
    }
    public float GetTrapChance()
    {
        float t = Mathf.Clamp01(timeElapsed / timeToReachMaxDifficulty);
        return Mathf.Lerp(0.05f, 0.3f, t); 
    }

    // --- Ajout du système de vagues dynamiques ---
    [Header("⚡ Réglages des vagues de tension")]
    [SerializeField, Tooltip("Durée d'une phase avant un pic de difficulté (en secondes)")]
    private float waveDuration = 30f;

    [SerializeField, Tooltip("Augmentation de la difficulté à chaque vague (plus c'est haut, plus c'est brutal)")]
    private float intensityBoost = 0.5f;

    [SerializeField, Range(0.5f, 1f), Tooltip("Facteur de détente après le pic (1 = pas de relâchement)")]
    private float relaxFactor = 0.8f;

    [SerializeField, Tooltip("Durée du relâchement (en secondes)")]
    private float relaxDuration = 5f;

    [SerializeField, Tooltip("Active ou désactive le feedback visuel (flash, son, etc.)")]
    private bool enableWaveFeedback = true;

    private float waveTimer = 0f;
    private int waveCount = 0;

    void LateUpdate()
    {
        waveTimer += Time.deltaTime;

        // Détection des paliers de tension
        if (waveTimer >= waveDuration)
        {
            waveTimer = 0f;
            waveCount++;

            // Boost temporaire
            timeToReachMaxDifficulty = Mathf.Max(10f, timeToReachMaxDifficulty - intensityBoost);

            // Relax phase
            StartCoroutine(RelaxPhase());

            // Feedback visuel / sonore
            GameManager.Instance?.TriggerDifficultyFlash(waveCount);
        }
    }

    private System.Collections.IEnumerator RelaxPhase()
    {
        yield return new WaitForSeconds(5f);
        timeToReachMaxDifficulty /= relaxFactor;
    }


}
