using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2Int gridGap;
    [SerializeField] private Tilemap tilemap;

    private bool requiresGeneration = true;
    private Grid grid;

    // Grid Bounds
    public int xMin, xMax, zMin, zMax;

    // Grid tiles
    private List<GameTile> tiles = new List<GameTile>();
    public GameTileData[,] masterTileGrid;


    void Awake()
    {
        // Initialize components
        grid = GetComponent<Grid>();
        tilemap = GetComponentInChildren<Tilemap>();

        
        if(tilemap == null) {Debug.LogError("Tilemap is not attached to the GameGrid."); return;}

        // Put tiles in a list and find the bounds of the grid
        foreach(var tile in tilemap.GetComponentsInChildren<GameTile>())
        {
            tiles.Add(tile);

            // Set horizontal bounds of the grid
            if (tile.transform.position.x > xMax) { xMax = Mathf.CeilToInt(tile.transform.position.x); }
            else if(tile.transform.position.x < xMin) { xMin = Mathf.FloorToInt(tile.transform.position.x); };

            // Set vertical bounds of the grid
            if(tile.transform.position.z > zMax) { zMax = Mathf.CeilToInt(tile.transform.position.z); }
            else if(tile.transform.position.z < zMin) { zMin = Mathf.FloorToInt(tile.transform.position.z); };
        }

        // Initialize grid array
        masterTileGrid = new GameTileData[xMax, zMax];

        // Fill array with tiles
        foreach(var tile in tilemap.GetComponentsInChildren<GameTile>())
        {
            (int indexX, int indexZ) = GetGridIndex(tile.gameObject);
            
            masterTileGrid[indexX, indexZ] = tile.tileData;
            tile.gridIndexX = indexX;
            tile.gridIndexZ = indexZ;
        }

        //PrintGridInConsole();

        // Generate grid
        if(tiles.Count <= 0)
        { Debug.LogError("No tiles found");}
        
        else { GenerateGrid(); }
    }

    // When the something in the editor changes, update the map (Editor Only)
    private void OnValidate() => requiresGeneration = true;

    void GenerateGrid()
    {
        //Debug.Log("Generating Grid...");
        Debug.Log($"Grid Bounds = minBounds: X{xMin},Z{zMin} + maxBounds: X{xMax},Z{zMax}");
    }

    public (int indexX, int indexY) GetGridIndex(GameObject tile)
    {
        return (Mathf.FloorToInt(tile.transform.position.x / grid.cellSize.x), Mathf.FloorToInt(tile.transform.position.z / grid.cellSize.z)) ;
    }

    // Used for debugging the master grid
    void PrintGridInConsole()
    {
        for(int i = 0; i < zMax; i++)
        {
            for (int j = 0; j < xMax; j++)
            {
                Debug.Log(masterTileGrid[j, i].TilePosition);
            }
        }
    }
}
