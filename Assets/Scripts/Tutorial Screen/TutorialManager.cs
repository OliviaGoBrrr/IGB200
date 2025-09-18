using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


[System.Serializable]
public class TutorialStep
{
    public VideoClip videoClip;
    public GameObject[] elementsToShow; 
}

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Components")]
    public GameObject tutorialScreenObject; 
    public VideoPlayer tutorialVideoPlayer;
    public RawImage tutorialVideoDisplay;

    [Header("Tutorial Content")]
    public TutorialStep[] tutorialSteps; 

    private int currentStepIndex = 0;

    void Start()
    {
        
        ShowTutorial();
    }

   
    public void ShowTutorial()
    {
        tutorialScreenObject.SetActive(true);
        

        // Start with the first tutorial step
        if (tutorialSteps.Length > 0)
        {
            currentStepIndex = 0;
            PlayTutorialStep(currentStepIndex);
        }
    }


    public void HideTutorial()
    {
        tutorialScreenObject.SetActive(false);
        tutorialVideoPlayer.Stop();
        HideAllTutorialElements(); // Make sure all extra UI is hidden
        
    }


    public void NextTutorialStep()
    {
        if (tutorialSteps.Length == 0) return;

        
        currentStepIndex = (currentStepIndex + 1) % tutorialSteps.Length;
        PlayTutorialStep(currentStepIndex);
    }


    private void PlayTutorialStep(int index)
    {
        if (index < 0 || index >= tutorialSteps.Length) return;


        HideAllTutorialElements();

        TutorialStep currentStep = tutorialSteps[index];

        foreach (GameObject element in currentStep.elementsToShow)
        {
            if (element != null)
            {
                element.SetActive(true);
            }
        }

        tutorialVideoPlayer.clip = currentStep.videoClip;
        tutorialVideoPlayer.Play();
    }


    private void HideAllTutorialElements()
    {
        foreach (TutorialStep step in tutorialSteps)
        {
            foreach (GameObject element in step.elementsToShow)
            {
                if (element != null)
                {
                    element.SetActive(false);
                }
            }
        }
    }
}