using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using TMPro;
//using static System.TimeZoneInfo;
//using UnityEditor.SearchService;
//using UnityEngine.Rendering;
//using UnityEditor.ShaderGraph.Internal;

public class CustomiseMenu : UIAnimations
{
    private VisualElement ui;

    private SceneLoader sceneLoader;

    private Button backButton;

    private TemplateContainer settings;
    private Button settingsButton;

    private Button currentlySelectedSkin;
    private Button currentlySelectedHat;
    private Button currentlySelectedClothes;
    private Button currentlySelectedHairColour;
    private Button currentlySelectedBangsStyle;
    private Button currentlySelectedHairStyle;
    private Button currentlySelectedEyes;
    private Button currentlySelectedAccessory;

    private Button currentCategory; // last used category
    private string currentCategorySwitch = "Skin"; // used for switch
    private VisualElement currentOptionsPanel;

    // Category Buttons
    private Button skinCategory;
    private Button clothesCategory;
    private Button hairCategory;
    private Button eyesCategory;
    private Button accessoryCategory;

    // Options Panels
    private VisualElement skinPanel;
    private VisualElement clothesPanel;
    private VisualElement hairPanel;
    private VisualElement eyesPanel;
    private VisualElement accessoryPanel;

    // Option Buttons
    private Button[] skinColours = new Button[9];
    private Button[] clothesColours = new Button[9];
    private Button[] hairColours = new Button[13];
    private Button[] eyeColours = new Button[9];

    private Button[] bangsOptions = new Button[9];

    private Button[] hairOptions = new Button[9];

    private Button[] accessoryOptions = new Button[5];

    private Button[] hatOptions = new Button[4];

    // character display elements
    private VisualElement characterBangs;
    private VisualElement characterHighlights;
    private VisualElement characterEyes;
    private VisualElement characterSclera;
    private VisualElement characterHead;
    private VisualElement characterBody;
    private VisualElement characterHair;
    private VisualElement characterHat;
    private VisualElement characterAccessory;

    private Array allButtons;

    //private CustomiseData customisationData;

    private Color newColour;

    

    private Sprite[] bangsSprites = new Sprite[9];

    private Sprite[] hairSprites = new Sprite[9];

    private Sprite[] accessorySprites = new Sprite[5];

    private Sprite[] hatSprites = new Sprite[4];

    private Sprite eyeStyle;
    private Sprite highlightStyle;

    void Awake()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        ui = GetComponent<UIDocument>().rootVisualElement;

        for (int i = 0; i < skinColours.Length; i++)
        {
            skinColours[i] = ui.Q<Button>("SkinOption" + (i + 1));
        }

        for (int i = 0; i < clothesColours.Length; i++)
        {
            clothesColours[i] = ui.Q<Button>("ClothesOption" + (i + 1));
        }

        for (int i = 0; i < hairColours.Length; i++)
        {
            hairColours[i] = ui.Q<Button>("HairOption" + (i + 1));
        }

        for (int i = 0; i < eyeColours.Length; i++)
        {
            eyeColours[i] = ui.Q<Button>("EyesOption" + (i + 1));
        }

        for (int i = 0; i < bangsOptions.Length; i++)
        {
            bangsOptions[i] = ui.Q<Button>("BangsStyle" + (i + 1));
            bangsSprites[i] = Resources.Load<Sprite>("Sprites/PlayerCharacter/Bangs/PlayerCharacter_Bangs_0" + i);
        }

        for (int i = 0; i < hairOptions.Length; i++)
        {
            hairOptions[i] = ui.Q<Button>("HairStyle" + (i + 1));
            hairSprites[i] = Resources.Load<Sprite>("Sprites/PlayerCharacter/Hair/PlayerCharacter_Hair_0" + i);
        }

        for (int i = 0; i < accessoryOptions.Length; i++)
        {
            accessoryOptions[i] = ui.Q<Button>("AccessoryOption" + (i + 1));
            accessorySprites[i] = Resources.Load<Sprite>("Sprites/PlayerCharacter/PlayerCharacter_Accessories_0" + i);
        }

