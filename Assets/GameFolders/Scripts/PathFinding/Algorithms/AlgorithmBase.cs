
public class AlgorithmBase 
{
    protected bool searchActive = false;
    protected Cell currentCell;
    protected PathFinder pathFinder;
    protected PathListHandler pathListHandler;

    protected void InitializeAlgo(Cell startCell)
    {
        searchActive = true;
        currentCell = startCell;
        pathListHandler.ClearLists();
       pathListHandler.SetOpenCellList();
    }
}
