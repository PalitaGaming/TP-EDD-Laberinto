using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Floor,
    Wall,
    Start,
    End
}

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public TileType type = TileType.Floor;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    public void SetType(TileType newType)
    {
        type = newType;
        UpdateColor();
    }

    void UpdateColor()
    {
        switch (type)
        {
            case TileType.Floor:
                sr.color = Color.white;
                break;
            case TileType.Wall:
                sr.color = Color.black;
                break;
            case TileType.Start:
                sr.color = Color.blue;
                break;
            case TileType.End:
                sr.color = Color.red;
                break;
        }
    }

    void OnMouseDown()
    {
        FindObjectOfType<GridManager>().PaintTile(this);
    }
}

