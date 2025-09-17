using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class LevelSelectManager : MonoBehaviour
{
    
    private SceneLoader sceneLoader;

    private VisualElement ui;

    private Button settingsButton;
    private TemplateContainer settings;

    private Button backButton;

    private Button level1;
    private Button level2;
    private Button level3;

    private Array allLevels;


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


        level1 = ui.Q<Button>("LevelButton1");
        level2 = ui.Q<Button>("LevelButton2");
        level3 = ui.Q<Button>("LevelButton3");

        allLevels = new Button[]
        {
            level1, level2, level3 // add level vars to this array and it will auto-write out the other bit of code needed
        };

        foreach (Button button in allLevels)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }
    }

    private void OnSettingsButtonClicked()
    {
        print("Settings Pressed!");
        settings.style.display = DisplayStyle.Flex; // visibility = true
    }

    private void OnBackButtonClicked()
    {
        sceneLoader.LoadNextScene("Main Menu");
    }

    private void Clickable_clickedWithEventInfo(EventBase obj)
    {
        var button = (Button)obj.target;
        string selectedLevel = button.text;
        sceneLoader.LoadNextScene("GameLevel" + button.text);
    }

    /*
    public void LoadLevel(string sceneToLoad)
    {
        // Check if the sceneLoader reference is assigned to prevent errors.
        if (sceneLoader != null)
        {
            sceneLoader.LoadNextScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("SceneLoader is not assigned. Please assign the SceneLoader GameObject to the SceneLoader variable in the Inspector.");
        }
    }
    */
}
