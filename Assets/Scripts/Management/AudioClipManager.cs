using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup group;
    public bool loop;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 3f)] public float pitch;
    [HideInInspector] public AudioSource source;
}

public class AudioClipManager : MonoBehaviour {

    public Sound[] sounds;

    private bool hasBeenInitiaded;

	void Start()
    {
        if (hasBeenInitiaded) return;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.group;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        hasBeenInitiaded = true;
    }
	
	public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) return;
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.Stop();
    }
}
