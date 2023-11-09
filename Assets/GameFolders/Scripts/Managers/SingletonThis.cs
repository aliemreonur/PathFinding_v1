using UnityEngine;

public class SingletonThis<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance => _instance;
    private static T _instance;

    protected void Awake()
    {
        _instance = this as T;
    }
}
