using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickup : MonoBehaviour
{
    public int amount = 5;
    public float lifetime = 10f;


    void Start()
    {
        Destroy(gameObject, lifetime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddSouls(amount);
            Destroy(gameObject);
        }
    }
}