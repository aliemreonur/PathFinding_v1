using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Astar : IPathAlgorithm
{
    private AlgorithmHandler _algorithmHandler;
    private PathListHandler _pathListHandler;

    private Cell _currentCell;
    private bool _searchActive, _endCellFound;
    private List<Cell> _calculatedList;

    public Astar(AlgorithmHandler algorithmHandler, PathListHandler pathListHandler)
    {
        _algorithmHandler = algorithmHandler;
        _pathListHandler = pathListHandler;
        _calculatedList = new();
    }

    public async void CalculateShortestPath(Cell activeCell, Cell endCell)
    {
        InitializeAlgo();
        int iterations = 0;

        while (_pathListHandler.openCells.Count > 0 && _searchActive && iterations < 1000)
        {
            _currentCell.CalculateCost(true, true, endCell);
            _pathListHandler.CellVisited(_currentCell);

            if (_calculatedList.Contains(_currentCell))
                _calculatedList.Remove(_currentCell);

            iterations++;
            if (iterations == 999)
                Debug.Log("I really tried bro :(");

            foreach (var neighbour in _currentCell.neighboursList)
            {
                if (_pathListHandler.closedCells.Contains(neighbour) || _calculatedList.Contains(neighbour))
                    continue;

                if (neighbour.ParentCell == null)
                    neighbour.SetParentCell(_currentCell);

                neighbour.CalculateCost(true, true, endCell);

                _calculatedList.Add(neighbour); //now these can be selected

                await Awaitable.WaitForSecondsAsync(0.01f);

                _algorithmHandler.pathFinder.CellInspected(activeCell, neighbour);

                if (neighbour == endCell)
                {
                    _endCellFound = true;
                    _algorithmHandler.CreateThePath(neighbour);
                    return;
                }
            }
            CheckSearchEnd();
        }
    }

    private void CheckSearchEnd()
    {
        _searchActive = !_endCellFound;

        if (_calculatedList.Count > 0)
            _currentCell = AssignNextCell();
        else
            _searchActive = false;
    }

    private Cell AssignNextCell()
    {
        Cell cellToReturn = _calculatedList.OrderBy(c => c.FCost).FirstOrDefault();

        if (_calculatedList.Count == 1)
            return cellToReturn;

        if (_calculatedList[0].FCost == _calculatedList[1].FCost)
        {
            int currentF = _calculatedList[0].FCost;
            int currentH = _calculatedList[0].HCost;
            foreach(var cell in _calculatedList)
            {
                if (cell.FCost > currentF)
                    continue;

                if(cell.HCost<currentH)
                {
                    currentH = cell.HCost;
                    cellToReturn = cell;
                }
            }
        }

        return cellToReturn;
    }

    private void InitializeAlgo()
    {
        _searchActive = true;
        _endCellFound = false;
        _currentCell = _algorithmHandler.startCell;
        _calculatedList.Clear();
    }

}
