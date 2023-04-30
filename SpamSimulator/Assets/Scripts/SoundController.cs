using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundController : MonoBehaviour
{
    [System.Serializable]
    public class NamedAudioClip {
        public AudioClip clip;
        public string name;
    }

    public NamedAudioClip[] soundEffects;
    public NamedAudioClip[] musicTracks;
    [Range(0.0f, 1.0f)]
    public float soundEffectsVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    public float musicVolume = 1.0f;
    public AudioSource effectsSource;
    public AudioSource musicSource;
    public bool playStartupSound = false;

    void Start () {
        if (playStartupSound) {
            this.PlayAudioClip("Start Up");
        }   
    }

    public void PlayAudioClip(string name) {
        foreach(NamedAudioClip audioClip in soundEffects) {
            if (audioClip.name == name) {
                effectsSource.clip = audioClip.clip;
                effectsSource.Play();
                break;
            }
        }
    }

    public void PlayMusic(string name) {
        foreach (NamedAudioClip audioClip in musicTracks)
        {
            Debug.Log(audioClip.name);
            if (audioClip.name == name)
            {
                musicSource.clip = audioClip.clip;
                musicSource.Play();
                break;
            }
        }
    }

    public void SetSoundEffectsVolume (float volume) {
        soundEffectsVolume = volume;
        effectsSource.volume = soundEffectsVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }
}
