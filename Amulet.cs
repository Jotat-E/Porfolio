using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Amulet")]
public class Amulet : ScriptableObject
{
    public string amuletName = "Amulet";
    public string description = "Gives a passive buff";


    public virtual void OnEquip(GameObject wearer) { }
    public virtual void OnUnequip(GameObject wearer) { }
}