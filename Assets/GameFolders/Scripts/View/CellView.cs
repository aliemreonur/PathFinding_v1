using UnityEngine;
using TMPro;

public class CellView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshPro _fText;
    [SerializeField] private TextMeshPro _gText;
    [SerializeField] private TextMeshPro _hText;
    private Color _defaultColor;
    private Color _startColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
            Debug.LogError("The cell could not gather its mat");
        _startColor = _spriteRenderer.color;
        SetDefaultColor();
    }

    public void ChangeInterestPointColor(bool isStart)
    {
        _spriteRenderer.color = isStart ? Color.green : Color.red;
        SetDefaultColor();
    }

    public void CellVisited()
    {
        Flash();
    }

    public void CellOnPath()
    {
        ChangeColor(Color.cyan); 
    }

    public void BlockedCell(bool isBlocked)
    {
        _spriteRenderer.color = isBlocked ? Color.black : _startColor;
        _gText.SetText("");
        _hText.SetText("");
        _fText.SetText("");
        SetDefaultColor();
    }

    public void Reset(bool isNewMap = false)
    {
        _defaultColor = isNewMap? _startColor : _defaultColor;
        _spriteRenderer.color = isNewMap ? _startColor : _defaultColor;
        _fText.SetText("");
    }

    public void CellCostUpdated(int gCost, int hCost, int totalCost)
    {
        _gText.SetText(gCost > 10000 ? "" : gCost.ToString());
        _hText.SetText(hCost > 10000 ? "" : hCost.ToString());
        _fText.SetText(totalCost > 10000 ? "" : totalCost.ToString());
    }

    private void SetDefaultColor()
    {
        _defaultColor = _spriteRenderer.color;
    }

    private async void Flash()
    {
        ChangeColor(Color.yellow);
        await Awaitable.WaitForSecondsAsync(0.01f);
        ChangeColor(Color.gray);
    }

    private void ChangeColor(Color color)
    {
        _spriteRenderer.color = color;
    }

}
