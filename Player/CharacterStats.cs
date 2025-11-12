using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats básicas")]
    public int maxHealth = 100;
    public int currentHealth;
    public float speed = 5f;
    public float baseDamage = 10f;
    public float knockbackForce = 1f;
    public float cooldownMultiplier = 1f;

    [Header("Artefactos equipados")]
    public int maxArtifactSlots = 5;
    public List<Artifact> equippedArtifacts = new List<Artifact>();
    private int usedSlots = 0;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void EquipArtifact(Artifact artifact)
    {
        if (!equippedArtifacts.Contains(artifact))
        {
            if (usedSlots + artifact.slotCost <= maxArtifactSlots)
            {
                equippedArtifacts.Add(artifact);
                usedSlots += artifact.slotCost;
                artifact.OnEquip(this);
                Debug.Log("Equipado: " + artifact.artifactName + " (" + artifact.slotCost + " ranura/s)");
            }
            else
            {
                Debug.Log("No hay espacio para equipar " + artifact.artifactName);
            }
        }
    }

    public void UnequipArtifact(Artifact artifact)
    {
        if (equippedArtifacts.Contains(artifact))
        {
            equippedArtifacts.Remove(artifact);
            usedSlots -= artifact.slotCost;
            artifact.OnUnequip(this);
            Debug.Log("Desequipado: " + artifact.artifactName);
        }
    }

    public int GetUsedSlots()
    {
        return usedSlots;
    }

    public int GetFreeSlots()
    {
        return maxArtifactSlots - usedSlots;
    }
}
