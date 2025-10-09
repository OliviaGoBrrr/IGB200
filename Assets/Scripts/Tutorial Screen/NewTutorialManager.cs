using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;


public class SequentialUIController : MonoBehaviour
{
    
    private const string StepContainerName = "Steps-Container"; 
    private const string NextButtonName = "ForwardButton";
    private const string BackButtonName = "BackButton";
    private const string StartButtonName = "StartButton";

    
    private List<VisualElement> _steps = new List<VisualElement>();
    private int _currentStepIndex = 0;

    
    private Button _forwardsButton;
    private Button _backButton;
    private Button _startButton;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("SequentialUIController requires a UIDocument component.");
            return;
        }

        VisualElement root = uiDocument.rootVisualElement;

        
        _forwardsButton = root.Q<Button>(NextButtonName);
        _backButton = root.Q<Button>(BackButtonName);
        _startButton = root.Q<Button>(StartButtonName);

        if (_forwardsButton != null)
        {
            _forwardsButton.clicked += OnNextClicked;
        }
        if (_backButton != null)
        {
            _backButton.clicked += OnBackClicked;
        }
        if (_startButton != null)
        {
            _startButton.clicked += OnFinishClicked;
        }

        // 2. Automatically populate the list of steps
        PopulateStepsList(root);

        // 3. Initialize the UI to show only the first step
        InitializeStepsVisibility();
    }

   
    private void PopulateStepsList(VisualElement root)
    {
        VisualElement stepContainer = root.Q<VisualElement>(StepContainerName);

        if (stepContainer == null)
        {
            Debug.LogError($"Could not find the main step container named '{StepContainerName}'.");
            return;
        }

        // The list of steps is all the immediate children of the step container.
        foreach (VisualElement step in stepContainer.Children())
        {
            _steps.Add(step);
        }

        if (_steps.Count == 0)
        {
            Debug.LogWarning($"The container '{StepContainerName}' has no children (steps) defined.");
        }
    }

    
    private void InitializeStepsVisibility()
    {
        _currentStepIndex = 0;
        UpdateStepVisibility();
    }

   
    private void OnNextClicked()
    {
        if (_currentStepIndex < _steps.Count - 1)
        {
            _currentStepIndex++;
            UpdateStepVisibility();
        }
    }

    
    private void OnBackClicked()
    {
        if (_currentStepIndex > 0)
        {
            _currentStepIndex--;
            UpdateStepVisibility();
        }
    }

   
    private void OnFinishClicked()
    {
        Debug.Log("Tutorial/Wizard finished! Closing UI.");
        GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
    }

    
    private void UpdateStepVisibility()
    {
        for (int i = 0; i < _steps.Count; i++)
        {
            VisualElement step = _steps[i];

            
            if (i == _currentStepIndex)
            {
                step.style.display = DisplayStyle.Flex;
            }
            
            else
            {
                step.style.display = DisplayStyle.None;
            }
        }

       
    }

   
}
