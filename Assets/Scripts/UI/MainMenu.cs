using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
//using UnityEditor.SearchService;
using UnityEngine.Rendering;

public class MainMenu : UIAnimations
{
    private VisualElement ui;

    private Button playButton;

    private Button settingsButton;
    private TemplateContainer settings;

    private Button exitButton;

    private Button customiseButton;
    private Button customisePanelButton;

    private VisualElement koalaContainer;
    private Button koalaButton;

    private SceneLoader sceneLoader;
    bool sceneLoad = false; // Prevents scenes from loading multiple times

    // customisation stuffs
    private VisualElement characterBangs;
    private VisualElement characterHighlights;
    private VisualElement characterEyes;
    private VisualElement characterSclera;
    private VisualElement characterHead;
    private VisualElement characterBody;
    private VisualElement characterHair;
    private VisualElement characterHat;

    private Sprite bangsStyle;
    private Sprite hairStyle;
    

    [SerializeField] private Sprite characterFaceSmile;
    [SerializeField] private Sprite characterFaceNormal;

    [SerializeField] private Sprite characterBodySmile;
    [SerializeField] private Sprite characterBodyNormal;

    [SerializeField] private Sprite characterEyeSmile;
    [SerializeField] private Sprite characterEyeNormal;

    private Color newColour;

    private float koalaScale = 1f;

    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;
        bangsStyle = Resources.Load<Sprite>("Sprites/PlayerCharacter/Bangs/PlayerCharacter_Bangs_0" + CustomiseData.bangsType);
        hairStyle = Resources.Load<Sprite>("Sprites/PlayerCharacter/Hair/PlayerCharacter_Hair_0" + CustomiseData.hairType);
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


        koalaContainer = ui.Q<VisualElement>("KoalaContainer");
        koalaButton = ui.Q<Button>("KoalaButton");
        koalaButton.clicked += OnKoalaClicked;

        // Hover Button

        customiseButton.RegisterCallback<PointerEnterEvent>(OnCustomisePointerEnterEvent, TrickleDown.TrickleDown);
        customisePanelButton.RegisterCallback<PointerEnterEvent>(OnCustomisePointerEnterEvent, TrickleDown.TrickleDown);

        customiseButton.RegisterCallback<PointerLeaveEvent>(OnCustomisePointerLeaveEvent, TrickleDown.TrickleDown);
        customisePanelButton.RegisterCallback<PointerLeaveEvent>(OnCustomisePointerLeaveEvent, TrickleDown.TrickleDown);

        playButton.RegisterCallback<PointerEnterEvent>(OnPlayPointerEnterEvent, TrickleDown.TrickleDown);
        playButton.RegisterCallback<PointerLeaveEvent>(OnPlayPointerLeaveEvent, TrickleDown.TrickleDown);

        // Customisation Stuff

        characterBangs = ui.Q<VisualElement>("CharacterBangs");
        characterHighlights = ui.Q<VisualElement>("CharacterHighlights");
        characterEyes = ui.Q<VisualElement>("CharacterEyes");
        characterSclera = ui.Q<VisualElement>("CharacterSclera");
        characterHead = ui.Q<VisualElement>("CharacterHead");
        characterBody = ui.Q<VisualElement>("CharacterBody");
        characterHair = ui.Q<VisualElement>("CharacterHair");
        
        characterHat = ui.Q<VisualElement>("CharacterHat");

        // set character base colours

        ColorUtility.TryParseHtmlString(CustomiseData.skinColour, out newColour);
        characterHead.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(CustomiseData.clothesColour, out newColour);
        characterBody.style.unityBackgroundImageTintColor = newColour;
        characterHat.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(CustomiseData.hairColour, out newColour);
        characterHair.style.unityBackgroundImageTintColor = newColour;
        characterBangs.style.unityBackgroundImageTintColor = newColour;

        if (CustomiseData.alienMode == true)
        {
            characterEyes.style.unityBackgroundImageTintColor = Color.black;
            characterSclera.style.unityBackgroundImageTintColor = Color.black;
        }
        else
        {
            ColorUtility.TryParseHtmlString(CustomiseData.eyeColour, out newColour);
            characterEyes.style.unityBackgroundImageTintColor = newColour;

            characterSclera.style.unityBackgroundImageTintColor = Color.white;
        }
            

        // set character hair & bangs style

        characterBangs.style.backgroundImage = new StyleBackground(bangsStyle);
        characterHair.style.backgroundImage = new StyleBackground(hairStyle);
    }


    // Hover Methods

    private void OnCustomisePointerEnterEvent(PointerEnterEvent evt)
    {
        characterHead.style.backgroundImage = new StyleBackground(characterFaceSmile);
        characterBody.style.backgroundImage = new StyleBackground(characterBodySmile);
        characterEyes.style.backgroundImage = new StyleBackground(characterEyeSmile);

        characterSclera.style.display = DisplayStyle.None;
        characterHighlights.style.display = DisplayStyle.None;
    }

    private void OnCustomisePointerLeaveEvent(PointerLeaveEvent evt)
    {
        characterHead.style.backgroundImage = new StyleBackground(characterFaceNormal);
        characterBody.style.backgroundImage = new StyleBackground(characterBodyNormal);
        characterEyes.style.backgroundImage = new StyleBackground(characterEyeNormal);

        characterSclera.style.display = DisplayStyle.Flex;
        characterHighlights.style.display = DisplayStyle.Flex;
    }

    private void OnPlayPointerEnterEvent(PointerEnterEvent evt)
    {
        //playButton.style.scale = new Scale(new Vector3(1.1f, 1.1f, 1));
    }

    private void OnPlayPointerLeaveEvent(PointerLeaveEvent evt)
    {
        //playButton.style.scale = new Scale(new Vector3(1f, 1f, 1));
    }

    // Click Methods

    private void OnKoalaClicked()
    {
        DOTween.KillAll();

        DOTween.To(() => koalaScale, x => koalaScale = x, 0.8f, 0.08f).SetEase(Ease.OutSine).OnUpdate(() =>
        {
            koalaContainer.style.scale = new Vector2(koalaScale, koalaScale);
        }).OnComplete(() =>
        {
            DOTween.To(() => koalaScale, x => koalaScale = x, 1f, 0.8f).SetEase(Ease.OutElastic).OnUpdate(() =>
            {
                koalaContainer.style.scale = new Vector2(koalaScale, koalaScale);
            });
        });
        FindAnyObjectByType<MenuAudio>().PlaySqueak(0);
    }

    private void OnPlayButtonClicked()
    {
        ButtonPressed(playButton);

        if (sceneLoad == false)
        {
            sceneLoad = true;
            FindAnyObjectByType<MenuAudio>().PlayButtonClick(0);
            sceneLoader.LoadNextScene("Level Select");
        }

        //playButton.style.scale = new Scale(new Vector3(0.8f, 0.8f, 1));
    }

    private void OnCustomiseButtonClicked()
    {
        if (sceneLoad == false)
        {
            sceneLoad = true;
            FindAnyObjectByType<MenuAudio>().PlayButtonClick(4);
            sceneLoader.LoadNextScene("Customise Menu");
        }
    }

    private void OnSettingsButtonClicked()
    {
        ButtonPressed(settingsButton);

        FindAnyObjectByType<MenuAudio>().PlayButtonClick(8);
        settings.style.display = DisplayStyle.Flex; // visibility = true
    }

    private void OnExitButtonClicked()
    {
        ButtonPressed(exitButton);

        print("Exit Pressed!");
        //Application.Quit();
    }

}
