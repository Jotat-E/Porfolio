using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSeeds : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    public float inventoryAppleSeeds = 5;

    public void Interact()
    {
        if (inventoryAppleSeeds > 0)
        {
            inventoryAppleSeeds = inventoryAppleSeeds - 1f;
            playerInventory.playerAppleSeeds += 1f;
            Debug.Log("En la caja quedan " + inventoryAppleSeeds + " semillas de manzana");
            Debug.Log("Ahora en el inventario tienes " + playerInventory.playerAppleSeeds + " manzanas");
        }
        else
        {
            Debug.Log("No quedan semillas de manzanaen la caja");
        }
    }

    public void AddApples(int amount)
    {
        inventoryAppleSeeds += amount;
        Debug.Log("Has comprado 1 semilla de manzana, ahora en la caja hay " + inventoryAppleSeeds + " semillas de manzana");
    }
}
