using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    public bool active = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Activate(collision.transform);
            }
        }
    }


    public void Activate(Transform player)
    {
        active = true;

        var stats = player.GetComponent<CharacterStats>();
        if (stats != null) stats.Heal(stats.maxHealth);


    }
}