using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DistanceCalculator 
{
    public static int CalculateCellCost(Cell startCell, Cell endCell)
    {
        if (endCell == null)
            Debug.Log("End cell is null");
        if (startCell == null)
            Debug.Log("Start cell is null");
        int xDistance = Mathf.Abs(startCell.xPos - endCell.xPos);
        int yDistance = Mathf.Abs(startCell.yPos - endCell.yPos);

        int diagonalAmount = Mathf.Min(xDistance, yDistance);
        int maxAmount = Mathf.Max(xDistance, yDistance);
        int nonDiagonalAmount = maxAmount - diagonalAmount;

        return nonDiagonalAmount * 10 + diagonalAmount * 14;
    }
}
