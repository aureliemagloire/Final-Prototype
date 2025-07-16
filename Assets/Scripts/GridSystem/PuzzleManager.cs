using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridContainer;
    public GameObject puzzlePanel;
    private bool puzzleActive;

    public int gridSize = 5;
    public Color[] tileColors;

    private PuzzleTile[,] tiles;
    private PuzzleTile selectedTile;

    void Update()
    {
        if (!puzzleActive) return;
    }

    public void StartPuzzle()
    {
        Time.timeScale = 0f;
        puzzlePanel.SetActive(true);
        puzzleActive = true;
        selectedTile = null;

        GenerateGrid();
    }

    void GenerateGrid()
    {
        foreach (Transform child in gridContainer) Destroy(child.gameObject);

        tiles = new PuzzleTile[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject obj = Instantiate(tilePrefab, gridContainer);
                PuzzleTile tile = obj.GetComponent<PuzzleTile>();

                int colorIndex = Random.Range(0, tileColors.Length);
                tile.Init(this, x, y, tileColors[colorIndex], colorIndex);

                tiles[x, y] = tile;
            }
        }
    }

    public void OnTileClicked(PuzzleTile tile)
    {
        if (selectedTile == null)
        {
            selectedTile = tile;
            tile.Highlight(true);
        }
        else
        {
            if (IsNeighbor(selectedTile, tile))
            {
                if (selectedTile.colorIndex >= 0 && tile.colorIndex >= 0)
                {
                    SwapTiles(selectedTile, tile);
                    ClearMatches();
                }
            }

            selectedTile.Highlight(false);
            selectedTile = null;
        }
    }

    bool IsNeighbor(PuzzleTile a, PuzzleTile b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx + dy) == 1;
    }

    void SwapTiles(PuzzleTile a, PuzzleTile b)
    {
        if (a == null || b == null || a.colorIndex < 0 || b.colorIndex < 0)
        {
            Debug.LogWarning("Invalid tile swap attempt.");
            return;
        }

        int tempIndex = a.colorIndex;
        a.colorIndex = b.colorIndex;
        b.colorIndex = tempIndex;

        a.image.color = GetColor(a.colorIndex);
        b.image.color = GetColor(b.colorIndex);
    }

    void ClearMatches()
{
    List<PuzzleTile> toClear = new List<PuzzleTile>();

    // Horizontal matches
    for (int y = 0; y < gridSize; y++)
    {
        int streak = 1;
        for (int x = 1; x <= gridSize; x++)
        {
            bool isMatch = (x < gridSize &&
                tiles[x, y].colorIndex == tiles[x - 1, y].colorIndex &&
                tiles[x, y].colorIndex != -1);

            if (isMatch)
            {
                streak++;
            }
            else
            {
                if (streak >= 3)
                {
                    for (int i = 0; i < streak; i++)
                    {
                        toClear.Add(tiles[x - 1 - i, y]);
                    }
                }
                streak = 1;
            }
        }
    }

    // Vertical matches
    for (int x = 0; x < gridSize; x++)
    {
        int streak = 1;
        for (int y = 1; y <= gridSize; y++)
        {
            bool isMatch = (y < gridSize &&
                tiles[x, y].colorIndex == tiles[x, y - 1].colorIndex &&
                tiles[x, y].colorIndex != -1);

            if (isMatch)
            {
                streak++;
            }
            else
            {
                if (streak >= 3)
                {
                    for (int i = 0; i < streak; i++)
                    {
                        toClear.Add(tiles[x, y - 1 - i]);
                    }
                }
                streak = 1;
            }
        }
    }

    // Clear matched tiles
    foreach (var tile in toClear)
    {
        tile.Clear();
    }

    if (toClear.Count >= 3)
    {
        Debug.Log("🎉 Cleared " + toClear.Count + " tile(s)!");
        RefillGrid();

        // Chain reaction: check again after refill
        Invoke(nameof(ClearMatches), 0.1f);
    }
    else
    {
        Debug.Log("🔄 No more matches.");
    }
}


    void RefillGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (tiles[x, y].colorIndex == -1)
                {
                    // Look above for a non-empty tile
                    for (int k = y + 1; k < gridSize; k++)
                    {
                        if (tiles[x, k].colorIndex != -1)
                        {
                            tiles[x, y].colorIndex = tiles[x, k].colorIndex;
                            tiles[x, y].image.color = GetColor(tiles[x, y].colorIndex);

                            tiles[x, k].colorIndex = -1;
                            tiles[x, k].image.color = Color.clear;
                            break;
                        }
                    }

                    // If still empty, spawn new tile
                    if (tiles[x, y].colorIndex == -1)
                    {
                        int newColor = Random.Range(0, tileColors.Length);
                        tiles[x, y].colorIndex = newColor;
                        tiles[x, y].image.color = GetColor(newColor);
                    }
                }
            }
        }
    }

    public void CompletePuzzle()
    {
        EndPuzzle(true);
    }

    void EndPuzzle(bool success)
    {
        puzzleActive = false;
        puzzlePanel.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log(success ? "✅ Puzzle solved!" : "❌ Puzzle failed.");
    }

    public Color GetColor(int index) => tileColors[index];
    public bool IsActive() => puzzleActive;
}

