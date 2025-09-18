using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;


[System.Serializable]
public class TutorialStep
{
    
    public VideoClip videoClip;
    public GameObject[] elementsToShow; 
}

public class UIManager : MonoBehaviour
{

    public GameObject LoadingScreen;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject GameWinScreen;


    public GameObject TutorialScreen;
    public VideoPlayer tutorialVideoPlayer;
    public RawImage tutorialVideoDisplay;

    public GameObject UI; // temp fix

    public TutorialStep[] tutorialSteps; 

    private int currentVideoIndex = 0;


 
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
        ShowTutorialScreen();
        UpdatePlayerUI();
        GameOverScreen.SetActive(false);
        PauseScreen.SetActive(false);

        //UI.SetActive(false);
    }

    void Update()
    {
       
    }


    public void ShowTutorialScreen()
    {
        TutorialScreen.SetActive(true);
        

        // Start with the first tutorial step
        if (tutorialSteps.Length > 0)
        {
            currentVideoIndex = 0;
            ShowTutorialStep(currentVideoIndex);
        }
    }


    public void HideTutorialScreen()
    {
        TutorialScreen.SetActive(false);
        tutorialVideoPlayer.Stop();
        HideAllTutorialElements(); // Make sure all extra UI is hidden
        //UI.SetActive(true);
    }


    public void NextTutorialVideo()
    {
        if (tutorialSteps.Length == 0) return;

        // Move to the next index, wrapping around
        currentVideoIndex = (currentVideoIndex + 1) % tutorialSteps.Length;
        ShowTutorialStep(currentVideoIndex);
    }


    private void ShowTutorialStep(int index)
    {
        if (index < 0 || index >= tutorialSteps.Length) return;

        
        HideAllTutorialElements();

        
        TutorialStep currentStep = tutorialSteps[index];

     
        foreach (GameObject element in currentStep.elementsToShow)
        {
            if (element != null)
            {
                element.SetActive(true);
            }
        }

        
        tutorialVideoPlayer.clip = currentStep.videoClip;
        tutorialVideoPlayer.Play();
    }

    
    private void HideAllTutorialElements()
    {
        foreach (TutorialStep step in tutorialSteps)
        {
            foreach (GameObject element in step.elementsToShow)
            {
                if (element != null)
                {
                    element.SetActive(false);
                }
            }
        }
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