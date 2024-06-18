using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class LevelConfig
{
    public string levelName;
    public WaveConfigScriptableObject[] waves;
    public float spawnerStartDelay = 3f;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private LevelConfig[] levels;
    private int currentLevelIndex = 0;
    private int currentWaveIndex = 0;

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
        InitializeLevel();
    }

    public void InitializeLevel()
    {
        currentWaveIndex = 0;
        StartCoroutine(StartSpawnersWithDelay(levels[currentLevelIndex].spawnerStartDelay));
    }

    public LevelConfig GetCurrentLevelConfig()
    {
        return levels[currentLevelIndex];
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex;
    }

    public void SetCurrentWaveIndex(int waveIndex)
    {
        currentWaveIndex = waveIndex;
    }

    public void SetCurrentLevelIndex(int levelIndex)
    {
        currentLevelIndex = levelIndex;
    }

    public void EndCurrentLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            SceneManager.LoadScene(levels[currentLevelIndex].levelName);
            SceneManager.sceneLoaded += OnLevelLoaded;
        }
        else
        {
            SceneManager.LoadScene("Win");
        }
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        InitializeLevel();
    }

    private IEnumerator StartSpawnersWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.StartNextWave();
    }

    public void ResetLevel()
    {
        currentLevelIndex = 0;
        currentWaveIndex = 0;
    }

    public int GetLevelsCount()
    {
        return levels.Length;
    }
}
