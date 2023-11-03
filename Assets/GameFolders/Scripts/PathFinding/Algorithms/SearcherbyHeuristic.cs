using System.Collections.Generic;
using UnityEngine;


public class SearcherbyHeuristic : IPathAlgorithm
{
    private bool _aStarOn = false;
    private AlgorithmBase _algorithmBase;
    private PathListHandler _pathListHandler;

    private Cell _currentCell;
    private bool _searchActive;
    private List<Cell> _closedList;

    public SearcherbyHeuristic(AlgorithmBase algorithmBase, PathListHandler pathListHandler) 
    {
        _algorithmBase = algorithmBase;
        _pathListHandler = pathListHandler;
        _closedList = new();
    }

    public async void CalculateShortestPath(Cell activeCell, Cell endCell)
    {
        InitializeAlgo();
        int iterations = 0;

        while (_pathListHandler.openCells.Count > 0 && _searchActive && iterations < 1000)
        {
            _currentCell.CalculateHCost(endCell);
            _pathListHandler.CellVisited(_currentCell);
            _closedList.Add(_currentCell);

            iterations++;
            if (iterations == 999)
                Debug.Log("I really tried bro :(");

            foreach (var neighbour in _currentCell.neighboursList)
            {
                if (_closedList.Contains(neighbour))
                    continue;

                _pathListHandler.CellVisited(neighbour);
                neighbour.CalculateHCost(endCell);
                neighbour.SetParentCell(_currentCell);
                await Awaitable.WaitForSecondsAsync(0.01f);
                _algorithmBase.pathFinder.CellInspected(activeCell, neighbour);

                if (neighbour == endCell)
                {
                    _searchActive = false;
                    foreach (var cell in neighbour.neighboursList)
                        if (cell.HCost < _currentCell.HCost)
                            neighbour.SetParentCell(cell);
                    _algorithmBase.CreateThePath(neighbour);
                    Debug.Log(iterations);
                    return;
                }
            }

            var currentHCost = 99999;
            Cell nextCell = _currentCell;

            foreach (var cell in _pathListHandler.visitedCells)
            {
                if (_closedList.Contains(cell))
                    continue;
                if (cell.HCost < currentHCost)
                {
                    currentHCost = cell.HCost;
                    nextCell = cell;
                }
            }
            _currentCell = nextCell;
        }
    }

    private void InitializeAlgo()
    {
        _searchActive = true;
        _currentCell = _algorithmBase.startCell;
        _closedList.Clear();
    }


}
