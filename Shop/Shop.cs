using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Tienda")]
    public List<ShopItem> items;
    public Transform contentParent; 
    public GameObject shopButtonPrefab; 

    void Start()
    {
        foreach (var item in items)
        {
            GameObject btnGO = Instantiate(shopButtonPrefab, contentParent);
            var img = btnGO.transform.Find("Icon").GetComponent<Image>();
            var txt = btnGO.transform.Find("Cost").GetComponent<Text>();
            img.sprite = item.icon;
            txt.text = item.cost.ToString();

            Button btn = btnGO.GetComponent<Button>();
            btn.onClick.AddListener(() => TryBuy(item));
        }
    }

    void TryBuy(ShopItem item)
    {
        if (CurrencyManager.Instance.SpendCoins(item.cost))
        {
            if (item.prefabToGive != null)
                Instantiate(item.prefabToGive, Vector3.zero, Quaternion.identity);
            Debug.Log($"Compraste {item.itemName} por {item.cost} monedas");
        }
        else Debug.Log("No tienes monedas suficientes");
    }
}
