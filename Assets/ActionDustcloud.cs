using UnityEngine;
using System.Collections;
using UnityEditor;
using DG.Tweening;

public class ActionDustcloud : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Timer());
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(0.2f, 0.15f);
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.75f);
        transform.DOScale(0f, 0.4f);
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
        yield return null;
    }
    
}
