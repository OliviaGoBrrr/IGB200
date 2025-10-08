using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
public class GameScreen : UIAnimations
{
    private SceneLoader sceneLoader;

    private VisualElement ui;

    private Button settingsButton;
    private TemplateContainer settings;

    private TemplateContainer gameOver;

    private Button nextLevel;
    private VisualElement winPage;
    private VisualElement losePage;
    private VisualElement[] stars = new VisualElement[3];

    private Button playSimButton;
    private Button undoButton;
    private Button restartButton;

    private Button dryGrassButton;
    private Button waterButton;
    private Button fireButton;

    private Label dryGrassText;
    private Label waterText;
    private Label fireText;

    private VisualElement playContainer;
    private VisualElement actionContainer;

    public GameManager gameManager;

    private Button backButton;
    bool sceneLoad = false;

    [SerializeField] private DraggableItem dryObject;
    [SerializeField] private DraggableItem waterObject;
    [SerializeField] private DraggableItem fireObject;

    private Color starColour = new Color(1, 0.866f, 0.2f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i] = ui.Q<VisualElement>("Star" + (i + 1));
        }
    }

    private void OnEnable()
    {
        playContainer = ui.Q<VisualElement>("PlayStuffRight");
        actionContainer = ui.Q<VisualElement>("ActionStuffLeft");

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

        gameOver = ui.Q<TemplateContainer>("GameOver");
        gameOver.style.display = DisplayStyle.None;

        backButton = ui.Q<Button>("BackButton");
        backButton.clicked += OnBackButtonClicked;

        nextLevel = gameOver.Q<Button>("NextLevelButton");
        winPage = gameOver.Q<VisualElement>("Win");
        losePage = gameOver.Q<VisualElement>("Lose");

        dryGrassButton = ui.Q<Button>("GrassButton");
        waterButton = ui.Q<Button>("WaterButton");
        fireButton = ui.Q<Button>("FireButton");

        dryGrassText = ui.Q<Label>("GrassText");
        waterText = ui.Q<Label>("WaterText");
        fireText = ui.Q<Label>("FireText");

        if (dryObject.itemUses == 0)
        {
            dryGrassButton.style.display = DisplayStyle.None;
        }
        else
        {
            dryGrassButton.style.display = DisplayStyle.Flex;
            dryGrassText.text = dryObject.itemUses.ToString();
        }

        if (waterObject.itemUses == 0)
        {
            waterButton.style.display = DisplayStyle.None;
        }
        else
        {
            waterButton.style.display = DisplayStyle.Flex;
            waterText.text = waterObject.itemUses.ToString();
        }

        if (fireObject.itemUses == 0)
        {
            fireButton.style.display = DisplayStyle.None;
        }
        else
        {
            fireButton.style.display = DisplayStyle.Flex;
            fireText.text = fireObject.itemUses.ToString();
        }
    }

    public void DisplayGameWin(int starCount)
    {
        winPage.style.display = DisplayStyle.Flex;
        losePage.style.display = DisplayStyle.None;

        for (int i = 0; i < starCount; i++)
        {
            stars[i].style.unityBackgroundImageTintColor = starColour;
        }

        nextLevel.style.display = DisplayStyle.Flex; // button
        gameOver.style.display = DisplayStyle.Flex;
    }

    public void DisplayGameLose()
    {
        losePage.style.display = DisplayStyle.Flex;
        winPage.style.display = DisplayStyle.None;

        nextLevel.style.display = DisplayStyle.None;
        gameOver.style.display = DisplayStyle.Flex;
    }

    private void OnPlaySimButtonClicked()
    {
        dryObject.Hide();
        waterObject.Hide();
        fireObject.Hide();

        dryGrassButton.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f, 0.47f);
        waterButton.style.unityBackgroundImageTintColor = new Color(0.5f, 0.8f, 1f, 0.47f);
        fireButton.style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f, 0.47f);

        MoveSideButtons(actionContainer, 1);
        MoveSideButtons(playContainer, -1);

        ButtonPressed(playSimButton);
        gameManager.PlaySimulation();
    }

    private void OnUndoButtonClicked()
    {
        ButtonPressed(undoButton);
        gameManager.UndoPlayerAction();
        dryGrassText.text = "x" + dryObject.itemUses.ToString();
        waterText.text = "x" + waterObject.itemUses.ToString();
        fireText.text = "x" + fireObject.itemUses.ToString();
    }

    public void UpdateAllText()
    {
        dryGrassText.text = "x" + dryObject.itemUses.ToString();
        waterText.text = "x" + waterObject.itemUses.ToString();
        fireText.text = "x" + fireObject.itemUses.ToString();
    }

    private void OnRestartButtonClicked()
    {
        ButtonPressed(restartButton);
        sceneLoader.ReloadCurrentScene();
    }

    private void OnSettingsButtonClicked()
    {
        ButtonPressed(settingsButton);
        settings.style.display = DisplayStyle.Flex; // visibility = true
        FindAnyObjectByType<MenuAudio>().PlayButtonClick(10);
    }

    private void OnBackButtonClicked()
    {
        ButtonPressed(backButton);
        if (sceneLoad == false)
        {
            sceneLoad = true;
            FindAnyObjectByType<SceneAudio>().PlayButtonClick(6);
            sceneLoader.LoadNextScene("Level Select");
            StartCoroutine(FindAnyObjectByType<SceneAudio>().DestroySelf(0.5f));
        }
    }


    private void MoveSideButtons(VisualElement element, float direction)
    {
        float buttonPosX = 0f;
        DOTween.To(() => buttonPosX, x => buttonPosX = x, 20f * direction, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            element.style.translate = new Translate(buttonPosX, 0);
        }).OnComplete(() =>
        {
            DOTween.To(() => buttonPosX, x => buttonPosX = x, -110f * direction, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
            {
                element.style.translate = new Translate(buttonPosX, 0);
            });
        });
    }
}
