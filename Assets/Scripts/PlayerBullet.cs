using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage;
    public Enemy.DamageType damageType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, damageType);
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // Normalización del vector de dirección y aritmética básica (escalado)
        rb.velocity = direction.normalized * 10f;
    }
}
