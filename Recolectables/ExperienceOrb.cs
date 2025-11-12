using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    [Tooltip("Cuánta XP da este orbe")]
    public int xpValue = 1;
    [Tooltip("Velocidad a la que se acerca al jugador")]
    public float attractSpeed = 5f;
    [Tooltip("Segundos antes de desaparecer si no se recoge")]
    public float lifeTime = 8f;

    public float pickupDelay = 0.3f;

    private Transform player;
    private Collider2D col2d;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        col2d = GetComponent<Collider2D>();

        col2d.enabled = false;
        Invoke(nameof(EnablePickup), pickupDelay);
        Destroy(gameObject, lifeTime);
    }

    void EnablePickup()
    {
        col2d.enabled = true;
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
            var exp = col.GetComponent<PlayerExperience>();
            if (exp != null)
                exp.AddXp(xpValue);
            Destroy(gameObject);
        }
    }
}