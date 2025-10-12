using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        PLAYING = 0,
        PREP_PHASE,
        PAUSED,
        GAME_OVER,
        GAME_WIN
    }

    public int ScoreTotal;
    bool ScoreCounting = false;

    // Events
    public static event System.Action OnRoundAdvanced;
    public static event System.Action OnPlayerAction;
    public static event System.Action OnGameOver;

    public bool draggableSelected;
    public DraggableItem selectedDraggable;

    public bool devKeysOn;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIManager uiManager;
    public Camera sceneCamera;
    public PlayerAnimator playerAnimator;
    public GameState state;
    private GameState prevGameState;
    public int roundCount = 1;

    public List<GameTile.TileStates> burnableTileStates = new List<GameTile.TileStates>();
    public List<GameTile.TileStates> rewardableStates = new List<GameTile.TileStates>();
    public List<GameTileData> rewardTiles = new List<GameTileData>();

    [Header("Player Actions")]
    public List<GameTileData> tilesChanged = new List<GameTileData>();
    public List<DraggableItem> itemsUsed = new List<DraggableItem>();

    [Header("Game Scoring")]
    [SerializeField] private int playerScore;
    [SerializeField] private float[] scoreThresholds = new float[3];
    [SerializeField] private float initialGrassPercent;
    [SerializeField] private float finalGrassPercent;
    [HideInInspector] public int currentActionCount;
    private Vector3 lastMousePosition;

    [Header("Simulation Settings")]
    [SerializeField] private float simTime = 1.0f;
    private float simTimer;

    [Header("Decoration")]
    public List<GameObject> DryDecorations;

    void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        sceneCamera = FindFirstObjectByType<Camera>();
        playerAnimator = FindFirstObjectByType<PlayerAnimator>();
        FindFirstObjectByType<SceneAudio>().MagProgressWipe();
    }

    void Start()
    { 

    }

    void GameReady()
    {
        // Reposition camera to the center of the map 
        Vector3 cameraPos = sceneCamera.transform.position;
        cameraPos.x = (gridManager.xMax + gridManager.xMin) / 2;
        sceneCamera.transform.position = cameraPos;
        initialGrassPercent = gridManager.GetPercentOfTileInGrid(GameTile.TileStates.GRASS);

        foreach (var tile in gridManager.masterTileGrid)
        {
            if (tile.GameTile != null && rewardableStates.Contains(tile.TileState))
            {
                rewardTiles.Add(tile);
            }
        }

        state = GameState.PREP_PHASE;
        uiManager.LoadingScreen.SetActive(false);
    }

    void Update()
    {
        // Pausing
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(state != GameState.PAUSED)
            {
                SetGameState(GameState.PAUSED);
            }
            else if(state == GameState.PAUSED)
            {
                SetGameState(prevGameState);
            }
        }

        if (devKeysOn)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameWin(2);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                GameOver();
            }
        }

        // Timer logic for simulation
        if (state == GameState.PLAYING)
        {
            simTimer += Time.deltaTime;

            if(gridManager.GetPercentOfTileInGrid(GameTile.TileStates.BURNING) == 0)
            {
                // Scoring
                if(ScoreCounting == false)
                {
                    StartCoroutine(FindAnyObjectByType<SceneAudio>().EndCrackle());
                    ScoreCounting = true;
                    StartCoroutine(ScoreCount());
                }
                
                
                finalGrassPercent = 100 * (gridManager.GetPercentOfTileInGrid(GameTile.TileStates.GRASS) / initialGrassPercent);

                
            }

            if(simTimer > simTime)
            {
                simTimer = 0;
                AdvanceRound();
            }
        }

    }

    IEnumerator ScoreCount()
    {
        float timeBetweenTiles = 0.25f;
        foreach (GameTile tile in gridManager.tileList)
        {
            yield return new WaitForSeconds(timeBetweenTiles);
            if (tile.tileState == GameTile.TileStates.BURNT && tile.scoreWhenBurnt == true)
            {
                ScoreTotal += tile.tileScore;
                FindAnyObjectByType<SceneAudio>().ScoreGained();
                tile.ScoreCounted(true);
            }
            else if (tile.tileState != GameTile.TileStates.BURNT && tile.scoreWhenBurnt != true)
            {
                ScoreTotal += tile.tileScore;
                FindAnyObjectByType<SceneAudio>().ScoreGained();
                tile.ScoreCounted(true);
            }
            else 
            { 
                FindAnyObjectByType<SceneAudio>().ScoreUngained();
                tile.ScoreCounted(false);
            }
            if (timeBetweenTiles > 0.1f) { timeBetweenTiles -= 0.008f; }
            Debug.Log(ScoreTotal.ToString());
        }
        yield return new WaitForSeconds(2f);

        int stars = 3;
        for (int i = 0; i < scoreThresholds.Length; i++)
        {
            if (ScoreTotal >= scoreThresholds[i])
            {
                GameWin(stars);
                continue;
            }
            else
            {
                stars--;
            }
        }

        if (stars == 0)
        {
            GameOver();
        }
        uiManager.scoreText.SetText($"Score: {ScoreTotal}");
        yield return null;
    }

    /// <summary>
    /// Sets the state of the game. Executes through a state machine.
    /// </summary>
    public void SetGameState(GameState newState)
    {
        // Check if we're already in the state
        if (state == newState)
        {
            return;
        }

        prevGameState = state;

        state = newState;

        // this is kinda jank
        switch (state)
        {
            case GameState.PREP_PHASE:
                if(prevGameState == GameState.PAUSED)
                {
                    PauseUnpauseGame();
                }
                break;

            case GameState.PLAYING:
                if(prevGameState == GameState.PAUSED)
                {
                    PauseUnpauseGame();
                }
                break;

            case GameState.PAUSED:
                PauseUnpauseGame();
                break;

            case GameState.GAME_OVER:
                break;

            case GameState.GAME_WIN:
                break;
        }
    }

    /// <summary>
    /// Set the state of the game to Playing. 
    /// Throws an error if there are no burning tiles currently on the map (The Player must select one tiles before Playing).
    /// </summary>
    public bool PlaySimulation()
    {
        // Checks to see if theres at least one burning tile on the grid
        if (gridManager.tileList.Exists(tile => tile.tileState == GameTile.TileStates.BURNING) == false)
        {
            Debug.LogError("There are no Burning tiles to start simulation. Please make one tile a Burning tile");
            return false;
        }

        FindAnyObjectByType<SceneAudio>().PlayButtonClick(5);
        SetGameState(GameState.PLAYING);
        return true;
        
    }

    /// <summary>
    /// Simulates "ticks". Invokes actions on objects that get updated each tick.
    /// </summary>
    public void AdvanceRound(int ticks = 1)
    {
        for (int i = 0; i < ticks; i++)
        {
            roundCount++;
            OnRoundAdvanced?.Invoke();
        }

        //sceneAudio.DrumBeat(1f); // Plays drum beat
        if (ScoreCounting == false)
        {
            FindAnyObjectByType<SceneAudio>().DrumBeat(1f); // Plays drum beat
        }
    }

    /// <summary>
    /// Executes logic when a player makes an action in game. 
    /// Updates tiles accordingly.
    /// </summary>
    public void PlayerActionTaken(GameTile.TileStates changeState, DraggableItem item, AudioClip actionSound, float intensity)
    {
        // Find the tile on the grid
        var tileGrid = gridManager.masterTileGrid;

        Vector3 selectCellPos = GetSelectedGridPosition(true);

        // Therefore, if y is less than 0, don't do the action
        if (selectCellPos.y < 0)
        {
            return;
        }

        

        int cellX = Mathf.FloorToInt(selectCellPos.x);
        int cellZ = Mathf.FloorToInt(selectCellPos.z);

        GameTile selectTile = tileGrid[cellX, cellZ].GameTile;

        // Can the player change the tile's state

        if (!selectTile.CanBeChanged(changeState)) { return; }

        selectTile.tileState = changeState;

        tilesChanged.Add(selectTile.tileData);
        itemsUsed.Add(item);

        Debug.Log(selectTile.tileData.TileState);

        // Use a switch statement to handle different actions
        switch (changeState)
        {
            case GameTile.TileStates.DRY_GRASS:
                selectTile.wetness = 0;
                break;

            case GameTile.TileStates.WET_GRASS:
                selectTile.wetness++;
                break;

            default:
                selectTile.tileState = changeState;
                break;
        }
        // Adds dry decorations to the dry deco action
        


        item.itemUses--;
        playerAnimator.Animate(selectTile);

        // Update tile visual and state based on the new wetness value
        StartCoroutine(JustADelay(selectTile, changeState, actionSound, intensity));

        OnPlayerAction?.Invoke();
    }
    IEnumerator JustADelay(GameTile selectTile, GameTile.TileStates changeState, AudioClip actionSound, float intensity)
    {
        yield return new WaitForSeconds(0.8f);
        FindAnyObjectByType<SceneAudio>().PlayGameSound(actionSound, intensity);
        selectTile.TileStateUpdate();
        if (changeState == GameTile.TileStates.DRY_GRASS)
        {
            foreach (GameObject deco in DryDecorations)
            {
                Instantiate(deco, selectTile.transform);
            }
            selectTile.GetComponent<GameTile>().DecorationUpdate();
            // Also updates the score
            selectTile.GetComponent<GameTile>().SetTileScore();
        }
        yield return null;
    }

    public void UndoPlayerAction()
    {
        if (tilesChanged.Count <= 0 || itemsUsed.Count <= 0)
        {
            Debug.Log("Nothing to Undo");
            return;
        }

        // Finds the last tile changed by the player, and reverts it back to its previous state.


        var undoingTiles = tilesChanged.ToArray();
        var undoingItems = itemsUsed.ToArray();

        var lastTile = undoingTiles[undoingTiles.Length - 1];
        var lastItem = undoingItems[undoingItems.Length - 1];

        lastTile.GameTile.tileState = lastTile.TileState;
       
        lastTile.GameTile.TileStateUpdate();
        tilesChanged.RemoveAt(tilesChanged.Count - 1);

        lastTile.GameTile.UndoTrigger();

        lastItem.EnableDraggable(1);
        itemsUsed.RemoveAt(itemsUsed.Count - 1);
    }

    /// <summary>
    /// Finds a position on the grid based from where the mouse is on the screen.
    /// Can be set to snap to the center of a tile, otherwise will follow the mouse fluidly.
    /// </summary>
    public Vector3 GetSelectedGridPosition(bool snapToGrid = false)
    {
        Vector3 mousePositon = Input.mousePosition;
        mousePositon.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePositon);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lastMousePosition = hit.point;

            if (snapToGrid)
            {
                var snapX = Mathf.FloorToInt(lastMousePosition.x) + (gridManager.grid.cellSize.x / 2);
                var snapZ = Mathf.FloorToInt(lastMousePosition.z) + (gridManager.grid.cellSize.z / 2);

                lastMousePosition = new Vector3(snapX, 0.5f, snapZ);
            }
        }
        else
        {
            lastMousePosition = new Vector3(0, -1, 0);
        }

        return lastMousePosition;
    }


    public void SetPlayerScore()
    {
        // Check if the initial dry grass tiles were burnt.
        foreach (var tile in rewardTiles)
        {
            if (tile.TileState == GameTile.TileStates.DRY_GRASS && tile.GameTile.tileState == GameTile.TileStates.BURNT)
            {
                playerScore += tile.GameTile.tileScore;
            }

            else if (tile.TileState == GameTile.TileStates.ANIMAL || tile.TileState == GameTile.TileStates.GRASS)
            {
                if(tile.GameTile.tileState != GameTile.TileStates.BURNT)
                {
                    playerScore += tile.GameTile.tileScore;
                }
            }
        }

        uiManager.scoreText.SetText($"Score: {playerScore}");
    }

    public void PauseUnpauseGame()
    {
        if(prevGameState != GameState.PAUSED)
        {
            Time.timeScale = 0;
            //uiManager.PauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }
        else
        {
            Time.timeScale = 1;
            uiManager.PauseScreen.SetActive(false);
            Debug.Log("Game is unpaused");
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        state = GameState.GAME_OVER;
        //Time.timeScale = 0.0f;
        OnGameOver?.Invoke();
        
    }

    public void GameWin(int starCount)
    {
        
        state = GameState.GAME_WIN;

        uiManager.DisplayGameWinUI(starCount);

        ScoreData.CalculateLevel(starCount); // send off to score data where itll save for level select

        Debug.Log("Game win biiiitch");
    }

    private void OnEnable()
    {
        GridManager.GridLoadingCompleted += GameReady;
    }

    private void OnDisable()
    {
        GridManager.GridLoadingCompleted -= GameReady;
    }

}
