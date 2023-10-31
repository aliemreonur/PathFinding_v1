using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathListHandler 
{
    public List<Cell> openCells => _openCells;
    public List<Cell> visitedCells => _visitedCells;

    private List<Cell> _openCells;
    private List<Cell> _visitedCells;
    private MapView _mapView;


    public PathListHandler()
    {
        _mapView = MapView.Instance;
        _openCells = new List<Cell>();
        _visitedCells = new List<Cell>();
    }

    public void SetOpenCellList()
    {
        foreach (var cell in _mapView.map.AllCells)
        {
            if (!cell.IsBlocked)
                _openCells.Add(cell);
        }
    }

    public void ClearLists()
    {
        _openCells.Clear();
        _visitedCells.Clear();
    }

    public void CellVisited(Cell visitedCell)
    {
        _openCells.Remove(visitedCell);
        _visitedCells.Add(visitedCell);
    }


}
