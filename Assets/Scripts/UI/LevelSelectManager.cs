using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using TMPro;
//using static UnityEditor.Rendering.FilterWindow;
//using static UnityEngine.Rendering.DebugUI.MessageBox;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class LevelSelectManager : UIAnimations
{
    private SceneLoader sceneLoader;

    private VisualElement ui;

    private Button settingsButton;
    private TemplateContainer settings;

    private Button backButton;

    private VisualElement[] levelContainers = new VisualElement[8];

    private Array allLevels;
    bool sceneLoad = false; // Prevents scenes from loading multiple times


    private Color starColour = new Color(1, 0.866f, 0.2f);

    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;

        // connects the var to the correct element in the hierarchy

        for (int i = 0; i < levelContainers.Length; i++)
        {
            levelContainers[i] = ui.Q<TemplateContainer>("LevelContainer" + (i + 1));
            SetStarColour(i, levelContainers[i]);
            print("Level " + i + " score is " + ScoreData.levelScores[i]);
        }
    }

    private void OnEnable()
    {
        settingsButton = ui.Q<Button>("SettingsButton");
        settingsButton.clicked += OnSettingsButtonClicked;

        settings = ui.Q<TemplateContainer>("Settings");
        settings.style.display = DisplayStyle.None;

        backButton = ui.Q<Button>("BackButton");
        backButton.clicked += OnBackButtonClicked;

        foreach (VisualElement element in levelContainers)
        {
            element.Q<Button>("LevelButton").clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }
    }

    private void OnSettingsButtonClicked()
    {
        ButtonPressed(settingsButton);
        FindAnyObjectByType<MenuAudio>().PlayButtonClick(8);
        settings.style.display = DisplayStyle.Flex; // visibility = true
    }

    private void OnBackButtonClicked()
    {
        ButtonPressed(backButton);
        if (sceneLoad == false)
        {
            sceneLoad = true;
            FindAnyObjectByType<MenuAudio>().PlayButtonClick(6);
            sceneLoader.LoadNextScene("Main Menu");
        }
    }

    private void Clickable_clickedWithEventInfo(EventBase obj)
    {
        ButtonPressed((Button)obj.target);
        if (sceneLoad == false)
        {
            sceneLoad = true;
            FindAnyObjectByType<MenuAudio>().PlayButtonClick(0);
            var button = (Button)obj.target;
            string selectedLevel = button.text;
            ScoreData.currentLevel = int.Parse(selectedLevel); // set current level
            sceneLoader.LoadNextScene("GameLevel" + button.text);
            StartCoroutine(FindAnyObjectByType<MenuAudio>().DestroySelf(0.5f));
        }
    }


    private void SetStarColour(int level, VisualElement levelContainer)
    {
        if (ScoreData.levelScores[level] == 0) return; // dont check if level at 0 stars

        //int i = 0;
        for (int i = 0; i < ScoreData.levelScores[level]; i++)
        {
            levelContainer.Q<VisualElement>("Star" + (i + 1)).style.unityBackgroundImageTintColor = starColour;
            print("star " + (i + 1) + " is filled in");
        }
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
