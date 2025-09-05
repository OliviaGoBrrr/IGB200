using System.Threading;
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

    
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIManager uiManager;
    public Camera sceneCamera;
    public GameState state;
    private GameState prevGameState;

    // Player Actions
    public int roundCount = 1;
    public int maxActions = 3;
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

        // Timer logic for simulation
        if (state == GameState.PLAYING)
        {
            simTimer += Time.deltaTime;

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
                GameOver();
                break;

            case GameState.GAME_WIN:
                GameWin();
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

        /*
         * 
                 // If there are no more burning tiles, player wins the game


        *
        */
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

        // Update tile
        selectTile.tileState = changeState;
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
            uiManager.PauseScreen.SetActive(true);
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
        Time.timeScale = 0.0f;
        OnGameOver?.Invoke();
    }

    public void GameWin()
    {
        // Game win logic
        Debug.Log("Game Win");
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
