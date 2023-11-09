using UnityEngine;

public class InterestPointsHandler 
{
    #region Fields & Properties
    public Cell StartCell => _startCell;
    public Cell EndCell => _endCell;

    private Map _map;
    private Cell _startCell;
    private Cell _endCell;
    #endregion

    public InterestPointsHandler(Map map)
    {
        _map = map;
        AssignInterestPoints();
        GameManager.Instance.OnMapRestart += AssignInterestPoints;
    }

    #region Methods
    public void DeregisterEvents()
    {
        GameManager.Instance.OnMapRestart -= AssignInterestPoints;
    }

    private Cell AssignRandomPoint(bool isStart)
    {
        int randomX = 0;
        int randomY = 0;

        int iterations = 0;
        int maxIterations = 500;
        bool success = false;
        Cell randomCell = null;

        while (iterations < maxIterations && !success)
        {
            randomX = Random.Range(0, _map.Width);
            randomY = Random.Range(0, _map.Height);

            randomCell = _map.AllCells[randomX, randomY];

            if(!isStart)
            {
                //check if the cell is close
                int distanceToStart = DistanceCalculator.CalculateCellCost(_startCell, randomCell);
                if (distanceToStart < 30)
                    continue;
            }

            if (!randomCell.IsBlocked)
            {
                randomCell.AssignedAsInterestPoint(isStart);
                success = true;
            }
            else
                randomCell = null;
            iterations++;
        }

        return randomCell;
    }

    private void AssignInterestPoints()
    {
        _startCell = AssignRandomPoint(true);
        _endCell = AssignRandomPoint(false);
    }
    #endregion

}
