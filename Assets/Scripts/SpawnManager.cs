using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    private WaveConfigScriptableObject currentWaveConfig;
    private bool isSpawning = false;
    private int currentWaveIndex = 0;

    private Transform[] currentLevelSpawners;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevelSpawners = GetSpawnersForCurrentLevel();
    }

    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex;
    }

    public void StartWave(int waveIndex, WaveConfigScriptableObject waveConfig)
    {
        currentWaveIndex = waveIndex;
        currentWaveConfig = waveConfig;

        if (currentWaveConfig == null)
        {
            Debug.LogError("WaveConfigScriptableObject is null.");
            return;
        }

        if (currentLevelSpawners == null || currentLevelSpawners.Length == 0)
        {
            Debug.LogError("No spawners found for the current level.");
            return;
        }

        isSpawning = !currentWaveConfig.disableDuringWave;

        if (isSpawning)
        {
            foreach (var spawner in currentLevelSpawners)
            {
                if (spawner != null)
                {
                    StartCoroutine(SpawnEnemiesFromSpawner(spawner));
                }
                else
                {
                    Debug.LogError("Spawner is null. Make sure all spawners are assigned.");
                }
            }
        }
    }

    private Transform[] GetSpawnersForCurrentLevel()
    {
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Spawner");
        Transform[] spawners = new Transform[spawnerObjects.Length];
        for (int i = 0; i < spawnerObjects.Length; i++)
        {
            spawners[i] = spawnerObjects[i].transform;
        }
        return spawners;
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
        isSpawning = false;
    }

    private IEnumerator SpawnEnemiesFromSpawner(Transform spawner)
    {
        while (isSpawning)
        {
            float totalProbability = 0f;
            foreach (var enemy in currentWaveConfig.enemies)
            {
                totalProbability += enemy.spawnProbability;
            }

            float randomValue = Random.Range(0f, totalProbability);
            float cumulativeProbability = 0f;

            foreach (var enemy in currentWaveConfig.enemies)
            {
                cumulativeProbability += enemy.spawnProbability;
                if (randomValue <= cumulativeProbability)
                {
                    Instantiate(enemy.enemyPrefab, spawner.position, Quaternion.identity);
                    break;
                }
            }

            yield return new WaitForSeconds(currentWaveConfig.spawnInterval);
        }
    }
}
