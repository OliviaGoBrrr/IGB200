using DG.Tweening;
using UnityEngine;

public class FireBit : MonoBehaviour
{
    float bobSpeed;
    bool upwards;

    private float bounds = 0.1f;
    public void Awake()
    {
        if (Random.Range(0, 2) == 0) { upwards = true; }
        else { upwards = false; }
        transform.position = new Vector3(0, Random.Range(-bounds, bounds), 0);
    }
    public void FadeIn()
    {
        GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        DOTween.To(()=> bobSpeed, x=>bobSpeed = x, 0.1f, 0.5f);
    }
    public void FadeOut()
    {
        GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        DOTween.To(() => bobSpeed, x => bobSpeed = x, 0, 0.5f);
    }
    private void Update()
    {
        switch (upwards)
        {
            case true:
                transform.position += new Vector3(0, bobSpeed * Time.deltaTime, 0);
                if (transform.position.y > bounds) { upwards = false; }
                break;
            case false:
                transform.position += new Vector3(0, -bobSpeed * Time.deltaTime, 0);
                if (transform.position.y > -bounds) { upwards = true; }
                break;
        }
    }
}
