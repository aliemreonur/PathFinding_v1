using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathAlgorithm 
{
    void CalculateShortestPath(Cell startCell, Cell endCell);
    
}
