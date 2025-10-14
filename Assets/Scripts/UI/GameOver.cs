using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : UIAnimations
{
    private VisualElement ui;

    private SceneLoader sceneLoader;
    bool sceneLoad = false; // Prevents scenes from loading multiple times

    private Button homeButton;
    private Button restartButton;
    private Button levelSelectButton;


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

        levelSelectButton = ui.Q<Button>("LevelSelectButton");
        levelSelectButton.clicked += OnBackButtonClicked;
    }


    private void OnHomeButtonClicked()
    {
        ButtonPressed(homeButton);
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
        ButtonPressed(restartButton);
        if (sceneLoad == false)
        {
            sceneLoad = true;
            //FindAnyObjectByType<MenuAudio>().PlayButtonClick(0);
            sceneLoader.LoadNextScene("GameLevel" + ScoreData.currentLevel);
            StartCoroutine(FindAnyObjectByType<SceneAudio>().DestroySelf(0.5f));
        }
    }
    /*
    private void OnNextLevelButtonClicked()
    {
        ButtonPressed(nextLevelButton);
        if (sceneLoad == false)
        {
            sceneLoad = true;
            //FindAnyObjectByType<MenuAudio>().PlayButtonClick(0);
            if (ScoreData.currentLevel == 8)
            {
                sceneLoader.LoadNextScene("Main Menu");
            }
            else
            {
                ScoreData.currentLevel += 1;
                sceneLoader.LoadNextScene("GameLevel" + ScoreData.currentLevel);
            }
            StartCoroutine(FindAnyObjectByType<SceneAudio>().DestroySelf(0.5f));
        }
    }
    */

    private void OnBackButtonClicked()
    {
        ButtonPressed(levelSelectButton);
        if (sceneLoad == false)
        {
            sceneLoad = true;
            //FindAnyObjectByType<MenuAudio>().PlayButtonClick(0);
            sceneLoader.LoadNextScene("Level Select");
            StartCoroutine(FindAnyObjectByType<SceneAudio>().DestroySelf(0.5f));
        }
    }
}
