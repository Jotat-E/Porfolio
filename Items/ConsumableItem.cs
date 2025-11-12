using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RogueLite/Item/Consumable")]
public class ConsumableItem : Item
{
    public int healAmount;
    public float duration;

    public void Use(GameObject user)
    {
        var stats = user.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.currentHealth =
                Mathf.Min(stats.currentHealth + healAmount, stats.maxHealth);
            Debug.Log($"[ConsumableItem] Usado: {itemName}");
        }
    }
}
