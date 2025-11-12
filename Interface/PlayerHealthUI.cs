using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public CharacterStats playerStats;
    public Image healthFill;

    private void Update()
    {
        if (playerStats != null && healthFill != null)
        {
            float fill = playerStats.currentHealth / playerStats.maxHealth;
            healthFill.fillAmount = Mathf.Clamp01(fill);
        }
    }
}
