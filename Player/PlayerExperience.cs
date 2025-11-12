using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerExperience : MonoBehaviour
{
    [Header("Nivel y Experiencia")]
    public int level = 1;
    public int currentXp = 0;
    public int xpToNextLevel = 100;

    public UnityEvent<int, int, int> onXpChanged;
    public void AddXp(int amount)
    {
        currentXp += amount;

        while (currentXp >= xpToNextLevel)
        {
            currentXp -= xpToNextLevel;
            level++;
            xpToNextLevel = Mathf.CeilToInt(xpToNextLevel * 1.2f);
        }

        onXpChanged?.Invoke(level, currentXp, xpToNextLevel);
    }
}
