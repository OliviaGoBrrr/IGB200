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

    [SerializeField] private GridManager gridManager;
    public Camera sceneCamera;
    public GameState state;
    public static event System.Action OnRoundAdvanced;

    public int roundCount = 1;
    public int maxActions = 3;
    
    private Vector3 lastMousePosition;
    void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        
    }

    public void AdvanceRound()
    {
        roundCount++;
        OnRoundAdvanced?.Invoke();
    }

    public Vector3 GetSelectedGridPosition()
    {
        Vector3 mousePositon = Input.mousePosition;
        mousePositon.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePositon);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lastMousePosition = hit.point;
        }

        return lastMousePosition;
    }

}
