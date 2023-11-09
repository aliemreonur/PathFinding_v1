using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class PrefabLoader
{
    #region Fields & Properties
    public Action OnAssetLoaded;
    public CellView objPrefab => _objPrefab;
    private AsyncOperationHandle<GameObject> _prefabLoader;
    private CellView _objPrefab;
    #endregion

    public PrefabLoader()
    {
        LoadAsset();
    }

    private void LoadAsset()
    {
        _prefabLoader = Addressables.LoadAssetAsync<GameObject>("Prefabs/CellView");
        _prefabLoader.Completed += OnPrefabLoaded;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            _objPrefab = obj.Result.GetComponent<CellView>();
            if (_objPrefab == null)
                Debug.LogError("Could not gather the Cellveiw");
            OnAssetLoaded?.Invoke();
        }
        else
            Debug.LogError("Failed to load the Cellview Asset");
    }
}
