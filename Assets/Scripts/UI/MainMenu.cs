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
    

    private SceneLoader sceneLoader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }

    private void OnPlayButtonClicked()
    {
        sceneLoader.LoadNextScene("GameLevel");
    }

    private void OnSettingsButtonClicked()
    {
        print("Settings Pressed!");
        settings.style.display = DisplayStyle.Flex;

    }

    private void OnExitButtonClicked()
    {
        print("Exit Pressed!");
        //Application.Quit();
    }

}
