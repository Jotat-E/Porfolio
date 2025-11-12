using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Roguelike/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;

    [Tooltip("Porcentaje de escalado por fuerza: 1 = 10%, 2 = 20%, ..., 10 = 100%")]
    public float strengthScalingPercent = 10f;

    [Tooltip("Porcentaje de escalado por destreza")]
    public float dexterityScalingPercent = 10f;

    [Tooltip("Porcentaje de escalado por inteligencia")]
    public float intelligenceScalingPercent = 10f;
}
