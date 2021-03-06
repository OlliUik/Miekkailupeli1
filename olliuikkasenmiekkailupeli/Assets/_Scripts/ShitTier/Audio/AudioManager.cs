﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance = null;

    public Sound[] sounds;

    public enum AudioChannel { Master, Sfx, Music, };

    public float masterVolumePercent { get; private set; }
    public float sfxVolumePercent { get; private set; }
    public float musicVolumePercent { get; private set; }

    public AudioSource musicSource, sfxSource;

    public void AddVolume(int index)
    {
        if(index == 0)
        {
            musicVolumePercent += 0.1f; // works fine with standalone inputmanager
        }
        if(index == 1)
        {
            sfxVolumePercent += 0.1f;
        }        
        Debug.Log("add");
    }

    public void LessVolume(int index)
    {
        if (index == 0)
        {
            musicVolumePercent -= 0.1f;
        }
        else if(index == 1)
        {
            sfxVolumePercent -= 0.1f;
        }
        Debug.Log("less");
    }

    private void Update()
    {        
        musicSource.volume = musicVolumePercent;
        sfxSource.volume = sfxVolumePercent;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
        }

        musicSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        musicVolumePercent = 0.1f;
        sfxVolumePercent = 0.1f;
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("sound not found");
            return;
        }
        s.source.volume = sfxVolumePercent;
        s.source.Play();
    }

    public void PlaySoundeffect(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("sound not found");
            return;
        }
        s.source.volume = sfxVolumePercent;
        s.source.PlayOneShot(s.clip);
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent);
        }
    }

    [SerializeField] AudioClip[] lightHitSounds = new AudioClip[4];
    [SerializeField] AudioClip[] swordClashSounds = new AudioClip[4];
    [SerializeField] AudioClip[] swordsSwingSounds = new AudioClip[3];
    [SerializeField] AudioClip[] hmphSounds = new AudioClip[7];

    public void PlayLightHitSound()
    {
        AudioClip clip = lightHitSounds[UnityEngine.Random.Range(0, lightHitSounds.Length)];
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySwordClashSound()
    {
        AudioClip clip = swordClashSounds[UnityEngine.Random.Range(0, swordClashSounds.Length)];
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySwordsSwingSound()
    {
        AudioClip clip = swordsSwingSounds[UnityEngine.Random.Range(0, swordsSwingSounds.Length)];
        sfxSource.PlayOneShot(clip);
    }

    public void PlayHmmph()
    {
        AudioClip clip = hmphSounds[UnityEngine.Random.Range(0, hmphSounds.Length)];
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void FadeInMusic()
    {
        StartCoroutine(AnimateMusicIn(1f));
    }

    IEnumerator AnimateMusicIn(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSource.volume = Mathf.Lerp(0, musicVolumePercent, percent);
            yield return null;
        }
    }

    public void FadeOutMusic()
    {
        StartCoroutine(AnimateMusicOut(1f));
    }

    IEnumerator AnimateMusicOut(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSource.volume = Mathf.Lerp(musicSource.volume, 0f, percent);
            yield return null;
        }
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
        }

        //musicSource.volume = musicVolumePercent;
        //sfxSource.volume = sfxVolumePercent;
        //musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        //musicSources[1].volume = musicVolumePercent * masterVolumePercent;
    }
}
