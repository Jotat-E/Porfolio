using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBase : ScriptableObject
{
    public string spellName = "New Spell";
    public float cooldown = 3f;


    public abstract void Cast(Transform caster, Vector2 direction);
}