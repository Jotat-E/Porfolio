using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCooldownAmulet", menuName = "Artifacts/Cooldown Amulet")]
public class CooldownAmulet : Artifact
{
    [Header("Mechanics Cooldown Artifact")]
    [Range(0f, 1f)]
    public float cooldownReduction = 0.2f;

    private void OnEnable()
    {
        slotCost = 2;
    }

    public override void OnEquip(CharacterStats character)
    {
        character.cooldownMultiplier -= cooldownReduction;
    }

    public override void OnUnequip(CharacterStats character)
    {
        character.cooldownMultiplier += cooldownReduction;
    }
}