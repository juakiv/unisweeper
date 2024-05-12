using TMPro;
using UnityEngine;

public class MinesweeperGrid : MonoBehaviour
{
    private const float CellGap = 0.1f;
    
    public GameObject gridCellPrefab;
    
    public int width = 10;
    public int height = 10;
    public int bombCount = 10;

    private Cell[,] _grid;

    public GameObject playTimeText;
    public GameObject flagsLeftText;
    public GameObject backgroundRect;
    public GameObject topTexts;
    
    private TMP_Text _textMeshPro;
    private TMP_Text _flagsLeftTextMeshPro;
    private int _playTime;

    private void Start()
    {
        _textMeshPro = playTimeText.GetComponent<TMP_Text>();
        _flagsLeftTextMeshPro = flagsLeftText.GetComponent<TMP_Text>();
        topTexts.SetActive(false);
        
        _playTime = 0;
        
        backgroundRect.transform.localScale = new Vector3(width + 4, height + 6, 1);
        
        var position = backgroundRect.transform.position;
        position = new Vector3(position.x, (height + 3) / 2f, position.z);
        backgroundRect.transform.position = position;
        backgroundRect.GetComponentInChildren<MeshRenderer>().material.color = Color.gray;
        
        // // move the grid to the center of the screen in X direction
        // transform.position = new Vector3(-width / 2f, 0.5f, 9);
        //
        //
        // playTimeText.transform.position = new Vector3(0, height + 2.5f, 8.5f);
    }

    public void StartGame()
    {
        topTexts.SetActive(true);
        _textMeshPro.text = "Time: 0 s";
        _flagsLeftTextMeshPro.text = "Flags: " + bombCount;
        _playTime = 0;
        
        GenerateGrid();
        
        // move the grid to the center of the screen in X direction
        transform.position = new Vector3(-width / 2f, 0.5f, 9);
        backgroundRect.transform.localScale = new Vector3(width + 4, height + 5, 1);
        
        var position = backgroundRect.transform.position;
        position = new Vector3(position.x, (height + 2.5f) / 2f, position.z);
        backgroundRect.transform.position = position;
        backgroundRect.GetComponentInChildren<MeshRenderer>().material.color = Color.gray;

        topTexts.transform.position = new Vector3(0, height + 2f, 8.5f);
        
        playTimeText.GetComponent<RectTransform>().sizeDelta = new Vector2(width, playTimeText.GetComponent<RectTransform>().sizeDelta.y);
        flagsLeftText.GetComponent<RectTransform>().sizeDelta = new Vector2(width, flagsLeftText.GetComponent<RectTransform>().sizeDelta.y);
        
        // start counting play time in seconds
        InvokeRepeating(nameof(IncrementPlayTime), 1, 1);
    }
    
    private void IncrementPlayTime()
    {
        _playTime++;
        _textMeshPro.text = "Time: " + _playTime.ToString() + " s";
    }

    public int GetFlagsLeft()
    {
        int flaggedCells = 0;
        foreach (Cell cell in _grid)
        {
            if (cell.isFlagged)
            {
                flaggedCells++;
            }
        }
        
        return bombCount - flaggedCells;
    }

    public void UpdateFlagsLeft()
    {
        _flagsLeftTextMeshPro.text = "Flags: " + GetFlagsLeft();
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
                cellComponent.x = x;
                cellComponent.y = y;
                cellComponent.grid = this;
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

    public void RevealAdjacentCells(int x, int y)
    {
        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                int neighbourX = x + xOffset;
                int neighbourY = y + yOffset;
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    Cell neighbourCell = _grid[neighbourX, neighbourY];
                    if (!neighbourCell.isRevealed)
                    {
                        neighbourCell.Reveal();
                    }
                }
            }
        }
    }

    public void RevealAllBombs()
    {
        foreach (Cell cell in _grid)
        {
            if (cell.isBomb && !cell.isRevealed)
            {
                cell.Reveal();
            }
        }
    }

    public bool IsWinner()
    {
        return false;
    }
}
