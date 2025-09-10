using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using UnityEditor.SearchService;
using UnityEngine.Rendering;

public class CustomiseMenu : MonoBehaviour
{
    private VisualElement ui;

    private SceneLoader sceneLoader;

    private Button currentlySelectedSkin;
    private string currentCategory = "Skin";

    // Buttons
    private Button skinOption1;
    private Button skinOption2;
    private Button skinOption3;
    private Button skinOption4;
    private Button skinOption5;
    private Button skinOption6;

    private Button throwAway;

    private Array skinButtons;

    private Button hairButton;

    private TemplateContainer settings;

    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;

        
    }

    private void OnEnable()
    {
        skinOption1 = ui.Q<Button>("SkinOption1");
        skinOption2 = ui.Q<Button>("SkinOption2");
        skinOption3 = ui.Q<Button>("SkinOption3");
        skinOption4 = ui.Q<Button>("SkinOption4");
        skinOption5 = ui.Q<Button>("SkinOption5");
        skinOption6 = ui.Q<Button>("SkinOption6");

        currentlySelectedSkin = skinOption1;

        skinButtons = new Button[] { skinOption1, skinOption2, skinOption3, skinOption4, skinOption5, skinOption6 };

        foreach (Button button in skinButtons)
        {
            button.clickable.clickedWithEventInfo += Clickable_clickedWithEventInfo;
            print("a");
        }

        //skinOption1

        //skinOption1.RegisterCallback<ClickEvent>(OptionSelected(skinOption1, 1));


    }

    private void clicked()
    {
        print("this clicked");
    }

    private void Clickable_clickedWithEventInfo(EventBase obj)
    {
        var button = (Button)obj.target;
        print("clicked");
        switch(currentCategory)
        {
            case "Skin":
                OptionSelected(button, currentlySelectedSkin);
                currentlySelectedSkin = button;
                print(currentlySelectedSkin);
                break;
            case "Clothes":
                break;
            case "Hair":
                break;
            case "Eyes":
                break;
        }
    }

    private void OptionSelected(Button button, Button currentlySelected)
    {
        ChangeBorderSize(currentlySelected, -1);
        ChangeBorderSize(button, 1);
    }

    private void ChangeBorderSize(Button button, int direction)
    {
        button.style.borderTopWidth = 4 * direction;
        button.style.borderBottomWidth = 4 * direction;
        button.style.borderLeftWidth = 4 * direction;
        button.style.borderRightWidth = 4 * direction;
    }
}
