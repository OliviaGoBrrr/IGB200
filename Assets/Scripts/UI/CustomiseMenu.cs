using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using UnityEditor.SearchService;
using UnityEngine.Rendering;

public class CustomiseMenu : MonoBehaviour
{
    private VisualElement ui;

    private SceneLoader sceneLoader;

    private Button currentlySelectedSkin;
    private Button currentlySelectedClothes;
    private Button currentlySelectedHair;
    private Button currentlySelectedEyes;

    private Button BUTTONcurrentCategory;
    private string currentCategory = "Skin";
    private VisualElement currentOptionsPanel;

    // Category Buttons
    private Button skinCategory;
    private Button clothesCategory;
    private Button hairCategory;
    private Button eyesCategory;

    // Options Panels
    private VisualElement skinPanel;
    private VisualElement clothesPanel;
    private VisualElement hairPanel;
    private VisualElement eyesPanel;

    // Option Buttons
    private Button skinOption1;
    private Button skinOption2;
    private Button skinOption3;
    private Button skinOption4;
    private Button skinOption5;
    private Button skinOption6;

    private Button clothesOption1;
    private Button clothesOption2;
    private Button clothesOption3;
    private Button clothesOption4;
    private Button clothesOption5;
    private Button clothesOption6;

    private Button hairOption1;
    private Button hairOption2;
    private Button hairOption3;
    private Button hairOption4;
    private Button hairOption5;
    private Button hairOption6;

    private Button eyesOption1;
    private Button eyesOption2;
    private Button eyesOption3;
    private Button eyesOption4;
    private Button eyesOption5;
    private Button eyesOption6;

    // character display elements
    private VisualElement characterBangs;
    private VisualElement characterHighlights;
    private VisualElement characterEyes;
    private VisualElement characterSclera;
    private VisualElement characterHead;
    private VisualElement characterBody;
    private VisualElement characterHair;
    private VisualElement characterHat;

    private Array allButtons;

    private CustomiseData customisationData;

    private Color newColour;

    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;

