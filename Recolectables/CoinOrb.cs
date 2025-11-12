using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinOrb : MonoBehaviour
{
    [Tooltip("Cuántas monedas da este orbe")]
    public int coinValue = 1;
    [Tooltip("Velocidad a la que se acerca al jugador")]
    public float attractSpeed = 5f;
    [Tooltip("Duración antes de auto?destruirse")]
    public float lifeTime = 8f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                attractSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            CurrencyManager.Instance.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }
}
