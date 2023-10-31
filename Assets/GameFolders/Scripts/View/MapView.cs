using UnityEngine;
using System;

public class MapView : MonoBehaviour
{
    public Action<byte, byte> OnMapSet;
    [SerializeField] private CellView _cellViefPrefab;
    public static MapView Instance => _instance;
    public CellView[,] allCellViews;
    public Map map => _map;

    public byte MapWidth;
    public byte MapHeight;

    private static MapView _instance;
    private Map _map;

    private void Awake()
    {
        _instance = this;

      
    }

    private void Start()
    {
        _map = new Map(MapWidth, MapHeight);
        allCellViews = new CellView[MapWidth, MapHeight];
        GenerateMap();
    }

    private void OnDisable()
    {
        _map.DeregisterEvents();
    }

    private void GenerateMap()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                Vector2 spawnPos = new Vector2(x, y);
                CellView instantiatedCellView = Instantiate(_cellViefPrefab, spawnPos, Quaternion.identity, transform);
                allCellViews[x, y] = instantiatedCellView;

                Cell associatedCell = _map.AllCells[x,y];
                associatedCell.AssignCellView(instantiatedCellView);
                if (associatedCell.IsBlocked)
                    instantiatedCellView.BlockedCell(true);
            }
        }
        OnMapSet?.Invoke(MapWidth, MapHeight);
    }

}
