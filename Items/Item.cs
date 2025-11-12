using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RogueLite/Item/Base")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;

    public virtual void OnPickup(GameObject picker) { }
}