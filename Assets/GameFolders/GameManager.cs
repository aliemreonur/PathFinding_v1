using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action OnMapRestart;
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void RestartMap()
    {
        OnMapRestart?.Invoke(); //set random cells blocked
        //foreach cell in 
    }
}
