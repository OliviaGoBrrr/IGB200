using UnityEngine;

public class SceneAudio : MonoBehaviour
{
    [SerializeField] DrumBeatsScriptable drumBeats;
    [SerializeField] AudioSource environmentalMusicAudio;
    [SerializeField] AudioSource drumMusicAudio;
    [SerializeField] AudioSource gameSoundFXAudio;
    [SerializeField] AudioSource UISoundFXAudio;

    [SerializeField] AudioClip buttonClick;
    public void DrumBeat(float intensity)
    {
        drumMusicAudio.volume = intensity;
        drumMusicAudio.pitch = Mathf.Pow(1.059463f, Random.Range(0, 2)); // Randomly increases pitch
        drumMusicAudio.resource = drumBeats.DrumBeats[Random.Range(0, drumBeats.DrumBeats.Count)];
        drumMusicAudio.Play();
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
}
