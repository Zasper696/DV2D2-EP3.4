using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public enum DamageType // Asegúrate de que esto sea público
    {
        Physical,
        Magical,
        Fire,
        Ice,
        Poison
    }

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    private float lastFireTime;

    [Header("Attack Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpawnRadius = 1f;
    [SerializeField] private float fireCooldown = 0.5f;

    [Header("Animation Settings")]
    [SerializeField] private float deathAnimationTime = 1f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Damage Colors")]
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private int lowHealthThreshold = 2;
    [SerializeField] private Color physicalDamageColor = Color.gray;
    [SerializeField] private Color magicalDamageColor = Color.blue;
    [SerializeField] private Color fireDamageColor = Color.red;
    [SerializeField] private Color iceDamageColor = Color.cyan;
    [SerializeField] private Color poisonDamageColor = Color.green;
    [SerializeField] private float damageColorDuration = 1f;

    [Header("Fire Damage Settings")]
    [SerializeField] private float fireDamageCooldownIncrease = 2f;
    [SerializeField] private float fireDamageDuration = 5f;

    [Header("Ice Damage Settings")]
    [SerializeField] private float iceDamageSpeedReduction = 2f;
    [SerializeField] private float iceDamageDuration = 5f;

    [Header("Poison Damage Settings")]
    [SerializeField] private int poisonDamagePerSecond = 1;
    [SerializeField] private float poisonDuration = 5f;

    private Coroutine poisonCoroutine;
    private Animator animator;

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
        currentHealth = maxHealth;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Attack();
    }

    private void Move()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;
        if (Input.GetKey(KeyCode.S)) moveY = -1f;
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
            spriteRenderer.flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
            spriteRenderer.flipX = false;
        }

        // Normalización del vector de movimiento
        Vector2 moveDir = new Vector2(moveX, moveY).normalized;
        // Aritmética básica de vectores (escalado y traducción)
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        if (moveDir.magnitude > 0.1f) // Magnitud del vector
        {
            if (moveY > 0)
                animator.SetInteger("Player", 1);
            else if (moveY < 0)
                animator.SetInteger("Player", 2);
            else if (moveX != 0)
                animator.SetInteger("Player", 3);
        }
        else
        {
            animator.SetInteger("Player", 0);
        }
    }

    private void Attack()
    {
        if (Time.time <= lastFireTime + fireCooldown)
            return;

        Vector2 direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
            animator.SetInteger("Player", 4);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
            animator.SetInteger("Player", 5);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
            animator.SetInteger("Player", 6);
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.right;
            animator.SetInteger("Player", 6);
            spriteRenderer.flipX = false;
        }

        if (direction != Vector2.zero)
        {
            Shoot(direction);
        }
    }

    private void Shoot(Vector2 direction)
    {
        lastFireTime = Time.time;
        Vector3 bulletSpawnPosition = transform.position + (Vector3)(direction * bulletSpawnRadius);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        // Normalización del vector de dirección y aritmética básica (escalado)
        bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * 10f;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
        UIManager.Instance.UpdatePlayerHealthUI(currentHealth);
    }

    public void TakeDamage(int damage, DamageType damageType)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ShowDamageEffect(damageType));
            if (damageType == DamageType.Poison)
            {
                if (poisonCoroutine != null)
                {
                    StopCoroutine(poisonCoroutine);
                }
                poisonCoroutine = StartCoroutine(ApplyPoisonDamage());
            }
            else if (damageType == DamageType.Fire)
            {
                StartCoroutine(ApplyFireDamage());
            }
            else if (damageType == DamageType.Ice)
            {
                StartCoroutine(ApplyIceDamage());
            }
        }

        if (currentHealth <= lowHealthThreshold)
        {
            spriteRenderer.color = lowHealthColor;
        }

        // Actualizar la UI después de tomar daño
        UIManager.Instance.UpdatePlayerHealthUI(currentHealth);
    }

    private IEnumerator ShowDamageEffect(DamageType damageType)
    {
        Color damageColor = GetDamageColor(damageType);
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageColorDuration);
        spriteRenderer.color = Color.white;
    }

    private Color GetDamageColor(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Physical:
                return physicalDamageColor;
            case DamageType.Magical:
                return magicalDamageColor;
            case DamageType.Fire:
                return fireDamageColor;
            case DamageType.Ice:
                return iceDamageColor;
            case DamageType.Poison:
                return poisonDamageColor;
            default:
                return Color.white;
        }
    }

    private IEnumerator ApplyPoisonDamage()
    {
        float elapsedTime = 0f;
        while (elapsedTime < poisonDuration)
        {
            currentHealth -= poisonDamagePerSecond;
            if (currentHealth <= 0)
            {
                Die();
                yield break;
            }
            elapsedTime += 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ApplyFireDamage()
    {
        float elapsedTime = 0f;
        float originalCooldown = fireCooldown;
        fireCooldown += fireDamageCooldownIncrease;

        while (elapsedTime < fireDamageDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fireCooldown = originalCooldown;
    }

    private IEnumerator ApplyIceDamage()
    {
        float elapsedTime = 0f;
        float originalMoveSpeed = moveSpeed;
        moveSpeed -= iceDamageSpeedReduction;

        while (elapsedTime < iceDamageDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moveSpeed = originalMoveSpeed;
    }

    private void Die()
    {
        animator.SetInteger("Player", 7); // Player_Death
        StartCoroutine(DieAnimationCoroutine());
    }

    private IEnumerator DieAnimationCoroutine()
    {
        yield return new WaitForSeconds(deathAnimationTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
    }
}
