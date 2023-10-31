using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

[RequireComponent(typeof(MapView))]
public class PathFinder : MonoBehaviour
{
    private InterestPointsHandler _interestPointsHandler;

    public Cell startCell => _interestPointsHandler.StartCell;
    public Cell endCell => _interestPointsHandler.EndCell;
    public AlgorithmType activeAlgo;
    private Cell _currentCell;

    private MapView _mapView;
    private bool _searchActive = false;

    private IPathAlgorithm _searchByNeighbours, _greedyAlgorithm, _aStarAlgorithm;
    private IPathAlgorithm _activeAlgorithm;
    private PathListHandler _pathListHandler;

    private void Awake()
    {
        _mapView = GetComponent<MapView>();
        _pathListHandler = new PathListHandler();
        _searchByNeighbours = new SearcherbyNeighbours(this, _pathListHandler);
    }

    private void OnEnable()
    {
        _mapView.OnMapSet += SetInterestPoints;
    }

    private void OnDisable()
    {
        _mapView.OnMapSet -= SetInterestPoints;
        _interestPointsHandler.DeregisterEvents();
    }

    public void Reset()
    {
        foreach (var cell in _mapView.map.AllCells)
            cell.Reset();
    }

    public void SetAlgorithm(int algoID)
    {
        switch(algoID)
        {
            case 1:
                activeAlgo = AlgorithmType.Bridths;
                _activeAlgorithm = _searchByNeighbours;
                break;
            case 2:
                activeAlgo = AlgorithmType.Dijkstra;
                _activeAlgorithm = _searchByNeighbours;
                break;
            case 3:
                activeAlgo = AlgorithmType.Greedy;
                _activeAlgorithm = _greedyAlgorithm;
                break;
            default:
                activeAlgo = AlgorithmType.AStar;
                _activeAlgorithm = _aStarAlgorithm;
                break;
        }

        _activeAlgorithm.CalculateShortestPath(startCell, endCell);

    }

    /*
    public async void GreedyAlgorihm() //some cases result in endless loop
    {
        InitializeAlgo();
        int iterations = 0;

        while (_currentCell.neighboursList.Count > 0 && _searchActive && iterations<500)
        {
            _currentCell.CalculateHCost(endCell);
            iterations++;
            if (iterations == 499)
                Debug.Log("HMMM");

            foreach (var neighbour in _currentCell.neighboursList)
            {
                if (_visitedCells.Contains(neighbour))
                    continue;

                CellInspected(_currentCell, neighbour);
                neighbour.CalculateHCost(endCell);
                await Awaitable.WaitForSecondsAsync(0.01f);

                HandleLists(neighbour, false);
                if (neighbour == endCell)
                {
                    await CreatePath(neighbour);
                    _searchActive = false;
                    return;
                }
            }

            //search the visited cells for the lowest HCost

            var currentHCost = 9999;
            Cell nextCell = _currentCell;
            foreach (var cell in _visitedCells)
            {
                if (cell.HCost < currentHCost)
                {
                    currentHCost = cell.HCost;
                    nextCell = cell;
                }
            }
            _currentCell = nextCell;
        }

    }
    */

    public async Task CreatePath(Cell neighbour, bool dijkstraOn = false)
    {
        if(dijkstraOn)
             neighbour.CalculateGCost();

        Cell cell = neighbour.ParentCell;
        
        cell.ChangeColor(true);
        cell = cell.ParentCell;
        if (cell != null)
        {
            while (cell != startCell)
            {
                await Awaitable.WaitForSecondsAsync(.15f);
                cell.ChangeColor(true);
                cell = cell.ParentCell;
            }
        }
        return;
    }

    public void CellInspected(Cell activeCell, Cell neighbourCell)
    {
        if (neighbourCell != startCell && neighbourCell != endCell)
            neighbourCell.ChangeColor(false);

        if (neighbourCell.ParentCell == null && neighbourCell != startCell)
            neighbourCell.SetParentCell(activeCell);
    }


    private void SetInterestPoints(byte dummy, byte dummy2) // :)
    {
        _interestPointsHandler = new InterestPointsHandler(_mapView.map);
    }
}