        customisationData = GetComponent<CustomiseData>();

    }

    private void OnEnable()
    {
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


        skinOption1 = ui.Q<Button>("SkinOption1");
        skinOption2 = ui.Q<Button>("SkinOption2");
        skinOption3 = ui.Q<Button>("SkinOption3");
        skinOption4 = ui.Q<Button>("SkinOption4");
        skinOption5 = ui.Q<Button>("SkinOption5");
        skinOption6 = ui.Q<Button>("SkinOption6");

        clothesOption1 = ui.Q<Button>("ClothesOption1");
        clothesOption2 = ui.Q<Button>("ClothesOption2");
        clothesOption3 = ui.Q<Button>("ClothesOption3");
        clothesOption4 = ui.Q<Button>("ClothesOption4");
        clothesOption5 = ui.Q<Button>("ClothesOption5");
        clothesOption6 = ui.Q<Button>("ClothesOption6");

        hairOption1 = ui.Q<Button>("HairOption1");
        hairOption2 = ui.Q<Button>("HairOption2");
        hairOption3 = ui.Q<Button>("HairOption3");
        hairOption4 = ui.Q<Button>("HairOption4");
        hairOption5 = ui.Q<Button>("HairOption5");
        hairOption6 = ui.Q<Button>("HairOption6");

        eyesOption1 = ui.Q<Button>("EyesOption1");
        eyesOption2 = ui.Q<Button>("EyesOption2");
        eyesOption3 = ui.Q<Button>("EyesOption3");
        eyesOption4 = ui.Q<Button>("EyesOption4");
        eyesOption5 = ui.Q<Button>("EyesOption5");
        eyesOption6 = ui.Q<Button>("EyesOption6");


        characterBangs = ui.Q<VisualElement>("CharacterBangs");
        characterHighlights = ui.Q<VisualElement>("CharacterHighlights");
        characterEyes = ui.Q<VisualElement>("CharacterEyes");
        characterSclera = ui.Q<VisualElement>("CharacterSclera");
        characterHead = ui.Q<VisualElement>("CharacterHead");
        characterBody = ui.Q<VisualElement>("CharacterBody");
        characterHair = ui.Q<VisualElement>("CharacterHair");
        characterHat = ui.Q<VisualElement>("CharacterHat");


        // setting a ton of vars so when checked for the first time they contain an object
        BUTTONcurrentCategory = skinCategory;
        currentlySelectedSkin = skinOption1;
        currentlySelectedClothes = clothesOption1;
        currentlySelectedHair = hairOption1;
        currentlySelectedEyes = eyesOption1;
        currentOptionsPanel = skinPanel;

        // the everything array
        allButtons = new Button[] {
            skinOption1, skinOption2, skinOption3, skinOption4, skinOption5, skinOption6,
            clothesOption1, clothesOption2, clothesOption3, clothesOption4, clothesOption5, clothesOption6,
            hairOption1, hairOption2, hairOption3, hairOption4, hairOption5, hairOption6,
            eyesOption1, eyesOption2, eyesOption3, eyesOption4, eyesOption5, eyesOption6
        };

        foreach (Button button in allButtons)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
            print("a");
        }


        // set character colours immediately

        ColorUtility.TryParseHtmlString(customisationData.skinColour, out newColour);
        characterHead.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(customisationData.clothesColour, out newColour);
        characterBody.style.unityBackgroundImageTintColor = newColour;
        characterHat.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(customisationData.hairColour, out newColour);
        characterHair.style.unityBackgroundImageTintColor = newColour;
        characterBangs.style.unityBackgroundImageTintColor = newColour;

        ColorUtility.TryParseHtmlString(customisationData.eyeColour, out newColour);
        characterEyes.style.unityBackgroundImageTintColor = newColour;
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
    private void CategoryClicked(Button category, String categoryName, VisualElement optionsPanel)
    {
        // goes down
        if (BUTTONcurrentCategory != category)
        {
            BUTTONcurrentCategory.style.translate = new Translate(0, 0, 0);
            currentOptionsPanel.style.display = DisplayStyle.None;
        }

        // goes up
        
        category.style.translate = new Translate(0, -15, 0);


        optionsPanel.style.display = DisplayStyle.Flex;
        currentOptionsPanel = optionsPanel;
        currentCategory = categoryName;
        BUTTONcurrentCategory = category;
    }

    private void Clickable_clickedWithEventInfo(EventBase obj)
    {
        var button = (Button)obj.target;
        switch(currentCategory)
        {
            case "Skin":
                OptionSelected(button, currentlySelectedSkin);

                currentlySelectedSkin = button;

                print("skin colour changed to " + button.text);
                customisationData.skinColour = button.text;

                ColorUtility.TryParseHtmlString(button.text, out newColour);
                characterHead.style.unityBackgroundImageTintColor = newColour;

                break;
            case "Clothes":
                OptionSelected(button, currentlySelectedClothes);

                currentlySelectedClothes = button;

                print("clothes colour changed to " + button.text);
                customisationData.clothesColour = button.text;

                ColorUtility.TryParseHtmlString(button.text, out newColour);
                characterBody.style.unityBackgroundImageTintColor = newColour;
                characterHat.style.unityBackgroundImageTintColor = newColour;

                break;
            case "Hair":
                OptionSelected(button, currentlySelectedHair);

                currentlySelectedHair = button;

                print("hair colour changed to " + button.text);
                customisationData.hairColour = button.text;

                ColorUtility.TryParseHtmlString(button.text, out newColour);
                characterHair.style.unityBackgroundImageTintColor = newColour;
                characterBangs.style.unityBackgroundImageTintColor = newColour;

                break;
            case "Eyes":
                OptionSelected(button, currentlySelectedEyes);

                currentlySelectedEyes = button;

                print("eye colour changed to " + button.text);
                customisationData.eyeColour = button.text;

                ColorUtility.TryParseHtmlString(button.text, out newColour);
                characterEyes.style.unityBackgroundImageTintColor = newColour;

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
        float buttonTop = button.style.borderTopWidth.value;
        float buttonBottom = button.style.borderBottomWidth.value;
        float buttonLeft = button.style.borderLeftWidth.value;
        float buttonRight = button.style.borderRightWidth.value;
        DOTween.To(() => buttonTop, x => buttonTop = x, 6.0f * direction, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            button.style.borderTopWidth = buttonTop;
        });

        DOTween.To(() => buttonBottom, x => buttonBottom = x, 6.0f * direction, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            button.style.borderBottomWidth = buttonBottom;
        });

        DOTween.To(() => buttonLeft, x => buttonLeft = x, 6.0f * direction, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            button.style.borderLeftWidth = buttonLeft;
        });

        DOTween.To(() => buttonRight, x => buttonRight = x, 6.0f * direction, 0.25f).SetEase(Ease.OutCubic).OnUpdate(() =>
        {
            button.style.borderRightWidth = buttonRight;
        });
    }
}
