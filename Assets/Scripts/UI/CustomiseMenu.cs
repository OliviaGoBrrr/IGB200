using UnityEngine;
using UnityEngine.UIElements;

public class CustomiseMenu : MonoBehaviour
{
    private VisualElement ui;

    private SceneLoader sceneLoader;

    private Button currentlySelectedSkin;

    // Buttons
    private Button skinOption1;
    private Button skinOption2;
    private Button skinOption3;
    private Button skinOption4;
    private Button skinOption5;
    private Button skinOption6;



    private Button hairButton;

    private TemplateContainer settings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {


    }

    private void OptionSelected(Button button, int category)
    {
        
    }

    private void ChangeBorderSize(Button button, int direction)
    {
        button.style.borderTopWidth = 4 * direction;
        button.style.borderBottomWidth = 4 * direction;
        button.style.borderLeftWidth = 4 * direction;
        button.style.borderRightWidth = 4 * direction;
    }
}
