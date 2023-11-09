using UnityEngine;

public class AlgorithmHandler 
{
    #region Fields & Properties
    public int maxIterations = MapView.Instance.MapHeight * MapView.Instance.MapWidth * 2;
    public AlgorithmType algorithmType => _algorithmType;
    public IPathAlgorithm activeAlgo { get; private set; }
    public PathFinder pathFinder { get; private set; } 

    private PathListHandler _pathListHandler;
    private PathHandler _pathHandler;

    public Cell startCell => pathFinder.InterestPointsHandler.StartCell;
    public Cell endCell => pathFinder.InterestPointsHandler.EndCell;

    private AlgorithmType _algorithmType;
    private IPathAlgorithm _searchByNeighbours, _greedyAlgorithm, _aStarAlgorithm;
    private float _time;
    #endregion

    public AlgorithmHandler(PathFinder pathFinder)
    {
        this.pathFinder = pathFinder;
        _pathListHandler = new PathListHandler();
        _searchByNeighbours = new SearcherbyNeighbours(this, _pathListHandler);
        _greedyAlgorithm = new Greedy(this, _pathListHandler);
        _aStarAlgorithm = new Astar(this, _pathListHandler);
        _pathHandler = new PathHandler();
    }


    public void SetAlgorithm(int algoID)
    {
        switch (algoID)
        {
            case 1:
                _algorithmType = AlgorithmType.Bridths;
                activeAlgo = _searchByNeighbours;
                break;
            case 2:
                _algorithmType = AlgorithmType.Dijkstra;
                activeAlgo = _searchByNeighbours;
                break;
            case 3:
                _algorithmType = AlgorithmType.Greedy;
                activeAlgo = _greedyAlgorithm;
                break;
            default:
                _algorithmType = AlgorithmType.AStar;
                activeAlgo = _aStarAlgorithm;
                break;
        }
        ResetLists();
        activeAlgo.CalculateShortestPath(startCell, endCell);
    }

    public void CreateThePath(Cell lastCell)
    {
        var totalTime = Time.time - _time;
        Debug.Log($"The algorithm took {totalTime} seconds to complete using the {_algorithmType}");
         _pathHandler.CreatePath(startCell, lastCell);
    }

    protected void ResetLists()
    {
        pathFinder.Reset();
        _pathListHandler.ClearLists();
        _pathListHandler.SetOpenCellList();
        _time = Time.time;
    }

}
