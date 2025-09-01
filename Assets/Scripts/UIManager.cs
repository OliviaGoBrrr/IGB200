using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private GameObject GameOverScreen;
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
