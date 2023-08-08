using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public int width = 300;
    public int height = 300;
    public GameObject cornerTilePrefab;
    public GameObject openTilePrefab;
    public GameObject straightTilePrefab;
    public GameObject singleTilePrefab;
    public GameObject endTilePrefab;

    private Vector3[,] grid;

    private void Start()
    {
        // Generate the grid
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new Vector3[width, height];

        // Generate the white lines for the grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                grid[x, y] = position;
                // TODO: Draw white lines on the canvas using this position
            }
        }
    }

    private void Update()
    {
        // Check for mouse click to place tiles
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            int x = Mathf.FloorToInt(worldPosition.x);
            int y = Mathf.FloorToInt(worldPosition.y);

            // Place a random tile at the clicked position
            PlaceRandomTile(x, y);
        }
    }

    private void PlaceRandomTile(int x, int y)
    {
        int randomTileType = Random.Range(0, 5); // 0, 1, 2, 3, or 4

        switch (randomTileType)
        {
            case 0:
                Instantiate(cornerTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                break;
            case 1:
                Instantiate(openTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                break;
            case 2:
                Instantiate(straightTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                break;
            case 3:
                Instantiate(singleTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                break;
            case 4:
                Instantiate(endTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                break;
        }
    }
}
