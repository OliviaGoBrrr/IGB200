using DG.Tweening;
using UnityEngine;

public class BurnableDecoration : MonoBehaviour
{
    private SpriteRenderer sprite2D;
    [SerializeField] ParticleSystem triggerPar;
    private bool triggered = false;
    void Awake()
    {
        sprite2D = GetComponent<SpriteRenderer>();
        foreach (Transform childTransform in transform)
        {
            GameObject child = childTransform.gameObject;
            ParticleSystem particle = child.GetComponent<ParticleSystem>();
            if (particle != null) { triggerPar = particle; }
        }
    }
    public void TriggerBurn()
    {
        if (triggered == false)
        {
            triggered = true;
            sprite2D.DOFade(0, 2);
            triggerPar.Play();
        }
    }
}
