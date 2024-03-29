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
    public AudioSource musicSource;
    public bool playBlueScreenSound = false;
    public bool playMusic = false;

    void Start () {
        foreach(NamedAudioClip clip in soundEffects) {
            GameObject soundObject = new GameObject();
            soundObject.name = clip.name + " Source";
            AudioSource soundSource = soundObject.AddComponent<AudioSource>();
            soundObject.transform.SetParent(transform);
        }

        if (playBlueScreenSound) {
            this.PlaySoundEffect("BIOS Beep");
        }

        if (playMusic) {
            PlayMusic("Music");
        }
    }

    public void PlaySoundEffect(string name) {
        foreach(NamedAudioClip audioClip in soundEffects) {
            if (audioClip.name == name) {
                Transform soundObject = transform.Find(audioClip.name + " Source");
                AudioSource effectSource = soundObject.GetComponent<AudioSource>();
                effectSource.clip = audioClip.clip;
                effectSource.volume = soundEffectsVolume;
                if (audioClip.name == "YouveGotMail") {
                    effectSource.volume = 0.45f * soundEffectsVolume;
                }
                effectSource.Play();
                break;
            }
        }
    }

    public void PlayMusic(string name) {
        foreach (NamedAudioClip audioClip in musicTracks)
        {
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
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }
}
