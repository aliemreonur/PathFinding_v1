using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapView : MonoBehaviour
{
    public static MapView Instance;
    [SerializeField] private CellView _cellViefPrefab;
    public CellView[,] allCellViews;
    public Map map => _map;

    public int MapWidth;
    public int MapHeight;

    private static MapView _instance;
    private Map _map;

    private void Awake()
    {
        SingletonThis();
        _map = new Map(MapWidth, MapHeight);
        allCellViews = new CellView[MapWidth, MapHeight];

        GenerateMap();
    }

    private void SingletonThis()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
        else if (this != _instance)
        {
            Destroy(this.gameObject);
        }
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
                    instantiatedCellView.BlockedCell();

                //check if the cell is blocked
            }
        }
    }

}
