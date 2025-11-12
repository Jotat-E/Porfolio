using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 30;
    int currentHealth;
    public int soulsValue = 5;


    private void Awake()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int dmg, Transform source)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) Die(source.position);
    }


    void Die(Vector3 deathPos)
    {
        GameManager.Instance.SpawnSouls(deathPos, soulsValue);
        Destroy(gameObject);
    }
}