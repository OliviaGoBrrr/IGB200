using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrumBeats", menuName = "ScriptableObjects/DrumBeats")]
public class DrumBeatsScriptable : ScriptableObject
{
    [Tooltip("The audio clips that will be used for drum beats")]
    [SerializeField] List<AudioClip> drumBeats;
    public List<AudioClip> DrumBeats { get => drumBeats; private set => drumBeats = value; }
}
