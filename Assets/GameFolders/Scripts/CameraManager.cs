using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    #region Fields&Properties
    private byte _width, _height;
    private Camera _camera;

    private float _aspectRatio;
    private float _targetRatio;
    private float _multiplier;
    #endregion

    #region MonoMethods
    private void Awake()
    {
        _camera = Camera.main;
        if (_camera == null)
            Debug.LogError("The camera manager could not gather the camera");
    }

    private void Start()
    {
        _aspectRatio = (float)Screen.width / (float)Screen.height;
        _multiplier = Mathf.Max(Screen.width / 1080, Screen.height / 1920);
        MapView.Instance.OnMapSet += ArrangeDimensions;
    }

    private void OnDisable()
    {
        MapView.Instance.OnMapSet -= ArrangeDimensions;
    }
    #endregion

    #region Private Methods
    private void ArrangeDimensions(byte width, byte height)
    {
        _width = width;
        _height = height;
        _targetRatio = (float)_width / (float)_height;
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        SetOrthographicSize();
        float xPos;
        float yPos;

        xPos = (_width / 2) - 0.5f;
        yPos = _height / 2;
        _camera.transform.position = new Vector3(xPos, yPos, -1);
    }

    private void SetOrthographicSize()
    {
        if (_aspectRatio >= _targetRatio)
        {
            _camera.orthographicSize = Mathf.RoundToInt((_height / 2) + 4); 
        }
        else
        {
            float sizeDifference = _targetRatio / _aspectRatio;
            _camera.orthographicSize = Mathf.RoundToInt(_height / 2 * sizeDifference) +4; //normal mode : +2?
        }
    }
    #endregion
}