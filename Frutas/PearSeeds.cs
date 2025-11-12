using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearSeeds : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    public float inventoryPearSeeds = 5;

    public void Interact()
    {
        if (inventoryPearSeeds > 0)
        {
            inventoryPearSeeds = inventoryPearSeeds - 1f;
            playerInventory.pearInventory += 1f;
            Debug.Log("En la caja quedan " + inventoryPearSeeds + " semillas de pera");
            Debug.Log("Ahora en el inventario tienes " + playerInventory.pearInventory + " peras");
        }
        else
        {
            Debug.Log("No quedan semillas de pera en la caja");
        }
    }

    public void AddPears(int amount)
    {
        inventoryPearSeeds += amount;
        Debug.Log("Has comprado 1 semilla de pera, ahora en la caja hay " + inventoryPearSeeds + " semillas de pera");
    }
}
