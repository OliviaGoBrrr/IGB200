using System.Collections;
using UnityEngine;

public class WeirdCamFix : MonoBehaviour
{
    public Vector3 pos;
    void Start()
    {
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.05f);
        transform.position = pos;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
