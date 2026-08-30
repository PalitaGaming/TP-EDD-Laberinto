using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 12;
    public int height = 12;

    public GameObject tilePrefab;
    public GameObject character;

    public Tile[,] grid;

    private List<Tile> currentPath;

    private bool isMoving= false;

    public TileType currentBrush = TileType.Wall;

    public void SetBrushToWall() => currentBrush = TileType.Wall;
    public void SetBrushToFloor() => currentBrush = TileType.Floor;
    public void SetBrushToStart() => currentBrush = TileType.Start;
    public void SetBrushToEnd() => currentBrush = TileType.End;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float offsetX = width / 2f;
                float offsetY = height / 2f;

                GameObject obj = Instantiate(
                    tilePrefab,
                    new Vector3(x - offsetX + 0.5f, y - offsetY + 0.5f, 0),
                    Quaternion.identity
                );
                Tile tile = obj.GetComponent<Tile>();
                tile.worldPosition = obj.transform.position;

                tile.x = x;
                tile.y = y;

                grid[x, y] = tile;
            }
        }
    }

    public void PaintTile(Tile tile)
    {
        if (currentBrush == TileType.Start)
        {
            RemoveOldTile(TileType.Start);
        }

        if (currentBrush == TileType.End)
        {
            RemoveOldTile(TileType.End);
        }

        ClearPath();

        tile.SetType(currentBrush);
    }

    void RemoveOldTile(TileType type)
    {
        foreach (Tile t in grid)
        {
            if (t.type == type)
            {
                t.SetType(TileType.Floor);
            }
        }
    }

    public Tile GetStart()
    {
        foreach (Tile t in grid)
            if (t.type == TileType.Start)
                return t;
        return null;
    }

    public Tile GetEnd()
    {
        foreach (Tile t in grid)
            if (t.type == TileType.End)
                return t;
        return null;
    }

    void ClearPath()
    {
        if (currentPath == null)
            return;

        foreach (Tile tile in currentPath)
        {
            tile.ClearPath();
        }

        currentPath = null;
    }

    public Pathfinding pathfinding;
    public TMPro.TextMeshProUGUI solutionText;

    public void Solve()
    {
        if (isMoving)
            return;

        Tile start = GetStart();
        Tile end = GetEnd();

        if (start == null || end == null)
        {
            solutionText.text = "Falta entrada o salida";
            return;
        }

        currentPath = pathfinding.Dijkstra(start, end);

        if (currentPath == null)
        {
            solutionText.text = "No tiene solución";
        }
        else
        {
            solutionText.text = "Tiene solución";

            character.transform.position = start.worldPosition;

            StartCoroutine(MoveCharacter(currentPath));
        }
    }

    IEnumerator MoveCharacter(List<Tile> path)
    {
        isMoving = true;

        foreach (Tile tile in path)
        {
            character.transform.position = tile.worldPosition;

            if (tile.type == TileType.Floor)
            {
                tile.MarkPath();
            }

            yield return new WaitForSeconds(0.3f);
        }

        isMoving = false;
    }

    IEnumerator MoveCharacterBack()
    {
        isMoving = true;

        for (int i = currentPath.Count - 1; i >= 0; i--)
        {
            Tile tile = currentPath[i];

            character.transform.position = tile.worldPosition;

            yield return new WaitForSeconds(0.3f);
        }

        isMoving = false;
    }
    public void ReturnToStart()
    {
        if (isMoving)
            return;

        if (currentPath == null || currentPath.Count == 0)
            return;

        StartCoroutine(MoveCharacterBack());
    }
}

