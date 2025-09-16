using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Currently used character parts. 0 is body, 1 is hat, 2 is head, 3 is sclera")]
    public List<Animator> animTargets;
    

    [Header("Character parts with variants")]
    public List<GameObject> bangList;
    public List<GameObject> hairList;
    public List<GameObject> eyesList;
    public List<GameObject> highlightList;

    public void Awake()
    {
        animTargets[0].GetComponent<SpriteRenderer>().color = hexToColor(CustomiseData.clothesColour);
        animTargets[1].GetComponent<SpriteRenderer>().color = hexToColor(CustomiseData.clothesColour);
        animTargets[2].GetComponent<SpriteRenderer>().color = hexToColor(CustomiseData.skinColour);

        Console.WriteLine(hexToColor(CustomiseData.skinColour));

        bangList[CustomiseData.hairType].GetComponent<SpriteRenderer>().color = new Color(hexToColor(CustomiseData.hairColour).r, hexToColor(CustomiseData.hairColour).g, hexToColor(CustomiseData.hairColour).b, 1);
        hairList[CustomiseData.hairType].GetComponent<SpriteRenderer>().color = new Color(hexToColor(CustomiseData.hairColour).r, hexToColor(CustomiseData.hairColour).g, hexToColor(CustomiseData.hairColour).b, 1);
        eyesList[CustomiseData.eyeType].GetComponent<SpriteRenderer>().color = new Color(hexToColor(CustomiseData.eyeColour).r, hexToColor(CustomiseData.eyeColour).g, hexToColor(CustomiseData.eyeColour).b, 1);
        highlightList[CustomiseData.eyeType].GetComponent<SpriteRenderer>().color = new Color(hexToColor(CustomiseData.eyeColour).r, hexToColor(CustomiseData.eyeColour).g, hexToColor(CustomiseData.eyeColour).b, 1);
        animTargets.Add(bangList[CustomiseData.hairType].GetComponent<Animator>());
        animTargets.Add(hairList[CustomiseData.hairType].GetComponent<Animator>());
        animTargets.Add(eyesList[CustomiseData.eyeType].GetComponent<Animator>());
        animTargets.Add(highlightList[CustomiseData.eyeType].GetComponent<Animator>());

        
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
    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
}
