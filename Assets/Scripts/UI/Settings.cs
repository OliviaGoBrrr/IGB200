using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using UnityEditor.SearchService;
using UnityEngine.Rendering;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    private VisualElement ui;
    private TemplateContainer settings;
    private VisualElement settingsPage;

    private Button backButton;
    private Button backBGButton;

    private Button volumeButton;
    private Button musicButton;

    [SerializeField] AudioMixer audioMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        
    }
    private void OnEnable()
    {
        settings = ui.Q<TemplateContainer>("Settings");

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
        settings.style.display = DisplayStyle.None;
        FindAnyObjectByType<MenuAudio>().PlayButtonClick(10);
    }

    private void OnVolumeButtonClicked()
    {
        audioMixer.GetFloat("VolumeSoundFX", out float SoundFXVol);
        if (SoundFXVol == 0) { audioMixer.SetFloat("VolumeSoundFX", -80f); }
        else { audioMixer.SetFloat("VolumeSoundFX", 0f); }
        FindAnyObjectByType<MenuAudio>().PlayButtonClick(2);
    }

    private void OnMusicButtonClicked()
    {
        audioMixer.GetFloat("VolumeMusic", out float MusicVol);
        if (MusicVol == 0) { audioMixer.SetFloat("VolumeMusic", -80f); }
        else { audioMixer.SetFloat("VolumeMusic", 0f); }
        FindAnyObjectByType<MenuAudio>().PlayButtonClick(6);
    }
}
