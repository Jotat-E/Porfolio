using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Core Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    public int baseDamage = 0;


    [Header("Attributes")]
    public int strength = 5;
    public int dexterity = 5;
    public int intelligence = 5;


    [Header("Multipliers")]
    public float moveSpeedMultiplier = 1f;


    private void Awake()
    {
        currentHealth = maxHealth;
    }


    public int GetAttackBonus()
    {
        return strength * 2;
    }


    public void TakeDamage(int dmg)
    {
        int final = Mathf.Max(0, dmg);
        currentHealth -= final;
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }


    void Die()
    {
        GameManager.Instance.PlayerDied(transform.position);
        gameObject.SetActive(false);
    }
}