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
    bool gamer = true;

    public Bird inst;
    void Start()
    {
        StartCoroutine(SpawnBird());
    }
    IEnumerator SpawnBird()
    {
        if (gamer == true)
        {
            gamer = false;
            yield return new WaitForSeconds(Random.Range(0, maxFreq/2));
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(minFreq, maxFreq));
        }   
        Bird instance = Instantiate(inst, transform);
        instance.SetValues(Random.Range(minSpeed, maxSpeed), goingRight, Random.Range(negativeOffset, positiveOffset));
        StartCoroutine(SpawnBird());
        yield return null;
    }
}
