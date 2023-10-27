using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private TextMeshPro _costText;
    private Color _defaultColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
            Debug.LogError("The cell could not gather its mat");
        _costText = GetComponentInChildren<TextMeshPro>();
        if (_costText == null)
            Debug.Log("The cell could not gather its TMPro");
        SetDefaultColor(Color.white);
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
        _spriteRenderer.color = Color.green;
      
    }

    public void BlockedCell()
    {
        _spriteRenderer.color = Color.black;
        _defaultColor = Color.black;
    }

    public void Reset()
    {
        _spriteRenderer.color = _defaultColor;
        _costText.SetText("");
    }

    public void CellCostUpdated(int amount)
    {
        _costText.SetText(amount.ToString());
    }

    private void SetDefaultColor(Color colorToSet)
    {
        _defaultColor = colorToSet;
    }
}
