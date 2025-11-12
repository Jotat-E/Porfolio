using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    [Tooltip("Arrastra aquí el ScriptableObject de Item")]
    public Item itemData;

    private SpriteRenderer sr;
    private Collider2D col;
    private bool canBePickedUp = false;

    [Tooltip("Retraso en segundos antes de que el pickup pueda ser recogido")]
    public float pickupEnableDelay = 0.3f;

    void Awake()
    {
        Debug.Log($"[ItemPickup] Awakened para item: {itemData?.itemName}");

        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (itemData != null && itemData.icon != null)
            sr.sprite = itemData.icon;

        col.isTrigger = true;
        col.enabled = false;

        Invoke(nameof(EnablePickup), pickupEnableDelay);
    }

    private void EnablePickup()
    {
        col.enabled = true;
        canBePickedUp = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBePickedUp || !other.CompareTag("Player"))
            return;

        if (itemData is PassiveItem passive)
        {
            passive.OnPickup(other.gameObject);
        }
        else if (itemData is ConsumableItem consumable)
        {
            var inv = other.GetComponent<PlayerInventory>();
            inv?.AddConsumable(consumable);
        }

        Destroy(gameObject);
    }
}
