using UnityEngine;

public class MinesweeperGrid : MonoBehaviour
{
    private const float CellGap = 0.1f;
    
    public GameObject gridCellPrefab;
    
    public int width = 10;
    public int height = 10;
    public int bombCount = 10;

    private Cell[,] _grid;

    private void Start()
    {
        GenerateGrid();
        
        // move the grid to the center of the screen in X direction
        transform.position = new Vector3(-width / 2f, 0, 0);
    }
    
    private void GenerateGrid()
    {
        _grid = new Cell[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newCell = Instantiate(gridCellPrefab, new Vector3(x + x * CellGap, y + y * CellGap, 0),
                    Quaternion.identity);
                newCell.transform.SetParent(transform);
                
                Cell cellComponent = newCell.GetComponent<Cell>();
                _grid[x, y] = cellComponent;
            }
        }

        PlaceBombs();
    }

    private void PlaceBombs()
    {
        int bombsPlaced = 0;
        while (bombsPlaced < bombCount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            
            Cell randomCell = _grid[x, y];
            if (!randomCell.isBomb)
            {
                randomCell.isBomb = true;
                bombsPlaced++;
            }
        }
        
        CalculateAdjacentBombCounts();
    }

    private void CalculateAdjacentBombCounts()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell currentCell = _grid[x, y];
                if (!currentCell.isBomb)
                {
                    currentCell.adjacentBombCount = GetAdjacentBombCount(x, y);
                }
            }
        }
    }

    private int GetAdjacentBombCount(int x, int y)
    {
        int adjacentBombCount = 0;
        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                int neighbourX = x + xOffset;
                int neighbourY = y + yOffset;
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    Cell neighbourCell = _grid[neighbourX, neighbourY];
                    if (neighbourCell.isBomb)
                    {
                        adjacentBombCount++;
                    }
                }
            }
        }
        return adjacentBombCount;
    }
}
