using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEquip : MonoBehaviour
{
    public Inventory inventory;
    public Artifact testArtifact; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddArtifact(testArtifact);
            inventory.EquipArtifact(testArtifact);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            inventory.UnequipArtifact(testArtifact);
        }
    }
}
