using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using UnityEditor.SearchService;
using UnityEngine.Rendering;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class Settings : MonoBehaviour
{
    private VisualElement ui;
    private TemplateContainer settings;
    private VisualElement settingsPage;

    private Button backButton;
    private Button backBGButton;

    private Button volumeButton;
    private Button musicButton;
    
    private Sprite volumeOn;
    private Sprite volumeOff;
    private Sprite musicOn;
    private Sprite musicOff;
    
    [SerializeField] AudioMixer audioMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;

        // Get sprites from folder
        volumeOn = Resources.Load<Sprite>("UI Load in Code/VolumeOn");
        volumeOff = Resources.Load<Sprite>("UI Load in Code/VolumeOff");

        musicOn = Resources.Load<Sprite>("UI Load in Code/MusicOn");
        musicOff = Resources.Load<Sprite>("UI Load in Code/MusicOff");

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
        if (SoundFXVol == 0)
        { 
            audioMixer.SetFloat("VolumeSoundFX", -80f);


            volumeButton.style.backgroundImage = new StyleBackground(volumeOff);
        }
        else
        { 
            audioMixer.SetFloat("VolumeSoundFX", 0f);

            
            volumeButton.style.backgroundImage = new StyleBackground(volumeOn);
        }
        FindAnyObjectByType<MenuAudio>().PlayButtonClick(2);
    }

    private void OnMusicButtonClicked()
    {
        audioMixer.GetFloat("VolumeMusic", out float MusicVol);
        if (MusicVol == 0) 
        { 
            audioMixer.SetFloat("VolumeMusic", -80f);
            musicButton.style.backgroundImage = new StyleBackground(musicOff);
        }
        else 
        { 
            audioMixer.SetFloat("VolumeMusic", 0f);
            musicButton.style.backgroundImage = new StyleBackground(musicOn);
        }
        FindAnyObjectByType<MenuAudio>().PlayButtonClick(6);

        
    }
}
