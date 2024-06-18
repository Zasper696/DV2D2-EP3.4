using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    public string sceneToLoad = "Level 1"; // Nombre de la escena de nivel 1

    public void ReplayGame()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Scene to load is not set.");
            return;
        }

        // Reiniciar los valores necesarios en los managers
        LevelManager.Instance.SetCurrentLevelIndex(0);
        LevelManager.Instance.SetCurrentWaveIndex(0);

        // Reiniciar el GameManager
        GameManager.Instance.RestartGame();

        // Cargar la escena especificada
        SceneManager.LoadScene(sceneToLoad);
    }
}
