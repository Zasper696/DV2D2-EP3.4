using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Health Settings")]
    public int playerMaxHealth = 100;
    private int playerCurrentHealth;

    private enum GameState { Start, WaveActive, Transition, GameOver }
    private GameState currentState;

    private int enemiesDefeatedInWave = 0;
    private SpawnManager[] spawners;
    private float nextRoundCountdown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        spawners = FindObjectsOfType<SpawnManager>();
        playerCurrentHealth = playerMaxHealth;
        UIManager.Instance.UpdatePlayerHealthUI(playerCurrentHealth);
        StartNextLevel();
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Start:
                StartNextWave();
                break;
            case GameState.WaveActive:
                // Lógica de la oleada activa
                break;
            case GameState.Transition:
                nextRoundCountdown -= Time.deltaTime;
                if (nextRoundCountdown <= 0)
                {
                    StartNextWave();
                }
                UIManager.Instance.UpdateNextRoundCountdownUI(nextRoundCountdown);
                break;
            case GameState.GameOver:
                // Lógica del fin del juego
                break;
        }
    }

    private void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.Start:
                StartNextWave();
                break;
            case GameState.WaveActive:
                break;
            case GameState.Transition:
                nextRoundCountdown = 5f;
                UIManager.Instance.UpdateNextRoundCountdownUI(nextRoundCountdown);
                break;
            case GameState.GameOver:
                break;
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        playerCurrentHealth -= damage;
        UIManager.Instance.UpdatePlayerHealthUI(playerCurrentHealth);

        if (playerCurrentHealth <= 0)
        {
            Debug.Log("Player has been defeated!");
            ChangeState(GameState.GameOver);
            SceneManager.LoadScene("Lose");
        }
    }

    public int GetPlayerCurrentHealth()
    {
        return playerCurrentHealth;
    }

    public void EnemyDefeated()
    {
        enemiesDefeatedInWave++;
        UIManager.Instance.UpdateEnemyCounterUI(GetRemainingEnemies());

        if (enemiesDefeatedInWave >= LevelManager.Instance.GetCurrentLevelConfig().waves[LevelManager.Instance.GetCurrentWaveIndex()].numberOfEnemies)
        {
            EndCurrentWave();
        }
    }

    public void StartNextWave()
    {
        if (LevelManager.Instance.GetCurrentWaveIndex() < LevelManager.Instance.GetCurrentLevelConfig().waves.Length)
        {
            enemiesDefeatedInWave = 0;
            UIManager.Instance.UpdateWaveText(LevelManager.Instance.GetCurrentWaveIndex() + 1);
            UIManager.Instance.UpdateEnemyCounterUI(LevelManager.Instance.GetCurrentLevelConfig().waves[LevelManager.Instance.GetCurrentWaveIndex()].numberOfEnemies);

            ChangeState(GameState.WaveActive);

            foreach (SpawnManager spawner in spawners)
            {
                spawner.StartWave(LevelManager.Instance.GetCurrentWaveIndex(), LevelManager.Instance.GetCurrentLevelConfig().waves[LevelManager.Instance.GetCurrentWaveIndex()]);
            }
        }
        else
        {
            EndCurrentLevel();
        }
    }

    public void EndCurrentWave()
    {
        foreach (SpawnManager spawner in spawners)
        {
            spawner.StopSpawning();
        }

        DestroyAllEnemies();

        LevelManager.Instance.SetCurrentWaveIndex(LevelManager.Instance.GetCurrentWaveIndex() + 1);
        if (LevelManager.Instance.GetCurrentWaveIndex() < LevelManager.Instance.GetCurrentLevelConfig().waves.Length)
        {
            ChangeState(GameState.Transition);
        }
        else
        {
            EndCurrentLevel();
        }
    }

    private void DestroyAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }

    public void EndCurrentLevel()
    {
        LevelManager.Instance.SetCurrentLevelIndex(LevelManager.Instance.GetCurrentLevelIndex() + 1);
        if (LevelManager.Instance.GetCurrentLevelIndex() < LevelManager.Instance.GetLevelsCount())
        {
            int currentHealth = Player.Instance.GetCurrentHealth();
            SceneManager.LoadScene(LevelManager.Instance.GetCurrentLevelConfig().levelName);
            Player.Instance.SetCurrentHealth(currentHealth);
            SceneManager.sceneLoaded += OnLevelLoaded;
        }
        else
        {
            ChangeState(GameState.GameOver);
            SceneManager.LoadScene("Win");
        }
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        if (scene.name != "Win" && scene.name != "Lose")
        {
            UIManager.Instance.ResetUIReferences();
            InitializeGame();
        }
    }

    private void StartNextLevel()
    {
        LevelManager.Instance.SetCurrentWaveIndex(0);
        StartNextWave();
    }

    public void RestartGame()
    {
        LevelManager.Instance.SetCurrentLevelIndex(0);
        LevelManager.Instance.SetCurrentWaveIndex(0);
        SceneManager.LoadScene(LevelManager.Instance.GetCurrentLevelConfig().levelName);
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    public int GetCurrentWave()
    {
        return LevelManager.Instance.GetCurrentWaveIndex() + 1;
    }

    public int GetRemainingEnemies()
    {
        if (LevelManager.Instance.GetCurrentWaveIndex() >= LevelManager.Instance.GetCurrentLevelConfig().waves.Length || LevelManager.Instance.GetCurrentWaveIndex() < 0)
        {
            return 0;
        }
        return LevelManager.Instance.GetCurrentLevelConfig().waves[LevelManager.Instance.GetCurrentWaveIndex()].numberOfEnemies - enemiesDefeatedInWave;
    }

    public float GetNextRoundCountdown()
    {
        return nextRoundCountdown;
    }

    public void LoadNextLevel(string sceneName)
    {
        int currentHealth = Player.Instance.GetCurrentHealth();
        SceneManager.LoadScene(sceneName);
        Player.Instance.SetCurrentHealth(currentHealth);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (scene.name != "Win" && scene.name != "Lose")
        {
            UIManager.Instance.ResetUIReferences();
            InitializeGame();
        }
    }
}
