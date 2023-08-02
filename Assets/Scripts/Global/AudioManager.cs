// needed when working with sounds
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Fields
    
    // Instance of audio manager
    private static AudioManager instance;
    
    // Array of game sounds
    public Sound[] sounds;

    #endregion

    #region Methods

    void Awake()
    {
        // Bind the audio manager to this scripts instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Destroy if there is 2 and stops code calls with return
            Destroy(gameObject);
            return;
        }
        // Makes object global and doesnt destroy it when changing scenes
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        // Plays bgm when initialized
        Play("Bgm1");
    }

    /// <summary>
    /// Plays a sound when called
    /// </summary>
    /// <param name="name">The name of the sound being called</param>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }
    
    /// <summary>
    /// Pauses a sound when called
    /// </summary>
    /// <param name="name">The name of the sound being called</param>
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Pause();
    }
    
    #endregion
}
