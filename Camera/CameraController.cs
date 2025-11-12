using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    Transform target;
    float halfHeight, halfWidth;
    Vector2 minBounds, maxBounds;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        var cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;

        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null)
            SetTarget(playerGO.transform);
        else
            Debug.LogError("No he encontrado ningún objeto con tag Player");
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetBounds(float minX, float maxX, float minY, float maxY)
    {
        minBounds = new Vector2(minX, minY);
        maxBounds = new Vector2(maxX, maxY);
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 pos = target.position;

        pos.x = Mathf.Clamp(pos.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        pos.y = Mathf.Clamp(pos.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        pos.z = transform.position.z;

        transform.position = pos;
    }
}