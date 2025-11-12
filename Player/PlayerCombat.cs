using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class PlayerCombat : MonoBehaviour
{
    public Weapon weapon;
    private CharacterStats stats;
    private float lastAttackTime = 0f;

    [Header("Combate")]
    public float attackRadius = 1f;
    public float knockbackForce = 5f;

    void Start()
    {
        stats = GetComponent<CharacterStats>();
        if (weapon == null)
            Debug.LogError("PlayerCombat: No tienes asignada la Weapon.", this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            Time.time >= lastAttackTime + (1f / stats.attackSpeed))
        {
            lastAttackTime = Time.time;
            Attack();
        }
    }

    void Attack()
    {
        float dmg = CalculateDamage();
        Debug.Log($"Ataque con {weapon.weaponName}: infliges {dmg:F1} de daño");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D col in hits)
        {
            if (!col.CompareTag("Enemy")) continue;

            CharacterStats enemyStats = col.GetComponent<CharacterStats>();
            if (enemyStats != null)
                enemyStats.TakeDamage(dmg);

            EnemyAI enemyAI = col.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                Vector2 knockDir = (col.transform.position - transform.position).normalized;
                enemyAI.ApplySelfKnockback(knockDir);
            }
        }
    }

    float CalculateDamage()
    {
        if (weapon == null) return stats.damage;

        float baseDmg = stats.damage;
        return GetStrengthBonus(baseDmg) + GetDexterityBonus(baseDmg) + GetIntelligenceBonus(baseDmg);
    }

    float GetStrengthBonus(float baseDmg)
    {
        return (stats.strength / Mathf.Max(stats.strength, 1f)) *
               (weapon.strengthScalingPercent / Mathf.Max(weapon.strengthScalingPercent, 1f)) *
               (baseDmg * (stats.strength + weapon.strengthScalingPercent / 10f) / 10f);
    }

    float GetDexterityBonus(float baseDmg)
    {
        return (stats.dexterity / Mathf.Max(stats.dexterity, 1f)) *
               (weapon.dexterityScalingPercent / Mathf.Max(weapon.dexterityScalingPercent, 1f)) *
               (baseDmg * (stats.dexterity + weapon.dexterityScalingPercent / 10f) / 10f);
    }

    float GetIntelligenceBonus(float baseDmg)
    {
        return (stats.intelligence / Mathf.Max(stats.intelligence, 1f)) *
               (weapon.intelligenceScalingPercent / Mathf.Max(weapon.intelligenceScalingPercent, 1f)) *
               (baseDmg * (stats.intelligence + weapon.intelligenceScalingPercent / 10f) / 10f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}