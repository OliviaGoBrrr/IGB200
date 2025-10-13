using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using DG.Tweening;
using NUnit.Framework;
using UnityEditor.Animations;
using UnityEngine;
//using UnityEditor.ShaderKeywordFilter;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Currently used character parts. 0 ~ 8 is Bangs, Hair, Eyes, Highlight, Sclera, Head, Body, Hat and Accessory respectively")]
    public List<Animator> animTargets;
    

    [Header("Character parts with variants")]

    //[SerializeField] private SpriteRenderer 

    private Color newColour;

    private GridManager grid;
    private Coroutine walkRoutine;
    bool firstCall = true;

    [SerializeField] GameObject actionDustcloud;

    private enum PlayerState
    {
        Idle,
        Walking,
        Action
    }
    public void Awake()
    {
        //animTargets[0].GetComponent<SpriteRenderer>().color = hexToColor(CustomiseData.clothesColour);
        //animTargets[1].GetComponent<SpriteRenderer>().color = hexToColor(CustomiseData.clothesColour);
        //animTargets[2].GetComponent<SpriteRenderer>().color = hexToColor(CustomiseData.skinColour);
        Console.WriteLine(hexToColor(CustomiseData.skinColour));

        //bangList[CustomiseData.hairType].GetComponent<SpriteRenderer>().color = new Color(hexToColor(CustomiseData.hairColour).r, hexToColor(CustomiseData.hairColour).g, hexToColor(CustomiseData.hairColour).b, 1);
        //hairList[CustomiseData.hairType].GetComponent<SpriteRenderer>().color = new Color(hexToColor(CustomiseData.hairColour).r, hexToColor(CustomiseData.hairColour).g, hexToColor(CustomiseData.hairColour).b, 1);
        //eyesList[CustomiseData.eyeType].GetComponent<SpriteRenderer>().color = new Color(hexToColor(CustomiseData.eyeColour).r, hexToColor(CustomiseData.eyeColour).g, hexToColor(CustomiseData.eyeColour).b, 1);
        //highlightList[CustomiseData.eyeType].GetComponent<SpriteRenderer>().color = new Color(hexToColor(CustomiseData.eyeColour).r, hexToColor(CustomiseData.eyeColour).g, hexToColor(CustomiseData.eyeColour).b, 1);

        ColorUtility.TryParseHtmlString(CustomiseData.skinColour, out newColour);
        animTargets[5].GetComponent<SpriteRenderer>().material.color = newColour; // change Head colour

        ColorUtility.TryParseHtmlString(CustomiseData.clothesColour, out newColour);
        animTargets[6].GetComponent<SpriteRenderer>().material.color = newColour; // change Body colour
        animTargets[7].GetComponent<SpriteRenderer>().material.color = newColour; // change Hat colour

        ColorUtility.TryParseHtmlString(CustomiseData.hairColour, out newColour);
        animTargets[0].GetComponent<SpriteRenderer>().material.color = newColour; // change Bangs colour
        animTargets[1].GetComponent<SpriteRenderer>().material.color = newColour; // change Hair colour
        


        if (CustomiseData.alienMode == true)
        {
            animTargets[2].GetComponent<SpriteRenderer>().material.color = new Color(0, 0, 0, 1);
            animTargets[4].GetComponent<SpriteRenderer>().material.color = Color.black;
        }
        else
        {
            ColorUtility.TryParseHtmlString(CustomiseData.eyeColour, out newColour);
            animTargets[2].GetComponent<SpriteRenderer>().material.color = newColour; // change Eye colour

            animTargets[4].GetComponent<SpriteRenderer>().material.color = Color.white; // change Sclera colour
        }
            animTargets[3].GetComponent<SpriteRenderer>().material.color = Color.white; // change Highlight colour

        // read the current character save
        // load the correct bangs, eyes, hair, highlight
        // load the correct colours and such
        // set it on the appropriate objects

        grid = FindAnyObjectByType<GridManager>();
    }
    private void Start()
    {
        walkRoutine = StartCoroutine(WalkWait(0f));
        StartCoroutine(BlinkLoop());
    }

    public void Animate(GameTile selectTile)
    {
        StopCoroutine(walkRoutine);
        DOTween.Kill(transform);
        if (selectTile.L == true)
        {
            foreach (var animTarget in animTargets)
            {
                animTarget.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(30, 0, 0);
            }
            
            transform.DOMove(selectTile.transform.position + new Vector3(-1, 1.1f, 0), 0.5f);
        }
        else
        {
            foreach (var animTarget in animTargets)
            {
                animTarget.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(-30, -180, 0);
            }
            transform.DOMove(selectTile.transform.position + new Vector3(1, 1.1f, 0), 0.5f);
        }


        StartCoroutine(QuickWalk(0.5f, 2, selectTile));
        
    }
    IEnumerator QuickWalk(float time, float speed, GameTile selectTile)
    {
        animTargets[0].SetBool("WalkEnd", false);
        foreach (var animTarget in animTargets)
        {
            animTarget.SetTrigger("WalkStart");
        }
        animTargets[0].speed = speed;
        yield return new WaitForSeconds(time);
        animTargets[0].speed = 1;
        animTargets[0].SetBool("WalkEnd", true);
        foreach (var animTarget in animTargets)
        {
            animTarget.ResetTrigger("WalkStart");
            animTarget.SetTrigger("Action");
        }
        yield return new WaitForSeconds(0.15f);
        Instantiate(actionDustcloud, selectTile.transform.position + new Vector3(0, 1.1f, -0.5f), Quaternion.Euler(30, 0, 0));
        walkRoutine = StartCoroutine(WalkWait(2f));
        yield return null;
    }
    IEnumerator WalkWait(float additionalTime)
    {
        yield return new WaitForSeconds(additionalTime);
        if(firstCall == true) { firstCall = false; }
        else {
            foreach (var animTarget in animTargets)
            {
                animTarget.SetBool("WalkEnd", true);
            }
        }   
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 5f));
        PlayerWalk();
        yield return null;
    }
    
    private void PlayerWalk()
    {
        Vector3 target = grid.tileList[UnityEngine.Random.Range(0, grid.tileList.Count)].transform.position + new Vector3(UnityEngine.Random.Range(-0.4f, 0.4f), 1.1f, UnityEngine.Random.Range(-0.2f, 0.6f));
        transform.DOMove(target, Vector3.Distance(target, transform.position));
        if (target.x < transform.position.x)
        { 
            foreach (var animTarget in animTargets)
            {
                animTarget.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(-30, -180, 0);
            }
        }
        else
        {
            foreach (var animTarget in animTargets)
            {
                animTarget.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(30, -0, 0);
            }
        }
        foreach (var animTarget in animTargets)
        {
            animTarget.SetBool("WalkEnd", false);
        }
        foreach (var animTarget in animTargets)
        {
            animTarget.SetTrigger("WalkStart");
        }
        walkRoutine = StartCoroutine(WalkWait(Vector3.Distance(target, transform.position)));
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
    IEnumerator BlinkLoop()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 8f));
        foreach (var animTarget in animTargets)
        {
            animTarget.SetTrigger("Blink");
        }
        StartCoroutine(BlinkLoop());
        yield return null;
    }
}
