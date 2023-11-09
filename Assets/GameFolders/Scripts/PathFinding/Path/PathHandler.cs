using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PathHandler 
{
    private List<Cell> _cellsList;

    public PathHandler()
    {
        _cellsList = new();
    }

    private void SetList(Cell startCell, Cell lastCell)
    {
        _cellsList.Clear();
        Cell currentCell = lastCell;
        while(currentCell != startCell)
        {
            _cellsList.Add(currentCell);
            currentCell = currentCell.ParentCell;
        }
    }

    public async Awaitable CreatePath(Cell startCell, Cell lastCell)
    {
        SetList(startCell, lastCell);
        while(_cellsList.Count>1)
        {
            Cell currentCell = _cellsList.Last();
            currentCell.ChangeColor(true);
            await Awaitable.WaitForSecondsAsync(.15f);
            _cellsList.Remove(currentCell);
        }
        return;
    }
}
