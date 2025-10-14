using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SproutableDecoration : MonoBehaviour
{
    private SpriteRenderer sprite2D;
    [SerializeField] ParticleSystem triggerPar;
    private bool triggered = false;
    [SerializeField] List<Sprite> flowerOptions;
    void Awake()
    {
        sprite2D = GetComponent<SpriteRenderer>();
        sprite2D.sprite = flowerOptions[Random.Range(0, flowerOptions.Count)];
        foreach (Transform childTransform in transform)
        {
            GameObject child = childTransform.gameObject;
            ParticleSystem particle = child.GetComponent<ParticleSystem>();
            if (particle != null) { triggerPar = particle; }
        }
    }
    public void TriggerSprout()
    {
        if (triggered == false)
        {
            triggered = true;
            sprite2D.DOFade(1, 2);
            triggerPar.Play();
        }
    }
}
