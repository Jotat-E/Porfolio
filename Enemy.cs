using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private int defaultMaxHealth;
    private Vector3 defaultScale;
    private GameManager gameManager;

    void Awake()
    {
        defaultMaxHealth = maxHealth;
        defaultScale = transform.localScale;
        currentHealth = maxHealth;
    }

    public void Initialize(GameManager gm, int health)
    {
        gameManager = gm;
        maxHealth = health;
        defaultMaxHealth = health;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gameManager != null) gameManager.OnEnemyDefeated();
        Destroy(gameObject);
    }

    public void ResetEnemy()
    {
        maxHealth = defaultMaxHealth;
        currentHealth = defaultMaxHealth;
        transform.localScale = defaultScale;
        transform.position = Vector3.zero;
    }
}
