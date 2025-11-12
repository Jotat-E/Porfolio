using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    [Header("Vida")]
    public float maxHealth = 100f;
    public float healthRegen = 1f;
    [HideInInspector] public float currentHealth;

    [Header("Defensa y Velocidad de movimiento")]
    public float defense = 5f;
    public float speed = 5f;

    [Header("Daño base")]
    public float damage = 10f;

    [Header("Escalado de atributos")]
    public float strength = 10f;
    public float dexterity = 10f;
    public float intelligence = 10f;

    [Header("Velocidad de ataque")]
    public float attackSpeed = 1f;

    [Header("Suerte u otros")]
    public float luck = 1f;

    [Header("Recompensa al morir")]
    public int xpRewardMin = 10;
    public int xpRewardMax = 15;
    public GameObject xpOrbPrefab;
    public float orbSpawnRadius = 0.5f;

    [Header("Monedas al morir")]
    public int coinRewardMin = 1;
    public int coinRewardMax = 3;
    public GameObject coinOrbPrefab;

    public UnityEvent onDeath;

    [HideInInspector] public Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegen * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float incomingDamage)
    {
        float mitigated = Mathf.Max(incomingDamage - defense, 0f);
        currentHealth -= mitigated;
        Debug.Log($"{gameObject.name} recibió {mitigated:F1} de daño (Defensa {defense})");
        if (currentHealth <= 0f) Die();
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        DropXP();
        DropCoins();

        Debug.Log($"{gameObject.name} ha muerto. Suelta orbes de XP y monedas.");
        onDeath?.Invoke();
        Destroy(gameObject);
    }

    void DropXP()
    {
        int xpAmount = Random.Range(xpRewardMin, xpRewardMax + 1);
        for (int i = 0; i < xpAmount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * orbSpawnRadius;
            GameObject orb = Instantiate(xpOrbPrefab, (Vector2)transform.position + offset, Quaternion.identity);
            var orbScript = orb.GetComponent<ExperienceOrb>();
            if (orbScript != null) orbScript.xpValue = 1;
        }
    }

    void DropCoins()
    {
        int coinAmount = Random.Range(coinRewardMin, coinRewardMax + 1);
        for (int i = 0; i < coinAmount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * orbSpawnRadius;
            GameObject orb = Instantiate(coinOrbPrefab, (Vector2)transform.position + offset, Quaternion.identity);
            var orbScript = orb.GetComponent<CoinOrb>();
            if (orbScript != null) orbScript.coinValue = 1;
        }
    }
}