using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;


public class SequentialUIController : MonoBehaviour
{
    
    private const string StepContainerName = "Steps-Container"; 
    private const string NextButtonName = "ForwardButton";
   

    
    private List<VisualElement> _steps = new List<VisualElement>();
    private int _currentStepIndex = 0;

    
    private Button _forwardsButton;
    

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
        

        if (_forwardsButton != null)
        {
            _forwardsButton.clicked += OnNextClicked;
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
        
        else if (_currentStepIndex == _steps.Count - 1)
        {
            HideTutorialUI();
        }
        
        else if (_steps.Count == 1)
        {
            HideTutorialUI();
        }
    }


    private void HideTutorialUI()
    {
        Debug.Log("Tutorial finished! Closing UI.");
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
