using UnityEngine;
using System;
using System.Threading.Tasks;

public class CostHandler 
{
    public int CellCost { get; private set; }
    public int HCost { get; private set; }
    public int GCost { get; private set; } 
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
        ResetCosts();
    }

    public async Task SetGCost()
    {
        //CellCost = 0;
        GCost = 0;
        if (_cell.ParentCell == null)
            return;

        CalculateUnitCost();
        GCost = _unitCostToParent + _cell.ParentCell.GCost;
        CheckForABetterCost();
        CellCost = GCost;
    }

    public void CalculateHeuristicCost(Cell _endCell)
    {
        HCost = DistanceCalculator.CalculateCellCost(_cell, _endCell);
        Debug.Log(HCost);
    }

    public void ResetCosts()
    {
        CellCost = INITCOST;
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
