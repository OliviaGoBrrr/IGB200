using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2Int gridGap;
    [SerializeField] private Tilemap tilemap;

    private bool requiresGeneration = true;
    //private Camera playerCam;
    private Grid grid;

    // Grid Bounds
    private int xMin;
    private int xMax;
    private int zMin;
    private int zMax;

    private List<GameTile> tiles = new List<GameTile>();
    private GameTileData[,] masterTileGrid;

    private Vector2 currentGap;
    private Vector2 gapVel;

    void Awake()
    {
        grid = GetComponent<Grid>();
        tilemap = GetComponentInChildren<Tilemap>();
        //playerCam = Camera.main;
        currentGap = gridGap;

        foreach(var tile in tilemap.GetComponentsInChildren<GameTile>())
        {
            Debug.Log("A tile has been added to the tile list");
            tiles.Add(tile);

            // Set horizontal bounds of the grid
            if (tile.transform.position.x > xMax) { xMax = (int)tile.transform.position.x; }
            else if(tile.transform.position.x < xMin) { xMin = (int)tile.transform.position.x; };

            // Set vertical bounds of the grid
            if(tile.transform.position.z > zMax) { zMax = (int)tile.transform.position.z;}
            else if(tile.transform.position.z < zMin) { zMin = (int)tile.transform.position.z; };
        }

        if(tiles.Count <= 0)
        {
            Debug.LogError("No tiles found");
        }
        else
        {
            GenerateGrid();
        }
    }

    // When the something in the editor changes, update the map (Editor Only)
    private void OnValidate() => requiresGeneration = true;

    void GenerateGrid()
    {
        Debug.Log($"Grid Bounds = minBounds: X{xMin},Z{zMin} + maxBounds: X{xMax},Z{zMax}");
        Debug.Log("Generating Grid...");
        
        //var coordinates = new List<Vector3Int>();
    }
}
