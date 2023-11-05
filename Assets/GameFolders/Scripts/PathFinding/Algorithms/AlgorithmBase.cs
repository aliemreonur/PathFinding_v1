using System.Threading.Tasks;

using UnityEngine;

public class AlgorithmBase 
{
    public AlgorithmType algorithmType => _algorithmType;
    public IPathAlgorithm activeAlgo { get; private set; }
    public PathFinder pathFinder { get; private set; } 

    private PathListHandler _pathListHandler;
    private PathHandler _pathHandler;

    public Cell startCell => pathFinder.InterestPointsHandler.StartCell;
    public Cell endCell => pathFinder.InterestPointsHandler.EndCell;

    private AlgorithmType _algorithmType;
    private IPathAlgorithm _searchByNeighbours, _searchByHeuristic, _greedyAlgorithm, _aStarAlgorithm;

    public AlgorithmBase(PathFinder pathFinder)
    {
        this.pathFinder = pathFinder;
        _pathListHandler = new PathListHandler();
        _searchByNeighbours = new SearcherbyNeighbours(this, _pathListHandler);
        _searchByHeuristic = new SearcherbyHeuristic(this, _pathListHandler);
        _aStarAlgorithm = new AstarAlgorithm(this, _pathListHandler);
        _pathHandler = new PathHandler();
    }


    protected void InitializeAlgo()
    {
        _pathListHandler.ClearLists();
        _pathListHandler.SetOpenCellList();
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
                activeAlgo = _searchByHeuristic;
                break;
            default:
                _algorithmType = AlgorithmType.AStar;
                activeAlgo = _aStarAlgorithm;
                break;
        }
        InitializeAlgo();
        activeAlgo.CalculateShortestPath(startCell, endCell);
    }

    public void CreateThePath(Cell lastCell)
    {
         _pathHandler.CreatePath(startCell, lastCell);
    }

}
