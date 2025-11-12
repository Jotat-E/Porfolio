using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 velocity;
    public int damage = 10;
    public float lifetime = 5f;


    public void Init(Vector2 vel, int dmg)
    {
        velocity = vel;
        damage = dmg;
        Destroy(gameObject, lifetime);
    }


    private void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var e = collision.GetComponent<Enemy>();
            if (e != null) e.TakeDamage(damage, transform);
            Destroy(gameObject);
        }
    }
}