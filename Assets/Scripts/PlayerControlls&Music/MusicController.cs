using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource dialogSource;
    public AudioClip audioClip;
    public float SFXVolume;
    public float musicVolume;
    public float dialogVolume;
    // Start is called before the first frame update
    void Start()
    {
        
        DontDestroyOnLoad(gameObject);
        PlaySongLooped(audioClip);
    }

    void Update()
    {
        musicSource.volume = PlayerPrefs.GetFloat("music volume") * PlayerPrefs.GetFloat("master volume");
        SFXSource.volume = PlayerPrefs.GetFloat("sfx volume") * PlayerPrefs.GetFloat("master volume");
        dialogSource.volume = PlayerPrefs.GetFloat("dialog volume") * PlayerPrefs.GetFloat("master volume");
    }



    public void PlaySongLooped(AudioClip song)
    {
        musicSource.loop = true;
        musicSource.clip = song;
        musicSource.volume = musicVolume;
        musicSource.Play();

    }

    public void StopSong()
    {
        musicSource.Stop();
    }

    public void PlaySoundEfect(AudioClip sound)
    {

        SFXSource.PlayOneShot(sound);
    }
    public void PlayDialog(AudioClip sound)
    {

        dialogSource.PlayOneShot(sound);
    }

    public void StopAllSounds()
    {
        musicSource.Stop();
        SFXSource.Stop();
        dialogSource.Stop();

    }
}
