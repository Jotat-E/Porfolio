using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Configuración de consumibles")]
    public int maxConsumables = 5;

    private List<ConsumableItem> consumables = new();

    public IReadOnlyList<ConsumableItem> Consumables => consumables;

    public void AddConsumable(ConsumableItem item)
    {
        if (consumables.Count >= maxConsumables)
        {
            Debug.LogWarning("Inventario lleno");
            return;
        }
        consumables.Add(item);
        UIManager.Instance.UpdateConsumables(consumables);
    }

    public void UseConsumable(int index)
    {
        if (index < 0 || index >= consumables.Count) return;
        consumables[index].Use(gameObject);
        consumables.RemoveAt(index);
        UIManager.Instance.UpdateConsumables(consumables);
    }
}