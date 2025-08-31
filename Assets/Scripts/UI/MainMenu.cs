using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using UnityEditor.SearchService;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private VisualElement ui;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button settingsButton;

    [SerializeField]
    private Button exitButton;

    private SceneLoader sceneLoader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;
        OnEnable();
    }

    private void OnEnable()
    {
        // Fetch UI elements by name
        playButton = ui.Q<Button>("PlayButton");
        playButton.clicked += OnPlayButtonClicked;

        settingsButton = ui.Q<Button>("SettingsButton");
        settingsButton.clicked += OnSettingsButtonClicked;

        exitButton = ui.Q<Button>("ExitButton");
        exitButton.clicked += OnExitButtonClicked;
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("AAA");

        sceneLoader.LoadNextScene("Main Menu");
    }

    private void OnSettingsButtonClicked()
    {

    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

}
