// DamageManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

    [System.Serializable]
    public class DamageConfig
    {
        public GameObject enemyPrefab;
        public Enemy.DamageType damageType;
    }

    [System.Serializable]
    public class PlayerDamageConfig : DamageConfig
    {
        public int damageAmount;
    }

    [System.Serializable]
    public class EnemyDamageConfig : DamageConfig
    {
        public GameObject bulletPrefab;
        public int damageAmount;
    }

    [SerializeField] private List<PlayerDamageConfig> playerDamageConfigs;
    [SerializeField] private List<EnemyDamageConfig> enemyDamageConfigs;

    private List<DamageInfo> playerDamageQueue = new List<DamageInfo>();
    private List<DamageInfo> enemyDamageQueue = new List<DamageInfo>();

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
        StartCoroutine(ProcessDamageQueue());
    }

    private IEnumerator ProcessDamageQueue()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            ApplyDamage();
        }
    }

    public void RegisterPlayerDamage(int damage, Player.DamageType damageType)
    {
        playerDamageQueue.Add(new DamageInfo(damage, damageType));
    }

    public void RegisterEnemyDamage(GameObject enemy, int damage, Enemy.DamageType damageType)
    {
        enemyDamageQueue.Add(new DamageInfo(enemy, damage, damageType));
    }

    private void ApplyDamage()
    {
        foreach (var damage in playerDamageQueue)
        {
            FindObjectOfType<Player>().TakeDamage(damage.Damage, damage.PlayerDamageType);
        }
        playerDamageQueue.Clear();

        foreach (var damage in enemyDamageQueue)
        {
            if (damage.Target != null)
            {
                Enemy enemy = damage.Target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage.Damage, damage.EnemyDamageType);
                }
            }
        }
        enemyDamageQueue.Clear();
    }

    private class DamageInfo
    {
        public GameObject Target { get; }
        public int Damage { get; }
        public Player.DamageType PlayerDamageType { get; }
        public Enemy.DamageType EnemyDamageType { get; }

        public DamageInfo(int damage, Player.DamageType damageType)
        {
            Damage = damage;
            PlayerDamageType = damageType;
        }

        public DamageInfo(GameObject target, int damage, Enemy.DamageType damageType)
        {
            Target = target;
            Damage = damage;
            EnemyDamageType = damageType;
        }
    }
}
