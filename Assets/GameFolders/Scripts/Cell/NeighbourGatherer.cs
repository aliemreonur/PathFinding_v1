using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourGatherer
{
    private Cell _parentCell;
    public List<Cell> neighbourCells { get; private set; }
    private Map _map;

    public NeighbourGatherer(Cell parentCell, Map map)
    {
        _parentCell = parentCell; 
        neighbourCells = new List<Cell>();
        _map = map;
        _map.OnMapLoaded += GetNeighbours;
        GameManager.Instance.OnMapRestart += GetNeighbours;
    }

    public void DeregisterEvents()
    {
        GameManager.Instance.OnMapRestart -= GetNeighbours;
    }

    private void GetNeighbours()
    {
        neighbourCells.Clear();
        foreach (var direction in Directions.MoveableDirections)
        {
            int xToCheck = _parentCell.xPos + direction.x;
            int yToCheck = _parentCell.yPos + direction.y;
            if (!CheckIfPointValid(xToCheck, yToCheck))
                continue;

            Cell neighbourCell = _map.AllCells[xToCheck, yToCheck];
            if (!neighbourCell.IsBlocked)
                neighbourCells.Add(neighbourCell);
        }
    }

    private bool CheckIfPointValid(int neighbourX, int neighbourY)
    {
        if (neighbourX < 0 || neighbourY < 0 || neighbourX > _map.Width - 1 || neighbourY > _map.Height - 1)
            return false;
        return true;
    }

}
