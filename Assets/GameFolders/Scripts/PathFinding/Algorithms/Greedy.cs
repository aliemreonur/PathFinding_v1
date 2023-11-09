using UnityEngine;
using System.Linq;

public class Greedy : IPathAlgorithm
{
    #region Fields
    private AlgorithmHandler _algorthimHandler;
    private PathListHandler _pathListHandler;

    private Cell _currentCell;
    private bool _searchActive, _endCellFound;
    #endregion

    #region Const
    public Greedy(AlgorithmHandler algorithmBase, PathListHandler pathListHandler) 
    {
        _algorthimHandler = algorithmBase;
        _pathListHandler = pathListHandler;
    }
    #endregion

    #region Methods 
    public async void CalculateShortestPath(Cell activeCell, Cell endCell)
    {
        InitializeAlgo();
        int iterations = 0;

        while (_pathListHandler.openCells.Count > 0 && _searchActive && iterations < _algorthimHandler.maxIterations)
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

                _algorthimHandler.pathFinder.CellInspected(activeCell, neighbour);

                if (neighbour == endCell)
                {
                    _endCellFound = true;
                    _algorthimHandler.CreateThePath(neighbour);
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
        return cellToReturn;
    }

    private void InitializeAlgo()
    {
        _searchActive = true;
        _endCellFound = false;
        _currentCell = _algorthimHandler.startCell;
    }
    #endregion

}
