using System.Collections;
using System.Collections.Generic;
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
        _startColor = _spriteRenderer.material.color;
    }

    public void ChangeInterestPointColor(bool isStart)
    {
        _spriteRenderer.color = isStart ? Color.green : Color.red;
        _defaultColor = _spriteRenderer.color;
    }

    public void CellVisited()
    {
        _spriteRenderer.color = Color.gray;
    }

    public void CellOnPath()
    {
        Debug.Log("On path");
        _spriteRenderer.color = Color.cyan;
      
    }

    public void BlockedCell(bool isBlocked)
    {
        _spriteRenderer.color = isBlocked ? Color.black : _startColor;
        _defaultColor = isBlocked ? Color.black : _startColor;
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

}
