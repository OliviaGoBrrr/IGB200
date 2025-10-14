using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using TMPro;
//using static UnityEditor.Rendering.FilterWindow;
//using static UnityEngine.Rendering.DebugUI.MessageBox;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using System.Linq;

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

    private int starTotal;

    private Color starColour = new Color(1, 0.866f, 0.2f);

    void Awake()
    {
        starTotal = ScoreData.levelScores.Sum();

        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;

        // connects the var to the correct element in the hierarchy

        for (int i = 0; i < levelContainers.Length; i++)
        {
            levelContainers[i] = ui.Q<TemplateContainer>("LevelContainer" + (i + 1));
            SetLevelNumber(i, levelContainers[i]);
            SetLevelState(i, levelContainers[i]);
            //print("Level " + i + " score is " + ScoreData.levelScores[i]);
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
        var button = (Button)obj.target;

        if (button.Q<VisualElement>("LockIcon").style.display == DisplayStyle.Flex)
        {
            return;
        }

        ButtonPressed((Button)obj.target);
        if (sceneLoad == false)
        {
            sceneLoad = true;
            FindAnyObjectByType<MenuAudio>().PlayButtonClick(0);
            
            string selectedLevel = button.text;
            ScoreData.currentLevel = int.Parse(selectedLevel); // set current level
            sceneLoader.LoadNextScene("GameLevel" + button.text);
            StartCoroutine(FindAnyObjectByType<MenuAudio>().DestroySelf(0.5f));
        }
    }


    private void SetLevelState(int level, VisualElement levelContainer)
    {
        SetStarColour(level, levelContainer);

        SetLevelUnlock(level, levelContainer);
    }


    private void SetStarColour(int level, VisualElement levelContainer)
    {
        if (ScoreData.levelScores[level] == 0) return; // dont check if level at 0 stars

        for (int i = 0; i < ScoreData.levelScores[level]; i++)
        {
            levelContainer.Q<VisualElement>("Star" + (i + 1)).style.unityBackgroundImageTintColor = starColour;
        }
    }

    private void SetLevelUnlock(int level, VisualElement levelContainer)
    {
        print(ScoreData.levelStarRequirements[level] + " , " + starTotal);
        if (ScoreData.levelStarRequirements[level] <= starTotal)
        {
            levelContainer.Q<Button>("LevelButton").style.color = new StyleColor(new Color(0.94f, 0.87f, 0.815f, 1f));
            levelContainer.Q<VisualElement>("LockIcon").style.display = DisplayStyle.None;
            levelContainer.Q<VisualElement>("AllStars").style.display = DisplayStyle.Flex;
        }
        else
        {
            levelContainer.Q<Button>("LevelButton").style.color = new StyleColor(new Color(0.94f, 0.87f, 0.815f, 0f));
            levelContainer.Q<VisualElement>("LockIcon").style.display = DisplayStyle.Flex;
            levelContainer.Q<VisualElement>("AllStars").style.display = DisplayStyle.None;
        }
    }

    private void SetLevelNumber(int level, VisualElement levelContainer)
    {
        levelContainer.Q<Button>("LevelButton").text = (level + 1).ToString();
    }
}
