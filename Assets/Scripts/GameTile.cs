using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameTile;
using System;
public struct GameTileData
{
    public GameTileData(GameTile gameTile, int burnRounds, TileStates tileState, Vector3 tilePosition, bool canBeBurnt)
    {
        this.TileState = tileState;
        this.RoundsToBurn = burnRounds;
        this.TilePosition = tilePosition;
        this.GameTile = gameTile;
        this.CanBeBurnt = canBeBurnt;
    }

    public GameTile GameTile;
    public int RoundsToBurn;
    public TileStates TileState;
    public Vector3 TilePosition;
    public bool CanBeBurnt;
}

public class GameTile: MonoBehaviour
{
    public enum TileStates
    {
        GRASS = 0,
        BURNING,
        BURNT,
        ANIMAL,
        DITCH
    }

    // References
    private GridManager gridManager;
    private GameManager gameManager;
    [SerializeField] public GameObject highlight;

    // Tile Properties
    public GameTileData tileData;
    [SerializeField] private bool canBeBurnt;
    [SerializeField] private int roundsToBurn;
    [SerializeField] private int wetness; // Not implemented yet
    public TileStates tileState;
    [SerializeField] private Material[] tileMaterials;
    public int gridIndexX, gridIndexZ;
    private Vector3 tilePosition;

    // Neighbours
    private List<GameTile> tileNeighbours = new List<GameTile>();
    
    // Neighbour bools (currently unused)
    [SerializeField] 
    private bool
        TL, T, TR,
        L,      R,
        BL, B, BR;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gridManager = GetComponentInParent<GridManager>();
        tilePosition = transform.position;
        tileData = new GameTileData(this, roundsToBurn, tileState, tilePosition, canBeBurnt);
    }

    

    void Start()
    {
        TileStateUpdate();
        tileNeighbours = GetNeighbours();
    }

    private void OnValidate()
    {
        TileStateUpdate();
    }

    public void RoundAdvanced()
    {
        if(tileState == TileStates.BURNING)
        {
            roundsToBurn -= 1;
            AttemptFireSpread();
            if (roundsToBurn < 0)
            {
                tileState = TileStates.BURNT;
                TileStateUpdate();
            }
        }
    }

    public void AttemptFireSpread()
    {
        // THIS IS BAD LOGIC I KNOW IM SORRY

        bool tileFound = false;

        // Checks all the neighbours. If a neighbour can be spread to, set true
        foreach (var tile in tileNeighbours)
        {
            if(tile.canBeBurnt) { tileFound = true; break; }
        }

        // If no tile is found, don't spread
        if (!tileFound) { return;}

        // Will eventually find a valid neighbour to spread to
        while (tileFound)
        {
            int choice = UnityEngine.Random.Range(0, tileNeighbours.Count);

            if (tileNeighbours[choice].canBeBurnt)
            {
                tileFound = false;
                if(tileNeighbours[choice].tileState == TileStates.ANIMAL)
                {
                    gameManager.SetGameState(GameManager.GameState.GAME_OVER);
                }

                tileNeighbours[choice].tileState = TileStates.BURNING;
                tileNeighbours[choice].TileStateUpdate();
            }
        }
    }

    // Function to be called when a GameTile's state gets updated
    public void TileStateUpdate() 
    { 
        // Change the material to the appropriate material
        gameObject.GetComponent<Renderer>().material = tileMaterials[(int)tileState];

        // Change if the tile can be burnt or not
        if (tileState == TileStates.BURNING || 
            tileState == TileStates.BURNT || 
            tileState == TileStates.DITCH)
        {
            canBeBurnt = false;
        }
        else
        {
            canBeBurnt = true;
        }
    }

    private void OnEnable()
    {
        GameManager.OnRoundAdvanced += RoundAdvanced;
    }

    private void OnDisable()
    {
        GameManager.OnRoundAdvanced -= RoundAdvanced;
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    public List<GameTile> GetNeighbours()
    {
        var neighbourResults = new List<GameTile>();

        // Left
        if (gridIndexX > 0) 
        {
            if (gridManager.masterTileGrid[gridIndexX - 1, gridIndexZ].GameTile != null)
            {
                L = true;
                neighbourResults.Add(gridManager.masterTileGrid[gridIndexX - 1, gridIndexZ].GameTile);
            }
        } 
        // Right
        if (gridIndexX < gridManager.xMax - 1) 
        {
            if (gridManager.masterTileGrid[gridIndexX + 1, gridIndexZ].GameTile != null)
            {
                R = true;
                neighbourResults.Add(gridManager.masterTileGrid[gridIndexX + 1, gridIndexZ].GameTile);
            }
        } 
        // Bottom
        if (gridIndexZ > 0) 
        { 
            if(gridManager.masterTileGrid[gridIndexX, gridIndexZ - 1].GameTile != null)
            {
                B = true;
                neighbourResults.Add(gridManager.masterTileGrid[gridIndexX, gridIndexZ - 1].GameTile);
            }
        } 
        // Top
        if (gridIndexZ < gridManager.zMax - 1) 
        { 
            if(gridManager.masterTileGrid[gridIndexX, gridIndexZ + 1].GameTile != null)
            {
                T = true;
                neighbourResults.Add(gridManager.masterTileGrid[gridIndexX, gridIndexZ + 1].GameTile);
            }
        } 
        // Bottom Left
        if (gridIndexX > 0 && gridIndexZ > 0) 
        { 
            if(gridManager.masterTileGrid[gridIndexX - 1, gridIndexZ - 1].GameTile != null)
            {
                BL = true;
                neighbourResults.Add(gridManager.masterTileGrid[gridIndexX - 1, gridIndexZ - 1].GameTile);
            }
        }
        // Bottom Right
        if (gridIndexX < gridManager.xMax - 1 && gridIndexZ > 0) 
        { 
            if(gridManager.masterTileGrid[gridIndexX + 1, gridIndexZ - 1].GameTile != null)
            {
                BR = true;
                neighbourResults.Add(gridManager.masterTileGrid[gridIndexX + 1, gridIndexZ - 1].GameTile);
            }
        }
        // Top Left
        if(gridIndexX > 0 && gridIndexZ < gridManager.zMax - 1) 
        { 
            if(gridManager.masterTileGrid[gridIndexX - 1, gridIndexZ + 1].GameTile != null)
            {
                TL = true;
                neighbourResults.Add(gridManager.masterTileGrid[gridIndexX - 1, gridIndexZ + 1].GameTile);
            }
        }
        // Top Right
        if(gridIndexX < gridManager.xMax - 1 && gridIndexZ < gridManager.zMax - 1) 
        { 
            if(gridManager.masterTileGrid[gridIndexX + 1, gridIndexZ + 1].GameTile != null)
            {
                TR = true;
                neighbourResults.Add(gridManager.masterTileGrid[gridIndexX + 1, gridIndexZ + 1].GameTile);
            }
        }

        // Ensure no tiles are null
        foreach(GameTile tile in neighbourResults)
        {
            if (tile == null)
            {
                neighbourResults.Remove(tile);
            }
        }

        return neighbourResults;
    }
}



