using System.Collections.Generic;

[System.Serializable]
public class Cell
{
    public List<Cell> neighboursList => _neighbourGatherer.neighbourCells;
    public int FCost => _costHandler.GCost + _costHandler.HCost;
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

    public void DeregisterEvents()
    {
        _neighbourGatherer.DeregisterEvents();
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

    public void SetBlockedStatus(bool isBlocked =true)
    {
        _isBlocked = isBlocked;
        if (isBlocked && _cellView != null)
            _cellView.BlockedCell(isBlocked);
    }

    public void AssignedAsInterestPoint(bool isStart)
    {
        _isBlocked = true;
        _cellView.ChangeInterestPointColor(isStart);
    }

    public void CalculateCost(bool gCost, bool hCost, Cell endCell = null)
    {
        if (gCost && !hCost)
        {
            CalculateGCost();
            _costHandler.SearchForABetterParent(gCost, hCost);
        }
 
        else if (!gCost && hCost && endCell != null)
        {
            CalculateHCost(endCell);
        }

        else if(gCost && hCost && endCell != null)
        {
            CalculateGCost();
            CalculateHCost(endCell);
        }

        _cellView.CellCostUpdated(_costHandler.GCost, _costHandler.HCost, FCost);
    }

    private void CalculateGCost() 
    {
        _costHandler.SetGCost();
    }

    private void CalculateHCost(Cell endCell)
    {
        _costHandler.CalculateHeuristicCost(endCell);
    }

    public void Reset(bool isNewMap = false)
    {
        _costHandler.ResetCosts();
        ParentCell = null;
        _cellView.Reset(isNewMap);
    }
}

