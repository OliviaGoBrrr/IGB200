using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using UnityEditor.SearchService;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{
    private VisualElement ui;

    private Button playButton;

    private Button settingsButton;
    private TemplateContainer settings;

    private Button exitButton;

    private Button customiseButton;

    private Button levelButton;


    private SceneLoader sceneLoader;

    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        // Fetch UI elements by name
        playButton = ui.Q<Button>("PlayButton");
        playButton.clicked += OnPlayButtonClicked;

        settingsButton = ui.Q<Button>("SettingsButton");
        settingsButton.clicked += OnSettingsButtonClicked;

        settings = ui.Q<TemplateContainer>("Settings");
        settings.style.display = DisplayStyle.None;

        exitButton = ui.Q<Button>("ExitButton");
        exitButton.clicked += OnExitButtonClicked;

        customiseButton = ui.Q<Button>("CustomiseButton");
        customiseButton.clicked += OnCustomiseButtonClicked;

        levelButton = ui.Q<Button>("LevelButton");
        levelButton.clicked += OnLevelButtonClicked;
    }

    private void OnPlayButtonClicked()
    {
        sceneLoader.LoadNextScene("GameLevel1");
    }

    private void OnCustomiseButtonClicked()
    {
        sceneLoader.LoadNextScene("Customise Menu");
    }

    private void OnLevelButtonClicked()
    {
        sceneLoader.LoadNextScene("Level Select");
    }

    private void OnSettingsButtonClicked()
    {
        print("Settings Pressed!");
        settings.style.display = DisplayStyle.Flex; // visibility = true

    }

    private void OnExitButtonClicked()
    {
        print("Exit Pressed!");
        //Application.Quit();
    }

}
