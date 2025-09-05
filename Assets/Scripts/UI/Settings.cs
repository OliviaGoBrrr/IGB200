using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using UnityEditor.SearchService;
using UnityEngine.Rendering;

public class Settings : MonoBehaviour
{
    private VisualElement ui;
    private VisualElement settingsPage;

    private Button backButton;
    private Button backBGButton;

    private Button volumeButton;

    private Button musicButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        settingsPage = ui.Q<VisualElement>("SettingsPage");

        backButton = ui.Q<Button>("SettingsBackButton");
        backButton.clicked += OnBackButtonClicked;

        backBGButton = ui.Q<Button>("SettingsBGButton");
        backBGButton.clicked += OnBackButtonClicked;

        volumeButton = ui.Q<Button>("SettingsVolumeButton");
        volumeButton.clicked += OnVolumeButtonClicked;

        musicButton = ui.Q<Button>("SettingsMusicButton");
        musicButton.clicked += OnMusicButtonClicked;
    }

    private void OnBackButtonClicked()
    {
        settingsPage.style.display = DisplayStyle.None;
    }

    private void OnVolumeButtonClicked()
    {
        print("Volume toggled");
    }

    private void OnMusicButtonClicked()
    {
        print("music toggled");
    }
}
