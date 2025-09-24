using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using TMPro;
using static UnityEditor.Rendering.FilterWindow;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using UnityEngine.SocialPlatforms.Impl;

public class LevelSelectManager : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private VisualElement ui;

    private Button settingsButton;
    private TemplateContainer settings;

    private Button backButton;

    private VisualElement level1Container;
    private VisualElement level2Container;
    private VisualElement level3Container;
    private VisualElement level4Container;
    private VisualElement level5Container;
    private VisualElement level6Container;
    private VisualElement level7Container;
    private VisualElement level8Container;

    private VisualElement[] levelContainers = new VisualElement[8];

    private Array allLevels;
    bool sceneLoad = false; // Prevents scenes from loading multiple times


    private Color starColour = new Color(1, 0.925f, 0.513f);

    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;

        // connects the var to the correct element in the hierarchy

        for (int i = 0; i < levelContainers.Length; i++)
        {
            levelContainers[i] = ui.Q<TemplateContainer>("LevelContainer" + (i + 1));
            print(levelContainers[i]);
            SetStarColour(i, levelContainers[i]);
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
        print("Settings Pressed!");
        settings.style.display = DisplayStyle.Flex; // visibility = true
    }

    private void OnBackButtonClicked()
    {
        if (sceneLoad == false)
        {
            sceneLoad = true;
            FindAnyObjectByType<MenuAudio>().PlayButtonClick(6);
            sceneLoader.LoadNextScene("Main Menu");
        }
    }

    private void Clickable_clickedWithEventInfo(EventBase obj)
    {
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
        if (ScoreData.levelScores[level] == 3) return; // dont check if its already at 3 stars

        int i = 0;
        while (i < ScoreData.levelScores[level])
        {
            levelContainer.Q<VisualElement>("Star" + (i + 1)).style.unityBackgroundImageTintColor = starColour;
            i++;
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
