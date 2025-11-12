using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCount : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    private SpriteRenderer sr;
    private Color originalColor;
    public bool playerInRange = false;
    public float grouwnTime = 5f;
    private bool isPlanted = false;
    private bool isGrown = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isPlanted)
            {
                isPlanted = true;
                sr.color = Color.yellow;
                StartCoroutine(Grow());
            }
            else if (isGrown)
            {
                Debug.Log("Has recolectado la fruta!");
                playerInventory.appleInventory += 1;
                ResetCorp();
            }
            else
            {
                Debug.Log("Todavía está creciendo...");
            }
        }

        if (isPlanted && !isGrown)
        {
            sr.color = Color.yellow;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }

        if (other.CompareTag("Player") && !isGrown)
        {
            sr.color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }

        if (other.CompareTag("Player") && !isGrown)
        {
            sr.color = originalColor;
        }
    }

    private IEnumerator Grow()
    {
        if(playerInventory.playerAppleSeeds > 0)
        {
            playerInventory.playerAppleSeeds -= 1f;

            yield return new WaitForSeconds(grouwnTime);

            isGrown = true;
            sr.color = Color.green;
            Debug.Log("¡La verdura ha crecido!");
        }
        else
        {
            Debug.Log("No tienes suficientes manzanas");
            isPlanted = false;
            sr.color = originalColor;
        }

    }

    private void ResetCorp()
    {
        isGrown = false;
        isPlanted = false;
        sr.color = originalColor;
    }

    public bool CanHarvest()
    {
        return isGrown;
    }
}
