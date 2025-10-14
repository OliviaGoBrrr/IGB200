using DG.Tweening;
using UnityEngine;

public class BurnableDecoration : MonoBehaviour
{
    private SpriteRenderer sprite2D;
    [SerializeField] ParticleSystem triggerPar;
    private bool triggered = false;
    public bool dryDeco = true;
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
            sprite2D.enabled = false;
            triggerPar.Play();
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
