using UnityEngine.Audio; // needed when working with sounds
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // array of game sounds
    public Sound[] sounds;
    // instance of audio manager
    public static AudioManager instance;

    void Awake()
    {
        // bind the audio manager to this scripts instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // destroy if there is 2 and stops code calls with return
            Destroy(gameObject);
            return;
        }
        // makes object global and doesnt destroy
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
        // plays bgm when called
        Play("Bgm1");
        // FindObjectOfType<AudioManager>().Play("SoundNameHere"); calls sound from anywhere
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Pause();
    }
}