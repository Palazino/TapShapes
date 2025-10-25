using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Réglage position du Score Popup")]
    public float popupOffsetX = 0f;
    public float popupOffsetY = 0f;

    [Header("Canvas principal (Screen Space Overlay)")]
    public Canvas mainCanvas;
    private Transform canvasTransform;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI livesText;

    [Header("Monnaie")]
    public int currency = 0;
    public TMP_Text currencyText;

    [Header("UI Fin de partie")]
    public GameObject gameOverUI;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI bestComboText;
    public TextMeshProUGUI timeSurvivedText;

    [Header("Score popup")]
    public GameObject scorePopPrefab;

    [Header("Vie")]
    public int lives = 3;

    private int score = 0;
    private bool isGameOver = false;

    [Header("Combo & Multiplicateur")]
    public float comboDuration = 3f;
    private float comboTimer;
    private int currentCombo = 0;
    private int bestCombo = 0;
    private float currentMultiplier = 1f;

    private float gameTimer = 0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        canvasTransform = mainCanvas.transform;

        if (UpgradeEffects.Instance != null)
        {
            lives += UpgradeEffects.Instance.extraLives;
            currency += UpgradeEffects.Instance.startGems;
        }

        LoadCurrency();
        UpdateScoreUI();
        UpdateLivesUI();
        UpdateComboUI();
        UpdateCurrencyUI();
    }

    void Update()
    {
        if (isGameOver) return;

        gameTimer += Time.deltaTime;

        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0)
            {
                currentCombo = 0;
                currentMultiplier = 1f;
                UpdateComboUI();
            }
        }
    }

    public void AddScore(int amount, Vector3 worldPosition)
    {
        // Appliquer les bonus d'amélioration
        if (UpgradeEffects.Instance != null)
            amount += UpgradeEffects.Instance.bonusScorePerClick;

        // Appliquer le multiplicateur du combo
        int finalScore = Mathf.RoundToInt(amount * currentMultiplier);
        score += finalScore;

        // Mettre à jour l'UI principale
        UpdateScoreUI();

        // Créer le popup avec le bon score (dans le Canvas)
        if (scorePopPrefab != null && mainCanvas != null)
        {
            // Récupération du RectTransform du Canvas
            RectTransform canvasRect = mainCanvas.GetComponent<RectTransform>();
            Vector2 anchoredPos;

            Camera cam = Camera.main;

            // Conversion monde → écran
            Vector2 screenPoint = cam.WorldToScreenPoint(worldPosition);

            // Conversion écran → coordonnées locales du Canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPoint,
                mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
                out anchoredPos
            );

            // Application des offsets manuels
            anchoredPos += new Vector2(popupOffsetX, popupOffsetY);

            // Instanciation dans le Canvas
            GameObject popup = Instantiate(scorePopPrefab, canvasRect);
            RectTransform rect = popup.GetComponent<RectTransform>();
            rect.anchoredPosition = anchoredPos;
            rect.localScale = Vector3.one;

            // Appliquer le texte
            ScorePop popScript = popup.GetComponent<ScorePop>();
            if (popScript != null)
                popScript.SetText("+" + finalScore);

            Debug.Log($"Popup Score = +{finalScore}");
        }

        // Gérer le combo
        AddCombo();

        // Chance de drop de gemme, modifiée par l'upgrade si active
        float dropChance = 0.15f;
        if (UpgradeEffects.Instance != null)
            dropChance += UpgradeEffects.Instance.gemDropBonus;

        if (Random.value < dropChance)
            AddCurrency(1);
    }

    void AddCombo()
    {
        comboTimer = comboDuration;
        currentCombo++;

        if (currentCombo > bestCombo)
            bestCombo = currentCombo;

        currentMultiplier = 1f + (currentCombo * 0.1f);
        UpdateComboUI();
    }

    public void LoseLife()
    {
        if (isGameOver) return;

        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            TriggerGameOver();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score : " + score;
    }

    void UpdateComboUI()
    {
        if (comboText != null)
            comboText.text = "COMBO : " + currentCombo;

        if (multiplierText != null)
            multiplierText.text = "MULTI : x" + currentMultiplier.ToString("F1");
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "VIES : " + lives;
    }

    public void TriggerGameOver()
    {
        isGameOver = true;

        if (finalScoreText != null)
            finalScoreText.text = "Score : " + score;

        if (bestComboText != null)
            bestComboText.text = "Combo Max : " + bestCombo;

        if (timeSurvivedText != null)
        {
            int minutes = Mathf.FloorToInt(gameTimer / 60f);
            int seconds = Mathf.FloorToInt(gameTimer % 60f);
            timeSurvivedText.text = $"Temps : {minutes:00}:{seconds:00}";
        }

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Stop le spawn des formes
        ShapeSpawner spawner = FindObjectOfType<ShapeSpawner>();
        if (spawner != null)
            spawner.TriggerMassFall(15f);
    }

    public void InstantGameOver()
    {
        lives = 0;
        UpdateLivesUI();
        TriggerGameOver();
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        UpdateCurrencyUI();
        SaveCurrency();
    }

    public bool SpendCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            UpdateCurrencyUI();
            SaveCurrency();
            return true;
        }
        return false;
    }

    void UpdateCurrencyUI()
    {
        if (currencyText)
            currencyText.text = currency + " 💎";
    }

    #region Sauvegarde des gemmes

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt("PlayerCurrency", currency);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        currency = PlayerPrefs.GetInt("PlayerCurrency", 0); // 0 par défaut si rien n’est encore sauvegardé
        UpdateCurrencyUI();
    }

    #endregion
}
