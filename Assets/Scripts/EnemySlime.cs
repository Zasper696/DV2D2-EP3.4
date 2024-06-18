// EnemySlime.cs
using UnityEngine;

public class EnemySlime : Enemy
{
    [SerializeField] private Color lowHealthColor = Color.red;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void TakeDamage(int damage, DamageType damageType)
    {
        base.TakeDamage(damage, damageType);
        if (health == 1)
        {
            ChangeColor(lowHealthColor);
        }
    }

    private void ChangeColor(Color newColor)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = newColor;
        }
    }

    protected override void Die()
    {
        base.Die();
    }
}
