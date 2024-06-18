using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

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

    public void StartWave()
    {
        GameManager.Instance.StartNextWave();
    }

    public void EndWave()
    {
        GameManager.Instance.EndCurrentWave();
    }
}
