using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject gameManagerPrefab;
    public GameObject uiPrefab;
    public GameObject damageManagerPrefab;
    public GameObject healthManagerPrefab;
    public GameObject levelManagerPrefab;
    public GameObject spawnManagerPrefab;
    public GameObject waveManagerPrefab;
    public GameObject canvasPrefab; // Añadido para el Canvas

    private void Awake()
    {
        // Comprobar y cargar el Player
        if (Player.Instance == null)
        {
            Instantiate(playerPrefab);
        }

        // Comprobar y cargar el GameManager
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab);
        }

        // Comprobar y cargar el UIManager
        if (UIManager.Instance == null)
        {
            Instantiate(uiPrefab);
        }

        // Comprobar y cargar el DamageManager
        if (DamageManager.Instance == null)
        {
            Instantiate(damageManagerPrefab);
        }

        // Comprobar y cargar el HealthManager
        if (HealthManager.Instance == null)
        {
            Instantiate(healthManagerPrefab);
        }

        // Comprobar y cargar el LevelManager
        if (LevelManager.Instance == null)
        {
            Instantiate(levelManagerPrefab);
        }

        // Comprobar y cargar el SpawnManager
        if (SpawnManager.Instance == null)
        {
            Instantiate(spawnManagerPrefab);
        }

        // Comprobar y cargar el WaveManager
        if (WaveManager.Instance == null)
        {
            Instantiate(waveManagerPrefab);
        }

        // Comprobar y cargar el Canvas
        if (GameObject.FindWithTag("Canvas") == null) // Usando un tag para identificar el Canvas
        {
            Instantiate(canvasPrefab);
        }
    }
}
