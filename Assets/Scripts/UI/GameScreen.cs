using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private VisualElement ui;

    private Button settingsButton;
    private TemplateContainer settings;

    private Button playSimButton;
    private Button undoButton;
    private Button restartButton;

    public GameManager gameManager;

    private Button backButton;
    bool sceneLoad = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        playSimButton = ui.Q<Button>("PlaySimButton");
        playSimButton.clicked += OnPlaySimButtonClicked;

        undoButton = ui.Q<Button>("UndoButton");
        undoButton.clicked += OnUndoButtonClicked;

        restartButton = ui.Q<Button>("RestartButton");
        restartButton.clicked += OnRestartButtonClicked;

        settingsButton = ui.Q<Button>("SettingsButton");
        settingsButton.clicked += OnSettingsButtonClicked;

        settings = ui.Q<TemplateContainer>("Settings");
        settings.style.display = DisplayStyle.None;

        backButton = ui.Q<Button>("BackButton");
        backButton.clicked += OnBackButtonClicked;
    }

    private void OnPlaySimButtonClicked()
    {
        gameManager.PlaySimulation();
    }

    private void OnUndoButtonClicked()
    {
        gameManager.UndoPlayerAction();
    }

    private void OnRestartButtonClicked()
    {
        sceneLoader.ReloadCurrentScene();
    }

    private void OnSettingsButtonClicked()
    {
        settings.style.display = DisplayStyle.Flex; // visibility = true
        FindAnyObjectByType<MenuAudio>().PlayButtonClick(10);
    }

    private void OnBackButtonClicked()
    {
        if (sceneLoad == false)
        {
            sceneLoad = true;
            FindAnyObjectByType<SceneAudio>().PlayButtonClick(6);
            sceneLoader.LoadNextScene("Level Select");
            StartCoroutine(FindAnyObjectByType<SceneAudio>().DestroySelf(0.5f));
        }
    }
}
