using UnityEngine;
using System.Linq;

public class Greedy : IPathAlgorithm
{
    private AlgorithmHandler _algorithmBase;
    private PathListHandler _pathListHandler;

    private Cell _currentCell;
    private bool _searchActive, _endCellFound;

    public Greedy(AlgorithmHandler algorithmBase, PathListHandler pathListHandler) 
    {
        _algorithmBase = algorithmBase;
        _pathListHandler = pathListHandler;
    }

    public async void CalculateShortestPath(Cell activeCell, Cell endCell)
    {
        InitializeAlgo();
        int iterations = 0;

        while (_pathListHandler.openCells.Count > 0 && _searchActive && iterations < 1000)
        {
            _currentCell.CalculateCost(false, true, endCell);
            _pathListHandler.CellVisited(_currentCell);

            iterations++;
            if (iterations == 999)
                Debug.Log("I really tried bro :(");

            foreach (var neighbour in _currentCell.neighboursList)
            {
                if (_pathListHandler.closedCells.Contains(neighbour))
                    continue;

                if (neighbour.ParentCell == null)
                    neighbour.SetParentCell(_currentCell);

                neighbour.CalculateCost(false, true, endCell);

                await Awaitable.WaitForSecondsAsync(0.01f);

                _algorithmBase.pathFinder.CellInspected(activeCell, neighbour);

                if (neighbour == endCell)
                {
                    _endCellFound = true;
                    _algorithmBase.CreateThePath(neighbour);
                    return;
                }
            }
            _searchActive = !_endCellFound;
            
            _currentCell = AssignNextCell();
        }
    }

    private Cell AssignNextCell()
    {
        Cell cellToReturn =  _pathListHandler.openCells.OrderBy(c => c.HCost).FirstOrDefault();
        cellToReturn.SetParentCell(_currentCell);
        return cellToReturn;
    }

    private void InitializeAlgo()
    {
        _searchActive = true;
        _endCellFound = false;
        _currentCell = _algorithmBase.startCell;
    }


}
