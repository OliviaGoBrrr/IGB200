using UnityEngine;
using System.Collections;

public class ActionDustcloud : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        yield return null;
    }
}
