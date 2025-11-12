using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RogueLite/ItemEffects/StatModifier")]
public class StatModifierEffect : ItemEffect
{
    public float bonusDefense;
    public float bonusMaxHealth;

    public override void Apply(GameObject user)
    {
        var stats = user.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.defense += bonusDefense;
            stats.maxHealth += bonusMaxHealth;
            stats.currentHealth += bonusMaxHealth;
        }
    }
}