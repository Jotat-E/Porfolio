using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 3f;

    [Header("Ataque")]
    public float damage = 5f;
    public float attackCooldown = 1f;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    private Transform player;
    private Rigidbody2D rb;
    private float lastAttackTime = 0f;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
                isKnockedBack = false;
            return;
        }

        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * speed;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") &&
            Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            var playerStats = collision.collider.GetComponent<CharacterStats>();
            if (playerStats != null)
                playerStats.TakeDamage(damage);

            var pm = collision.collider.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                Vector2 pushDir = (collision.transform.position - transform.position).normalized;
                pm.ApplyKnockback(pushDir, knockbackForce, knockbackDuration);
            }
        }
    }

    public void ApplySelfKnockback(Vector2 direction)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
    }
}