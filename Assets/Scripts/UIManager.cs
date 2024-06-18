using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player Health UI")]
    public Slider playerHealthSlider;
    public Text playerHealthText;

    [Header("Enemy Counter UI")]
    public Text enemyCounterText;

    [Header("Wave Counter UI")]
    public Text waveText;

    [Header("Next Round Countdown UI")]
    public Text nextRoundText;

    [Header("UI Prefab")]
    public GameObject uiPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InstantiateUIPrefab();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeUI();
    }

    private void InstantiateUIPrefab()
    {
        if (uiPrefab != null)
        {
            GameObject uiInstance = Instantiate(uiPrefab);
            DontDestroyOnLoad(uiInstance);
            AssignUIElements();
        }
        else
        {
            Debug.LogError("UI Prefab is not assigned.");
        }
    }

    private void AssignUIElements()
    {
        playerHealthSlider = GameObject.FindWithTag("UI 5")?.GetComponent<Slider>();
        playerHealthText = GameObject.FindWithTag("UI 4")?.GetComponent<Text>();
        enemyCounterText = GameObject.FindWithTag("UI 1")?.GetComponent<Text>();
        waveText = GameObject.FindWithTag("UI 2")?.GetComponent<Text>();
        nextRoundText = GameObject.FindWithTag("UI 3")?.GetComponent<Text>();

        if (playerHealthSlider == null) Debug.LogError("Player Health Slider not found!");
        if (playerHealthText == null) Debug.LogError("Player Health Text not found!");
        if (enemyCounterText == null) Debug.LogError("Enemy Counter Text not found!");
        if (waveText == null) Debug.LogError("Wave Text not found!");
        if (nextRoundText == null) Debug.LogError("Next Round Text not found!");
    }

    private void InitializeUI()
    {
        if (playerHealthSlider == null || playerHealthText == null ||
            enemyCounterText == null || waveText == null || nextRoundText == null)
        {
            Debug.LogError("UI elements not assigned in the inspector.");
            return;
        }

        UpdatePlayerHealthUI(GameManager.Instance.GetPlayerCurrentHealth());
        UpdateEnemyCounterUI(GameManager.Instance.GetRemainingEnemies());
        UpdateWaveText(LevelManager.Instance.GetCurrentWaveIndex() + 1);
    }

    public void UpdatePlayerHealthUI(int currentHealth)
    {
        if (playerHealthSlider != null && playerHealthText != null)
        {
            Debug.Log($"Updating Player Health UI: {currentHealth}");
            playerHealthSlider.value = currentHealth;
            playerHealthText.text = "Player: " + currentHealth;
        }
    }

    public void UpdateEnemyCounterUI(int remainingEnemies)
    {
        if (enemyCounterText != null)
        {
            Debug.Log($"Updating Enemy Counter UI: {remainingEnemies}");
            enemyCounterText.text = "Enemies Left: " + remainingEnemies;
        }
    }

    public void UpdateWaveText(int currentWave)
    {
        if (waveText != null)
        {
            Debug.Log($"Updating Wave Text: {currentWave}");
            waveText.text = "Actual Wave: " + currentWave;
        }
    }

    public void UpdateNextRoundCountdownUI(float countdown)
    {
        if (nextRoundText != null)
        {
            Debug.Log($"Updating Next Round Countdown UI: {countdown:F1}");
            nextRoundText.text = "Next Round In: " + countdown.ToString("F1") + "s";
        }
    }

    public void ResetUIReferences()
    {
        // Reasignar referencias de UI usando los nombres especificados
        AssignUIElements();

        if (playerHealthSlider == null || playerHealthText == null || enemyCounterText == null || waveText == null || nextRoundText == null)
        {
            Debug.LogError("Some UI elements could not be found after scene load.");
        }
        else
        {
            Debug.Log("UI elements reassigned successfully.");
            InitializeUI(); // Reinitialize UI with correct values
        }
    }
}
