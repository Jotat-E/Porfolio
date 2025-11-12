using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private AppleSeeds appleInventory;
    [SerializeField] private PearSeeds pearInventory;
    [SerializeField] private Money moneyInventory;

    private SpriteRenderer sr;
    private Color originalColor;
    private bool playerInRage = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        shopPanel.SetActive(false);
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
            shopPanel.SetActive(!shopPanel.activeSelf);
        }

        if (!playerInRage && shopPanel.activeSelf == true)
        {
            shopPanel.SetActive(false);
        }
    }

    public void BuyApple()
    {
        appleInventory.AddApples(1);
        moneyInventory.Pay(1);
    }    
    
    public void BuyPear()
    {
        pearInventory.AddPears(1);
        moneyInventory.Pay(2);
    }
}
