using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Artifact : ScriptableObject
{
    [Header("Info")]
    public string artifactName;
    [TextArea] public string description;

    [Header("Balanceo")]
    public int slotCost = 1;

    [Header("Visual")]
    public Sprite icon;

    public abstract void OnEquip(CharacterStats character);
    public abstract void OnUnequip(CharacterStats character);
}