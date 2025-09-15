using UnityEngine;

public class SceneAudio : MonoBehaviour
{
    [SerializeField] DrumBeatsScriptable drumBeats;
    [SerializeField] AudioSource environmentalMusicAudio;
    [SerializeField] AudioSource drumMusicAudio;
    [Tooltip("Causes the drum to beat once. Intensity scales from 0~1")]
    public void DrumBeat(float intensity)
    {
        drumMusicAudio.volume = intensity;
        drumMusicAudio.pitch = Mathf.Pow(1.059463f, Random.Range(0, 2)); // Randomly increases pitch
        drumMusicAudio.resource = drumBeats.DrumBeats[Random.Range(0, drumBeats.DrumBeats.Count)];
        drumMusicAudio.Play();
    }
}
