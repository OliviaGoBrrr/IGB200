using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private Canvas canvas;
    public GameObject LoadingScreen;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    
    public GameObject GameWinScreen;
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
        UpdatePlayerUI();
        GameOverScreen.SetActive(false);
        PauseScreen.SetActive(false);
    }

    void Update()
    {
        // Warning timer
        if (warningTextObject.gameObject.activeSelf)
        {
            warningTimer -= Time.deltaTime;

            if(warningTimer < 0)
            {
                warningTextObject.gameObject.SetActive(false);
            }
        }

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
        // Sets the text in the UI to whatever is set
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
