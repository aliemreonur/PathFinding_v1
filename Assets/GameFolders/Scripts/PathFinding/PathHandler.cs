using UnityEngine;

public class PathHandler 
{
    public async Awaitable CreatePath(Cell startCell, Cell lastCell)
    {
        Cell cell = lastCell.ParentCell;

        cell.ChangeColor(true);
        cell = cell.ParentCell;
        if (cell != null)
        {
            while (cell != startCell)
            {
                await Awaitable.WaitForSecondsAsync(.15f);
                cell.ChangeColor(true);
                cell = cell.ParentCell;
            }
        }
        return;
    }
}
