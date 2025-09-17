using UnityEngine;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private VisualElement ui;

    private Button settingsButton;
    private TemplateContainer settings;

    private Button backButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        settingsButton = ui.Q<Button>("SettingsButton");
        settingsButton.clicked += OnSettingsButtonClicked;

        settings = ui.Q<TemplateContainer>("Settings");
        settings.style.display = DisplayStyle.None;

        backButton = ui.Q<Button>("BackButton");
        backButton.clicked += OnBackButtonClicked;
    }

    private void OnSettingsButtonClicked()
    {
        settings.style.display = DisplayStyle.Flex; // visibility = true
    }

    private void OnBackButtonClicked()
    {
        sceneLoader.LoadNextScene("Level Select");
    }
}
