using UnityEngine;

public class GameTile: MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int roundsToBurn;
    [SerializeField] private bool canBeBurned;
    [SerializeField] private bool isBurning;
    private Vector3 tilePosition;


    private GameTileData tileData;

    // Placeholder
    private float burnTimer = 10.0f;

    void Awake()
    {
        tilePosition = transform.position;
        tileData = new GameTileData(roundsToBurn, canBeBurned, isBurning, tilePosition);
    }

    void Update()
    {
        if (isBurning)
        {
            burnTimer -= Time.deltaTime;
            
            if (burnTimer < 0)
            {
                burnTimer = 10.0f;
            }
        }
    }
}

public struct GameTileData
{
    public GameTileData(int burnRounds, bool canBeBurned, bool isBurning, Vector3 tilePosition)
    {
        this.IsBurning = isBurning;
        this.CanBeBurned = canBeBurned;
        this.RoundsToBurn = burnRounds;
        this.TilePosition = tilePosition;
    }

    public int RoundsToBurn;
    public bool CanBeBurned;
    public bool IsBurning;

    public Vector3 TilePosition;
}