        for (int i = 0; i < hatOptions.Length; i++)
        {
            hatOptions[i] = ui.Q<Button>("HatStyle" + (i + 1));
            hatSprites[i] = Resources.Load<Sprite>("Sprites/PlayerCharacter/PlayerCharacter_Hat_0" + i);
        }

        eyeStyle = Resources.Load<Sprite>("Sprites/PlayerCharacter/PlayerCharacter_Eyes_0" + CustomiseData.eyeType);
        highlightStyle = Resources.Load<Sprite>("Sprites/PlayerCharacter/PlayerCharacter_Highlight_0" + CustomiseData.highlightType);

    }

    private void OnEnable()
    {
        backButton = ui.Q<Button>("BackButton");
        backButton.clicked += OnBackButtonClicked;

        settingsButton = ui.Q<Button>("SettingsButton");
        settingsButton.clicked += OnSettingsButtonClicked;

        settings = ui.Q<TemplateContainer>("Settings");
        settings.style.display = DisplayStyle.None;


        skinPanel = ui.Q<VisualElement>("SkinOptionsPanel");
        skinCategory = ui.Q<Button>("SkinCategory");
        skinCategory.clicked += SkinCategoryClicked;

        clothesPanel = ui.Q<VisualElement>("ClothesOptionsPanel");
        clothesCategory = ui.Q<Button>("ClothesCategory");
        clothesCategory.clicked += clothesCategoryClicked;

        hairPanel = ui.Q<VisualElement>("HairOptionsPanel");
        hairCategory = ui.Q<Button>("HairCategory");
        hairCategory.clicked += hairCategoryClicked;

        eyesPanel = ui.Q<VisualElement>("EyesOptionsPanel");
        eyesCategory = ui.Q<Button>("EyesCategory");
        eyesCategory.clicked += eyesCategoryClicked;

        accessoryPanel = ui.Q<VisualElement>("AccessoryOptionsPanel");
        accessoryCategory = ui.Q<Button>("AccessoryCategory");
        accessoryCategory.clicked += accessoryCategoryClicked;

        characterBangs = ui.Q<VisualElement>("CharacterBangs");
        characterHighlights = ui.Q<VisualElement>("CharacterHighlights");
        characterEyes = ui.Q<VisualElement>("CharacterEyes");
        characterSclera = ui.Q<VisualElement>("CharacterSclera");
        characterHead = ui.Q<VisualElement>("CharacterHead");
        characterBody = ui.Q<VisualElement>("CharacterBody");
        characterHair = ui.Q<VisualElement>("CharacterHair");
        characterHat = ui.Q<VisualElement>("CharacterHat");
        characterAccessory = ui.Q<VisualElement>("CharacterAccessory");


        // setting a ton of vars so when checked for the first time they contain an object
        currentCategory = skinCategory;
        currentlySelectedSkin = skinColours[CustomiseData.skinColourNumber];
        currentlySelectedClothes = clothesColours[CustomiseData.clothesColourNumber];
        currentlySelectedHairColour = hairColours[CustomiseData.hairColourNumber];
        currentlySelectedBangsStyle = bangsOptions[CustomiseData.bangsType];
        currentlySelectedHairStyle = hairOptions[CustomiseData.hairType];
        currentlySelectedAccessory = accessoryOptions[CustomiseData.accessoryType];
        currentlySelectedHat = hatOptions[CustomiseData.hatType];
        currentlySelectedEyes = eyeColours[0];
        currentOptionsPanel = skinPanel;

        foreach (Button button in skinColours)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }

        foreach (Button button in clothesColours)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }

        foreach (Button button in eyeColours)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }

        foreach (Button button in hairColours)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }

        foreach (Button button in bangsOptions)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }

        foreach (Button button in hairOptions)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }

        foreach (Button button in accessoryOptions)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }

        foreach (Button button in hatOptions)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
        }

        // set character base colours immediately

        ColorUtility.TryParseHtmlString(CustomiseData.skinColour, out newColour);
        characterHead.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(CustomiseData.clothesColour, out newColour);
        characterBody.style.unityBackgroundImageTintColor = newColour;

        if (CustomiseData.crownMode == false)
        {
            characterHat.style.unityBackgroundImageTintColor = newColour;
        }
        

        ColorUtility.TryParseHtmlString(CustomiseData.hairColour, out newColour);
        characterHair.style.unityBackgroundImageTintColor = newColour;
        characterBangs.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(CustomiseData.eyeColour, out newColour);
        characterEyes.style.unityBackgroundImageTintColor = newColour;

        if (CustomiseData.alienMode == true)
        {
            characterSclera.style.display = DisplayStyle.None;
        }
        else
        {
            characterSclera.style.display = DisplayStyle.Flex;
        }

        characterBangs.style.backgroundImage = new StyleBackground(bangsSprites[CustomiseData.bangsType]);
        characterHair.style.backgroundImage = new StyleBackground(hairSprites[CustomiseData.hairType]);

        characterEyes.style.backgroundImage = new StyleBackground(eyeStyle);
        characterHighlights.style.backgroundImage = new StyleBackground(highlightStyle);

        characterHat.style.backgroundImage = new StyleBackground(hatSprites[CustomiseData.hatType]);
        characterAccessory.style.backgroundImage = new StyleBackground(accessorySprites[CustomiseData.accessoryType]);



        // setting button borders
        OptionSelected(skinColours[CustomiseData.skinColourNumber], skinColours[0]);
        OptionSelected(clothesColours[CustomiseData.clothesColourNumber], clothesColours[0]);
        OptionSelected(hairColours[CustomiseData.hairColourNumber], hairColours[0]);
        OptionSelected(eyeColours[CustomiseData.eyeColourNumber], eyeColours[0]);

        OptionSelected(accessoryOptions[CustomiseData.accessoryType], accessoryOptions[0]);

        OptionSelected(bangsOptions[CustomiseData.bangsType], bangsOptions[0]);
        OptionSelected(hairOptions[CustomiseData.hairType], hairOptions[0]);
    }

    // i know this is inefficient, i just want to get the feature working for now
    private void SkinCategoryClicked()
    {
        CategoryClicked(skinCategory, "Skin", skinPanel);
    }
    private void clothesCategoryClicked()
    {
        CategoryClicked(clothesCategory, "Clothes", clothesPanel);
    }
    private void hairCategoryClicked()
    {
        CategoryClicked(hairCategory, "Hair", hairPanel);
    }
    private void eyesCategoryClicked()
    {
        CategoryClicked(eyesCategory, "Eyes", eyesPanel);
    }

    private void accessoryCategoryClicked()
    {
        CategoryClicked(accessoryCategory, "Accessories", accessoryPanel);
    }

    private void CategoryClicked(Button newCategory, String categoryName, VisualElement optionsPanel)
    {
        FindAnyObjectByType<MenuAudio>().PlayDrawer();
        // moves last category button down
        if (currentCategory != newCategory)
        {
            float butPosY = currentCategory.transform.position.y;

            DOTween.Kill("increaseCategory"); // kill other tween so no overlap

            DOTween.To(() => butPosY, x => butPosY = x, 0.0f, 0.25f).SetId("decreaseCategory").SetEase(Ease.OutCubic).OnUpdate(() =>
            {
                print(butPosY);
                currentCategory.transform.position = new Vector2(0, butPosY);
            }).OnComplete(() =>
            {
                currentCategory = newCategory;
            });

            //currentCategory.transform.position = new Vector2(0, 0);
            currentOptionsPanel.style.display = DisplayStyle.None;

        }

        
        // moves selected category button up

        float catPosY = newCategory.transform.position.y;

        //DOTween.Kill("decreaseCategory"); // kill other tween so no overlap

        DOTween.To(() => catPosY, x => catPosY = x, -15.0f, 0.25f).SetId("increaseCategory").SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            newCategory.transform.position = new Vector2(0, catPosY);
        });

        //category.style.translate = new Translate(0, -15, 0);

        optionsPanel.style.display = DisplayStyle.Flex;
        currentOptionsPanel = optionsPanel;
        currentCategorySwitch = categoryName;
    }

    private void Clickable_clickedWithEventInfo(EventBase obj)
    {
        FindAnyObjectByType<MenuAudio>().PlayRuffle();
        var button = (Button)obj.target;
        string onlyNumbersName;
        switch (currentCategorySwitch)
        {
            case "Skin":
                OptionSelected(button, currentlySelectedSkin);

                currentlySelectedSkin = button;

                if (button.name == "SkinOption9")
                {
                    CustomiseData.alienMode = true;
                    CustomiseData.eyeType = 1;
                    CustomiseData.highlightType = 1;
                    characterEyes.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("Sprites/PlayerCharacter/PlayerCharacter_Eyes_01"));
                    characterHighlights.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("Sprites/PlayerCharacter/PlayerCharacter_Highlight_01"));
                    characterSclera.style.display = DisplayStyle.None;

                }
                else
                {
                    CustomiseData.alienMode = false;
                    CustomiseData.eyeType = 0;
                    CustomiseData.highlightType = 0;
                    characterEyes.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("Sprites/PlayerCharacter/PlayerCharacter_Eyes_00"));
                    characterHighlights.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("Sprites/PlayerCharacter/PlayerCharacter_Highlight_00"));
                    characterSclera.style.display = DisplayStyle.Flex;
                }

                print("skin colour changed to " + button.text);
                CustomiseData.skinColour = button.text;

                onlyNumbersName = Regex.Match(button.name, @"\d+").Value;

                CustomiseData.skinColourNumber = int.Parse(onlyNumbersName) - 1;
                print(CustomiseData.skinColourNumber);

                ColorUtility.TryParseHtmlString(CustomiseData.skinColour, out newColour);
                characterHead.style.unityBackgroundImageTintColor = newColour;

                break;
            case "Clothes":
                string hatStyleType = button.text[0].ToString();
                switch (hatStyleType)
                {
                    case "h":
                        OptionSelected(button, currentlySelectedHat);

                        currentlySelectedHat = button;

                        print("hat changed to " + button.text);

                        if (button.name == "HatStyle4")
                        {
                            CustomiseData.crownMode = true;
                        }

                        int hatStyle = int.Parse(button.text[1].ToString());

                        CustomiseData.hatType = hatStyle - 1;
                        
                        if (CustomiseData.crownMode == true)
                        {
                            characterHat.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 1);
                        }
                        else
                        {
                            ColorUtility.TryParseHtmlString(CustomiseData.clothesColour, out newColour);
                            characterHat.style.unityBackgroundImageTintColor = newColour;
                        }

                            characterHat.style.backgroundImage = new StyleBackground(hatSprites[CustomiseData.hatType]);

                        break;
                    default:
                        OptionSelected(button, currentlySelectedClothes);

                        currentlySelectedClothes = button;

                        print("clothes colour changed to " + button.text);
                        CustomiseData.clothesColour = button.text;

                        onlyNumbersName = Regex.Match(button.name, @"\d+").Value;

                        CustomiseData.clothesColourNumber = int.Parse(onlyNumbersName) - 1;

                        ColorUtility.TryParseHtmlString(button.text, out newColour);
                        characterBody.style.unityBackgroundImageTintColor = newColour;

                        if (CustomiseData.crownMode == false)
                        {
                            characterHat.style.unityBackgroundImageTintColor = newColour;
                        }

                        break;
                }

                

                break;
            case "Hair":
                string styleType = button.text[0].ToString();
                switch (styleType)
                {
                    case "b": // bangs type
                        OptionSelected(button, currentlySelectedBangsStyle);

                        currentlySelectedBangsStyle = button;

                        
                        int bangsStyle = int.Parse(button.text[1].ToString());

                        print("bangs style changed to " + bangsStyle);


                        CustomiseData.bangsType = bangsStyle - 1;
                        characterBangs.style.backgroundImage = new StyleBackground(bangsSprites[CustomiseData.bangsType]);

                        break;
                    case "h": // hair type
                        OptionSelected(button, currentlySelectedHairStyle);

                        currentlySelectedHairStyle = button;

                        int hairStyle = int.Parse(button.text[1].ToString());

                        print("hair style changed to " + hairStyle);


                        CustomiseData.hairType = hairStyle - 1;
                        characterHair.style.backgroundImage = new StyleBackground(hairSprites[CustomiseData.hairType]);

                        break;
                    default: // hair colour
                        OptionSelected(button, currentlySelectedHairColour);

                        currentlySelectedHairColour = button;

                        print("hair colour changed to " + button.text);
                        CustomiseData.hairColour = button.text;

                        onlyNumbersName = Regex.Match(button.name, @"\d+").Value;

                        CustomiseData.hairColourNumber = int.Parse(onlyNumbersName) - 1;

                        ColorUtility.TryParseHtmlString(button.text, out newColour);
                        characterHair.style.unityBackgroundImageTintColor = newColour;
                        characterBangs.style.unityBackgroundImageTintColor = newColour;

                        break;
                }
                break;
            case "Eyes":
                OptionSelected(button, currentlySelectedEyes);

                currentlySelectedEyes = button;

                print("eye colour changed to " + button.text);
                CustomiseData.eyeColour = button.text;

                onlyNumbersName = Regex.Match(button.name, @"\d+").Value;

                CustomiseData.eyeColourNumber = int.Parse(onlyNumbersName) - 1;

                ColorUtility.TryParseHtmlString(button.text, out newColour);
                characterEyes.style.unityBackgroundImageTintColor = newColour;

                break;
            case "Accessories":
                OptionSelected(button, currentlySelectedAccessory);

                currentlySelectedAccessory = button;

                print("accessory changed to " + button.text);

                int accessoryStyle = int.Parse(button.text);

                CustomiseData.accessoryType = accessoryStyle - 1;

                characterAccessory.style.backgroundImage = new StyleBackground(accessorySprites[CustomiseData.accessoryType]);
                characterHair.style.backgroundImage = new StyleBackground(hairSprites[CustomiseData.hairType]);
                break;
        }
    }

    private void OptionSelected(Button button, Button currentlySelectedOption)
    {
        ChangeBorderSize(currentlySelectedOption, -1);
        ChangeBorderSize(button, 1);
    }

    private void ChangeBorderSize(Button button, int direction)
    {
        

        float goal;

        if (direction == 1)
        {
            goal = 6.0f;
        }
        else
        {
            goal = -2.0f;
        }

        float buttonTop = button.style.borderTopWidth.value;
        float buttonBottom = button.style.borderBottomWidth.value;
        float buttonLeft = button.style.borderLeftWidth.value;
        float buttonRight = button.style.borderRightWidth.value;

        DOTween.To(() => buttonTop, x => buttonTop = x, goal, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            button.style.borderTopWidth = buttonTop;
        });
        
        DOTween.To(() => buttonBottom, x => buttonBottom = x, goal, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            button.style.borderBottomWidth = buttonBottom;
        });

        DOTween.To(() => buttonLeft, x => buttonLeft = x, goal, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            button.style.borderLeftWidth = buttonLeft;
        });

        DOTween.To(() => buttonRight, x => buttonRight = x, goal, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            button.style.borderRightWidth = buttonRight;
        });
        
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
        FindAnyObjectByType<MenuAudio>().PlayButtonClick(10);
        sceneLoader.LoadNextScene("Main Menu");
    }
}
