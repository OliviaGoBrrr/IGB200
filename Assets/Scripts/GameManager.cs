using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        PLAYING = 0,
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
    public Camera sceneCamera;
    public GameState state;

    // Player Actions
    public int roundCount = 1;
    public int maxActions = 3;
    [HideInInspector] public int currentActionCount;

    private Vector3 lastMousePosition;
    void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        currentActionCount = maxActions;
    }

    void Start()
    {
        // Reposition camera to the center of the map 
        Vector3 cameraPos = sceneCamera.transform.position;
        cameraPos.x = (float)gridManager.xMax / 2;
        sceneCamera.transform.position = cameraPos;

        state = GameState.PLAYING;
    }

    void Update()
    {
        if(currentActionCount == 0)
        {
            AdvanceRound();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(state == GameState.PLAYING)
            {
                SetGameState(GameState.PAUSED);
            }
            else if(state == GameState.PAUSED)
            {
                SetGameState(GameState.PLAYING);
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

        state = newState;

        switch (state)
        {
            case GameState.PLAYING:
                PlayGame();
                break;

            case GameState.PAUSED:
                PauseGame();
                break;

            case GameState.GAME_OVER:
                GameOver();
                break;
            case GameState.GAME_WIN:
                GameWin();
                break;
        }
    }

    public void AdvanceRound()
    {
        roundCount++;
        currentActionCount = maxActions;
        OnRoundAdvanced?.Invoke();

        // If there are no more burning tiles, player wins the game
        if(gridManager.tileList.Exists(tile => tile.tileState == GameTile.TileStates.BURNING) == false)
        {
            SetGameState(GameState.GAME_WIN);
        }
    }

    public void PlayerActionTaken(GameTile.TileStates changeState, int actionCost)
    {
        Debug.Log("Before Invoke");
        // If the player doesn't have enough actions to do something, throw a warning
        if(actionCost > currentActionCount)
        {
            OnActionCostTooHigh?.Invoke();
            return;
        }

        // Find the tile on the grid
        var tileGrid = gridManager.masterTileGrid;

        Debug.Log(tileGrid.Length);

        Vector3 selectCellPos = GetSelectedGridPosition(true);

        Debug.Log(selectCellPos);

        int cellX = Mathf.FloorToInt(selectCellPos.x);
        int cellZ = Mathf.FloorToInt(selectCellPos.z);

        Debug.Log(cellX);
        Debug.Log(cellZ);

        GameTile selectTile = tileGrid[cellX, cellZ].GameTile;
        Debug.Log(selectTile);

        Debug.Log(selectTile.tileData.TilePosition);

        // If the tile is already the tile state, throw a warning
        if(selectTile.tileState == changeState)
        {
            Debug.Log("Gamer");
            // Show warning 
            return;
        }

        if(selectTile.tileState == GameTile.TileStates.ANIMAL)
        {
            // Show warning
            return;
        }

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

                lastMousePosition = new Vector3(snapX, lastMousePosition.y, snapZ);
            }
        }

        return lastMousePosition;
    }

    public void PlayGame()
    {
        // Play game logic
        Debug.Log("Game is playing");
    }

    public void PauseGame()
    {
        // Pause game logic
        Debug.Log("Game is paused");
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

}
