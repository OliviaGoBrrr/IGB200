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

    private Button dryGrassButton;
    private Button waterButton;
    private Button fireButton;

    private Label dryGrassText;
    private Label waterText;
    private Label fireText;

    public GameManager gameManager;

    private Button backButton;
    bool sceneLoad = false;

    [SerializeField] private DraggableItem dryObject;
    [SerializeField] private DraggableItem waterObject;
    [SerializeField] private DraggableItem fireObject;

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

    private void OnPlaySimButtonClicked()
    {
        gameManager.PlaySimulation();
    }

    private void OnUndoButtonClicked()
    {
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
