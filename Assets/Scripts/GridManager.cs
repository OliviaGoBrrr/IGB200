using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
//using UnityEngine.WSA;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    public static System.Action FindNeighbours;
    public static System.Action GridLoadingCompleted;

    // References
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameManager gameManager;
    public Grid grid;

    // Grid Bounds
    //[HideInInspector]
    public int xMin, xMax, zMin, zMax;

    // Grid tiles
    public List<GameTile> tileList = new List<GameTile>();
    public GameTileData[,] masterTileGrid;

    // Values when loading grid in
    private int expectedMethods;
    private int completedMethods;


    void Awake()
    {
        // Initialize components
        grid = GetComponent<Grid>();
        tilemap = GetComponentInChildren<Tilemap>();
        if(tilemap == null) {Debug.LogError("Tilemap is not attached to the GameGrid."); return;}
    }

    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
        Vector3 mousePosition = gameManager.GetSelectedGridPosition(true);
        mouseIndicator.transform.position = mousePosition;
    }

    public (int indexX, int indexY) GetGridIndex(GameObject tile)
    {
        return (Mathf.FloorToInt(tile.transform.position.x / grid.cellSize.x), Mathf.FloorToInt(tile.transform.position.z / grid.cellSize.z)) ;
    }

    private void GenerateGrid()
    {
        // Start with a reference for the mins, otherwise it'll always be 0
        xMin = (int)FindFirstObjectByType<GameTile>().transform.position.x;
        zMin = (int)FindFirstObjectByType<GameTile>().transform.position.z;

        // Put tiles in a list and find the bounds of the grid
        foreach (var tile in tilemap.GetComponentsInChildren<GameTile>())
        {
            tileList.Add(tile);
            expectedMethods++;

            // Set horizontal bounds of the grid
            if (tile.transform.position.x > xMax) { xMax = Mathf.CeilToInt(tile.transform.position.x); }
            if ((int)tile.transform.position.x < xMin) { xMin = Mathf.FloorToInt(tile.transform.position.x); }
            

            // Set vertical bounds of the grid
            if (tile.transform.position.z > zMax) { zMax = Mathf.CeilToInt(tile.transform.position.z); }
            if ((int)tile.transform.position.z < zMin) { zMin = Mathf.FloorToInt(tile.transform.position.z); }
            
        }

        // Initialize grid array
        masterTileGrid = new GameTileData[xMax, zMax];

        // Fill array with tiles
        foreach (var tile in tilemap.GetComponentsInChildren<GameTile>())
        {
            (int indexX, int indexZ) = GetGridIndex(tile.gameObject);

            masterTileGrid[indexX, indexZ] = tile.GetTileData();
            tile.gridIndexX = indexX;
            tile.gridIndexZ = indexZ;
        }

        FindNeighbours?.Invoke();
    }

    // Check to see if the grid is completed
    public void CompleteGridGeneration()
    {
        completedMethods++;
        if(completedMethods == expectedMethods)
        {
            GridLoadingCompleted?.Invoke();
        }
    }

    public float GetPercentOfTileInGrid(GameTile.TileStates tileForPercent)
    {
        float percent = 0f;
        int tileCount = 0;

        if(tileList.Count == 0)
        {
            Debug.LogError("Error - Trying to get percent of tiles in a grid, but there are no tiles in the grid manager");
            return percent;
        }

        foreach(var tile in tileList)
        {
            if(tile.tileState == tileForPercent)
            {
                tileCount++;
            }
        }

        percent = (float)tileCount / tileList.Count;

        return percent * 100;
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
