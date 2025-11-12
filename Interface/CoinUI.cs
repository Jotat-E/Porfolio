using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Start()
    {
        CurrencyManager.Instance.onCoinsChanged.AddListener(UpdateCoins);
        UpdateCoins(CurrencyManager.Instance.coins);
    }

    void UpdateCoins(int coins)
    {
        coinText.text = $"Coins: {coins}";
    }
}
