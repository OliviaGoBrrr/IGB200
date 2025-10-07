using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    private VisualElement ui;

    private SceneLoader sceneLoader;
    bool sceneLoad = false; // Prevents scenes from loading multiple times

    private Button homeButton;
    private Button restartButton;
    private Button nextLevelButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    // Update is called once per frame
    void OnEnable()
    {
        homeButton = ui.Q<Button>("HomeButton");
        homeButton.clicked += OnHomeButtonClicked;

        restartButton = ui.Q<Button>("GameOverRestartButton");
        restartButton.clicked += OnGameOverRestartButtonClicked;

        nextLevelButton = ui.Q<Button>("NextLevelButton");
        nextLevelButton.clicked += OnNextLevelButtonClicked;
    }


    private void OnHomeButtonClicked()
    {
        if (sceneLoad == false)
        {
            sceneLoad = true;
            //FindAnyObjectByType<MenuAudio>().PlayButtonClick(0);
            sceneLoader.LoadNextScene("Main Menu");
            StartCoroutine(FindAnyObjectByType<SceneAudio>().DestroySelf(0.5f));
        }
    }

    private void OnGameOverRestartButtonClicked()
    {
        if (sceneLoad == false)
        {
            sceneLoad = true;
            //FindAnyObjectByType<MenuAudio>().PlayButtonClick(0);
            sceneLoader.LoadNextScene("GameLevel" + ScoreData.currentLevel);
            StartCoroutine(FindAnyObjectByType<SceneAudio>().DestroySelf(0.5f));
        }
    }

    private void OnNextLevelButtonClicked()
    {
        if (sceneLoad == false)
        {
            sceneLoad = true;
            //FindAnyObjectByType<MenuAudio>().PlayButtonClick(0);
            ScoreData.currentLevel += 1;
            sceneLoader.LoadNextScene("GameLevel" + ScoreData.currentLevel);
        }
    }

}
