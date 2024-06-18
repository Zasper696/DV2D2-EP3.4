// Enemy.cs
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum DamageType
    {
        Physical,
        Magical,
        Fire,
        Ice,
        Poison
    }

    [SerializeField] protected int health;  // Cambiar a protected
    [SerializeField] private float speed;
    [SerializeField] private int damageToPlayer;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float stuckTimeThreshold = 3f;
    [SerializeField] private float minimumMovementThreshold = 0.1f;

    protected Vector2 movementDirection;
    protected Transform player;
    private Vector2 lastPosition;
    private float timeStuck = 0;

    private void Awake()
    {
        SetDefaultValues();
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ChooseNewDirection();
    }

    protected virtual void Update()
    {
        Move();
        CheckIfStuck();
        FollowPlayer();
    }

    private void Move()
    {
        transform.Translate(movementDirection * speed * Time.deltaTime);
    }

    private void FollowPlayer()
    {
        if (player && Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            movementDirection = (player.position - transform.position).normalized;
        }
    }

    private void CheckIfStuck()
    {
        if (Vector2.Distance(transform.position, lastPosition) < minimumMovementThreshold)
        {
            timeStuck += Time.deltaTime;
            if (timeStuck >= stuckTimeThreshold)
            {
                ChooseNewDirection();
                timeStuck = 0;
            }
        }
        else
        {
            timeStuck = 0;
        }
        lastPosition = transform.position;
    }

    protected void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        movementDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ChooseNewDirection();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageToPlayer, Player.DamageType.Physical); // Añadir DamageType
                Die();
            }
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            PlayerBullet bullet = collision.gameObject.GetComponent<PlayerBullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage, bullet.damageType); // Añadir DamageType
                Destroy(bullet.gameObject);
            }
        }
    }

    public virtual void TakeDamage(int damage, DamageType damageType)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Enemy died.");
        GameManager.Instance.EnemyDefeated();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void SetDefaultValues()
    {
        health = 3;
        speed = 2f;
        damageToPlayer = 1;
        detectionRadius = 5f;
    }
}
