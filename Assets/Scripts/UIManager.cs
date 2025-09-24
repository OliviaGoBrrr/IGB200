using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Screens")]
    public GameObject LoadingScreen;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject GameWinScreen;

    [Header("Player UI")]
    [SerializeField] private TMP_Text starCountText;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite filledStar;
    [SerializeField] public TMP_Text scoreText;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Start()
    {
        GameOverScreen.SetActive(false);
        PauseScreen.SetActive(false);
    }

    private void DisplayGameOverUI()
    {
        GameOverScreen.SetActive(true);
    }



    public void DisplayGameWinUI(int starCount)
    {
        GameWinScreen.SetActive(true);
        starCountText.SetText($"Stars: {starCount}");
        for (int i = 0; i < starCount; i++)
        {
            starImages[i].sprite = filledStar;
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += DisplayGameOverUI;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= DisplayGameOverUI;
    }
}