using UnityEngine;
using System;

public class CostHandler 
{
    public int CellCost { get; private set; }
    private const int INITCOST = 99999;
    private Cell _cell;
    private int _unitCostToParent;

    /// <summary>
    /// For each algorithm type, create an interface and create new instances
    /// Let the regarding class handle the cost calculation
    /// </summary>
    /// <param name="parentCell"></param>

    public CostHandler(Cell cell) 
    {
        _cell= cell;
        CellCost = INITCOST;
    }

    public void UpdateCellCost()
    {
        CellCost = 0;
        if (_cell.ParentCell == null)
            return;

        CalculateUnitCost();
        CellCost = _unitCostToParent + _cell.ParentCell.CellCost;
        CheckForABetterCost();
    }

    public void CheckForABetterCost()
    {
        if (_cell.ParentCell == null) return;
        foreach (var cell in _cell.neighboursList)
        {
            if (cell.CellCost < _cell.ParentCell.CellCost)
            {
                _cell.SetParentCell(cell);
                CalculateUnitCost();
                CellCost = Mathf.Min(_cell.ParentCell.CellCost + _unitCostToParent, CellCost);
            }
        }
    }

    private void CalculateUnitCost()
    {
        _unitCostToParent = DistanceCalculator.CalculateCellCost(_cell.ParentCell, _cell);
    }
}
