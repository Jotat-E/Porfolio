using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefabs de pickup")]
    [Tooltip("Prefab que contiene el script ItemPickup y un SpriteRenderer vacío")]
    public GameObject pickupPrefab;

    [Header("Posibles pasivos")]
    [Tooltip("Lista de PassiveItem que pueden nacer aquí")]
    public List<PassiveItem> possiblePassives = new();

    [Header("Posibles consumibles")]
    [Tooltip("Lista de ConsumableItem que pueden nacer aquí")]
    public List<ConsumableItem> possibleConsumables = new();

    [Header("Ajustes de spawn")]
    [Tooltip("Probabilidad de generar un pasivo (0–1)")]
    [Range(0f, 1f)] public float chancePassive = 0.5f;
    [Tooltip("Mínimo de consumibles a generar")]
    public int minConsumables = 0;
    [Tooltip("Máximo de consumibles (aleatorio entre min y max)")]
    public int maxConsumables = 1;

    public void SpawnAll()
    {
        Debug.Log($"[Spawner] ¡SpawnAll() invocado! Passivos: {possiblePassives.Count}, Consumibles: {possibleConsumables.Count}");

        if (possiblePassives.Count > 0 && Random.value <= chancePassive)
        {
            var chosen = possiblePassives[Random.Range(0, possiblePassives.Count)];
            SpawnPickup(chosen);
        }

        int count = Random.Range(minConsumables, maxConsumables + 1);
        for (int i = 0; i < count; i++)
        {
            if (possibleConsumables.Count == 0) break;
            var chosenC = possibleConsumables[Random.Range(0, possibleConsumables.Count)];
            SpawnPickup(chosenC);
        }
    }

    private void SpawnPickup(Item itemData)
    {
        var go = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        var ip = go.GetComponent<ItemPickup>();
        ip.itemData = itemData;

        var sr = go.GetComponent<SpriteRenderer>();
        if (sr != null && itemData.icon != null)
            sr.sprite = itemData.icon;
    }
}