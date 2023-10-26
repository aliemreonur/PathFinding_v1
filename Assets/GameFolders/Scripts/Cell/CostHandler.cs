using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CostHandler 
{
    public int CellCost { get; private set; }
    private Cell _parentCell;

    

    /// <summary>
    /// For each algorithm type, create an interface and create new instances
    /// Let the regarding class handle the cost calculation
    /// </summary>
    /// <param name="parentCell"></param>

    public CostHandler(Cell parentCell) 
    {
        _parentCell= parentCell;
    }

    public void CalculateCellCost(Cell startCell, Cell endCell) //type in the algorithm type here
    {
        CalculateEndPointCost(startCell, endCell);
    }

    private void CalculateEndPointCost(Cell startCell, Cell endCell)
    {
        //if (ParentCell != null)
        //    CellCost += ParentCell.CellCost;

        int xDistance = Mathf.Abs(_parentCell.xPos - endCell.xPos);
        int yDistance = Mathf.Abs(_parentCell.yPos - endCell.yPos);

        int diagonalAmount = Mathf.Min(xDistance, yDistance);
        int maxAmount = Mathf.Max(xDistance, yDistance);
        int nonDiagonalAmount = maxAmount - diagonalAmount;

        CellCost = nonDiagonalAmount * 10 + diagonalAmount * 14;
        //_cellView.CellCostUpdated(CellCost);
    }
}
