using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using Unity.VisualScripting;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using static GameTile;
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
    private Color defaultHighlightColor = new Color(1.0f,1.0f,1.0f, 0.373f);
    private Color wrongHighlightColor = new Color(1.0f,0.0f,0.0f,0.373f);

    [Header("Tile Properties")]
    public GameTileData tileData;
    [SerializeField] private bool canBeBurnt;
    [SerializeField] private int roundsToBurn;

    public int tileScore;
    public bool scoreWhenBurnt;

    private bool affected;
    public int wetness = 0;
    public TileStates tileState;
    [SerializeField] private Material[] tileMaterials;
    public int gridIndexX, gridIndexZ;
    private Vector3 tilePosition;

    [Header("Tile Decorations")]
    public List<BurnableDecoration> burnableDecorations;
    public List<SproutableDecoration> sproutableDecorations;
    public List<FireDecoration> fireDecorations;

    [Header("Spawnable Decorations")]
    [SerializeField] List<GameObject> dryDecorations;


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

        DecorationSpawn();
        DecorationUpdate();

    }
    public void DecorationSpawn()
    {
        if (tileState == TileStates.DRY_GRASS)
        {
            foreach (GameObject deco in dryDecorations)
            {
                for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
                {
                    Instantiate(deco, transform);
                }
            }
        }
    }
    public void DecorationUpdate()
    {
        burnableDecorations.Clear();
        sproutableDecorations.Clear();
        fireDecorations.Clear();
        foreach (Transform childTransform in transform)
        {
            GameObject child = childTransform.gameObject;
            BurnableDecoration decoration1 = child.GetComponent<BurnableDecoration>();
            if (decoration1 != null) { burnableDecorations.Add(decoration1); }
            SproutableDecoration decoration2 = child.GetComponent<SproutableDecoration>();
            if (decoration2 != null) { sproutableDecorations.Add(decoration2); }
            FireDecoration decoration3 = child.GetComponent<FireDecoration>();
            if (decoration3 != null) { fireDecorations.Add(decoration3); }
        }
    }

    
    void Start()
    {
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
                scoreWhenBurnt = false;
                break;
            case TileStates.DRY_GRASS:
                tileScore = 100;
                scoreWhenBurnt = true;
                break;
            case TileStates.GRASS:
                tileScore = 50;
                scoreWhenBurnt = false;
                break;
            default:
                tileScore = 0;
                break;
        }
    }

    private void OnValidate()
    {
        TileStateUpdate();
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

        // Update struct
        tileData.TileState = tileState;
        tileData.CanBeBurnt = canBeBurnt;

        if (tileState == TileStates.BURNING)
        {
            foreach (BurnableDecoration deco in burnableDecorations)
            {
                deco.TriggerBurn();
            }
            foreach (FireDecoration deco in fireDecorations)
            {
                deco.TriggerBurn();
            }
        }
        if (tileState == TileStates.BURNT)
        {
            foreach (FireDecoration deco in fireDecorations)
            {
                deco.Extinguish();
            }
        }
    }

    public void ScoreCounted(bool scoreGained)
    {
        if (scoreGained == true)
        {
            foreach (BurnableDecoration deco in burnableDecorations)
            {
                if (deco.hasScoreCountPar) { deco.TriggerCount(); }
            }
            foreach (SproutableDecoration deco in sproutableDecorations)
            {
                deco.TriggerSprout();
            }
        }
        StartCoroutine(Tweek());
    }
    IEnumerator Tweek()
    {
        transform.DORotate(new Vector3(0, 0, 5), 0.2f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(0.2f);
        transform.DORotate(new Vector3(0, 0, -5), 0.4f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(0.4f);
        transform.DORotate(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(0.2f);
        yield return null;
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
        if(tileState == TileStates.ANIMAL)
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

    // Game tile highlighting on mouse over
    private void OnMouseEnter()
    {
        if(gameManager.selectedDraggable != null)
        {
            if (!CanBeChanged(gameManager.selectedDraggable.changeState))
            {
                highlight.GetComponent<Renderer>().material.color = wrongHighlightColor;
            }
            else
            {
                highlight.GetComponent<Renderer>().material.color = defaultHighlightColor;
            }
        }
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
        highlight.GetComponent<Renderer>().material.color = defaultHighlightColor;
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
    public void UndoTrigger(TileStates transformInto)
    {
        DecorationUpdate();
        foreach (BurnableDecoration deco in burnableDecorations)
        {
            deco.UndoHelp(transformInto);
        }
        foreach (SproutableDecoration deco in sproutableDecorations)
        {
            deco.UndoHelp(transformInto);
        }
        foreach (FireDecoration deco in fireDecorations)
        {
            deco.Extinguish();
        }
    }
}



