using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletManager : MonoBehaviour
{
    public List<Amulet> equipped = new List<Amulet>();
    public int maxEquipped = 3;


    public void Equip(Amulet a)
    {
        if (equipped.Count >= maxEquipped) return;
        equipped.Add(a);
        a.OnEquip(gameObject);
    }


    public void Unequip(Amulet a)
    {
        if (equipped.Remove(a)) a.OnUnequip(gameObject);
    }
}