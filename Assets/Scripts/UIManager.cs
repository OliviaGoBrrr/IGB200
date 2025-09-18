using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Screens")]
    public GameObject LoadingScreen;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject GameWinScreen;

    [Header("Player UI")]
    [SerializeField] private TMP_Text starCountText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private string roundsString;
    [SerializeField] private string actionsString;

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

    private void DisplayGameOverUI()
    {
        GameOverScreen.SetActive(true);
    }

    public void UpdatePlayerUI()
    {
        roundText.SetText(roundsString + " " + gameManager.roundCount);
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
        GameManager.OnGameOver += DisplayGameOverUI;
    }

    private void OnDisable()
    {
        GameManager.OnRoundAdvanced -= UpdatePlayerUI;
        GameManager.OnPlayerAction -= UpdatePlayerUI;
        GameManager.OnGameOver -= DisplayGameOverUI;
    }
}