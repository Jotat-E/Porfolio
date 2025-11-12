using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "RogueLite/Item/Passive")]
public class PassiveItem : Item
{
    [Tooltip("Efectos que se aplicarán al recoger")]
    public List<ItemEffect> effects = new();

    public override void OnPickup(GameObject picker)
    {
        base.OnPickup(picker);
        foreach (var e in effects)
            e.Apply(picker);
        Debug.Log($"[PassiveItem] Recogido: {itemName}");
    }
}
