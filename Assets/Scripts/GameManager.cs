using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MAIN_MENU = 0,
        PLAYING,
        PAUSED,
        GAME_OVER
    }

    // Events
    public static event System.Action OnRoundAdvanced;
    public static event System.Action OnPlayerAction;
    public static event System.Action OnActionCostTooHigh;

    
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

    void Update()
    {
        if(currentActionCount == 0)
        {
            AdvanceRound();
        }
    }

    public void AdvanceRound()
    {
        roundCount++;
        currentActionCount = maxActions;
        OnRoundAdvanced?.Invoke();
    }

    public void PlayerActionTaken(GameTile.TileStates changeState, int actionCost)
    {
        // If the player has enough actions to do something
        if(actionCost > currentActionCount)
        {
            OnActionCostTooHigh?.Invoke();
            return;
        }

        // Find the tile on the grid
        var tileGrid = gridManager.masterTileGrid;

        Vector3 selectCellPos = GetSelectedGridPosition();

        int cellX = Mathf.FloorToInt(selectCellPos.x);
        int cellZ = Mathf.FloorToInt(selectCellPos.z);

        GameTile selectTile = tileGrid[cellX, cellZ].GameTile;

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

}
