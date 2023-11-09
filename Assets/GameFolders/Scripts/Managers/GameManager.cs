using System;
using UnityEngine;

public class GameManager : SingletonThis<GameManager>
{
    public Action OnMapRestart;
    [Range(0f, 0.5f)] public float BlockRatio;

    public void RestartMap()
    {
        OnMapRestart?.Invoke(); 
    }
}
