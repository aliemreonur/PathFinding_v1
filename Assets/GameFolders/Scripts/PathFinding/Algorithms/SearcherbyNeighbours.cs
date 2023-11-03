using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SearcherbyNeighbours :  IPathAlgorithm
{
    private Queue<Cell> _cellsQueue;
    private bool _dijkstraOn;
    private AlgorithmBase _algorithmBase;
    private PathListHandler _pathListHandler;

    private Cell _currentCell;
    private bool _searchActive;

    public SearcherbyNeighbours(AlgorithmBase algorithmBase, PathListHandler pathListHandler) 
    {
        _algorithmBase = algorithmBase;
        _pathListHandler = pathListHandler; 
        _cellsQueue = new();
    }

    public async void CalculateShortestPath(Cell startCell, Cell endCell)
    {
        InitializeAlgo();
        SetDijkaStatus();
        int iterations = 0;

        while (_pathListHandler.openCells.Count > 0 && _searchActive && iterations < 500)
        {
            iterations++;
            if (_dijkstraOn)
                _currentCell.CalculateGCost();
            await SearchByNeighbours(_currentCell, _dijkstraOn);
            _currentCell = _cellsQueue.Dequeue();
        }
    }

    private async Task SearchByNeighbours(Cell activeCell, bool dijkstraOn)
    {
        foreach (var neighbour in activeCell.neighboursList)
        {
            if (_pathListHandler.visitedCells.Contains(neighbour))
                continue;

            await Awaitable.WaitForSecondsAsync(0.01f);
            _pathListHandler.CellVisited(neighbour);
            _cellsQueue.Enqueue(neighbour);
            _algorithmBase.pathFinder.CellInspected(activeCell, neighbour);

            if (neighbour == _algorithmBase.endCell)
            {
                _searchActive = false;
                if (dijkstraOn)
                    neighbour.CalculateGCost(); //check this again
                _algorithmBase.CreateThePath(neighbour);
                return;
            }
        }
    }

    private void InitializeAlgo()
    {
        _searchActive = true;
        _currentCell = _algorithmBase.startCell;
        _cellsQueue.Clear();
    }

    private void SetDijkaStatus()
    {
        _dijkstraOn = _algorithmBase.algorithmType == AlgorithmType.Dijkstra ? true : false;
    }
}
