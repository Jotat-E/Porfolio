using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExperienceUI : MonoBehaviour
{
    public PlayerExperience playerExp;
    public Slider xpSlider;
    public TMP_Text levelText;

    void Start()
    {
        if (playerExp == null) Debug.LogError("Falta Player Experience", this);
        if (xpSlider == null) Debug.LogError("Falta XP Slider", this);
        if (levelText == null) Debug.LogError("Falta Level Text (TMP_Text)", this);

        if (playerExp != null)
            playerExp.onXpChanged.AddListener(UpdateXpUI);

        if (playerExp != null && xpSlider != null && levelText != null)
            UpdateXpUI(playerExp.level, playerExp.currentXp, playerExp.xpToNextLevel);
    }

    void UpdateXpUI(int level, int currentXp, int xpToNextLevel)
    {
        xpSlider.maxValue = xpToNextLevel;
        xpSlider.value = currentXp;
        levelText.text = $"Nivel {level}";
    }
}