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
    private VisualElement settingsPage;

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

        settingsPage = ui.Q<VisualElement>("SettingsPage");
        settingsPage.style.display = DisplayStyle.None;

        exitButton = ui.Q<Button>("ExitButton");
        exitButton.clicked += OnExitButtonClicked;
    }

    private void OnPlayButtonClicked()
    {
        sceneLoader.LoadNextScene("Main Menu");
    }

    private void OnSettingsButtonClicked()
    {
        print("Settings Pressed!");
        settingsPage.style.display = DisplayStyle.Flex;

    }

    private void OnExitButtonClicked()
    {
        print("Exit Pressed!");
        //Application.Quit();
    }

}
