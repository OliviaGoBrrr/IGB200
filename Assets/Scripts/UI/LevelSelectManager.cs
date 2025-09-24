using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using TMPro;

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

    private Array allLevels;
    bool sceneLoad = false; // Prevents scenes from loading multiple times

    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;

        level1Container = ui.Q<TemplateContainer>("LevelContainer1");
        level2Container = ui.Q<TemplateContainer>("LevelContainer2");
        level3Container = ui.Q<TemplateContainer>("LevelContainer3");
        level4Container = ui.Q<TemplateContainer>("LevelContainer4");
        level5Container = ui.Q<TemplateContainer>("LevelContainer5");
        level6Container = ui.Q<TemplateContainer>("LevelContainer6");
        level7Container = ui.Q<TemplateContainer>("LevelContainer7");
        level8Container = ui.Q<TemplateContainer>("LevelContainer8");
    }

    private void OnEnable()
    {
        settingsButton = ui.Q<Button>("SettingsButton");
        settingsButton.clicked += OnSettingsButtonClicked;

        settings = ui.Q<TemplateContainer>("Settings");
        settings.style.display = DisplayStyle.None;

        backButton = ui.Q<Button>("BackButton");
        backButton.clicked += OnBackButtonClicked;


        allLevels = new VisualElement[]
        {
            level1Container, level2Container, level3Container, level4Container, level5Container, level6Container, level7Container, level8Container // add level vars to this array and it will auto-write out the other bit of code needed
        };

        foreach (VisualElement element in allLevels)
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
            sceneLoader.LoadNextScene("GameLevel" + button.text);
            StartCoroutine(FindAnyObjectByType<MenuAudio>().DestroySelf(0.5f));
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
