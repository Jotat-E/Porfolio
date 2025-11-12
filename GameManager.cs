using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public int currentSouls = 0;


    [Header("Soul Monster")]
    public GameObject soulMonsterPrefab;
    List<GameObject> activeSoulMonsters = new List<GameObject>();


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    public void AddSouls(int amt)
    {
        currentSouls += amt;
    }


    public void SpawnSouls(Vector3 pos, int amount)
    {
        GameObject go = new GameObject("SoulsPickup");
        go.transform.position = pos;
        var pickup = go.AddComponent<SoulPickup>();
        pickup.amount = amount;
        var circle = go.AddComponent<CircleCollider2D>();
        circle.isTrigger = true;
    }


    public void PlayerDied(Vector3 deathPos)
    {
        int lostSouls = currentSouls;
        if (lostSouls <= 0) return;


        if (soulMonsterPrefab != null)
        {
            GameObject sm = Instantiate(soulMonsterPrefab, deathPos, Quaternion.identity);
            var smComp = sm.GetComponent<SoulMonster>();
            if (smComp != null) smComp.Init(lostSouls);
            activeSoulMonsters.Add(sm);
        }


        currentSouls = 0; 
    }


    public void RecoverSoulsFromMonster(SoulMonster monster)
    {
        AddSouls(monster.soulValue);
        activeSoulMonsters.Remove(monster.gameObject);
        Destroy(monster.gameObject);
    }


    public void DestroyAllSoulMonstersPermanently()
    {
        foreach (var go in activeSoulMonsters)
        {
            Destroy(go);
        }
        activeSoulMonsters.Clear();
    }
}