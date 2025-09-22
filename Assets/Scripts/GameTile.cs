using Unity.VisualScripting;
//using UnityEditor.EditorTools;
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
        DITCH,
        DRY_GRASS,
        WET_GRASS
    }

    // References
    [Header("References")]
    private GridManager gridManager;
    private GameManager gameManager;
    [SerializeField] public GameObject highlight;

    [Header("Tile Properties")]
    public GameTileData tileData;
    [SerializeField] private bool canBeBurnt;
    [SerializeField] private int roundsToBurn;
    public int tileScore;
    private bool affected;
    public int wetness = 0;
    public TileStates tileState;
    [SerializeField] private Material[] tileMaterials;
    public int gridIndexX, gridIndexZ;
    private Vector3 tilePosition;

    [Header("Tile Decorations")]
    public List<BurnableDecoration> burnableDecorations;
    public List<GameObject> growableDecorations;

   


    // Neighbours
    private List<GameTile> tileNeighbours = new List<GameTile>();
    
    // Neighbour bools (currently unused)
    [SerializeField] 
    public bool
        TL, T, TR,
        L,      R,
        BL, B, BR;

    void Awake()
    {
        tileData = new GameTileData(this, roundsToBurn, tileState, tilePosition, canBeBurnt);
        gameManager = FindFirstObjectByType<GameManager>();
        gridManager = GetComponentInParent<GridManager>();
        tilePosition = transform.position;

        foreach (Transform childTransform in transform)
        {
            GameObject child = childTransform.gameObject;
            BurnableDecoration decoration = child.GetComponent<BurnableDecoration>();
            if (decoration != null) { burnableDecorations.Add(decoration); }
        }

    }

    
    void Start()
    {
        if (gameManager != null)
        {
            if (gameManager.burnableTileStates.Contains(tileState))
            {
                canBeBurnt = true;
            }
            else
            {
                canBeBurnt = false;
            }
        }

        TileStateUpdate();
        SetTileScore();
        affected = false;
    }

    public void SetTileScore()
    {
        switch (tileState)
        {
            case TileStates.ANIMAL:
                tileScore = 300;
                break;
            case TileStates.DRY_GRASS:
                tileScore = 100;
                break;
            case TileStates.GRASS:
                tileScore = 50;
                break;
            default:
                tileScore = 0;
                break;
        }
    }

    private void OnValidate()
    {
        TileStateUpdate();

        /*
        if (tileState != TileStates.BURNING)
        {
            if (wetness >= 2)
            {
                tileState = TileStates.WET_GRASS;
            }
            else if (wetness == 1)
            {
                tileState = TileStates.GRASS;
            }
            else if (wetness == 0)
            {
                tileState = TileStates.DRY_GRASS;
            }
        }
        */
    }

    public void RoundAdvanced()
    {
        if(tileState == TileStates.BURNING && !affected)
        {
            roundsToBurn -= 1;
            if (roundsToBurn < 0)
            {
                tileState = TileStates.BURNT;
                TileStateUpdate();
                return;
            }

            AttemptFireSpread();
        }
        affected = false;
    }

    public void AttemptFireSpread()
    {
        var viableTiles = new List<GameTile>();

        // Checks all the neighbours. If a neighbour can be spread to, set true
        foreach (var tile in tileNeighbours)
        {
            if (tile.canBeBurnt && !affected) 
            { 
                viableTiles.Add(tile);
            }
        }

        // Spreads to all viable neighbours
        if (viableTiles.Count > 0)
        {
            foreach(var viableTile in viableTiles)
            {
                viableTile.affected = true;
                viableTile.tileState = TileStates.BURNING;
 
                viableTile.TileStateUpdate();
            }
        }
    }

    public GameTileData GetTileData()
    {
        return new GameTileData(this, roundsToBurn, tileState, tilePosition, canBeBurnt);
    }

    /// <summary>
    /// Function to be called when a GameTile's state gets updated
    /// </summary>
    public void TileStateUpdate() 
    {
        // Change the material to the appropriate material
        gameObject.GetComponent<Renderer>().material = tileMaterials[(int)tileState];

        if (gameManager != null)
        {
            if (gameManager.burnableTileStates.Contains(tileState))
            {
                canBeBurnt = true;
            }
            else
            {
                canBeBurnt = false;
            }
        }

        tileData.TileState = tileState;
        tileData.CanBeBurnt = canBeBurnt;

        if (tileState == TileStates.BURNING)
        {
            foreach (BurnableDecoration deco in burnableDecorations)
            {
                deco.TriggerBurn();
            }
        }
    }

    // Logic to check whether a tile can be changed by the player's action
    public bool CanBeChanged(TileStates changeToState)
    {
        // If the tile is already that tile type
        if (changeToState == tileState) { return false; }

        if(tileState == TileStates.DRY_GRASS && changeToState != TileStates.BURNING)
        {
            return false;
        }

        // Protected tiles
        if(!canBeBurnt) { return false; }

        return true;
    }

    private void OnEnable()
    {
        GameManager.OnRoundAdvanced += RoundAdvanced;
        GridManager.FindNeighbours += GetNeighbours;
    }

    private void OnDisable()
    {
        GameManager.OnRoundAdvanced -= RoundAdvanced;
        GridManager.FindNeighbours -= GetNeighbours;
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }



    public void GetNeighbours()
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
        /*
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
        */

        // Ensure no tiles are null
        foreach(GameTile tile in neighbourResults)
        {
            if (tile == null)
            {
                neighbourResults.Remove(tile);
            }
        }

        tileNeighbours = neighbourResults;

                gridManager.CompleteGridGeneration();
    }
}



