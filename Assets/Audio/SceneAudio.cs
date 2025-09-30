using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SceneAudio : MonoBehaviour
{
    bool crackleStart = false;
    [SerializeField] DrumBeatsScriptable drumBeats;
    [SerializeField] AudioSource environmentalMusicAudio;
    [SerializeField] AudioSource beatMusicAudio;
    [SerializeField] AudioSource drumMusicAudio;
    [SerializeField] AudioSource crackleMusicAudio;
    [SerializeField] AudioSource gameSoundFXAudio;
    [SerializeField] AudioSource UISoundFXAudio;

    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip scoreGained;
    [SerializeField] AudioClip scoreUnGained;
    public static SceneAudio instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void DrumBeat(float intensity)
    {
        drumMusicAudio.volume = intensity;
        drumMusicAudio.pitch = Mathf.Pow(1.059463f, Random.Range(0, 2)); // Randomly increases pitch
        drumMusicAudio.resource = drumBeats.DrumBeats[Random.Range(0, drumBeats.DrumBeats.Count)];
        drumMusicAudio.Play();
        if (crackleStart == false)
        {
            StartCrackle();
        }
    }
    private void StartCrackle()
    {
        crackleStart = true;
        crackleMusicAudio.Play();
    }
    public IEnumerator EndCrackle()
    {
        crackleMusicAudio.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        crackleMusicAudio.Stop();
        yield return null;
    }
    public void PlayGameSound(AudioClip clip, float intensity)
    {
        gameSoundFXAudio.volume = intensity;
        gameSoundFXAudio.pitch = Mathf.Pow(1.059463f, Random.Range(0, 2)); // Randomly increases pitch
        gameSoundFXAudio.resource = clip;
        gameSoundFXAudio.Play();
    }
    public void PlayButtonClick(int pitchMag)
    {
        UISoundFXAudio.pitch = Mathf.Pow(1.059463f, pitchMag); // Randomly increases pitch
        UISoundFXAudio.resource = buttonClick;
        UISoundFXAudio.Play();
    }
    public IEnumerator DestroySelf(float time)
    {
        environmentalMusicAudio.DOFade(0, time);
        beatMusicAudio.DOFade(0, time);
        drumMusicAudio.DOFade(0, time);
        crackleMusicAudio.DOFade(0, time);
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        yield return null;
    }
    public void ScoreGained()
    {
        gameSoundFXAudio.resource = scoreGained;
        gameSoundFXAudio.Play();
    }
    public void ScoreUngained()
    {
        gameSoundFXAudio.resource = scoreUnGained;
        gameSoundFXAudio.Play();
    }
}
