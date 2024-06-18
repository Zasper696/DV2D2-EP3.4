using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "ScriptableObjects/WaveConfig", order = 1)]
public class WaveConfigScriptableObject : ScriptableObject
{
    public int numberOfEnemies; // N�mero de enemigos en esta oleada // Atributo
    public EnemyPrefab[] enemies; // Lista de tipos de enemigos que pueden aparecer // Atributo
    public float spawnInterval; // Intervalo entre cada generaci�n // Atributo
    public float intervalBetweenWaves; // Intervalo entre oleadas // Atributo
    public bool disableDuringWave; // Si el spawn est� deshabilitado en esta oleada // Atributo
}

[System.Serializable]
public struct EnemyPrefab
{
    public GameObject enemyPrefab; // El prefab del enemigo // Atributo
    public float spawnProbability; // La probabilidad de que este enemigo se genere // Atributo
}
