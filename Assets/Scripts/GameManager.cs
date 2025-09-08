using System.Threading;
using UnityEditor;
using UnityEngine;

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

    // Events
    public static event System.Action OnRoundAdvanced;
    public static event System.Action OnPlayerAction;
    public static event System.Action OnActionCostTooHigh;
    public static event System.Action OnGameOver;

    public bool devKeysOn;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIManager uiManager;
    public Camera sceneCamera;
    public GameState state;
    private GameState prevGameState;

    // Player Actions
    public int roundCount = 1;
    public int maxActions = 3;
    [SerializeField] private float[] scoreThresholds = new float[3];
    [SerializeField] private float initialGrassPercent;
    [SerializeField] private float finalGrassPercent;
    [HideInInspector] public int currentActionCount;
    private Vector3 lastMousePosition;

    // Simulation setting
    [SerializeField] private float simTime = 1.0f;
    private float simTimer;

    void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        currentActionCount = maxActions;
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
        state = GameState.PREP_PHASE;
        uiManager.LoadingScreen.SetActive(false);
    }

    void Update()
    {
        if(currentActionCount == 0)
        {
            AdvanceRound();
        }

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
                finalGrassPercent = 100 * (gridManager.GetPercentOfTileInGrid(GameTile.TileStates.GRASS) / initialGrassPercent);

                int stars = 3;

                for (int i = 0; i < scoreThresholds.Length; i++)
                {
                    if (finalGrassPercent > scoreThresholds[i])
                    {
                        GameWin(stars);
                        break;
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
            }

            if(simTimer > simTime)
            {
                simTimer = 0;
                AdvanceRound();
            }
        }

    }

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

    public void PlaySimulation()
    {
        // Checks to see if theres at least one burning tile on the grid
        if (gridManager.tileList.Exists(tile => tile.tileState == GameTile.TileStates.BURNING) == false)
        {
            Debug.LogError("There are no Burning tiles to start simulation. Please make one tile a Burning tile");
            return;
        }

        SetGameState(GameState.PLAYING);
        
    }

    public void AdvanceRound()
    {
        roundCount++;
        currentActionCount = maxActions;
        OnRoundAdvanced?.Invoke();
    }

    public void PlayerActionTaken(GameTile.TileStates changeState, int actionCost)
    {
        // If the player doesn't have enough actions to do something, throw a warning
        if(actionCost > currentActionCount)
        {
            OnActionCostTooHigh?.Invoke();
            return;
        }

        // Find the tile on the grid
        var tileGrid = gridManager.masterTileGrid;

        Vector3 selectCellPos = GetSelectedGridPosition(true);

        // If the player selects outside of the grid, it'll return Vector3(0, -1, 0)
        // Therefore, if y is less than 0, don't do the action
        
        if(selectCellPos.y < 0)
        {
            return;
        }

        Debug.Log(selectCellPos);

        int cellX = Mathf.FloorToInt(selectCellPos.x);
        int cellZ = Mathf.FloorToInt(selectCellPos.z);

        GameTile selectTile = tileGrid[cellX, cellZ].GameTile;

        // Can the player change the tile's state
        if (!selectTile.CanBeChanged(changeState)) { return; }

        if(changeState == GameTile.TileStates.WET_GRASS)
        {
            selectTile.wetness++;
        }
        else
        {
            selectTile.tileState = changeState;
        }

        // Update tile
        selectTile.TileStateUpdate();

        currentActionCount -= actionCost;

        OnPlayerAction?.Invoke();
    }

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
