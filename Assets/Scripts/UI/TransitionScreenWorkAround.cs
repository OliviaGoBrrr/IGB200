using UnityEngine;
using UnityEngine.UI;

public class TransitionScreenWorkAround : MonoBehaviour
{
    // This script exists because unity does not allow for UI material properties to be accessed in the animation timeline
    
    public Material material;

    public float animation_progress = 1.0f;
    public float direction = 1.0f;

    void Awake()
    {
        material = GetComponent<Image>().material;
        animation_progress = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_AnimationProgress", animation_progress);
        material.SetFloat("_Direction", direction);
    }
}
