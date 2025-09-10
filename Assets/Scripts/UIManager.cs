using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    
    private Canvas canvas;
    public GameObject LoadingScreen;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject GameWinScreen;

    // tutorial screen
    public GameObject TutorialScreen;

    
    [SerializeField] private TMP_Text starCountText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private string roundsString;
    [SerializeField] private TMP_Text actionsText;
    [SerializeField] private string actionsString;
    private float warningTimer;
    private float warningTime = 2.0f;
    [SerializeField] private TMP_Text warningTextObject;
    private GameManager gameManager;


    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Start()
    {
        
        // Show the tutorial screen right at the start of the level
        ShowTutorialScreen();

        
        UpdatePlayerUI();
        GameOverScreen.SetActive(false);
        PauseScreen.SetActive(false);
    }

    void Update()
    {
        // Warning timer logic 
        if (warningTextObject.gameObject.activeSelf)
        {
            warningTimer -= Time.deltaTime;
            if (warningTimer < 0)
            {
                warningTextObject.gameObject.SetActive(false);
            }
        }
    }


    /// Shows the tutorial screen and pauses the game.

    public void ShowTutorialScreen()
    {
        TutorialScreen.SetActive(true);
        Time.timeScale = 0f;
    }
    
    /// Hides the tutorial screen and unpauses the game.
    
    public void HideTutorialScreen()
    {
        TutorialScreen.SetActive(false);
        Time.timeScale = 1f; 
    }

    

    private void DisplayWarning()
    {
        warningTextObject.gameObject.SetActive(true);
        warningTimer = warningTime;
    }

    private void DisplayGameOverUI()
    {
        GameOverScreen.SetActive(true);
    }

    public void UpdatePlayerUI()
    {
        roundText.SetText(roundsString + " " + gameManager.roundCount);
        actionsText.SetText(gameManager.currentActionCount + "/" + gameManager.maxActions + " " + actionsString);
    }

    public void DisplayGameWinUI(int starCount)
    {
        GameWinScreen.SetActive(true);
        starCountText.SetText($"Stars: {starCount}");
    }

    private void OnEnable()
    {
        GameManager.OnRoundAdvanced += UpdatePlayerUI;
        GameManager.OnPlayerAction += UpdatePlayerUI;
        GameManager.OnActionCostTooHigh += DisplayWarning;
        GameManager.OnGameOver += DisplayGameOverUI;
    }

    private void OnDisable()
    {
        GameManager.OnRoundAdvanced -= UpdatePlayerUI;
        GameManager.OnPlayerAction -= UpdatePlayerUI;
        GameManager.OnActionCostTooHigh -= DisplayWarning;
        GameManager.OnGameOver -= DisplayGameOverUI;
    }
}
