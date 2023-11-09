
public class CostHandler
{
    #region Fields & Properties
    public int HCost { get; private set; }
    public int GCost { get; private set; }
    private const int INITCOST = 99999;
    private Cell _cell;
    private int _unitCostToParent;
    #endregion

    public CostHandler(Cell cell)
    {
        _cell = cell;
        ResetCosts();
    }

    #region Public Methods

    public void SetGCost()
    {
        if (_cell.ParentCell == null)
        {
            GCost = 0;
            return;
        }
        CalculateUnitCost();
        CalculateG();
    }

    public void CalculateHeuristicCost(Cell endCell)
    {
        HCost = DistanceCalculator.CalculateCellCost(_cell, endCell);
    }

    public void ResetCosts()
    {
        HCost = INITCOST;
        GCost = INITCOST;
    }

    public void SearchForABetterParent(bool gCost, bool hCost)
    {
        if (_cell.ParentCell == null)
            return;
        
        var currentParentCost = _cell.ParentCell.GCost;
        
        foreach (var neighbourCell in _cell.neighboursList)
        {
            if (neighbourCell.ParentCell == null)
                continue;

            var neighbourCellCost = neighbourCell.GCost;

            if (neighbourCellCost < currentParentCost)
            {
                _cell.SetParentCell(neighbourCell);
                currentParentCost = neighbourCellCost;

                AssignNewParent(gCost, hCost, neighbourCell);
            }
        }
    }
    #endregion

    #region Private Methods
    private void AssignNewParent(bool gCost, bool hCost, Cell neighbourCell)
    {
        _cell.SetParentCell(neighbourCell);
        _cell.CalculateCost(gCost, hCost);
    }

    private void CalculateG()
    {
        GCost = _unitCostToParent + _cell.ParentCell.GCost;
    }

    private void CalculateUnitCost()
    {
        _unitCostToParent = DistanceCalculator.CalculateCellCost(_cell.ParentCell, _cell);
    }
    #endregion
}