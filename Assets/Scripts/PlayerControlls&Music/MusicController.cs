using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource dialogSource;
    public AudioClip audioClip;
    public List<AudioClip> mainMenuSongs;
    public List<AudioClip> voidSongs;
    public List<AudioClip> mainRoomSongs;
    public List<AudioClip> mailboxSongs;
    public List<AudioClip> sellAreaSongs;
    public GlobalDissolveCon gdc;

    public float fadeOutTime = 0.5f;
    private bool fading = false;
    private float currentFadeTime;

    private int currentZone = -1;
    private List<AudioClip>[] audioTracks = new List<AudioClip>[6]; // audioTracks[area][songNumber]

    void Start()
    {
        audioTracks[0] = mainMenuSongs;
        audioTracks[1] = voidSongs;
        audioTracks[2] = mainRoomSongs;
        audioTracks[3] = null; // what the sigma
        audioTracks[4] = mailboxSongs;
        audioTracks[5] = sellAreaSongs;
        DontDestroyOnLoad(gameObject);
        PlayAreaSongs(0);
    }

    void Update()
    {
        int area;
        try
        {
            gdc = GameObject.Find("Objects").GetComponent<GlobalDissolveCon>();
        } catch (NullReferenceException)
        {
            gdc = null;
            // in main scene
        }
        
        if (gdc)
        {
            area = gdc.trueArea + 2; // the void is -1. I add 2 to make it 1. If you are not in the main game area will be 0 (main menu music)
        } else
        {
            area = 0;
        }

        if (currentZone != area && !fading)
        {
            // switch song
            currentFadeTime = fadeOutTime;
            fading = true;
            
        }
        if (currentFadeTime <= 0 && fading)
        {
            fading = false;
            currentZone = area;
            PlayAreaSongs(currentZone);
        }
        if (!fading)
        {
            musicSource.volume = PlayerPrefs.GetFloat("music volume") * PlayerPrefs.GetFloat("master volume");

        }
        else
        {
            currentFadeTime -= Time.deltaTime;
            musicSource.volume = PlayerPrefs.GetFloat("music volume") * PlayerPrefs.GetFloat("master volume") * (currentFadeTime / fadeOutTime);

        }
        SFXSource.volume = PlayerPrefs.GetFloat("sfx volume") * PlayerPrefs.GetFloat("master volume");
        dialogSource.volume = PlayerPrefs.GetFloat("dialog volume") * PlayerPrefs.GetFloat("master volume");
    }

    private void PlayAreaSongs(int area)
    {

        if (area >= 0 && area < audioTracks.Length)
        {
            StopSong();
            AudioClip allSongs = JoinAudioClips(audioTracks[area], "Area " + area + " Songs");
            PlaySongLooped(allSongs);
            
        }

    }

    private AudioClip JoinAudioClips(List<AudioClip> audioClips, string name)
    {
        if (audioClips.Count == 1)
        {
            return audioClips[0];
        }

        // find how many samples are in all the clips
        int totalLength = 0;
        foreach (AudioClip clip in audioClips)
        {
            totalLength += clip.samples;
        }

        Debug.Log("totalLength: " + totalLength.ToString());

        float[] combinedData = new float[totalLength * audioClips[0].channels]; // make array to hold data from the clips
        int currentPosition = 0;
        foreach (AudioClip clip in audioClips)
        {
            Debug.Log("clip samples: " + clip.samples.ToString());
            float[] tempData = new float[clip.samples * clip.channels];
            clip.GetData(tempData, 0);

            Array.Copy(tempData, 0, combinedData, currentPosition, clip.samples * clip.channels);
            currentPosition += clip.samples * clip.channels;
            Debug.Log("currentPosition: " + currentPosition.ToString());
        }

        AudioClip combinedClip = AudioClip.Create(name, totalLength, audioClips[0].channels, audioClips[0].frequency,false);
        Debug.Log("combinedData: " + combinedData.Length.ToString());
        combinedClip.SetData(combinedData, 0);
        return combinedClip;

    }

    public void PlaySongLooped(AudioClip song)
    {
        musicSource.loop = true;
        musicSource.clip = song;
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
