using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public CharacterStats CharacterStats;

    [Header("Attack Settings")]
    public float attackRange = 1f;           
    public float attackRadius = 0.5f;        
    public float attackCooldown = 0.3f;     
    public LayerMask enemyLayers;         

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0))
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - transform.position).normalized;

        Vector3 attackPoint = transform.position + direction * attackRange;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRadius, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Golpeado: " + enemy.name);
            Vector2 knockbackDir = (enemy.transform.position - transform.position).normalized;
            enemy.GetComponent<Enemy>()?.TakeDamage(CharacterStats.baseDamage, knockbackDir, CharacterStats.knockbackForce); 
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector3 direction = (mousePos - transform.position).normalized;
        Vector3 attackPoint = transform.position + direction * attackRange;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint, attackRadius);
    }
}
