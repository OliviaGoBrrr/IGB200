using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class UIAnimations : MonoBehaviour
{
    
    public void test()
    {
        print("GA");
    }

    public void ButtonPressed(Button button)
    {
        button.style.translate = new Translate(5, 5);

        StartCoroutine(ButtonPressedWaitFrames(button));
        
    }

    private IEnumerator ButtonPressedWaitFrames(Button button)
    {
        yield return new WaitForSeconds(0.1f);
        button.style.translate = new Translate(0, 0);
    }



}
