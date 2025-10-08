using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class MenuAudio : MonoBehaviour
{
    [SerializeField] AudioSource MusicAudio;
    [SerializeField] AudioSource UISoundFXAudio;

    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip squeak;
    public static MenuAudio Instance;
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
        MusicAudio.DOFade(0.35f, 2);
    }
    public void PlayButtonClick(int pitchMag)
    {
        UISoundFXAudio.pitch = Mathf.Pow(1.059463f, pitchMag);
        UISoundFXAudio.resource = buttonClick;
        UISoundFXAudio.Play();
    }
    public void PlaySqueak(int pitchMag)
    {
        UISoundFXAudio.pitch = Mathf.Pow(1.059463f, pitchMag);
        UISoundFXAudio.resource = squeak;
        UISoundFXAudio.Play();
    }
    public IEnumerator DestroySelf(float time)
    {
        MusicAudio.DOFade(0, time);
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        yield return null;
    }
}
