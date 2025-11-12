using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMonster : MonoBehaviour
{
    public int soulValue = 0;
    public int maxHealth = 10;
    int currentHealth;


    public float degradeRate = 60f;


    public int soulValueProp => soulValue;


    public void Init(int souls)
    {
        soulValue = souls;
        currentHealth = Mathf.Clamp(souls / 2, 5, souls); 
                                                   
    }


    private void Update()
    {
        degradeRate -= Time.deltaTime;
        if (degradeRate <= 0f)
        {
            soulValue = Mathf.Max(0, soulValue - 1);
            degradeRate = 60f;
            if (soulValue <= 0) Destroy(gameObject);
        }
    }


    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }


    void OnDeath()
    {
        GameManager.Instance.AddSouls(soulValue);
        Destroy(gameObject);
    }
}