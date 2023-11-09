using UnityEngine;
using System;

public class MapView : SingletonThis<MapView>
{
    [SerializeField] private CellView _cellViefPrefab;
    public Action<byte, byte> OnMapSet;
    private byte MapWidth => _mapWidth;
    private byte MapHeight => _mapHeight;
    [SerializeField] private byte _mapWidth, _mapHeight;

    public CellView[,] allCellViews;
    public Map map => _map;

    private Map _map;

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
