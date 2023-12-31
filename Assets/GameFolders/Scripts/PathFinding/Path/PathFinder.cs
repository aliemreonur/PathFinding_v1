using UnityEngine;

[RequireComponent(typeof(MapView))]
public class PathFinder : MonoBehaviour
{
    #region Fields & Properties
    public InterestPointsHandler InterestPointsHandler => _interestPointsHandler;
    private InterestPointsHandler _interestPointsHandler;
    private MapView _mapView;
    private AlgorithmHandler _algorithmBase;
    #endregion

    #region MonoMethods
    private void Awake()
    {
        _mapView = GetComponent<MapView>();
        _algorithmBase = new AlgorithmHandler(this);
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
    #endregion

    #region Public Methods
    public void Reset()
    {
        foreach (var cell in _mapView.map.AllCells)
            cell.Reset();
    }

    public void SetAlgorithm(int algoID)
    {
        _algorithmBase.SetAlgorithm(algoID); 
    }

    public void CellInspected(Cell activeCell, Cell neighbourCell)
    {
        if (neighbourCell != _interestPointsHandler.StartCell && neighbourCell != _interestPointsHandler.EndCell)
            neighbourCell.ChangeColor(false);

        if (neighbourCell.ParentCell == null && neighbourCell != _interestPointsHandler.StartCell)
            neighbourCell.SetParentCell(activeCell);
    }
    #endregion

    private void SetInterestPoints(byte dummy, byte dummy2) // :)
    {
        _interestPointsHandler = new InterestPointsHandler(_mapView.map);
    }
}
