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

    public NamedAudioClip[] audioClips;
    
    [Range(0.0f, 1.0f)]
    public float soundEffectsVolume;
    
    private AudioSource audioSource;

    void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(string name) {
        foreach(NamedAudioClip audioClip in audioClips) {
            if (audioClip.name == name) {
                audioSource.clip = audioClip.clip;
                audioSource.Play();
                break;
            }
        }
    }

    public void SetSoundEffectsVolume (float volume) {
        soundEffectsVolume = volume;
        audioSource.volume = soundEffectsVolume;
    }
}
