using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Amulets/DoubleDash")]
public class ExampleAmulet_DoubleDash : Amulet
{
    public override void OnEquip(GameObject wearer)
    {
        var p = wearer.GetComponent<PlayerController>();
        if (p != null)
        {
            p.dashDuration += 0f;
        }
    }


    public override void OnUnequip(GameObject wearer)
    {

    }
}