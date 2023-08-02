using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// A class used for sounds with the sound manager
/// </summary>
[System.Serializable]
public class Sound
{
    // Allows calls via name
    public string name;
    // The audio clip being played
    public AudioClip clip;

    // Allows use of sliders within the editor
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public AudioSource source;
    public bool loop;
}