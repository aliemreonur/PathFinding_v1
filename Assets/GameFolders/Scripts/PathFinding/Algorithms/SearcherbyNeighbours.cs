using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SearcherbyNeighbours : AlgorithmBase, IPathAlgorithm
{
    private Queue<Cell> _cellsQueue;
    private bool _dijkstraOn;

    public SearcherbyNeighbours(PathFinder pathFinderToAssign, PathListHandler pathListHandlerToAssign) 
    {
        _cellsQueue = new();
        base.pathFinder = pathFinderToAssign;
        base.pathListHandler = pathListHandlerToAssign;
    }

    public async void CalculateShortestPath(Cell startCell, Cell endCell)
    {
        _dijkstraOn = pathFinder.activeAlgo == AlgorithmType.Dijkstra ? true : false;
        _cellsQueue.Clear();
        InitializeAlgo(startCell);
        int iterations = 0;

        while (pathListHandler.openCells.Count > 0 && searchActive && iterations < 500)
        {
            iterations++;

            if (_dijkstraOn)
                currentCell.CalculateGCost();
            await SearchByNeighbours(currentCell, _dijkstraOn);
            currentCell = _cellsQueue.Dequeue();
        }
    }

    private async Task SearchByNeighbours(Cell activeCell, bool dijkstraOn)
    {
        foreach (var neighbour in activeCell.neighboursList)
        {
            if (pathListHandler.visitedCells.Contains(neighbour))
                continue;

            await Awaitable.WaitForSecondsAsync(0.01f);
            pathListHandler.CellVisited(neighbour);
            _cellsQueue.Enqueue(neighbour);
            pathFinder.CellInspected(activeCell, neighbour);

            if (neighbour == pathFinder.endCell)
            {
                searchActive = false;
                //if (dijkstraOn)
                //    neighbour.CalculateGCost();
                await pathFinder.CreatePath(neighbour, dijkstraOn);
                return;
            }
        }
    }
}
