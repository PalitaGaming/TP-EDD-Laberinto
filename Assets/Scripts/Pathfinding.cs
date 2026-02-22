using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding : MonoBehaviour
{
    public GridManager gridManager;

    public List<Tile> Dijkstra(Tile start, Tile end)
    {
        List<Tile> openList = new List<Tile>();
        HashSet<Tile> closedList = new HashSet<Tile>();

        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, int> cost = new Dictionary<Tile, int>();

        openList.Add(start);
        cost[start] = 0;

        while (openList.Count > 0)
        {
            Tile current = openList[0];

            foreach (Tile t in openList)
                if (cost[t] < cost[current])
                    current = t;

            if (current == end)
                return ReconstructPath(cameFrom, end);

            openList.Remove(current);
            closedList.Add(current);

            foreach (Tile neighbor in GetNeighbors(current))
            {
                if (neighbor.type == TileType.Wall || closedList.Contains(neighbor))
                    continue;

                int newCost = cost[current] + 1;

                if (!cost.ContainsKey(neighbor) || newCost < cost[neighbor])
                {
                    cost[neighbor] = newCost;
                    cameFrom[neighbor] = current;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null;
    }

    List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();

        int x = tile.x;
        int y = tile.y;

        if (x > 0) neighbors.Add(gridManager.grid[x - 1, y]);
        if (x < gridManager.width - 1) neighbors.Add(gridManager.grid[x + 1, y]);
        if (y > 0) neighbors.Add(gridManager.grid[x, y - 1]);
        if (y < gridManager.height - 1) neighbors.Add(gridManager.grid[x, y + 1]);

        return neighbors;
    }

    List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile end)
    {
        List<Tile> path = new List<Tile>();
        Tile current = end;

        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }
}
