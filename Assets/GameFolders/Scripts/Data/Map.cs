using System;
using UnityEngine;

public class Map 
{
    public Action OnMapLoaded;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Cell[,]AllCells { get; private set; }

    private float _blockedCellRatio;

    public Map(int width, int height)
    {
        Width = width; Height = height;
        AllCells = new Cell[Width, Height];
        SetBlockRatio();
        GenerateMap();
        SetRandomCellsBlocked();
        GameManager.Instance.OnMapRestart += RestartingMap;
    }

    private void SetBlockRatio()
    {
        _blockedCellRatio = GameManager.Instance.BlockRatio;
    }

    private void GenerateMap()
    {
        for(int x=0; x<Width; x++)
        {
            for(int y=0; y<Height; y++)
            {
                Cell newCell = new Cell(this, x, y);
                AllCells[x, y] = newCell;
            }
        }
    }

    private void SetRandomCellsBlocked()
    {
        Debug.Log(_blockedCellRatio);
        int currentBlockedAmount = 0;
        int totalCells = Width * Height;
        int targetBlocked = Mathf.FloorToInt(totalCells * _blockedCellRatio);
        Debug.Log(totalCells + "," + targetBlocked);
        int randomX = 0;
        int randomY = 0;
        int iterations = 0;
        int maxIterations = totalCells * 2;

        do
        {
            randomX = UnityEngine.Random.Range(0, Width); 
            randomY = UnityEngine.Random.Range(0, Height);
            Cell cellToCheck = AllCells[randomX, randomY];

            if (!cellToCheck.IsBlocked)
            {
                cellToCheck.SetBlockedStatus();
                currentBlockedAmount++;
            }
            iterations++;

        }
        while (currentBlockedAmount< targetBlocked && iterations<maxIterations);

        OnMapLoaded?.Invoke();
    }

    public void DeregisterEvents()
    {
        GameManager.Instance.OnMapRestart -= RestartingMap;
    }

    private void RestartingMap()
    {
        foreach(var cell in AllCells)
        {
            cell.SetBlockedStatus(false);
            cell.Reset(true);
        }

        SetRandomCellsBlocked();

    }

}
