using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("Ajustes de rejilla")]
    public static int cellSize = 20;

    [Header("Prefabs de sala genéricas")]
    public List<GameObject> roomPrefabs;
    [Tooltip("Número total de salas a generar")]
    public int roomCount = 5;

    [Header("Sala de jefe")]
    [Tooltip("Prefab visual para la sala final de jefe")]
    public GameObject bossRoomPrefab;

    [Header("Decorador sala secreta")]
    public GameObject secretRoomDecoratorPrefab;

    public event Action<Vector2Int> OnRoomEnter;

    private struct PlacedRoom
    {
        public Vector2Int origin;
        public RoomData data;
        public GameObject roomObject;
        public Room roomBehaviour;
    }
    private Dictionary<Vector2Int, PlacedRoom> placedRooms = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GenerateGenericRooms();
        ConnectAllRooms();
        DecorateSecretRoom();
        SwapBossRoom();
        EnterRoom(Vector2Int.zero);
    }

    private void GenerateGenericRooms()
    {
        TryPlaceRoom(Vector2Int.zero, roomPrefabs[0]);
        for (int i = 1; i < roomCount; i++)
            TryPlaceNextRoom();
    }

    private bool TryPlaceNextRoom()
    {
        var candidates = GetAdjacentCandidates();
        Shuffle(candidates);
        foreach (var origin in candidates)
        {
            Shuffle(roomPrefabs);
            foreach (var prefab in roomPrefabs)
            {
                var data = prefab.GetComponent<RoomData>();
                if (data != null && CanPlaceAt(origin, data))
                    return TryPlaceRoom(origin, prefab);
            }
        }
        return false;
    }

    private List<Vector2Int> GetAdjacentCandidates()
    {
        var list = new List<Vector2Int>();
        foreach (var kv in placedRooms)
        {
            var o = kv.Key;
            var d = kv.Value.data;
            var dirs = new[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            foreach (var dir in dirs)
            {
                var cand = o + new Vector2Int(dir.x * d.WidthCells, dir.y * d.HeightCells);
                if (!placedRooms.ContainsKey(cand)) list.Add(cand);
            }
        }
        return list;
    }

    private bool TryPlaceRoom(Vector2Int origin, GameObject prefab)
    {
        var data = prefab.GetComponent<RoomData>();
        if (data == null) return false;

        Vector3 pos = new(
            origin.x * cellSize + data.width * 0.5f,
            origin.y * cellSize + data.height * 0.5f,
            0f
        );
        var go = Instantiate(prefab, pos, Quaternion.identity, transform);
        var rb = go.GetComponent<Room>();

        placedRooms[origin] = new PlacedRoom
        {
            origin = origin,
            data = data,
            roomObject = go,
            roomBehaviour = rb
        };
        return true;
    }

    private void ConnectAllRooms()
    {
        foreach (var kv in placedRooms)
        {
            var pos = kv.Key;
            var room = kv.Value.roomBehaviour;
            if (placedRooms.ContainsKey(pos + Vector2Int.up)) room.OpenDir(Vector2Int.up);
            if (placedRooms.ContainsKey(pos + Vector2Int.right)) room.OpenDir(Vector2Int.right);
            if (placedRooms.ContainsKey(pos + Vector2Int.down)) room.OpenDir(Vector2Int.down);
            if (placedRooms.ContainsKey(pos + Vector2Int.left)) room.OpenDir(Vector2Int.left);
        }
    }

    private void DecorateSecretRoom()
    {
        var cul = new List<Vector2Int>();
        foreach (var kv in placedRooms)
        {
            int cnt = 0;
            foreach (var d in new[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left })
                if (placedRooms.ContainsKey(kv.Key + d)) cnt++;
            if (cnt == 1 && kv.Key != Vector2Int.zero) cul.Add(kv.Key);
        }
        if (cul.Count > 0)
        {
            var sel = cul[UnityEngine.Random.Range(0, cul.Count)];
            var roomB = placedRooms[sel].roomBehaviour;
            roomB.isSecret = true;
            if (secretRoomDecoratorPrefab)
                Instantiate(secretRoomDecoratorPrefab,
                    roomB.transform.position, Quaternion.identity, roomB.transform);
        }
    }

    private void SwapBossRoom()
    {
        var dist = new Dictionary<Vector2Int, int> { { Vector2Int.zero, 0 } };
        var q = new Queue<Vector2Int>();
        q.Enqueue(Vector2Int.zero);

        Vector2Int best = Vector2Int.zero;
        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            int d0 = dist[cur];
            if (d0 > dist[best]) best = cur;
            foreach (var dir in new[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left })
            {
                var nb = cur + dir;
                if (placedRooms.ContainsKey(nb) && !dist.ContainsKey(nb))
                {
                    dist[nb] = d0 + 1;
                    q.Enqueue(nb);
                }
            }
        }

        var pr = placedRooms[best];
        Destroy(pr.roomObject);
        placedRooms.Remove(best);
        TryPlaceRoom(best, bossRoomPrefab);
    }

    public void EnterRoom(Vector2Int coords)
    {
        if (!placedRooms.TryGetValue(coords, out var pr)) return;
        OnRoomEnter?.Invoke(coords);
        pr.roomBehaviour?.ActivateRoom();
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    private bool CanPlaceAt(Vector2Int origin, RoomData data)
    {
        int w = data.WidthCells, h = data.HeightCells;
        for (int x = origin.x; x < origin.x + w; x++)
            for (int y = origin.y; y < origin.y + h; y++)
                if (placedRooms.ContainsKey(new Vector2Int(x, y))) return false;
        return true;
    }
}