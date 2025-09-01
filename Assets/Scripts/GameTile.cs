using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameTile;
using System;
public struct GameTileData
{
    public GameTileData(GameTile gameTile, int burnRounds, TileStates tileState, Vector3 tilePosition)
    {
        this.TileState = tileState;
        this.RoundsToBurn = burnRounds;
        this.TilePosition = tilePosition;
        this.GameTile = gameTile;
    }

    public GameTile GameTile;
    public int RoundsToBurn;
    public TileStates TileState;
    public Vector3 TilePosition;
}

public class GameTile: MonoBehaviour
{
    public enum TileStates
    {
        GRASS = 0,
        BURNING,
        BURNT
    }

    // References
    private GridManager gridManager;
    private GameManager gameManager;
    private Renderer renderer;
    [SerializeField] public GameObject highlight;

    // Tile Properties
    public GameTileData tileData;
    [SerializeField] 
    private int roundsToBurn;
    [SerializeField] 
    private int wetness; // Not implemented yet
    public TileStates tileState;
    [SerializeField] 
    private Material[] tileMaterials;
    
    [HideInInspector]
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
        gridManager = GetComponentInParent<GridManager>();
        tilePosition = transform.position;
        tileData = new GameTileData(this, roundsToBurn, tileState, tilePosition);
        renderer = gameObject.GetComponent<Renderer>();
    }

    void Start()
    {
        tileNeighbours = GetNeighbours();
        if (tileState == TileStates.BURNING)
        {
            TileStateUpdate();
        }
    }

    private void OnValidate() => gameObject.GetComponent<Renderer>().material = tileMaterials[(int)tileState];

    public void RoundAdvanced()
    {
        if(tileState == TileStates.BURNING)
        {
            roundsToBurn -= 1;
            if (roundsToBurn < 0)
            {
                tileState = TileStates.BURNT;
                TileStateUpdate();
            }
            else
            {
                AttemptFireSpread();
            }
        }
    }

    public void AttemptFireSpread()
    {
        foreach (var tile in tileNeighbours)
        {
            int choice = UnityEngine.Random.Range(0, tileNeighbours.Count);

            if (tileNeighbours[choice].tileState == TileStates.GRASS)
            {
                tileNeighbours[choice].tileState = TileStates.BURNING;
                tileNeighbours[choice].TileStateUpdate();
                break;
            }
            else if(tile.tileState == TileStates.GRASS)
            {
                tile.tileState = TileStates.BURNING;
                tile.TileStateUpdate();
            }
        }

    }

    public void TileStateUpdate() { renderer.material = tileMaterials[(int)tileState]; }

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

}



