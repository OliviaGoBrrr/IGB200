using UnityEngine;
using System.Collections.Generic;

public class BurnableDecoration : MonoBehaviour
{
    private SpriteRenderer sprite2D;
    [SerializeField] List<ParticleSystem> burnTriggerPar;
    [SerializeField] List<ParticleSystem> scoreCountTriggerPar;
    private bool triggered = false;
    [SerializeField] bool dryDeco = true;

    [SerializeField] Sprite burntSprite;
    [SerializeField] bool randomizePosition = false;

    public bool hasScoreCountPar = false;
    void Awake()
    {
        sprite2D = GetComponent<SpriteRenderer>();
        foreach (Transform childTransform in transform)
        {
            GameObject child = childTransform.gameObject;
            ParticleSystem particle = child.GetComponent<ParticleSystem>();
            if (particle != null && burnTriggerPar.Count == 0) { burnTriggerPar.Add(particle); } // Grabs particle in case it hasn't been attached manually
        }
        if (randomizePosition)
        {
            transform.localPosition = new Vector3(Random.Range(-0.35f, 0.35f), 0.58f, Random.Range(-0.4f, 0.4f));
        }
        if (scoreCountTriggerPar.Count > 0) { hasScoreCountPar = true; }
    }
    public void TriggerBurn()
    {
        if (triggered == false)
        {
            triggered = true;
            if (burntSprite == null) 
            {
                sprite2D.enabled = false;
            }
            else
            {
                sprite2D.sprite = burntSprite;
            }
            foreach (ParticleSystem particle in burnTriggerPar)
            {
                particle.Play();
            }  
        }
    }
    public void TriggerCount()
    {
        foreach (ParticleSystem particle in scoreCountTriggerPar)
        {
            particle.Play();
        }
    }
    public void UndoHelp(GameTile.TileStates transformInto)
    {
        switch(transformInto)
        {
            case GameTile.TileStates.GRASS:
                if(dryDeco == true)
                {
                    Destroy(gameObject);
                }
                else
                {
                    triggered = false;
                    sprite2D.enabled = true;
                }
                    break;
            case GameTile.TileStates.DRY_GRASS:
                if (dryDeco == true)
                {
                    triggered = false;
                    sprite2D.enabled = true;
                }
                else
                {
                    triggered = true;
                    sprite2D.enabled = false;
                }
                    break;
        }
    }
}
