using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/ProjectileSpell")]
public class ProjectileSpell : SpellBase
{
    public GameObject projectilePrefab;
    public int damage = 20;
    public float speed = 10f;


    public override void Cast(Transform caster, Vector2 direction)
    {
        if (projectilePrefab == null) return;
        GameObject p = Instantiate(projectilePrefab, caster.position, Quaternion.identity);
        var proj = p.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Init(direction.normalized * speed, damage);
        }
    }
}