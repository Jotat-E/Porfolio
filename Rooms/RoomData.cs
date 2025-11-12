using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomData : MonoBehaviour
{
    [Tooltip("Ancho de la sala en unidades")]
    public int width = 16;
    [Tooltip("Alto de la sala en unidades")]
    public int height = 10;

    [Header("Paredes/Pasillos")]
    public GameObject doorUp;
    public GameObject doorRight;
    public GameObject doorDown;
    public GameObject doorLeft;


    public int WidthCells => width / RoomManager.cellSize;
    public int HeightCells => height / RoomManager.cellSize;

    public void OpenDir(Vector2Int dir)
    {
        if (dir == Vector2Int.up) doorUp?.SetActive(false);
        else if (dir == Vector2Int.right) doorRight?.SetActive(false);
        else if (dir == Vector2Int.down) doorDown?.SetActive(false);
        else if (dir == Vector2Int.left) doorLeft?.SetActive(false);
    }
}
