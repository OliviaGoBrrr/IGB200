using UnityEngine;
using UnityEngine.UIElements;

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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        // Fetch UI elements by name
        playButton = ui.Q<Button>("PlayButton");
        playButton.clicked += OnPlayButtonClicked;

        settingsButton = ui.Q<Button>("SettingsButton");
        exitButton = ui.Q<Button>("ExitButton");
    }

    private void OnPlayButtonClicked()
    {

    }
}
