using DG.Tweening;
using UnityEngine;

public class FireBit : MonoBehaviour
{
    float bobSpeed;
    bool upwards;

    private float bounds = 0.05f;
    public void Awake()
    {
        if (Random.Range(0, 2) == 0) { upwards = true; }
        else { upwards = false; }
        transform.localPosition = new Vector3(transform.localPosition.x, Random.Range(-bounds, bounds), transform.localPosition.z);
    }
    public void FadeIn()
    {
        Debug.Log("fadein");
        GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        DOTween.To(()=> bobSpeed, x=>bobSpeed = x, 0.02f, 0.5f);
    }
    public void FadeOut()
    {
        Debug.Log("fadeout");
        GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        DOTween.To(() => bobSpeed, x => bobSpeed = x, 0, 0.5f);
    }
    private void Update()
    {
        
        switch (upwards)
        {
            case true:
                transform.localPosition += new Vector3(0, bobSpeed * Time.deltaTime, 0);
                if (transform.localPosition.y > bounds) { upwards = false; }
                break;
            case false:
                transform.localPosition -= new Vector3(0, bobSpeed * Time.deltaTime, 0);
                if (transform.localPosition.y < -bounds) { upwards = true; }
                break;
        }
        
    }
}
