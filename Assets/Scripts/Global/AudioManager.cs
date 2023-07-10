using UnityEngine.Audio; // needed when working with sounds
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Array of game sounds
    public Sound[] sounds;
    // Instance of audio manager
    public static AudioManager instance;

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
        // Plays bgm when called
        Play("Bgm1");
    }

    // Plays a sound
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }
    
    // Pauses a sound
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Pause();
    }
}