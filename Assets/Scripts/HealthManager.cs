// HealthManager.cs
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

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

    public void ApplyDamageToPlayer(int damage, Player.DamageType damageType)
    {
        Player.Instance.TakeDamage(damage, damageType);
    }

    public void ApplyDamageToEnemy(GameObject enemy, int damage, Enemy.DamageType damageType)
    {
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.TakeDamage(damage, damageType);
        }
    }
}
