using UnityEngine;

public class MinesweeperGrid : MonoBehaviour
{
    public GameObject gridCellPrefab;
    
    public int width = 10;
    public int height = 10;
    public float cellGap = 0.1f;
    
    public int bombCount = 10;

    private GameObject[,] _grid;

    private void Start()
    {
        GenerateGrid();
        
        // move the grid to the center of the screen in X direction
        transform.position = new Vector3(-width / 2f, 0, 0);
    }
    
    private void GenerateGrid()
    {
        _grid = new GameObject[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newCell = Instantiate(gridCellPrefab, new Vector3(x + x * cellGap, y + y * cellGap, 0),
                    Quaternion.identity);
                newCell.transform.SetParent(transform);
                _grid[x, y] = newCell;
            }
        }
    }
}
