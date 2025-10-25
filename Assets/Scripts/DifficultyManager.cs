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

}
