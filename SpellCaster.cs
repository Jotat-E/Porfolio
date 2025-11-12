using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public SpellBase[] equippedSpells = new SpellBase[2];
    float[] cooldownTimers;


    private void Awake()
    {
        cooldownTimers = new float[equippedSpells.Length];
    }


    private void Update()
    {
        for (int i = 0; i < cooldownTimers.Length; i++) if (cooldownTimers[i] > 0) cooldownTimers[i] -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(AimAndCast(0));
        if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(AimAndCast(1));
    }


    IEnumerator AimAndCast(int slot)
    {
        if (slot < 0 || slot >= equippedSpells.Length) yield break;
        var spell = equippedSpells[slot];
        if (spell == null) yield break;
        if (cooldownTimers[slot] > 0) yield break;


        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }


        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouseWorld - transform.position;
        spell.Cast(transform, dir);
        cooldownTimers[slot] = spell.cooldown;
    }
}