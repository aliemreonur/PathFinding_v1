using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using static UnityEngine.RuleTile.TilingRuleOutput;

[RequireComponent(typeof(MapView))]
public class PathFinder : MonoBehaviour
{
    private InterestPointsHandler _interestPointsHandler;

    private List<Cell> _openCells = new();
    private List<Cell> _visitedCells = new();
    private Queue<Cell> _cellsQueue= new Queue<Cell>();
    private Cell _startCell => _interestPointsHandler.StartCell;
    private Cell _endCell => _interestPointsHandler.EndCell;

    private MapView _mapView;

    private IPathAlgorithm _normalAlgorithm;

    private void Awake()
    {
        _mapView = GetComponent<MapView>();
    }

    private void Start()
    {
        _interestPointsHandler = new InterestPointsHandler(_mapView.map);
        //BreadthsFirstSearchAlgorithm();
        DijkstrasAlgorithm();
        //instantiate normal algorithm
    }

    private async void BreadthsFirstSearchAlgorithm() 
    {
        Cell currentCell = _startCell;
        ClearLists();
        SetOpenCellList();

        while(currentCell != _endCell && _openCells.Count>0)
        {
            foreach (var neighbour in currentCell.neighboursList)
            {
                if (_cellsQueue.Contains(neighbour)) //what about the cost?
                    continue;

                await Awaitable.WaitForSecondsAsync(0.025f);

                HandleLists(neighbour);
                CellInspected(currentCell, neighbour);

                if (neighbour == _endCell)
                {
                    Cell cell = neighbour.ParentCell;
                    cell.ChangeColor(true);
                    cell = cell.ParentCell;
                    if(cell != null)
                    {
                        while(cell != _startCell)
                        {
                            await Awaitable.WaitForSecondsAsync(.25f);
                            cell.ChangeColor(true);
                            cell = cell.ParentCell;
                        }
                    }
                    return;
                }    //handle end path green
            }
            currentCell = _cellsQueue.Dequeue();
        }
    }

    private async void DijkstrasAlgorithm() 
    {
        Cell currentCell = _startCell;
        ClearLists();
        SetOpenCellList();

        while (currentCell != _endCell && _openCells.Count > 0)
        {
            currentCell.CalculateCellCost();
            foreach (var neighbour in currentCell.neighboursList)
            {
                if (_cellsQueue.Contains(neighbour)) 
                    continue;

                await Awaitable.WaitForSecondsAsync(0.025f);
                HandleLists(neighbour);
                CellInspected(currentCell, neighbour);

                if (neighbour == _endCell)
                {
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
                } //handle end path
            }
           
            currentCell = _cellsQueue.Dequeue();
        }
    }

    private void HandleLists(Cell cellToHandle)
    {
        _cellsQueue.Enqueue(cellToHandle);

        if (_openCells.Contains(cellToHandle))
            _openCells.Remove(cellToHandle);

        if (!_visitedCells.Contains(cellToHandle)) //the difference between the open cells?
            _visitedCells.Add(cellToHandle);
    }

    private void CellInspected(Cell activeCell, Cell neighbourCell)
    {
        if (neighbourCell != _startCell && neighbourCell != _endCell)
            neighbourCell.ChangeColor(false);

        if (neighbourCell.ParentCell == null && neighbourCell != _startCell)
            neighbourCell.SetParentCell(activeCell);
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


}
