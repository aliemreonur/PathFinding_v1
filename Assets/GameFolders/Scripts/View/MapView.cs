using UnityEngine;
using System;

public class MapView : SingletonThis<MapView>
{
    #region Fields & Properties
    [SerializeField] private CellView _cellViefPrefab;
    [SerializeField] private byte _mapWidth, _mapHeight;
    public Action<byte, byte> OnMapSet;
    public byte MapWidth => _mapWidth;
    public byte MapHeight => _mapHeight;
    public CellView[,] allCellViews;
    public Map map => _map;

    private Map _map;
    private PrefabLoader _prefabLoader;
    #endregion

    private void Start()
    {
        SetMap();
        SetPrefabLoader();
    }

    private void SetMap()
    {
        _map = new Map(MapWidth, MapHeight);
        allCellViews = new CellView[MapWidth, MapHeight];
    }

    private void SetPrefabLoader()
    {
        _prefabLoader = new PrefabLoader();
        _prefabLoader.OnAssetLoaded += GenerateMap;
    }

    private void OnDisable()
    {
        _map.DeregisterEvents();
        _prefabLoader.OnAssetLoaded -= GenerateMap;
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
