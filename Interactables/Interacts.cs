using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacts : MonoBehaviour
{
    [SerializeField] private AppleSeeds appleInventory;
    [SerializeField] private PearSeeds pearInventory;

    private SpriteRenderer sr;
    private Color originalColor;
    public bool playerInRage = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.color = Color.yellow;
            playerInRage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.color = originalColor;
            playerInRage = false;
        }
    }

    private void Update()
    {
        if (playerInRage && Input.GetKeyDown(KeyCode.E))
        {
            if (appleInventory != null)
            {
                appleInventory.Interact();
            }             
            
            if (pearInventory != null)
            {
                pearInventory.Interact();
            }
        }
    }
}
