using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using UnityEditor.SearchService;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{
    public MenuAudio menuAudio;

    private VisualElement ui;

    private Button playButton;

    private Button settingsButton;
    private TemplateContainer settings;

    private Button exitButton;

    private Button customiseButton;
    private Button customisePanelButton;


    private SceneLoader sceneLoader;


    // customisation stuffs
    private VisualElement characterBangs;
    private VisualElement characterHighlights;
    private VisualElement characterEyes;
    private VisualElement characterSclera;
    private VisualElement characterHead;
    private VisualElement characterBody;
    private VisualElement characterHair;
    private VisualElement characterHat;

    [SerializeField] private Sprite characterFaceSmile;
    [SerializeField] private Sprite characterFaceNormal;

    [SerializeField] private Sprite characterBodySmile;
    [SerializeField] private Sprite characterBodyNormal;

    private Color newColour;

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
        customisePanelButton = ui.Q<Button>("CustomisePanelButton");
        customisePanelButton.clicked += OnCustomiseButtonClicked;

        customiseButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);
        customisePanelButton.RegisterCallback<PointerEnterEvent>(OnPointerEnterEvent, TrickleDown.TrickleDown);

        customiseButton.RegisterCallback<PointerLeaveEvent>(OnPointerLeaveEvent, TrickleDown.TrickleDown);
        customisePanelButton.RegisterCallback<PointerLeaveEvent>(OnPointerLeaveEvent, TrickleDown.TrickleDown);

        // Customisation Stuff

        characterBangs = ui.Q<VisualElement>("CharacterBangs");
        characterHighlights = ui.Q<VisualElement>("CharacterHighlights");
        characterEyes = ui.Q<VisualElement>("CharacterEyes");
        characterSclera = ui.Q<VisualElement>("CharacterSclera");
        characterHead = ui.Q<VisualElement>("CharacterHead");
        characterBody = ui.Q<VisualElement>("CharacterBody");
        characterHair = ui.Q<VisualElement>("CharacterHair");
        
        characterHat = ui.Q<VisualElement>("CharacterHat");

        // set character base colours immediately

        ColorUtility.TryParseHtmlString(CustomiseData.skinColour, out newColour);
        characterHead.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(CustomiseData.clothesColour, out newColour);
        print(newColour);
        characterBody.style.unityBackgroundImageTintColor = newColour;
        characterHat.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(CustomiseData.hairColour, out newColour);
        characterHair.style.unityBackgroundImageTintColor = newColour;
        characterBangs.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(CustomiseData.eyeColour, out newColour);
        characterEyes.style.unityBackgroundImageTintColor = newColour;

    }

    private void OnPointerEnterEvent(PointerEnterEvent evt)
    {
        characterHead.style.backgroundImage = new StyleBackground(characterFaceSmile);
        characterBody.style.backgroundImage = new StyleBackground(characterBodySmile);
    }

    private void OnPointerLeaveEvent(PointerLeaveEvent evt)
    {
        characterHead.style.backgroundImage = new StyleBackground(characterFaceNormal);
        characterBody.style.backgroundImage = new StyleBackground(characterBodyNormal);
    }

    private void OnPlayButtonClicked()
    {
        menuAudio.PlayButtonClick(1);
        sceneLoader.LoadNextScene("Level Select");
    }

    private void OnCustomiseButtonClicked()
    {
        menuAudio.PlayButtonClick(20);
        sceneLoader.LoadNextScene("Customise Menu");
    }

    private void OnSettingsButtonClicked()
    {
        settings.style.display = DisplayStyle.Flex; // visibility = true
        menuAudio.PlayButtonClick(5);
    }

    private void OnExitButtonClicked()
    {
        print("Exit Pressed!");
        //Application.Quit();
    }

}
