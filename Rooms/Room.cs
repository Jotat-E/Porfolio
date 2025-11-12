using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Enemigos")]
    public List<Transform> enemySpawnPoints;
    public List<GameObject> enemyPrefabs;

    [Header("Puertas (solo las de ESTA sala)")]
    public GameObject doorUp;
    public GameObject doorRight;
    public GameObject doorDown;
    public GameObject doorLeft;

    [Header("Ajustes de cierre")]
    [Tooltip("Retraso en segundos antes de cerrar puertas tras entrar")]
    public float doorCloseDelay = 0.3f;
    [Tooltip("Empuje al jugador dentro de la sala al entrar (unidades)")]
    public float playerPushInside = 0.5f;

    [HideInInspector] public bool isSecret = false;
    [HideInInspector] public bool isBossRoom = false;

    bool hasSpawned = false;
    int aliveEnemies = 0;

    RoomData data;
    BoxCollider2D roomTrigger;

    void Awake()
    {
        data = GetComponent<RoomData>();
        roomTrigger = GetComponent<BoxCollider2D>();
        roomTrigger.isTrigger = true;

        doorUp.SetActive(false);
        doorRight.SetActive(false);
        doorDown.SetActive(false);
        doorLeft.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(EnterRoomSequence(other.transform));
    }

    public void ActivateRoom()
    {
        StartCoroutine(EnterRoomSequence(null));
    }

    IEnumerator EnterRoomSequence(Transform player)
    {
        if (hasSpawned) yield break;
        hasSpawned = true;

        bool hasEnemies = enemySpawnPoints.Count > 0 && enemyPrefabs.Count > 0;
        if (hasEnemies)
        {
            foreach (var sp in enemySpawnPoints)
            {
                var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                var e = Instantiate(prefab, sp.position, Quaternion.identity);
                var stats = e.GetComponent<CharacterStats>();
                if (stats != null)
                {
                    aliveEnemies++;
                    stats.onDeath.AddListener(OnEnemyDeath);
                }
            }

            if (player != null)
            {
                Vector3 dir = (transform.position - player.position).normalized;
                player.position += dir * playerPushInside;
            }

            yield return new WaitForSeconds(doorCloseDelay);
            CloseAllDoors();
        }
    }

    void OnEnemyDeath()
    {
        aliveEnemies--;
        if (aliveEnemies <= 0)
            OpenAllDoors();
    }

    public void OpenDir(Vector2Int dir)
    {
        if (dir == Vector2Int.up) doorUp.SetActive(false);
        if (dir == Vector2Int.right) doorRight.SetActive(false);
        if (dir == Vector2Int.down) doorDown.SetActive(false);
        if (dir == Vector2Int.left) doorLeft.SetActive(false);
    }

    void CloseAllDoors()
    {
        doorUp.SetActive(true);
        doorRight.SetActive(true);
        doorDown.SetActive(true);
        doorLeft.SetActive(true);
    }

    void OpenAllDoors()
    {
        doorUp.SetActive(false);
        doorRight.SetActive(false);
        doorDown.SetActive(false);
        doorLeft.SetActive(false);
    }
}