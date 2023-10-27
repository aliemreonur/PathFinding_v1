using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public List<Cell> neighboursList => _neighbourGatherer.neighbourCells;
    public int CellCost => _costHandler.CellCost;
    public bool IsBlocked => _isBlocked;
    public Cell ParentCell { get; private set; }
    public int xPos => _xPos;
    public int yPos => _yPos;

    private bool _isBlocked;
    private int _xPos;
    private int _yPos;
    private Map _map;
    private CellView _cellView;
    private NeighbourGatherer _neighbourGatherer;
    private CostHandler _costHandler;


    public Cell(Map map, int x, int y)
    {
        _map = map;
        _xPos = x;
        _yPos = y;
        _isBlocked = false;
        _neighbourGatherer = new NeighbourGatherer(this, _map);
 
    }

    public void AssignCellView(CellView cellViewToAssign)
    {
        _cellView = cellViewToAssign;
        _costHandler = new CostHandler(this);
    }

    public void ChangeColor(bool onPath = false)
    {
        if (onPath)
            _cellView.CellOnPath();
        else
            _cellView.CellVisited();
 
    }

    public void SetParentCell(Cell parentCell)
    {
        ParentCell = parentCell;
    }

    public void SetBlockedOn()
    {
        _isBlocked = true;
    }

    public void AssignedAsInterestPoint(bool isStart)
    {
        _isBlocked = true;
        _cellView.ChangeInterestPointColor(isStart);
    }

    public void CalculateCellCost() //algorithmtype
    {
        _costHandler.UpdateCellCost();
        UpdateVisual();
    }

    public void CheckForAlternative()
    {
        _costHandler.CheckForABetterCost();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _cellView.CellCostUpdated(CellCost);
    }

}

