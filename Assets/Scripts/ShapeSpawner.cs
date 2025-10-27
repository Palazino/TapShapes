using UnityEngine;
using System.Collections.Generic;

public class ShapeSpawner : MonoBehaviour
{
    [Header("Formes normales et pièges")]
    public GameObject[] normalShapes;
    public GameObject[] trapShapes;

    [Header("Formes spéciales débloquées selon la difficulté")]
    public GameObject[] scorePenaltyShapes;   // Enlèvent des points
    public GameObject[] comboBreakerShapes;   // Cassent le combo
    public GameObject[] ghostShapes;          // Invisibles brièvement


    [Header("Zone de spawn")]
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    [Header("Fréquence de spawn")]
    public float baseSpawnInterval = 1f;

    [Header("Anti-chevauchement")]
    public float spawnPadding = 1f;
    public int maxSpawnAttempts = 10;

    private float timer;
    private List<GameObject> currentShapes = new List<GameObject>();

    private bool gameOver = false;

    void Start()
    {
        timer = baseSpawnInterval;
    }
    void Update()
    {
        if (gameOver) return;

        timer -= Time.deltaTime;
        currentShapes.RemoveAll(shape => shape == null);

        int maxShapes = DifficultyManager.Instance.GetMaxShapesOnScreen();

        if (timer <= 0f)
        {
            timer = baseSpawnInterval;

            if (currentShapes.Count < maxShapes)
            {
                SpawnShape();
            }
        }
    }

    void SpawnShape()
    {
        int attempts = 0;
        bool foundSpot = false;
        Vector2 spawnPos = Vector2.zero;

        while (attempts < maxSpawnAttempts && !foundSpot)
        {
            attempts++;

            spawnPos = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            foundSpot = true;

            foreach (GameObject shape in currentShapes)
            {
                if (shape == null) continue;
                if (Vector2.Distance(shape.transform.position, spawnPos) < spawnPadding)
                {
                    foundSpot = false;
                    break;
                }
            }
        }

        if (!foundSpot) return;

        GameObject shapeToSpawn = null;

        int tier = DifficultyManager.Instance.GetCurrentTier();
        float trapChance = DifficultyManager.Instance.GetTrapChance();
        bool shouldSpawnTrap = Random.value < trapChance;

        if (shouldSpawnTrap && trapShapes.Length > 0)
        {
            shapeToSpawn = trapShapes[Random.Range(0, trapShapes.Length)];
        }
        else
        {
            // Base : forme normale
            shapeToSpawn = normalShapes.Length > 0 ? normalShapes[Random.Range(0, normalShapes.Length)] : null;

            // Selon le tier, on ajoute des chances de formes spéciales
            if (tier >= 2 && scorePenaltyShapes.Length > 0 && Random.value < 0.1f)
                shapeToSpawn = scorePenaltyShapes[Random.Range(0, scorePenaltyShapes.Length)];

            if (tier >= 3 && comboBreakerShapes.Length > 0 && Random.value < 0.07f)
                shapeToSpawn = comboBreakerShapes[Random.Range(0, comboBreakerShapes.Length)];

            if (tier >= 4 && ghostShapes.Length > 0 && Random.value < 0.05f)
                shapeToSpawn = ghostShapes[Random.Range(0, ghostShapes.Length)];
        }


        if (shapeToSpawn == null) return;

        GameObject newShape = Instantiate(shapeToSpawn, spawnPos, Quaternion.identity);

        ShapeFade fade = newShape.GetComponent<ShapeFade>();

        if (fade != null)
        {
            float lifetime = DifficultyManager.Instance.GetCurrentShapeLifetime();

            if (UpgradeEffects.Instance != null)
                lifetime += UpgradeEffects.Instance.shapeLifetimeBonus;

            fade.lifeTime = lifetime;

        }

        currentShapes.Add(newShape);
    }

    public void TriggerMassFall(float fallForce)
    {
        gameOver = true;

        foreach (GameObject shape in currentShapes)
        {
            if (shape == null) continue;

            Collider2D col = shape.GetComponent<Collider2D>();
            if (col) col.enabled = false;

            ShapeFade fade = shape.GetComponent<ShapeFade>();
            if (fade)
            {
                fade.StopAllCoroutines(); 
                fade.enabled = false;
            }

            Rigidbody2D rb = shape.GetComponent<Rigidbody2D>();
            if (!rb)
                rb = shape.AddComponent<Rigidbody2D>();

            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(0f, -fallForce * 2f); 
        }

        currentShapes.Clear();
    }
}
