using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Referencias (Inspector)")]
    public GameObject ballPrefab;
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform arenaWalls;

    [Header("UI (Textos)")]
    public TMP_Text moneyText;
    public TMP_Text spawnLevelText;
    public TMP_Text speedLevelText;
    public TMP_Text powerLevelText;
    public TMP_Text prestigeMultiplierText;
    public TMP_Text prestigePreviewText;

    public TMP_Text spawnPriceText;
    public TMP_Text speedPriceText;
    public TMP_Text powerPriceText;

    [Header("Panel Prestigio")]
    public GameObject prestigeConfirmPanel;

    [Header("Economía / Prestigio")]
    public float money = 1f;               
    public float prestigeMultiplier = 1f;  
    public float totalDamage = 0f;        
    public float prestigeScaling = 1000f;  
    [HideInInspector] public float nextPrestigeBonus = 1f;

    [Header("Bolas")]
    public float baseBallSize = 1f;
    public float ballSize = 1f;
    public float baseBallSpeed = 5f;
    public float ballSpeed = 5f;
    public float baseBallDamage = 1f;
    public float ballDamage = 1f;
    public float baseMoneyPerHit = 1f;
    public float moneyPerHit = 1f;
    public float minSpawnRadius = 1.2f;

    [Header("Niveles de mejoras")]
    public int spawnLevel = 0; 
    public int speedLevel = 0;
    public int powerLevel = 0;

    [Header("Precios base y factor de crecimiento")]
    public float baseSpawnPrice = 1f;   
    public float baseSpeedPrice = 5f;   
    public float basePowerPrice = 10f;
    [Tooltip("Si pones 1.0 el precio no cambia. Usa 1.4-2.0 para ver aumentos")]
    public float priceGrowth = 1.6f;   

    [Header("Escalado enemigo")]
    public int baseEnemyHealth = 50;
    public float enemyHealthMultiplier = 1.5f;
    public float enemyGrowth = 0.25f;
    private int enemyLevel = 0;
    private float currentEnemyScale = 1f;
    private float defaultEnemyScale = 1f;

    private List<Ball> activeBalls = new List<Ball>();
    private Enemy currentEnemy;

    void Start()
    {
        prestigeMultiplier = PlayerPrefs.GetFloat("PrestigeMultiplier", 1f);

        ballSize = baseBallSize;
        ballSpeed = baseBallSpeed;
        ballDamage = baseBallDamage;
        moneyPerHit = baseMoneyPerHit;
        currentEnemyScale = defaultEnemyScale;

        if (prestigeConfirmPanel != null) prestigeConfirmPanel.SetActive(false);

        ResetRunToInitial();
        SpawnEnemy();
        UpdateUI();
    }

    void Update()
    {
        nextPrestigeBonus = 1f + (totalDamage / Mathf.Max(1f, prestigeScaling));
        if (prestigePreviewText != null) prestigePreviewText.text = $"Multiplicador si prestigias ahora: x{nextPrestigeBonus:F2}";
    }

    float GetSpawnPriceRaw(int level) => baseSpawnPrice * Mathf.Pow(priceGrowth, Mathf.Max(0, level));

    int GetNextSpawnPrice() => Mathf.CeilToInt(GetSpawnPriceRaw(spawnLevel));
    int GetNextSpeedPrice() => Mathf.CeilToInt(baseSpeedPrice * Mathf.Pow(priceGrowth, Mathf.Max(0, speedLevel)));
    int GetNextPowerPrice() => Mathf.CeilToInt(basePowerPrice * Mathf.Pow(priceGrowth, Mathf.Max(0, powerLevel)));

    public void SpawnBall()
    {
        int price = GetNextSpawnPrice();

        if (money < price)
        {
            Debug.Log($"No tienes suficiente dinero para generar una bola. Precio: {price}, Tienes: {money}");
            return;
        }

        money -= price;
        Debug.Log($"SpawnBall: cobrado {price}. Dinero restante: {money}");

        UpdateUI(); 

        if (ballPrefab == null)
        {
            Debug.LogWarning("GameManager: ballPrefab no asignado.");
            return;
        }

        Vector2 center = spawnPoint != null ? (Vector2)spawnPoint.position : Vector2.zero;
        Vector2 offset = Random.insideUnitCircle.normalized * (minSpawnRadius + Random.Range(0f, 0.4f));
        Vector2 spawnPos = center + offset;

        GameObject go = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        go.transform.localScale = Vector3.one * ballSize;

        Ball b = go.GetComponent<Ball>();
        if (b == null)
        {
            Debug.LogWarning("Ball prefab no tiene el script Ball.");
            Destroy(go);
            return;
        }

        b.Initialize(this, Mathf.RoundToInt(ballDamage), ballSpeed, moneyPerHit);
        activeBalls.Add(b);
    }

    public void RemoveBall(Ball b)
    {
        if (activeBalls.Contains(b)) activeBalls.Remove(b);
    }

    public void UpgradeSpawn()
    {
        int price = GetNextSpawnPrice();
        if (money < price)
        {
            Debug.Log($"No tienes dinero suficiente para mejorar Spawn. Precio: {price}, Tienes: {money}");
            return;
        }

        int prevPrice = GetNextSpawnPrice();
        money -= prevPrice;
        spawnLevel++;
        int newPrice = GetNextSpawnPrice();

        Debug.Log($"UpgradeSpawn: gastado {prevPrice}. Nuevo nivel: {spawnLevel}. Precio siguiente: {newPrice}. Dinero restante: {money}");
        UpdateUI();
    }

    public void UpgradeSpeed()
    {
        int price = GetNextSpeedPrice();
        if (money < price) { Debug.Log("No tienes dinero suficiente para mejorar Speed."); return; }

        money -= price;
        speedLevel++;
        ballSpeed += 1f;

        foreach (var b in activeBalls) if (b != null) b.SetSpeed(ballSpeed);
        UpdateUI();
    }

    public void UpgradePower()
    {
        int price = GetNextPowerPrice();
        if (money < price) { Debug.Log("No tienes dinero suficiente para mejorar Power."); return; }

        money -= price;
        powerLevel++;
        ballDamage += 1f;
        moneyPerHit += 1f;

        foreach (var b in activeBalls)
        {
            if (b != null)
            {
                b.SetDamage(Mathf.RoundToInt(ballDamage));
                b.SetMoneyPerHit(moneyPerHit);
            }
        }
        UpdateUI();
    }

    public void AddMoney(float amtBase)
    {
        money += amtBase * prestigeMultiplier;
        UpdateUI();
    }

    public void AddDamage(float dmg)
    {
        totalDamage += dmg;
    }

    public void ShowPrestigeConfirm()
    {
        if (prestigeConfirmPanel != null) prestigeConfirmPanel.SetActive(true);
    }

    public void CancelPrestige()
    {
        if (prestigeConfirmPanel != null) prestigeConfirmPanel.SetActive(false);
    }

    public void PrestigeReset()
    {
        if (prestigeConfirmPanel != null) prestigeConfirmPanel.SetActive(false);

        float gainedMultiplier = nextPrestigeBonus;
        prestigeMultiplier *= gainedMultiplier;

        PlayerPrefs.SetFloat("PrestigeMultiplier", prestigeMultiplier);
        PlayerPrefs.Save();

        money = 1f;
        totalDamage = 0f;

        spawnLevel = 0;
        speedLevel = 0;
        powerLevel = 0;

        ballSize = baseBallSize;
        ballSpeed = baseBallSpeed;
        ballDamage = baseBallDamage;
        moneyPerHit = baseMoneyPerHit;

        for (int i = activeBalls.Count - 1; i >= 0; i--)
            if (activeBalls[i] != null) Destroy(activeBalls[i].gameObject);
        activeBalls.Clear();

        if (currentEnemy != null) Destroy(currentEnemy.gameObject);
        currentEnemy = null;
        enemyLevel = 0;
        currentEnemyScale = defaultEnemyScale;

        SpawnEnemy();
        UpdateUI();
    }

    public void OnEnemyDefeated()
    {
        AddMoney(50f * Mathf.Max(1, enemyLevel)); 
        if (arenaWalls != null) arenaWalls.localScale += Vector3.one * enemyGrowth;
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("GameManager: enemyPrefab no asignado.");
            return;
        }

        enemyLevel++;
        int newHealth = Mathf.RoundToInt(baseEnemyHealth * Mathf.Pow(enemyHealthMultiplier, Mathf.Max(0, enemyLevel - 1)));

        Vector3 pos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);
        go.transform.localScale = Vector3.one * currentEnemyScale;

        currentEnemy = go.GetComponent<Enemy>();
        if (currentEnemy != null) currentEnemy.Initialize(this, newHealth);
        else Debug.LogWarning("Enemy prefab no tiene el script Enemy.");

        currentEnemyScale += enemyGrowth;
    }

    void UpdateUI()
    {
        if (moneyText != null) moneyText.text = $"Dinero: {money:F0}";
        if (spawnLevelText != null) spawnLevelText.text = $"Spawn Lv {spawnLevel}";
        if (speedLevelText != null) speedLevelText.text = $"Velocidad Lv {speedLevel}";
        if (powerLevelText != null) powerLevelText.text = $"Poder Lv {powerLevel}";
        if (prestigeMultiplierText != null) prestigeMultiplierText.text = $"Prestige x{prestigeMultiplier:F2}";

        if (spawnPriceText != null) spawnPriceText.text = $"Coste siguiente: {GetNextSpawnPrice():F0}";
        if (speedPriceText != null) speedPriceText.text = $"Coste siguiente: {GetNextSpeedPrice():F0}";
        if (powerPriceText != null) powerPriceText.text = $"Coste siguiente: {GetNextPowerPrice():F0}";
    }

    void ResetRunToInitial()
    {
        money = 1f;
        totalDamage = 0f;

        spawnLevel = 0;
        speedLevel = 0;
        powerLevel = 0;

        ballSize = baseBallSize;
        ballSpeed = baseBallSpeed;
        ballDamage = baseBallDamage;
        moneyPerHit = baseMoneyPerHit;

        for (int i = activeBalls.Count - 1; i >= 0; i--)
            if (activeBalls[i] != null) Destroy(activeBalls[i].gameObject);
        activeBalls.Clear();

        if (currentEnemy != null) Destroy(currentEnemy.gameObject);
        currentEnemy = null;
        enemyLevel = 0;
        currentEnemyScale = defaultEnemyScale;
    }
}
