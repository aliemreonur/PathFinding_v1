using UnityEngine;

public class CostHandler 
{
    public int HCost { get; private set; }
    public int GCost { get; private set; } 
    private const int INITCOST = 99999;
    private Cell _cell;
    private int _unitCostToParent;

    public CostHandler(Cell cell) 
    {
        _cell= cell;
        ResetCosts();
    }

    public void SetGCost()
    {
        GCost = INITCOST;
        if (_cell.ParentCell == null)
        {
            GCost = 0;
            return;
        }

        CalculateUnitCost();
        CheckForABetterCost();
        GCost = _unitCostToParent + _cell.ParentCell.GCost;
    }

    public void CalculateHeuristicCost(Cell _endCell)
    {
        HCost = DistanceCalculator.CalculateCellCost(_cell, _endCell);
    }

    public void ResetCosts()
    {
        HCost = INITCOST;
        GCost= INITCOST;
    }

    private void CheckForABetterCost()
    {
        if (_cell.ParentCell == null) return;
        foreach (var cell in _cell.neighboursList)
        {
            if (cell.GCost < _cell.ParentCell.GCost)
            {
                _cell.SetParentCell(cell);
                CalculateUnitCost();
                GCost = Mathf.Min(_cell.ParentCell.GCost + _unitCostToParent, GCost);
            }
        }
    }

    private void CalculateUnitCost()
    {
        _unitCostToParent = DistanceCalculator.CalculateCellCost(_cell.ParentCell, _cell);
    }


}
