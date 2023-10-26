using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private TextMeshPro _costText;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
            Debug.LogError("The cell could not gather its mat");
        _costText = GetComponentInChildren<TextMeshPro>();
        if (_costText == null)
            Debug.Log("The cell could not gather its TMPro");
    }

    public void ChangeInterestPointColor(bool isStart)
    {
        _spriteRenderer.color = isStart ? Color.green : Color.red;
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
    }

    public void CellCostUpdated(int amount)
    {
        _costText.SetText(amount.ToString());
    }
}
