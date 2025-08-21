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

    public GameState state;
    public static event System.Action OnRoundAdvanced;


    [SerializeField] private float burnTimer = 2.0f;
    [SerializeField] private GridManager gridManager;
    void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
    }

    void Update()
    {
        burnTimer -= Time.deltaTime;

        if(burnTimer <= 0)
        {
            OnRoundAdvanced?.Invoke();
            burnTimer = 2.0f;
        }
    }
}
