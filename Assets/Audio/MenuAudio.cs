using UnityEngine;
using UnityEngine.Audio;

public class MenuAudio : MonoBehaviour
{
    [SerializeField] AudioSource MusicAudio;
    [SerializeField] AudioSource UISoundFXAudio;

    [SerializeField] AudioClip buttonClick;
    public static MenuAudio Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayButtonClick(int pitchMag)
    {
        UISoundFXAudio.pitch = Mathf.Pow(1.059463f, pitchMag); // Randomly increases pitch
        UISoundFXAudio.resource = buttonClick;
        UISoundFXAudio.Play();
    }
}
