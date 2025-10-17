
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.U2D.Animation;

public class Bird : MonoBehaviour
{
    float Speed = 0;
    [SerializeField] List<SpriteLibraryAsset> birds;
    void Awake()
    {
        GetComponent<SpriteLibrary>().spriteLibraryAsset = birds[Random.Range(0, birds.Count)];
        Destroy(gameObject, 20);
    }
    public void SetValues(float speed, bool moveRight, float offset)
    {
        if (!moveRight) 
        {
            transform.rotation = Quaternion.Euler(-30, 180, transform.rotation.z);
        }
        Speed = speed;
        transform.position += new Vector3(0, 0, offset);
    }
    private void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }
}
