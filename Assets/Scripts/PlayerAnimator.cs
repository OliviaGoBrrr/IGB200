using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Currently used character parts")]
    public List<Animator> animTargets;

    [Header("Character parts with variants")]
    public List<GameObject> bangList;
    public List<GameObject> eyesList;
    public List<GameObject> hairList;
    public List<GameObject> highlightList;

    public void Awake()
    {
        // read the current character save
        // load the correct bangs, eyes, hair, highlight
        // load the correct colours and such
        // set it on the appropriate objects
    }
    public void Animate(Vector3 pos)
    {
        gameObject.transform.position = pos + new Vector3(-1, 1, 0);
        foreach (var animTarget in animTargets) 
        {
            animTarget.SetTrigger("Action");
        }
    }
}
