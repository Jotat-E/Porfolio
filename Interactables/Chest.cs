using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTableEntry
{
    [Tooltip("Prefab que se instancia si sale este loot (moneda, orbe, poción, objeto...).")]
    public GameObject itemPrefab;

    [Range(0f, 1f), Tooltip("Probabilidad de soltar este entry (0–1).")]
    public float dropChance = 0.5f;

    [Tooltip("Cantidad mínima a instanciar si sale.")]
    public int minAmount = 1;
    [Tooltip("Cantidad máxima a instanciar si sale.")]
    public int maxAmount = 1;
}

public class Chest : MonoBehaviour
{
    [Header("Loot Table")]
    [Tooltip("Define los posibles loot y sus probabilidades.")]
    public List<LootTableEntry> lootTable;

    [Header("Apertura")]
    [Tooltip("Collider trigger que detecta al jugador para abrir.")]
    public Collider2D openTrigger;

    [Header("Spawn de ítems")]
    [Tooltip("Radio en el que se dispersan los ítems al abrir.")]
    public float spawnRadius = 1f;

    [Header("Configuración de destrucción")]
    [Tooltip("Segundos tras apertura antes de destruir el cofre.")]
    public float destroyDelay = 1f;

    bool isOpened = false;

    void Awake()
    {
        var mainCol = GetComponent<Collider2D>();
        if (mainCol != null)
            mainCol.isTrigger = false;

        if (openTrigger == null)
            Debug.LogError($"[{name}] falta asignar openTrigger en {GetType().Name}", this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpened) return;
        if (other.CompareTag("Player"))
        {
            OpenChest();
        }
    }

    public void OpenChest()
    {
        isOpened = true;

        foreach (var entry in lootTable)
        {
            if (Random.value <= entry.dropChance)
            {
                int count = Random.Range(entry.minAmount, entry.maxAmount + 1);
                for (int i = 0; i < count; i++)
                {
                    Vector2 offset = Random.insideUnitCircle * spawnRadius;
                    Vector3 spawnPos = transform.position + (Vector3)offset;
                    Instantiate(entry.itemPrefab, spawnPos, Quaternion.identity);
                }
            }
        }

        Destroy(gameObject, destroyDelay);
    }
}