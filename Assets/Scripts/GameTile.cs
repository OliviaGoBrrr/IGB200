using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameTile;


public class GameTile: MonoBehaviour
{
    public enum TileStates
    {
        GRASS = 0,
        BURNING,
        BURNT
    }
    [SerializeField] private int roundsToBurn;
    [SerializeField] private TileStates tileState;
    [SerializeField] private Material[] tileMaterials;
    [SerializeField] private GridManager gridManager;
    public int gridIndexX, gridIndexZ;

    private Renderer renderer;
    private Vector3 tilePosition;
    
    public GameTileData tileData;
    private List<GameTile> tileNeighbours = new List<GameTile>();

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
            renderer.material = tileMaterials[(int)TileStates.BURNING];
        }
    }

    private void OnValidate() => gameObject.GetComponent<Renderer>().material = tileMaterials[(int)tileState];

    public void AdvanceRound()
    {
        if(tileState == TileStates.BURNING)
        {
            if (roundsToBurn <= 0)
            {
                tileState = TileStates.BURNT;
                renderer.material = tileMaterials[(int)TileStates.BURNT];
            }
            else
            {
                roundsToBurn -= 1;
            }
            AttemptFireSpread();
        }
    }

    public void AttemptFireSpread()
    {
        foreach(var tile in tileNeighbours)
        {
            if(tile.tileState != TileStates.BURNT && tile.tileState != TileStates.BURNING)
            {
                tile.tileState = TileStates.BURNING;
                tile.renderer.material = tileMaterials[(int)TileStates.BURNING];
                break;
            }
        }
    }

    public List<GameTile> GetNeighbours()
    {
        var neighbourResults = new List<GameTile>();

        // Left
        if (gridIndexX > 0) { neighbourResults.Add(gridManager.masterTileGrid[gridIndexX - 1, gridIndexZ].GameTile); } 
        // Right
        if (gridIndexX < gridManager.xMax - 1) { neighbourResults.Add(gridManager.masterTileGrid[gridIndexX + 1, gridIndexZ].GameTile); } 
        // Up
        if (gridIndexZ > 0) { neighbourResults.Add(gridManager.masterTileGrid[gridIndexX, gridIndexZ - 1].GameTile); } 
        // Down
        if (gridIndexZ < gridManager.zMax - 1) { neighbourResults.Add(gridManager.masterTileGrid[gridIndexX, gridIndexZ + 1].GameTile); } 
        // Top Left
        if (gridIndexX > 0 && gridIndexZ > 0) { neighbourResults.Add(gridManager.masterTileGrid[gridIndexX - 1, gridIndexZ - 1].GameTile); }
        // Top Right
        if (gridIndexX < gridManager.xMax - 1 && gridIndexZ > 0) { neighbourResults.Add(gridManager.masterTileGrid[gridIndexX + 1, gridIndexZ - 1].GameTile); };
        // Down Left
        if(gridIndexX > 0 && gridIndexZ < gridManager.zMax - 1) { neighbourResults.Add(gridManager.masterTileGrid[gridIndexX - 1, gridIndexZ + 1].GameTile); }
        // Down Right
        if(gridIndexX < gridManager.xMax - 1 && gridIndexZ < gridManager.zMax - 1) { neighbourResults.Add(gridManager.masterTileGrid[gridIndexX + 1, gridIndexZ + 1].GameTile); }

        return neighbourResults;
    }

    private void OnEnable()
    {
        GameManager.OnRoundAdvanced += AdvanceRound;
    }

    private void OnDisable()
    {
        GameManager.OnRoundAdvanced -= AdvanceRound;
    }

}

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

