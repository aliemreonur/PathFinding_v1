using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SearcherbyNeighbours :  IPathAlgorithm
{
    #region Fields
    private Queue<Cell> _cellsQueue;
    private bool _dijkstraOn;
    private AlgorithmHandler _algorithmHandler;
    private PathListHandler _pathListHandler;

    private Cell _currentCell;
    private bool _searchActive, _endCellReached;
    #endregion

    public SearcherbyNeighbours(AlgorithmHandler algorithmHandler, PathListHandler pathListHandler) 
    {
        _algorithmHandler = algorithmHandler;
        _pathListHandler = pathListHandler; 
        _cellsQueue = new();
    }

    public async void CalculateShortestPath(Cell startCell, Cell endCell)
    {
        InitializeAlgo();
        SetDijkaStatus();
        int iterations = 0;

        while (_pathListHandler.openCells.Count > 0 && _searchActive && iterations < _algorithmHandler.maxIterations)
        {
            iterations++;
            await SearchByNeighbours(_currentCell);
            _currentCell = _cellsQueue.Dequeue();
        }
    }

    private async Task SearchByNeighbours(Cell activeCell)
    {
        if (_dijkstraOn)
            _currentCell.CalculateCost(true, false);

        foreach (var neighbour in activeCell.neighboursList)
        {
            if (_pathListHandler.closedCells.Contains(neighbour))
                continue;

            await Awaitable.WaitForSecondsAsync(0.01f);
            _pathListHandler.CellVisited(neighbour);
            _cellsQueue.Enqueue(neighbour);
            _algorithmHandler.pathFinder.CellInspected(activeCell, neighbour);
            if (_dijkstraOn)
                neighbour.CalculateCost(true, false);

            if (neighbour == _algorithmHandler.endCell)
            {
                _endCellReached = true;
                if (_dijkstraOn)
                    neighbour.CalculateCost(true, false);
                _algorithmHandler.CreateThePath(neighbour);
                return;
            }
        }
        _searchActive = !_endCellReached;
    }

    private void InitializeAlgo()
    {
        _searchActive = true;
        _endCellReached = false;
        _currentCell = _algorithmHandler.startCell;
        _cellsQueue.Clear();
    }

    private void SetDijkaStatus()
    {
        _dijkstraOn = _algorithmHandler.algorithmType == AlgorithmType.Dijkstra ? true : false;
    }
}
