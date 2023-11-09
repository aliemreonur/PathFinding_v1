using System.Collections.Generic;

public class PathListHandler 
{
    #region Fields & Properties
    public List<Cell> openCells => _openCells;
    public List<Cell> closedCells => _closedCells;

    private List<Cell> _openCells;
    private List<Cell> _closedCells;
    private MapView _mapView;
    #endregion

    #region Const
    public PathListHandler()
    {
        _mapView = MapView.Instance;
        _openCells = new List<Cell>();
        _closedCells = new List<Cell>();
    }
    #endregion

    #region Methods
    public void SetOpenCellList()
    {
        foreach (var cell in _mapView.map.AllCells)
        {
            if (!cell.IsBlocked)
                _openCells.Add(cell);
        }
    }

    public void ClearLists()
    {
        _openCells.Clear();
        _closedCells.Clear();
    }

    public void CellVisited(Cell visitedCell)
    {
        if(_openCells.Contains(visitedCell))
            _openCells.Remove(visitedCell);

        if(!_closedCells.Contains(visitedCell))
            _closedCells.Add(visitedCell);
    }
    #endregion
}
