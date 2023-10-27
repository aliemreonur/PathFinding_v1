using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[System.Serializable]
public class Cell
{
    public List<Cell> neighboursList => _neighbourGatherer.neighbourCells;
    public int CellCost => _costHandler.CellCost;
    public int GCost => _costHandler.GCost;
    public int HCost => _costHandler.HCost;
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

    public async Task CalculateGCost() 
    {
        await _costHandler.SetGCost();
        UpdateVisual();
    }

    public void CalculateHCost(Cell endCell)
    {
        _costHandler.CalculateHeuristicCost(endCell);
        _cellView.HCostUpdated(HCost);
    }

    public void Reset()
    {
        _costHandler.ResetCosts();
        ParentCell = null;
        _cellView.Reset();
    }

    private void UpdateVisual()
    {
        _cellView.CellCostUpdated(CellCost);
    }

}

