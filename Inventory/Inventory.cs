using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Dependencias")]
    public CharacterStats characterStats;

    [Header("Artefactos que posee el jugador")]
    public List<Artifact> ownedArtifacts = new List<Artifact>();

    public void AddArtifact(Artifact artifact)
    {
        if (!ownedArtifacts.Contains(artifact))
        {
            ownedArtifacts.Add(artifact);
            Debug.Log("Añadido al inventario: " + artifact.artifactName);
        }
    }

    public void EquipArtifact(Artifact artifact)
    {
        if (ownedArtifacts.Contains(artifact))
        {
            characterStats.EquipArtifact(artifact);
        }
    }

    public void UnequipArtifact(Artifact artifact)
    {
        if (ownedArtifacts.Contains(artifact))
        {
            characterStats.UnequipArtifact(artifact);
        }
    }
}
