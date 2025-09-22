using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class FireDecoration : MonoBehaviour
{
    private List<FireBit> fireBits;
    private bool triggered = false;
    private void Awake()
    {
        foreach (Transform childTransform in transform)
        {
            GameObject child = childTransform.gameObject;
            FireBit bit = child.GetComponent<FireBit>();
            if (bit != null) { fireBits.Add(bit); }
        }
    }
    public void TriggerBurn()
    {
        if (triggered == false)
        {
            triggered = true;
            foreach (FireBit bit in fireBits) { bit.FadeIn(); }
        }
    }
    public void Extinguish()
    {
        foreach (FireBit bit in fireBits) { bit.FadeOut(); }
    }
}
