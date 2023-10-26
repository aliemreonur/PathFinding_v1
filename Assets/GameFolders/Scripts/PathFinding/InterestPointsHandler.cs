using UnityEngine;

public class InterestPointsHandler 
{
    public Cell StartCell => _startCell;
    public Cell EndCell => _endCell;

    private Map _map;
    private Cell _startCell;
    private Cell _endCell;

    public InterestPointsHandler(Map map)
    {
        _map = map;
        _startCell = AssignRandomPoint(true);
        _endCell = AssignRandomPoint(false);
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

}
