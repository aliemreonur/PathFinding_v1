using System.Collections.Generic;
using UnityEngine;


public class AstarAlgorithm : IPathAlgorithm
{
    private bool _aStarOn = false;
    private AlgorithmBase _algorithmBase;
    private PathListHandler _pathListHandler;

    private Cell _currentCell;
    private bool _searchActive;
    private List<Cell> _closedList;
    private List<Cell> _openList;

    public AstarAlgorithm(AlgorithmBase algorithmBase, PathListHandler pathListHandler)
    {
        _algorithmBase = algorithmBase;
        _pathListHandler = pathListHandler;
        _closedList = new();
        _openList = new();
    }

    //1-look for the neighbour with the lowest F cost
    //2- if two of the F costs are the same, then look for the H cost
    //3- take the neighbour with the lowest cost, add it to the closed list - making the current obj as the parent of the neighbour
    //4- HOW TO DECIDE/CHECK THAT THE NEXT CELL'S PARENT IS THE LOWEST
    //5- check for the neighbours' neighbours for the lowest cost again
    //6- if stuck, check 

    public async void CalculateShortestPath(Cell activeCell, Cell endCell)
    {
        InitializeAlgo();
        int iterations = 0;

        while (_pathListHandler.openCells.Count > 0 && _searchActive && iterations < 1000)
        {
            CheckForLowerCostParent();

            Debug.Log(iterations);
            _currentCell.CalculateCost(true, true, endCell);
            _pathListHandler.CellVisited(_currentCell);
            _closedList.Add(_currentCell);
            _openList.Remove(_currentCell);

            iterations++;
            if (iterations == 999)
                Debug.Log("I really tried bro :(");

            foreach (var neighbour in _currentCell.neighboursList)
            {
                if (_closedList.Contains(neighbour))
                    continue;

                _pathListHandler.CellVisited(neighbour);

                if (!_openList.Contains(neighbour)) //added later
                    _openList.Add(neighbour);

                neighbour.CalculateCost(true, true, endCell); //calculate the F cost
                neighbour.SetParentCell(_currentCell); //for now, the parent of the all of the neighbours is the current one?

                await Awaitable.WaitForSecondsAsync(0.01f);
                _algorithmBase.pathFinder.CellInspected(activeCell, neighbour);

                if (neighbour == endCell)
                {
                    _searchActive = false;
                    foreach (var cell in neighbour.neighboursList)
                    {
                        if (!_closedList.Contains(cell))
                            cell.CalculateCost(true, true, endCell);

                        if (cell.FCost < _currentCell.FCost || cell.GCost < _currentCell.GCost)
                            neighbour.SetParentCell(cell);
                    }

                    _algorithmBase.CreateThePath(neighbour);
                    return;
                }
            }

            AssignNextCell();
        }
    }

    private void CheckForLowerCostParent()
    {

        if (_currentCell.ParentCell == null) //we are at the start cell
            return;

        int currentG = _currentCell.GCost;
        foreach (var cell in _currentCell.neighboursList)
        {
            _currentCell.CalculateCost(true, false);
            if (cell.GCost < _currentCell.ParentCell.GCost)
            {
                currentG = cell.GCost;
                _currentCell.SetParentCell(cell);
            }
        }
    }

    private void AssignNextCell()
    {
        var currentCost = 99999;
        Cell nextCell = _currentCell;

        foreach (var cell in _pathListHandler.visitedCells)
        {
            if (_closedList.Contains(cell))
                continue;

            if (cell.FCost <= currentCost) 
            {
                currentCost = cell.FCost;
                nextCell = cell;
            }
        }
        _currentCell = nextCell;
    }

    private void InitializeAlgo()
    {
        _openList.Clear();
        _searchActive = true;
        _currentCell = _algorithmBase.startCell;
        _openList.Add(_currentCell); //added later
        _closedList.Clear();
    }


}
