using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(MapView))]
public class PathFinder : MonoBehaviour
{
    private InterestPointsHandler _interestPointsHandler;

    private List<Cell> _openCells = new();
    private List<Cell> _visitedCells = new();
    private Queue<Cell> _cellsQueue= new Queue<Cell>();
    private Cell _startCell => _interestPointsHandler.StartCell;
    private Cell _endCell => _interestPointsHandler.EndCell;
    private Cell _currentCell;

    private MapView _mapView;

    private IPathAlgorithm _normalAlgorithm;

    private void Awake()
    {
        _mapView = GetComponent<MapView>();
    }

    private void OnEnable()
    {
        _mapView.OnMapSet += SetInterestPoints;
    }

    private void OnDisable()
    {
        _mapView.OnMapSet -= SetInterestPoints;
    }

    public void Reset()
    {
        foreach (var cell in _mapView.map.AllCells)
            cell.Reset();
    }

    public async void GreedyAlgorihm()
    {
        InitializeAlgo();


        while (_currentCell != _endCell && _openCells.Count > 0)
        {
            _currentCell.CalculateHCost(_endCell);

            var currentHCost = 9999;
            Cell nextCell = _currentCell;

            foreach (var neighbour in _currentCell.neighboursList)
            {
                if (_visitedCells.Contains(neighbour))
                    continue;

                CellInspected(_currentCell, neighbour);
                neighbour.CalculateHCost(_endCell);
                await Awaitable.WaitForSecondsAsync(0.01f);

                HandleLists(neighbour, false);
                if (neighbour == _endCell)
                {
                    await CreatePath(neighbour);
                    return;
                }
            }
            
            //search the visited cells for the lowest HCost

            foreach(var cell in _visitedCells)
            {
                if (cell.HCost < currentHCost)
                {
                    currentHCost = cell.HCost;
                    nextCell = cell;
                }
            }

            //_cellsQueue.Enqueue(nextCell); 
            _currentCell = nextCell;
            //_currentCell = _cellsQueue.Dequeue();
        }

    }

    public async void BreadthsFirstSearchAlgorithm()
    {
        InitializeAlgo();

        while (_currentCell != _endCell && _openCells.Count > 0)
        {
            await Trial();
            _currentCell = _cellsQueue.Dequeue();
        }
    }

    private void InitializeAlgo()
    {
        _currentCell = _startCell;
        ClearLists();
        SetOpenCellList();
    }

    public async void DijkstrasAlgorithm() 
    {
        InitializeAlgo();
        while (_currentCell != _endCell && _openCells.Count > 0)
        {
            _currentCell.CalculateGCost();
            await Trial(true);
           
            _currentCell = _cellsQueue.Dequeue();
        }
    }

    private async Task CreatePath(Cell neighbour, bool dijkstraOn = false)
    {
        if(dijkstraOn)
            await neighbour.CalculateGCost();

        Cell cell = neighbour.ParentCell;
        
        cell.ChangeColor(true);
        cell = cell.ParentCell;
        if (cell != null)
        {
            while (cell != _startCell)
            {
                await Awaitable.WaitForSecondsAsync(.25f);
                cell.ChangeColor(true);
                cell = cell.ParentCell;
            }
        }
        return;
    }

    private void HandleLists(Cell cellToHandle, bool queueOn = true)
    {
        if (_openCells.Contains(cellToHandle))
            _openCells.Remove(cellToHandle);

        if (!_visitedCells.Contains(cellToHandle)) //the difference between the open cells?
            _visitedCells.Add(cellToHandle);
        if (!queueOn)
            return;
        
        _cellsQueue.Enqueue(cellToHandle);
    }

    private void CellInspected(Cell activeCell, Cell neighbourCell)
    {
        if (neighbourCell != _startCell && neighbourCell != _endCell)
            neighbourCell.ChangeColor(false);

        if (neighbourCell.ParentCell == null && neighbourCell != _startCell)
            neighbourCell.SetParentCell(activeCell);
    }


    private async Task Trial(bool dijkstraOn = false)
    {
        foreach (var neighbour in _currentCell.neighboursList)
        {
            if (_visitedCells.Contains(neighbour))
                continue;

            await Awaitable.WaitForSecondsAsync(0.01f);
            HandleLists(neighbour);
            CellInspected(_currentCell, neighbour);

            if (neighbour == _endCell)
            {
                if (dijkstraOn)
                    neighbour.CalculateGCost();
                await CreatePath(neighbour, dijkstraOn);
                return;
            }
        }
    }

    private void SetOpenCellList()
    {
        foreach(var cell in _mapView.map.AllCells)
        {
            if (!cell.IsBlocked)
                _openCells.Add(cell);     
        }
    }

    private void ClearLists()
    {
        _openCells.Clear();
        _visitedCells.Clear();
        _cellsQueue.Clear();
    }

    private void SetInterestPoints(byte dummy, byte dummy2) // :)
    {
        _interestPointsHandler = new InterestPointsHandler(_mapView.map);
    }
}
