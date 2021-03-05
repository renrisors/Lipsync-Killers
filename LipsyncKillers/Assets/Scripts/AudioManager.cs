using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

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
        Play("Theme");    
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    /* void OnDestroy()
    {
        Debug.Log("Destroyed Audio Manager!");

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            PlayerPrefs.SetFloat(s.name + "Volume", s.volume);
            PlayerPrefs.SetFloat(s.name + "Pitch", s.pitch);
            PlayerPrefs.SetInt(s.name + "Loop", IsTrue(s.loop));
            s.source.loop = s.loop;

        }
    } */
}
