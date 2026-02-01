using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    //public AudioMixer mixer;

    // MUSIC AND THE LIKE
    [SerializeField] private AudioClip[] _Music;
    private AudioSource[] sources;


    /*
     * 0 - happy intro
     * 1 - Basoon
     * 2 - Clarinet
     * 3 - Flute
     * 4 - Horn
     * 5 - Trumpet
     * 6 - Trombone
     * 7 - Tuba
     * 8 - Violin
     * 9 - Viola
     * 10 - Cello
     * 11 - Bass
     */

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        /*AudioMixerGroup musicGroup = null;
        var groups = mixer.FindMatchingGroups("Music");
        if (groups.Length > 0) musicGroup = groups[0];*/

        sources = new AudioSource[12];
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i] = this.gameObject.AddComponent<AudioSource>();
            sources[i].playOnAwake = false;
            sources[i].loop = true;
            //sources[i].outputAudioMixerGroup = musicGroup; //Bring back when there is a mixer
        }
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].clip = _Music[i];
        }
        for (int i = 1; i < sources.Length; i++)
        {
            sources[i].volume = 0f;
        }
        sources[0].volume = 1f;
        sources[0].Play();
    }
    public void PlayWaltz()
    {
        sources[0].Stop();

        for (int i = 1; i < sources.Length; i++)
        {
            sources[i].volume = 0f;
            sources[i].Play();
        }
        for (int i = 8; i < sources.Length; i++)
        {
            sources[i].volume = 1f;
        }
    }

    public void PlayIntro()
    {
        for(int i = 1; i < sources.Length; i++)
        {
            sources[i].volume = 0f;
            sources[i].Stop();
        }
        sources[0].volume = 1f;
        sources[0].Play();
    }
    public void AddMusic(int maskNumber) //Call this when a mask is dragged onto a dummy
    {
        sources[maskNumber].volume = 1f;
    }

    public void RemoveMusic(int maskNumber) //Call this when a mask is removed from a dummy
    {
        sources[maskNumber].volume = 0f;
    }

    public void StopWaltz()
    {
        for (int i = 1; i < sources.Length; i++)
        {
            sources[i].Stop();
        }
        //add record scratch sound effect here later
    }

    public void ResetMusic()
    {
        for (int i = 0; i < 7; i++)
        {
            sources[i].volume = 0f;
        }
    }

    #region Volume Controls
    /*public void SetSFXVolume(float volume)
    {
        // Convert 0–1 slider range to decibels (-80 dB to 0 dB)
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        mixer.SetFloat("SFXVolume", dB);
    }
    public void SetMusicvolume(float volume)
    {
        // Convert 0–1 slider range to decibels (-80 dB to 0 dB)
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        mixer.SetFloat("Music", dB);
    }*/
    #endregion
}