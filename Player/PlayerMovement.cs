using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterStats CharacterStats;

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(x, y);

        movement = movement.normalized;

        transform.Translate(movement * CharacterStats.speed * Time.deltaTime);
    }
}
