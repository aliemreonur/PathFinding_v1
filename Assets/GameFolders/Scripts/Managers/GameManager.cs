using System;

public class GameManager : SingletonThis<GameManager>
{
    public Action OnMapRestart;

    public void RestartMap()
    {
        OnMapRestart?.Invoke(); 
    }
}
