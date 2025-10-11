using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    public bool goingRight;
    public float minFreq;
    public float maxFreq;
    public float negativeOffset;
    public float positiveOffset;

    public Bird inst;
    void Start()
    {
        StartCoroutine(SpawnBird());
    }
    IEnumerator SpawnBird()
    {
        yield return new WaitForSeconds(Random.Range(minFreq, maxFreq));
        Bird instance = Instantiate(inst, transform);
        instance.SetValues(Random.Range(minSpeed, maxSpeed), goingRight, Random.Range(negativeOffset, positiveOffset));
        StartCoroutine(SpawnBird());
        yield return null;
    }
}
